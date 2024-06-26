using System;
using System.Collections.Generic;
using BYOJoystick.Actions;
using BYOJoystick.Bindings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BYOJoystick.UI.Scripts
{
    public class BindingCell : MonoBehaviour
    {
        public TextMeshProUGUI BindingText;
        public Button          BindButton;

        private ControlAction          _action;
        private IReadOnlyList<Binding> _bindings;
        private Guid                   _control;
        private bool                   _isKeyboard;

        public void Initialise(Action<BindingCell, Guid, bool, ControlAction> handleBind,
                               ControlAction                                  action,
                               IReadOnlyList<Binding>                         bindings,
                               Guid                                           control    = default,
                               bool                                           isKeyboard = false)
        {
            BindButton.onClick.AddListener(() =>
            {
                handleBind(this, _control, _isKeyboard, _action);
            });
            _action     = action;
            _bindings   = bindings;
            _control    = control;
            _isKeyboard = isKeyboard;
            UpdateText();
        }

        public void UpdateText()
        {
            if (_bindings.Count == 0)
            {
                BindingText.text = "---";
                return;
            }

            string displayText = string.Empty;
            for (int i = 0; i < _bindings.Count; i++)
            {
                var binding = _bindings[i];
                if (i == 0)
                    displayText += binding.GetDisplayString();
                else
                    displayText += $" or \n{binding.GetDisplayString()}";
            }

            BindingText.text = displayText;
        }
    }
}