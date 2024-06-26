using System;
using System.Collections.Generic;
using BYOJoystick.Actions;
using BYOJoystick.Bindings;
using BYOJoystick.UI.Scripts;
using SharpDX.DirectInput;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Manager = BYOJoystick.Managers.Base.Manager;

namespace BYOJoystick.UI
{
    public class BYOJUI : MonoBehaviour
    {
        public GameObject       VehicleSelection;
        public VehicleSelector  VehicleSelectorPrefab;
        public GameObject       Devices;
        public GameObject       DeviceHeaderPrefab;
        public Button           RefreshDevicesButton;
        public GameObject       CategoryActionsList;
        public CategoryActions  CategoryActionsPrefab;
        public GameObject       BindingGrid;
        public BindingCell      BindingCellPrefab;
        public GameObject       BindingCellSpacerPrefab;
        public GameObject       BindingCellInvalidPrefab;
        public BYOJBindingModal BYOJBindingModal;

        private readonly List<VehicleSelector> _selectors = new List<VehicleSelector>();
        private          Manager               _selectedManager;
        public           bool                  IsBinding => BYOJBindingModal.IsBinding;

        public void Initialise()
        {
            PopulateVehicleSelectors();
            PopulateDeviceHeaders();
            OnSelectVehicle(_selectors[0].ShortName);
            RefreshDevicesButton.onClick.AddListener(OnRefreshDevices);
        }

        private void PopulateVehicleSelectors()
        {
            foreach (Transform child in VehicleSelection.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (var manager in BYOJ.GetAllManagers())
            {
                var selector = Instantiate(VehicleSelectorPrefab, VehicleSelection.transform);
                _selectors.Add(selector);
                selector.Initialise(manager.GameName, manager.ShortName, OnSelectVehicle, OnSaveVehicle, OnLoadVehicle, OnCopyFromVehicle);
            }
        }

        private void PopulateDeviceHeaders()
        {
            var devices = BYOJ.GetAllJoysticks();

            foreach (Transform child in Devices.transform)
            {
                Destroy(child.gameObject);
            }

            Instantiate(DeviceHeaderPrefab, Devices.transform).GetComponentInChildren<TextMeshProUGUI>().text = "Keyboard";

            foreach (var device in devices)
            {
                Instantiate(DeviceHeaderPrefab, Devices.transform).GetComponentInChildren<TextMeshProUGUI>().text = device.Information.InstanceName;
            }
        }

        private void PopulateActions()
        {
            foreach (Transform child in CategoryActionsList.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (ActionCategory category in Enum.GetValues(typeof(ActionCategory)))
            {
                var actions = _selectedManager.ControlActionsByCategory[category];
                if (actions.Count == 0)
                    continue;
                var categoryActions = Instantiate(CategoryActionsPrefab, CategoryActionsList.transform);
                categoryActions.Initialise(category, actions);
            }
        }

        private void PopulateBindings()
        {
            foreach (Transform child in BindingGrid.transform)
            {
                Destroy(child.gameObject);
            }

            var joysticks = BYOJ.GetAllJoysticks();

            var gridLayout = BindingGrid.GetComponent<GridLayoutGroup>();
            gridLayout.constraintCount = 1 + joysticks.Count;

            var keyboardBindings = BYOJ.ConfigManager.GetKeyboardBindingStore(_selectedManager.ShortName);

            foreach (ActionCategory category in Enum.GetValues(typeof(ActionCategory)))
            {
                var actions = _selectedManager.ControlActionsByCategory[category];
                if (actions.Count == 0)
                    continue;

                Instantiate(BindingCellSpacerPrefab, BindingGrid.transform);

                foreach (var action in actions)
                {
                    var bindings = keyboardBindings.Get(action.Name);
                    if (category != ActionCategory.Modifier && action.Input == ActionInput.Button)
                    {
                        var cell = Instantiate(BindingCellPrefab, BindingGrid.transform);
                        cell.Initialise(OnBindAction, action, bindings, isKeyboard: true);
                    }
                    else
                        Instantiate(BindingCellInvalidPrefab, BindingGrid.transform);
                }
            }

            foreach (var device in joysticks)
            {
                var joystickBindings = BYOJ.ConfigManager.GetJoystickBindingStore(_selectedManager.ShortName, device.Information.InstanceGuid);

                foreach (ActionCategory category in Enum.GetValues(typeof(ActionCategory)))
                {
                    var actions = _selectedManager.ControlActionsByCategory[category];
                    if (actions.Count == 0)
                        continue;

                    Instantiate(BindingCellSpacerPrefab, BindingGrid.transform);

                    foreach (var action in actions)
                    {
                        var bindings = joystickBindings.Get(action.Name);
                        var cell     = Instantiate(BindingCellPrefab, BindingGrid.transform);
                        cell.Initialise(OnBindAction, action, bindings, device.Information.InstanceGuid);
                    }
                }
            }
        }

        private void OnSelectVehicle(string shortName)
        {
            foreach (var selector in _selectors)
            {
                selector.SetSelected(selector.ShortName == shortName);
            }

            _selectedManager = BYOJ.GetManager(shortName);
            PopulateActions();
            PopulateBindings();
        }

        private void OnSaveVehicle(string shortName)
        {
            BYOJ.ConfigManager.SaveBindings(shortName);
        }

        private void OnLoadVehicle(string shortName)
        {
            BYOJ.ConfigManager.LoadBindings(shortName);
            PopulateBindings();
            if (_selectedManager == BYOJ.ActiveManager)
                BYOJ.ReloadActiveManager();
        }

        private void OnCopyFromVehicle(string shortName)
        {
            var fromManager = BYOJ.GetManager(shortName);
            BYOJ.ConfigManager.CopyBindings(fromManager, _selectedManager);
            PopulateBindings();
            BYOJ.ReloadActiveManager();
        }

        private void OnBindAction(BindingCell cell, Guid joystickId, bool isKeyboard, ControlAction action)
        {
            BYOJBindingModal.gameObject.SetActive(true);
            if (isKeyboard)
            {
                var bindings = BYOJ.ConfigManager.GetKeyboardBindingStore(_selectedManager.ShortName).Get(action.Name);
                BYOJBindingModal.ShowBindingModal(cell, action, bindings, true, null, OnKeyboardBindingComplete, OnBindingCancelled);
            }
            else
            {
                var bindings = BYOJ.ConfigManager.GetJoystickBindingStore(_selectedManager.ShortName, joystickId).Get(action.Name);
                var joystick = BYOJ.GetConnectedJoystick(joystickId);
                if (joystick == null)
                {
                    BYOJBindingModal.gameObject.SetActive(false);
                    throw new InvalidOperationException($"No joystick found with ID {joystickId}");
                }

                BYOJBindingModal.ShowBindingModal(cell, action, bindings, false, joystick, OnJoystickBindingComplete, OnBindingCancelled);
            }
        }

        private void OnKeyboardBindingComplete(BindingCell   cell,
                                               ControlAction action,
                                               bool          isKeyboard,
                                               Joystick      joystick,
                                               List<Binding> newBindings)
        {
            BYOJBindingModal.gameObject.SetActive(false);

            var bindingStore = BYOJ.ConfigManager.GetKeyboardBindingStore(_selectedManager.ShortName);
            var bindings     = bindingStore.Get(action.Name);
            if (_selectedManager == BYOJ.ActiveManager)
            {
                foreach (var binding in bindings)
                {
                    BYOJ.StopListeningForKeyboardBinding(binding);
                }
            }

            bindings.Clear();

            foreach (var newBinding in newBindings)
            {
                bindings.Add(newBinding as KeyboardBinding);
                if (_selectedManager == BYOJ.ActiveManager)
                    BYOJ.StartListeningForKeyboardBinding(newBinding as KeyboardBinding);
            }

            cell.UpdateText();
        }

        private void OnJoystickBindingComplete(BindingCell   cell,
                                               ControlAction action,
                                               bool          isKeyboard,
                                               Joystick      joystick,
                                               List<Binding> newBindings)
        {
            BYOJBindingModal.gameObject.SetActive(false);

            var bindingStore = BYOJ.ConfigManager.GetJoystickBindingStore(_selectedManager.ShortName, joystick.Information.InstanceGuid);
            var bindings     = bindingStore.Get(action.Name);

            bool modifiersChanged = false;

            if (_selectedManager == BYOJ.ActiveManager)
            {
                foreach (var binding in bindings)
                {
                    BYOJ.StopListeningForJoystickBinding(binding);
                    if (binding.Target == "Modifier" || binding.RequiresModifier)
                        modifiersChanged = true;
                }
            }

            bindings.Clear();

            foreach (var newBinding in newBindings)
            {
                var binding = newBinding as JoystickBinding;
                bindings.Add(binding);
                if (_selectedManager != BYOJ.ActiveManager)
                    continue;
                if (binding!.Target == "Modifier" || binding.RequiresModifier)
                    modifiersChanged = true;
                BYOJ.StartListeningForJoystickBinding(binding);
                BYOJ.ListenForModifierPresses(binding);
            }

            cell.UpdateText();

            // Reload the active manager if anything to do with modifiers was changed to ensure correct behaviour
            if (modifiersChanged)
                BYOJ.ReloadActiveManager();
        }

        public void OnBindingCancelled()
        {
            BYOJBindingModal.gameObject.SetActive(false);
        }

        private void OnRefreshDevices()
        {
            BYOJ.UpdateConnectedJoysticks();
            PopulateDeviceHeaders();
            PopulateBindings();
        }
    }
}