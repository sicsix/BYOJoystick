using System;
using UnityEngine;

namespace BYOJoystick.Bindings
{
    [Serializable]
    public class KeyboardBinding : Binding
    {
        public KeyCode Key         { get; set; }
        public KeyCode ModifierKey { get; set; }

        private bool _active;

        public KeyboardBinding()
        {
        }

        public KeyboardBinding(string target, KeyCode key, KeyCode modifierKey = KeyCode.None)
        {
            Target      = target;
            Key         = key;
            ModifierKey = modifierKey;
        }

        public void UpdateState(bool isAnyModifierKeyPressed)
        {
            bool nowActive = Input.GetKey(Key) && ((ModifierKey != KeyCode.None && Input.GetKey(ModifierKey)) || (ModifierKey == KeyCode.None && !isAnyModifierKeyPressed));
            if (nowActive == _active)
                return;
            _active = nowActive;
            Action.Invoke(this);
        }

        public override bool GetAsBool()
        {
            return _active;
        }

        public override string GetDisplayString()
        {
            string modifierPrefix = ModifierKey switch
            {
                KeyCode.LeftShift    => "LShift+",
                KeyCode.RightShift   => "RShift+",
                KeyCode.LeftControl  => "LCtrl+",
                KeyCode.RightControl => "RCtrl+",
                KeyCode.LeftAlt      => "LAlt+",
                KeyCode.RightAlt     => "RAlt+",
                _                    => string.Empty
            };

            return $"{modifierPrefix}{Key}";
        }
    }
}