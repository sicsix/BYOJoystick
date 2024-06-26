using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using BYOJoystick.Bindings;
using Manager = BYOJoystick.Managers.Base.Manager;

namespace BYOJoystick.Config
{
    public class ConfigManager
    {
        private readonly Dictionary<string, VehicleBindingStore> _vehicleBindingStores = new Dictionary<string, VehicleBindingStore>();
        private readonly XmlSerializer                           _serializer           = new XmlSerializer(typeof(Bindings));

        public void Initialise(List<Manager> managers)
        {
            Plugin.Log("Initialising ConfigManager...");

            if (!Directory.Exists(PilotSaveManager.saveDataPath))
                Directory.CreateDirectory(PilotSaveManager.saveDataPath);

            foreach (var manager in managers)
            {
                LoadBindings(manager);
            }

            Plugin.Log("ConfigManager initialised.");
        }

        [Serializable]
        public class Bindings
        {
            public List<KeyboardBinding> KeyboardBindings { get; set; }
            public List<JoystickBinding> JoystickBindings { get; set; }
        }

        public void LoadBindings(string shortName)
        {
            LoadBindings(BYOJ.GetManager(shortName));
        }

        private void LoadBindings(Manager manager)
        {
            string path = $@"{PilotSaveManager.saveDataPath}\BYOJ_{manager.ShortName}.xml";

            Plugin.Log($"Loading bindings for {manager.ShortName} from {path}");
            if (!File.Exists(path))
                return;
            try
            {
                using var reader   = new StreamReader(path);
                var       bindings = (Bindings)_serializer.Deserialize(reader);

                var vehicleBindingStore = GetVehicleBindingStore(manager.ShortName);
                vehicleBindingStore.ClearAll();

                var keyboardBindings = vehicleBindingStore.GetKeyboardBindings();
                foreach (var binding in bindings.KeyboardBindings)
                {
                    if (manager.ControlActions.ContainsKey(binding.Target))
                        keyboardBindings.Add(binding.Target, binding);
                    else
                        Plugin.Log($"Binding target {binding.Target} not found in {manager.ShortName}");
                }

                foreach (var binding in bindings.JoystickBindings)
                {
                    if (manager.ControlActions.ContainsKey(binding.Target))
                        vehicleBindingStore.GetJoystickBindings(binding.JoystickId).Add(binding.Target, binding);
                    else
                        Plugin.Log($"Binding target {binding.Target} not found in {manager.ShortName}");
                }

                Plugin.Log($"Loaded {bindings.KeyboardBindings.Count} keyboard bindings and {bindings.JoystickBindings.Count} joystick bindings for {manager.ShortName}.");
            }
            catch (Exception ex)
            {
                Plugin.Log($"An error occurred while loading bindings: {ex.Message}");
            }
        }

        public void SaveBindings(string shortName)
        {
            SaveBindings(BYOJ.GetManager(shortName));
        }


        private void SaveBindings(Manager manager)
        {
            var vehicleBindingStore = GetVehicleBindingStore(manager.ShortName);

            var bindings = new Bindings
            {
                KeyboardBindings = vehicleBindingStore.GetKeyboardBindings().GetAllBindings(),
                JoystickBindings = vehicleBindingStore.GetAllJoystickBindings()
            };

            try
            {
                using var writer = new StreamWriter($@"{PilotSaveManager.saveDataPath}\BYOJ_{manager.ShortName}.xml");
                _serializer.Serialize(writer, bindings);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving bindings: {ex.Message}");
            }
        }

        public BindingStore<KeyboardBinding> GetKeyboardBindingStore(string vehicleName)
        {
            return GetVehicleBindingStore(vehicleName).GetKeyboardBindings();
        }

        public BindingStore<JoystickBinding> GetJoystickBindingStore(string vehicleName, Guid joystickId)
        {
            return GetVehicleBindingStore(vehicleName).GetJoystickBindings(joystickId);
        }

        public List<KeyboardBinding> GetKeyboardBindings(string vehicleName)
        {
            return GetVehicleBindingStore(vehicleName).GetKeyboardBindings().GetAllBindings();
        }

        public List<JoystickBinding> GetJoystickBindings(string vehicleName)
        {
            return GetVehicleBindingStore(vehicleName).GetAllJoystickBindings();
        }

        private VehicleBindingStore GetVehicleBindingStore(string vehicleName)
        {
            if (_vehicleBindingStores.TryGetValue(vehicleName, out var vehicleBindingStore))
                return vehicleBindingStore;

            vehicleBindingStore                = new VehicleBindingStore();
            _vehicleBindingStores[vehicleName] = vehicleBindingStore;
            return vehicleBindingStore;
        }

        public void CopyBindings(Manager fromManager, Manager toManager)
        {
            var fromStore = GetVehicleBindingStore(fromManager.ShortName);
            var toStore   = GetVehicleBindingStore(toManager.ShortName);
            toStore.ClearAll();

            var fromKeyboardBindingStore = fromStore.GetKeyboardBindings();
            var toKeyboardBindingStore   = toStore.GetKeyboardBindings();

            foreach (var binding in fromKeyboardBindingStore.GetAllBindings())
            {
                if (!toManager.ControlActions.TryGetValue(binding.Target, out var toAction))
                    continue;
                if (!fromManager.ControlActions.TryGetValue(binding.Target, out var fromAction))
                    continue;
                if (toAction.Input != fromAction.Input || toAction.Category != fromAction.Category)
                    continue;

                var newBinding = new KeyboardBinding(binding.Target, binding.Key, binding.ModifierKey);
                toKeyboardBindingStore.Add(binding.Target, newBinding);
            }

            foreach (var binding in fromStore.GetAllJoystickBindings())
            {
                if (!toManager.ControlActions.TryGetValue(binding.Target, out var toAction))
                    continue;
                if (!fromManager.ControlActions.TryGetValue(binding.Target, out var fromAction))
                    continue;
                if (toAction.Input != fromAction.Input || toAction.Category != fromAction.Category)
                    continue;

                var newBinding = new JoystickBinding(binding.Target, binding.JoystickId, binding.RequiresModifier, binding.Invert, binding.Deadzone, binding.Offset,
                                                     binding.POVDirection);
                toStore.GetJoystickBindings(binding.JoystickId).Add(binding.Target, newBinding);
            }
        }
    }
}