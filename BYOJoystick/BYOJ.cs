using System;
using System.Collections.Generic;
using System.Linq;
using BYOJoystick.Bindings;
using BYOJoystick.Config;
using BYOJoystick.Managers;
using BYOJoystick.UI;
using SharpDX.DirectInput;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using VTOLVR.Multiplayer;
using DeviceType = SharpDX.DirectInput.DeviceType;
using Manager = BYOJoystick.Managers.Base.Manager;

namespace BYOJoystick
{
    public class BYOJ : MonoBehaviour
    {
        public BYOJUI BYOJUI;

        public static readonly DirectInput   DirectInput   = new DirectInput();
        public static readonly ConfigManager ConfigManager = new ConfigManager();
        public static          Manager       ActiveManager;

        private static readonly Dictionary<string, Manager>    ManagerLookup          = new Dictionary<string, Manager>();
        private static readonly List<Manager>                  Managers               = new List<Manager>();
        private static readonly List<BYOJJoystick>             Joysticks              = new List<BYOJJoystick>();
        private static readonly Dictionary<Guid, BYOJJoystick> ConnectedJoysticks     = new Dictionary<Guid, BYOJJoystick>();
        private static readonly HashSet<Guid>                  ActiveJoysticks        = new HashSet<Guid>();
        private static readonly List<BYOJJoystick>             ActiveJoysticksList    = new List<BYOJJoystick>();
        private static readonly List<KeyboardBinding>          ActiveKeyboardBindings = new List<KeyboardBinding>();

        private static readonly Dictionary<Guid, InputEvent[]> Events          = new Dictionary<Guid, InputEvent[]>();
        private static readonly List<ActiveModifier>           ActiveModifiers = new List<ActiveModifier>();

        private List<JoystickUpdate> _updates = new List<JoystickUpdate>();

        private static ABObjectToggler _spSeatSwitcher;

        private class InputEvent
        {
            public Action<int, bool> Callback;
            public bool              ModifierExists;
        }

        private void Start()
        {
            var inputModule = GetComponentInChildren<StandaloneInputModule>();
            var eventSystem = GetComponentInChildren<EventSystem>();
            inputModule.forceModuleActive = true;
            eventSystem.enabled           = true;
            EventSystem.current           = eventSystem;

            UpdateConnectedJoysticks();

            AddManager(new AV42CManager());
            AddManager(new FA26BManager());
            AddManager(new F45AManager());
            AddManager(new AH94FrontManager());
            AddManager(new AH94RearManager());
            AddManager(new T55FrontManager());
            AddManager(new T55RearManager());
            AddManager(new EF24GFrontManager());
            AddManager(new EF24GRearManager());

            ConfigManager.Initialise(Managers);
            BYOJUI.Initialise();
        }

        private static void AddManager(Manager manager)
        {
            manager.Initialise();
            Managers.Add(manager);
            ManagerLookup.Add(manager.ShortName, manager);
        }

        public static Manager GetManager(string shortName)
        {
            if (!ManagerLookup.TryGetValue(shortName, out var manager))
                throw new InvalidOperationException($"Manager with short name {shortName} not found");
            return manager;
        }

        public static IReadOnlyList<Manager> GetAllManagers()
        {
            return Managers;
        }

        public static void UpdateConnectedJoysticks()
        {
            ConnectedJoysticks.Clear();
            Joysticks.Clear();
            var devices = DirectInput.GetDevices().Where(IsJoystick).ToList();
            Joysticks.AddRange(devices.Select(device => new BYOJJoystick(DirectInput, device.InstanceGuid)).ToList());
            for (int i = 0; i < Joysticks.Count; i++)
            {
                var joystick = Joysticks[i];
                if (!Events.ContainsKey(joystick.Information.InstanceGuid))
                {
                    Events[joystick.Information.InstanceGuid] = new InputEvent[268];
                    for (int j = 0; j < 268; j++)
                    {
                        Events[joystick.Information.InstanceGuid][j] = new InputEvent();
                    }
                }

                ConnectedJoysticks.Add(joystick.Information.InstanceGuid, joystick);
            }

            Joysticks.Sort((a, b) => string.Compare(a.Information.InstanceName, b.Information.InstanceName, StringComparison.Ordinal));
        }

        public static BYOJJoystick GetConnectedJoystick(Guid joystickId)
        {
            ConnectedJoysticks.TryGetValue(joystickId, out var joystick);
            return joystick;
        }

        public static IReadOnlyList<Joystick> GetAllJoysticks()
        {
            return Joysticks;
        }

        private static bool IsJoystick(DeviceInstance deviceInstance)
        {
            return deviceInstance.Type == DeviceType.Joystick
                || deviceInstance.Type == DeviceType.Gamepad
                || deviceInstance.Type == DeviceType.FirstPerson
                || deviceInstance.Type == DeviceType.Flight
                || deviceInstance.Type == DeviceType.Driving
                || deviceInstance.Type == DeviceType.Supplemental;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F10))
            {
                if (BYOJUI.gameObject.activeSelf)
                {
                    if (!BYOJUI.IsBinding)
                    {
                        BYOJUI.OnBindingCancelled();
                        BYOJUI.gameObject.SetActive(false);
                    }
                }
                else
                    BYOJUI.gameObject.SetActive(true);
            }

            CheckCurrentVehicle();

            if (ActiveManager == null)
                return;

            PollJoysticks();
            PollKeyboard();
            ActiveManager.RunPostUpdateControls();
        }

        private static void CheckCurrentVehicle()
        {
            if (!IsFlyingScene())
            {
                if (ActiveManager != null)
                    ClearActiveManager();
                return;
            }


            var playerActor = FlightSceneManager.instance?.playerActor;
            if (playerActor == null || !playerActor.alive)
            {
                if (ActiveManager != null)
                    ClearActiveManager();
                return;
            }

            if (ActiveManager == null || (ActiveManager != null && !VTOLMPUtils.IsMultiplayer() && ActiveManager.IsMulticrew && _spSeatSwitcher.isA != ActiveManager.IsSeatA))
                SelectManager();
        }

        public static bool IsFlyingScene()
        {
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            return buildIndex == 7 || buildIndex == 11 || Application.isEditor;
        }

        private static void SelectManager()
        {
            Plugin.Log("Selecting manager");
            var playerVehicleMaster = FlightSceneManager.instance?.playerVehicleMaster;
            if (playerVehicleMaster == null)
                return;
            Plugin.Log("Found player vehicle master");
            var playerVehicle = playerVehicleMaster.playerVehicle;
            if (playerVehicle == null)
                return;
            Plugin.Log("Found vehicle");
            var vehicle = playerVehicleMaster.gameObject;
            if (vehicle == null)
                return;

            if (VTOLMPUtils.IsMultiplayer())
                SelectManagerMultiPlayer(playerVehicle, vehicle);
            else
                SelectManagerSinglePlayer(playerVehicle, vehicle);
        }

        private static void SelectManagerMultiPlayer(PlayerVehicle playerVehicle, GameObject vehicle)
        {
            Plugin.Log("Selecting multiplayer manager");
            var player = VTOLMPSceneManager.instance.localPlayer;
            if (player == null)
                return;
            Plugin.Log($"Got local player {player.pilotName}");
            var slot = VTOLMPSceneManager.instance.GetSlot(player);
            if (slot == null)
                return;
            Plugin.Log($"Got seat index {slot.crewSeatIdx}");
            string shortName = GetShortName(playerVehicle.vehicleName, slot.crewSeatIdx == 0);
            SetActiveManager(GetManager(shortName), vehicle);
        }

        private static void SelectManagerSinglePlayer(PlayerVehicle playerVehicle, GameObject vehicle)
        {
            var pilotSwitcher = FindObjectOfType<AH94PilotSwitcher>();
            if (pilotSwitcher == null)
            {
                string shortName = GetShortName(playerVehicle.vehicleName);
                SetActiveManager(GetManager(shortName), vehicle);
            }
            else
            {
                _spSeatSwitcher = pilotSwitcher.GetComponent<ABObjectToggler>();
                string shortName = GetShortName(playerVehicle.vehicleName, _spSeatSwitcher.isA);
                SetActiveManager(GetManager(shortName), vehicle);
            }
        }

        private static string GetShortName(string vehicleName, bool isSeatA = true)
        {
            // A is the front seat, B is the rear seat, except for the AH-94 where it's the opposite
            return vehicleName switch
            {
                "AV-42C"  => "AV42C",
                "F/A-26B" => "FA26B",
                "F-45A"   => "F45A",
                "AH-94"   => isSeatA ? "AH94Rear" : "AH94Front",
                "T-55"    => isSeatA ? "T55Front" : "T55Rear",
                "EF-24G"  => isSeatA ? "EF24GFront" : "EF24GRear",
                _         => throw new InvalidOperationException($"Vehicle {vehicleName} not supported")
            };
        }

        public static void ReloadActiveManager()
        {
            SetActiveManager(ActiveManager);
        }

        private static void ClearActiveManager()
        {
            ActiveManager?.Clear();
            ActiveManager = null;

            foreach (var events in Events.Values)
            {
                for (int i = 0; i < events.Length; i++)
                {
                    events[i].Callback       = null;
                    events[i].ModifierExists = false;
                }
            }

            ActiveModifiers.Clear();
            ActiveJoysticks.Clear();
            ActiveJoysticksList.Clear();
            ActiveKeyboardBindings.Clear();
            Plugin.Log("Active manager cleared.");
        }

        private static void SetActiveManager(Manager manager, GameObject vehicle = null)
        {
            if (ActiveManager != null)
                ClearActiveManager();

            ActiveManager = manager;
            if (manager == null)
            {
                Plugin.Log("New manager is null, failed to set new manager.");
                return;
            }

            Plugin.Log($"Setting active manager to {manager.GameName}...");

            manager.MapControls(vehicle);

            Plugin.Log("Initialising bindings...");
            var keyboardBindings = ConfigManager.GetKeyboardBindings(manager.ShortName);
            Plugin.Log($"Processing {keyboardBindings.Count} keyboard bindings...");
            for (int i = 0; i < keyboardBindings.Count; i++)
            {
                StartListeningForKeyboardBinding(keyboardBindings[i]);
            }

            var joystickBindings = ConfigManager.GetJoystickBindings(manager.ShortName);
            Plugin.Log($"Processing {joystickBindings.Count} joystick bindings...");
            for (int i = 0; i < joystickBindings.Count; i++)
            {
                var binding = joystickBindings[i];
                if (!ConnectedJoysticks.ContainsKey(binding.JoystickId))
                    continue;

                StartListeningForJoystickBinding(binding);
            }

            for (int i = 0; i < joystickBindings.Count; i++)
            {
                ListenForModifierPresses(joystickBindings[i]);
            }

            Plugin.Log("Bindings initialised successfully.");
        }

        public static void StartListeningForKeyboardBinding(KeyboardBinding binding)
        {
            var action = ActiveManager.GetAction(binding.Target);
            binding.SetAction(action);
            ActiveKeyboardBindings.Add(binding);
        }

        public static void StopListeningForKeyboardBinding(KeyboardBinding binding)
        {
            ActiveKeyboardBindings.Remove(binding);
        }

        private struct ActiveModifier
        {
            public readonly Guid           Guid;
            public readonly JoystickOffset Offset;

            public ActiveModifier(Guid guid, JoystickOffset offset)
            {
                Guid   = guid;
                Offset = offset;
            }
        }

        public static void StartListeningForJoystickBinding(JoystickBinding binding)
        {
            if (!ActiveJoysticks.Contains(binding.JoystickId))
            {
                if (!ConnectedJoysticks.TryGetValue(binding.JoystickId, out var joystick))
                    return;
                ActiveJoysticksList.Add(joystick);
                ActiveJoysticks.Add(joystick.Information.InstanceGuid);
            }

            if (binding.Target == "Modifier")
                ActiveModifiers.Add(new ActiveModifier(binding.JoystickId, binding.Offset));
            else
            {
                var inputEvent = Events[binding.JoystickId][(int)binding.Offset];
                inputEvent.Callback       += binding.OnStateChange;
                inputEvent.ModifierExists =  binding.RequiresModifier || inputEvent.ModifierExists;
            }

            var action = ActiveManager.GetAction(binding.Target);
            binding.SetAction(action);
        }

        public static void ListenForModifierPresses(JoystickBinding binding)
        {
            for (int i = 0; i < ActiveModifiers.Count; i++)
            {
                var modifier   = ActiveModifiers[i];
                var inputEvent = Events[modifier.Guid][(int)modifier.Offset];
                inputEvent.Callback += binding.OnModifierChange;
            }
        }

        public static void StopListeningForJoystickBinding(JoystickBinding binding)
        {
            var inputEvent = Events[binding.JoystickId][(int)binding.Offset];
            inputEvent.Callback -= binding.OnStateChange;

            if (!binding.RequiresModifier || binding.Target == "Modifier")
                return;

            for (int i = 0; i < ActiveModifiers.Count; i++)
            {
                var modifier           = ActiveModifiers[i];
                var modifierInputEvent = Events[modifier.Guid][(int)modifier.Offset];
                modifierInputEvent.Callback -= binding.OnModifierChange;
            }
        }

        private void PollJoysticks()
        {
            if (BYOJUI.IsBinding)
                return;

            for (int i = 0; i < ActiveJoysticksList.Count; i++)
            {
                var joystick = ActiveJoysticksList[i];
                if (!DirectInput.IsDeviceAttached(joystick.Information.InstanceGuid))
                {
                    ActiveJoysticksList.RemoveAt(i--);
                    ActiveJoysticks.Remove(joystick.Information.InstanceGuid);
                    continue;
                }

                joystick.Poll();

                var inputEvents = Events[joystick.Information.InstanceGuid];

                joystick.GetBufferedData(ref _updates);
                for (int j = 0; j < _updates.Count; j++)
                {
                    var update     = _updates[j];
                    var inputEvent = inputEvents[update.RawOffset];
                    inputEvent.Callback?.Invoke(update.Value, inputEvent.ModifierExists);
                }
            }
        }

        private void PollKeyboard()
        {
            if (BYOJUI.IsBinding)
                return;

            bool isAnyModifierPressed = Input.GetKey(KeyCode.LeftShift)
                                     || Input.GetKey(KeyCode.RightShift)
                                     || Input.GetKey(KeyCode.LeftControl)
                                     || Input.GetKey(KeyCode.RightControl)
                                     || Input.GetKey(KeyCode.LeftAlt)
                                     || Input.GetKey(KeyCode.RightAlt);

            for (int i = 0; i < ActiveKeyboardBindings.Count; i++)
            {
                ActiveKeyboardBindings[i].UpdateState(isAnyModifierPressed);
            }
        }
    }
}