using System;
using System.Collections.Generic;
using BYOJoystick.Actions;
using BYOJoystick.Bindings;
using BYOJoystick.UI.Scripts;
using SharpDX.DirectInput;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BYOJoystick.UI
{
    public class BYOJBindingModal : MonoBehaviour
    {
        public TextMeshProUGUI Header;
        public TextMeshProUGUI DeviceName;
        public TextMeshProUGUI ActionName;

        public GameObject      AxisBindingPanel;
        public TextMeshProUGUI BoundAxisName;
        public Button          DetectAxisButton;
        public Button          ModifierAxisButton;
        public Button          InvertAxisButton;
        public Button          ClearAxisButton;
        public Button          IncreaseDeadzoneButton;
        public Button          DecreaseDeadzoneButton;
        public TextMeshProUGUI DeadzoneText;

        public GameObject    ButtonBindingPanel;
        public GameObject    ButtonBindingList;
        public GameObject    DividerPrefab;
        public ButtonBinding ButtonBindingPrefab;
        public Button        AddNewButtonButton;
        public Button        ClearAllButtonsButton;

        public Button CancelButton;
        public Button SaveButton;

        public GameObject      BYOJInputEntryModal;
        public TextMeshProUGUI BYOJInputEntryModalInstructions;

        public bool IsBinding;

        private ControlAction        _action;
        private List<Binding>        _bindings = new List<Binding>();
        private bool                 _isKeyboard;
        private BYOJJoystick         _joystick;
        private List<JoystickUpdate> _updates = new List<JoystickUpdate>();
        private bool                 _polledOnce;
        private float                _deadzone;

        private readonly Dictionary<JoystickOffset, int> _initialAxisValues = new Dictionary<JoystickOffset, int>();

        private const int AxisSensitivity = JoystickBinding.AxisMax / 4;

        public void ShowBindingModal(BindingCell                                                       cell,
                                     ControlAction                                                     action,
                                     IReadOnlyList<Binding>                                            bindings,
                                     bool                                                              isKeyboard,
                                     BYOJJoystick                                                      joystick,
                                     Action<BindingCell, ControlAction, bool, Joystick, List<Binding>> handleSave,
                                     Action                                                            handleCancel)
        {
            _action     = action;
            _bindings   = new List<Binding>(bindings);
            _isKeyboard = isKeyboard;
            _joystick   = joystick;

            Header.text     = $"Binding {action.Input}";
            DeviceName.text = isKeyboard ? "Keyboard" : $"{joystick.Information.ProductName}";
            ActionName.text = action.Name;

            SaveButton.onClick.RemoveAllListeners();
            SaveButton.onClick.AddListener(() =>
            {
                IsBinding = false;
                if (!isKeyboard && _bindings.Count != 0)
                {
                    if (_bindings[0] is JoystickBinding binding)
                        binding.Deadzone = _deadzone;
                }

                handleSave(cell, _action, _isKeyboard, _joystick, _bindings);
            });

            CancelButton.onClick.RemoveAllListeners();
            CancelButton.onClick.AddListener(() =>
            {
                IsBinding = false;
                handleCancel();
            });

            if (isKeyboard)
                CopyKeyboardBindings(bindings as IReadOnlyList<KeyboardBinding>);
            else
                CopyJoystickBindings(bindings as IReadOnlyList<JoystickBinding>);

            if (action.Input == ActionInput.Axis || action.Input == ActionInput.AxisCentered)
                OpenInputModalForAxis();
            else
                OpenInputModalForButton();
        }

        private void CopyKeyboardBindings(IReadOnlyList<KeyboardBinding> bindings)
        {
            _bindings.Clear();
            foreach (var binding in bindings)
            {
                _bindings.Add(new KeyboardBinding(binding.Target, binding.Key, binding.ModifierKey));
            }
        }

        private void CopyJoystickBindings(IReadOnlyList<JoystickBinding> bindings)
        {
            _bindings.Clear();
            foreach (var binding in bindings)
            {
                _bindings.Add(new JoystickBinding(binding.Target, binding.JoystickId, binding.RequiresModifier, binding.Invert, binding.Deadzone, binding.Offset,
                                                  binding.POVDirection));
            }
        }

        private void Update()
        {
            if (!IsBinding)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                    CancelButton.onClick.Invoke();
                return;
            }

            if (_isKeyboard)
                PollKeyboard();
            else
                PollJoyStick();
        }

        private void OpenInputModalForAxis()
        {
            AxisBindingPanel.SetActive(true);
            ButtonBindingPanel.SetActive(false);
            BoundAxisName.text = _bindings.Count == 0 ? "--" : _bindings[0].GetDisplayString();
            _deadzone          = _bindings.Count == 0 ? 0 : (_bindings[0] as JoystickBinding)!.Deadzone;
            DeadzoneText.text  = $"{_deadzone * 100f:0}%";

            IncreaseDeadzoneButton.onClick.RemoveAllListeners();
            IncreaseDeadzoneButton.onClick.AddListener(() =>
            {
                _deadzone         += 0.01f;
                _deadzone         =  Mathf.Clamp(_deadzone, 0, 0.25f);
                DeadzoneText.text =  $"{_deadzone * 100f:0}%";
            });

            DecreaseDeadzoneButton.onClick.RemoveAllListeners();
            DecreaseDeadzoneButton.onClick.AddListener(() =>
            {
                _deadzone         -= 0.01f;
                _deadzone         =  Mathf.Clamp(_deadzone, 0, 0.25f);
                DeadzoneText.text =  $"{_deadzone * 100f:0}%";
            });


            DetectAxisButton.onClick.RemoveAllListeners();
            DetectAxisButton.onClick.AddListener(() =>
            {
                IsBinding   = true;
                _polledOnce = false;
                _initialAxisValues.Clear();
                BYOJInputEntryModalInstructions.text = "Move the axis...";
                BYOJInputEntryModal.SetActive(true);
            });

            ModifierAxisButton.onClick.RemoveAllListeners();
            ModifierAxisButton.onClick.AddListener(() =>
            {
                var binding = _bindings[0] as JoystickBinding;
                binding!.RequiresModifier = !binding!.RequiresModifier;
                BoundAxisName.text        = _bindings[0].GetDisplayString();
            });

            InvertAxisButton.onClick.RemoveAllListeners();
            InvertAxisButton.onClick.AddListener(() =>
            {
                if (_bindings.Count == 0)
                    return;
                var binding = _bindings[0] as JoystickBinding;
                binding!.Invert    = !binding.Invert;
                BoundAxisName.text = _bindings[0].GetDisplayString();
            });

            ClearAxisButton.onClick.RemoveAllListeners();
            ClearAxisButton.onClick.AddListener(() =>
            {
                _bindings.Clear();
                BoundAxisName.text = "--";
            });
        }

        private void OpenInputModalForButton()
        {
            AxisBindingPanel.SetActive(false);
            ButtonBindingPanel.SetActive(true);

            PopulateButtonBindings();

            AddNewButtonButton.onClick.RemoveAllListeners();
            AddNewButtonButton.onClick.AddListener(() =>
            {
                IsBinding                            = true;
                _polledOnce                          = false;
                BYOJInputEntryModalInstructions.text = _isKeyboard ? "Press any key..." : "Press any button...";
                BYOJInputEntryModal.SetActive(true);
            });

            ClearAllButtonsButton.onClick.RemoveAllListeners();
            ClearAllButtonsButton.onClick.AddListener(() =>
            {
                _bindings.Clear();
                PopulateButtonBindings();
            });
        }

        private void PopulateButtonBindings()
        {
            foreach (Transform child in ButtonBindingList.transform)
            {
                Destroy(child.gameObject);
            }

            Instantiate(DividerPrefab, ButtonBindingList.transform);

            for (int i = 0; i < _bindings.Count; i++)
            {
                var binding       = _bindings[i];
                var buttonBinding = Instantiate(ButtonBindingPrefab, ButtonBindingList.transform);
                buttonBinding.Initialise(binding, binding1 =>
                {
                    _bindings.Remove(binding1);
                    PopulateButtonBindings();
                });
                Instantiate(DividerPrefab, ButtonBindingList.transform);
            }

            // Fix height of ButtonBindingPanel and ButtonBindingList
            int dividerCount = _bindings.Count + 1;
            float height = DividerPrefab.GetComponent<RectTransform>().sizeDelta.y * dividerCount + ButtonBindingPrefab.GetComponent<RectTransform>().sizeDelta.y * _bindings.Count;
            const float bindingPanelHeight = 70;
            const float bindingListHeight = 0;
            var buttonBindingPanelSizeDelta = ButtonBindingPanel.GetComponent<RectTransform>().sizeDelta;
            buttonBindingPanelSizeDelta.y                              = bindingPanelHeight + height;
            ButtonBindingPanel.GetComponent<RectTransform>().sizeDelta = buttonBindingPanelSizeDelta;
            var buttonBindingListSizeDelta = ButtonBindingList.GetComponent<RectTransform>().sizeDelta;
            buttonBindingListSizeDelta.y                              = bindingListHeight + height;
            ButtonBindingList.GetComponent<RectTransform>().sizeDelta = buttonBindingListSizeDelta;
        }

        private void PollJoyStick()
        {
            if (Input.GetKeyDown(KeyCode.Escape)
             || Input.GetKeyDown(KeyCode.F10)
             || Input.GetMouseButtonDown(1)
             || !BYOJ.DirectInput.IsDeviceAttached(_joystick.Information.InstanceGuid))
            {
                IsBinding = false;
                BYOJInputEntryModal.SetActive(false);
                return;
            }

            JoystickBinding binding = null;
            _joystick.Poll();
            _joystick.GetBufferedData(ref _updates);

            if (!_polledOnce)
            {
                _polledOnce = true;
                return;
            }

            for (int j = 0; j < _updates.Count; j++)
            {
                var update = _updates[j];
                if (_action.Input == ActionInput.Button)
                {
                    if (JoystickBinding.IsButton(update.Offset) && update.Value >= JoystickBinding.ButtonMax)
                        binding = new JoystickBinding(_action.Name, _joystick.Information.InstanceGuid, false, false, 0, update.Offset);
                    else if (JoystickBinding.IsPOV(update.Offset))
                    {
                        var povFacing = (POVFacing)update.Value;
                        if (povFacing == POVFacing.Up || povFacing == POVFacing.Right || povFacing == POVFacing.Down || povFacing == POVFacing.Left)
                            binding = new JoystickBinding(_action.Name, _joystick.Information.InstanceGuid, false, false, 0, update.Offset, povFacing);
                    }
                }
                else
                {
                    if (!JoystickBinding.IsAxis(update.Offset))
                        continue;
                    if (!_initialAxisValues.TryGetValue(update.Offset, out int initialValue))
                    {
                        _initialAxisValues[update.Offset] = update.Value;
                        continue;
                    }

                    if (Math.Abs(update.Value - initialValue) < AxisSensitivity)
                        continue;
                    binding = new JoystickBinding(_action.Name, _joystick.Information.InstanceGuid, false, false, _deadzone, update.Offset);
                }
            }

            if (binding != null)
            {
                IsBinding = false;
                BYOJInputEntryModal.SetActive(false);
                if (_action.Input == ActionInput.Button)
                {
                    _bindings.Add(binding);
                    PopulateButtonBindings();
                }
                else
                {
                    _bindings.Clear();
                    _bindings.Add(binding);
                    BoundAxisName.text = binding.GetDisplayString();
                }
            }
        }

        private void PollKeyboard()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.F10) || Input.GetMouseButtonDown(1))
            {
                IsBinding = false;
                BYOJInputEntryModal.SetActive(false);
                return;
            }

            var modifierKey = KeyCode.None;
            if (Input.GetKey(KeyCode.LeftAlt))
                modifierKey = KeyCode.LeftAlt;
            else if (Input.GetKey(KeyCode.LeftControl))
                modifierKey = KeyCode.LeftControl;
            else if (Input.GetKey(KeyCode.LeftShift))
                modifierKey = KeyCode.LeftShift;
            else if (Input.GetKey(KeyCode.RightAlt))
                modifierKey = KeyCode.RightAlt;
            else if (Input.GetKey(KeyCode.RightControl))
                modifierKey = KeyCode.RightControl;
            else if (Input.GetKey(KeyCode.RightShift))
                modifierKey = KeyCode.RightShift;

            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (key == KeyCode.None
                 || key == KeyCode.F9
                 || key == KeyCode.Mouse0
                 || key == KeyCode.Mouse1
                 || key == KeyCode.Mouse2
                 || (key >= KeyCode.RightShift && key <= KeyCode.AltGr))
                    continue;

                if (key == KeyCode.JoystickButton0)
                    break;

                if (!Input.GetKeyDown(key))
                    continue;

                IsBinding = false;
                _bindings.Add(new KeyboardBinding(_action.Name, key, modifierKey));
                PopulateButtonBindings();
                BYOJInputEntryModal.SetActive(false);
                return;
            }
        }
    }
}