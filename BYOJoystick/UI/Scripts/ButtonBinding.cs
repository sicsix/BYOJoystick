using System;
using BYOJoystick.Bindings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BYOJoystick.UI.Scripts
{
    public class ButtonBinding : MonoBehaviour
    {
        public TextMeshProUGUI ButtonText;
        public Button          ModifierButton;
        public Button          InvertButton;
        public Button          ClearButton;

        public void Initialise(Binding binding, Action<Binding> handleClear)
        {
            ButtonText.text = binding.GetDisplayString();

            if (binding is JoystickBinding)
            {
                ModifierButton.gameObject.SetActive(true);
                InvertButton.gameObject.SetActive(true);
            }
            else
            {
                ModifierButton.gameObject.SetActive(false);
                InvertButton.gameObject.SetActive(false);
            }

            ModifierButton.onClick.AddListener(() =>
            {
                if (!(binding is JoystickBinding joystickBinding))
                    return;
                joystickBinding.RequiresModifier = !joystickBinding.RequiresModifier;
                ButtonText.text                  = joystickBinding.GetDisplayString();
            });

            InvertButton.onClick.AddListener(() =>
            {
                if (!(binding is JoystickBinding joystickBinding))
                    return;
                joystickBinding.Invert = !joystickBinding.Invert;
                ButtonText.text        = joystickBinding.GetDisplayString();
            });

            ClearButton.onClick.AddListener(() => handleClear(binding));
        }
    }
}