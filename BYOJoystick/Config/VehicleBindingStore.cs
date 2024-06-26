using System;
using System.Collections.Generic;
using System.Linq;
using BYOJoystick.Bindings;

namespace BYOJoystick.Config
{
    public class VehicleBindingStore
    {
        private readonly BindingStore<KeyboardBinding> _keyboardBindings = new BindingStore<KeyboardBinding>();

        private readonly Dictionary<Guid, BindingStore<JoystickBinding>> _joystickBindings = new Dictionary<Guid, BindingStore<JoystickBinding>>();

        public BindingStore<KeyboardBinding> GetKeyboardBindings()
        {
            return _keyboardBindings;
        }

        public BindingStore<JoystickBinding> GetJoystickBindings(Guid joystickId)
        {
            if (_joystickBindings.TryGetValue(joystickId, out var bindingStore))
                return bindingStore;

            bindingStore                  = new BindingStore<JoystickBinding>();
            _joystickBindings[joystickId] = bindingStore;
            return bindingStore;
        }

        public List<JoystickBinding> GetAllJoystickBindings()
        {
            return _joystickBindings.Values.SelectMany(bindingStore => bindingStore.GetAllBindings()).ToList();
        }

        public void ClearAll()
        {
            _keyboardBindings.ClearAll();
            foreach (var bindingStore in _joystickBindings.Values)
            {
                bindingStore.ClearAll();
            }
        }
    }
}