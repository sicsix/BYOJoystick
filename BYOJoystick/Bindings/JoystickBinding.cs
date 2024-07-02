using System;
using System.Xml.Serialization;
using BYOJoystick.Actions;
using SharpDX.DirectInput;

namespace BYOJoystick.Bindings
{
    [Serializable]
    public class JoystickBinding : Binding
    {
        public Guid JoystickId { get; set; }

        public FriendlyOffsetName Input
        {
            get => (FriendlyOffsetName)Offset;
            set => Offset = (JoystickOffset)value;
        }

        public bool      RequiresModifier { get; set; }
        public bool      Invert           { get; set; }
        public float     Deadzone         { get; set; }
        public POVFacing POVDirection     { get; set; }

        [XmlIgnore]
        public JoystickOffset Offset { get; set; }

        public static bool IsButton(JoystickOffset offset)
        {
            return offset >= JoystickOffset.Buttons0 && offset <= JoystickOffset.Buttons127;
        }

        public static bool IsPOV(JoystickOffset offset)
        {
            return offset >= JoystickOffset.PointOfViewControllers0 && offset <= JoystickOffset.PointOfViewControllers3;
        }

        public static bool IsAxis(JoystickOffset offset)
        {
            return !IsButton(offset) && !IsPOV(offset);
        }


        private bool _modifierActive;
        private bool _buttonActive;
        private bool _axisActive;
        private int  _value;

        public const int AxisMax   = 65535;
        public const int ButtonMax = 128;

        public JoystickBinding()
        {
        }

        public JoystickBinding(string         target,
                               Guid           joystickId,
                               bool           requiresModifier,
                               bool           invert,
                               float          deadzone,
                               JoystickOffset offset,
                               POVFacing      povDirection = POVFacing.None)
        {
            Target           = target;
            JoystickId       = joystickId;
            RequiresModifier = requiresModifier;
            Invert           = invert;
            Deadzone         = deadzone;
            Offset           = offset;
            POVDirection     = povDirection;
        }

        public void OnModifierChange(int value, bool inputHasModifierBinding)
        {
            _modifierActive = value >= ButtonMax;
            if (Action.Input == ActionInput.AxisCentered
             && _axisActive
             && ((!_modifierActive && RequiresModifier) || (!RequiresModifier && _modifierActive && inputHasModifierBinding)))
                ResetCenteredAxis();
        }

        public void OnStateChange(int value, bool inputHasModifierBinding)
        {
            _value = value;

            if (Action.Input == ActionInput.Button)
            {
                bool nowActive = IsButtonPressed() && ((_modifierActive && RequiresModifier) || (!RequiresModifier && !_modifierActive));
                if (nowActive == _buttonActive)
                    return;
                _buttonActive = nowActive;
                Action.Invoke(this);
            }
            else
            {
                // inputHasModifierBinding is true when there is another binding on this axis that requires a modifier
                // This is used to ensure that axes that don't require a modifier keep working while a modifier is pressed
                // unless there is another binding on the axis that requires a modifier
                if ((_modifierActive && RequiresModifier) || (!RequiresModifier && (!_modifierActive || !inputHasModifierBinding)))
                {
                    _axisActive = true;
                    Action.Invoke(this);
                }
                else if (_axisActive)
                {
                    _axisActive = false;
                    if (Action.Input == ActionInput.AxisCentered)
                        ResetCenteredAxis();
                }
            }
        }

        public float GetAsFloat()
        {
            if (!IsAxis(Offset))
                return 0f;

            return Invert ? 1f - (float)_value / AxisMax : (float)_value / AxisMax;
        }

        public float GetAsFloatCentered()
        {
            if (!IsAxis(Offset))
                return 0f;

            float value = Invert ? 1f - (float)_value / AxisMax : (float)_value / AxisMax;
            value = (value - 0.5f) * 2f;
            return Math.Abs(value) < Deadzone ? 0f : value;
        }

        public float GetAsFloatCenteredNoDeadzone()
        {
            if (!IsAxis(Offset))
                return 0f;

            float value = Invert ? 1f - (float)_value / AxisMax : (float)_value / AxisMax;
            return (value - 0.5f) * 2f;
        }

        private void ResetCenteredAxis()
        {
            _value = AxisMax / 2;
            Action.Invoke(this);
        }

        private bool IsButtonPressed()
        {
            if (IsAxis(Offset))
                return false;

            if (IsPOV(Offset))
                return GetPovDirectionPressed();

            if (Invert)
                return _value < ButtonMax;
            return _value >= ButtonMax;
        }

        public override bool GetAsBool()
        {
            return _buttonActive;
        }

        public override string GetDisplayString()
        {
            if (IsAxis(Offset))
            {
                string modifierPrefix = RequiresModifier ? "Mod+" : "";
                string invertPrefix   = Invert ? "-" : "";
                return $"{modifierPrefix}{invertPrefix}{(FriendlyOffsetName)Offset} Axis";
            }

            if (IsButton(Offset))
            {
                string modifierPrefix = RequiresModifier ? "Mod+" : "";
                string invertPrefix   = Invert ? "Not " : "";
                return $"{modifierPrefix}{invertPrefix}{(FriendlyOffsetName)Offset}";
            }

            if (IsPOV(Offset))
            {
                string modifierPrefix = RequiresModifier ? "Mod+" : "";
                string invertPrefix   = Invert ? "Not " : "";
                return $"{modifierPrefix}{invertPrefix}{(FriendlyOffsetName)Offset} {POVDirection}";
            }

            return "ERROR";
        }

        private bool GetPovDirectionPressed()
        {
            return POVDirection switch
            {
                POVFacing.Up    => _value == (int)POVFacing.Up    || _value == (int)POVFacing.UpRight   || _value == (int)POVFacing.UpLeft,
                POVFacing.Right => _value == (int)POVFacing.Right || _value == (int)POVFacing.DownRight || _value == (int)POVFacing.UpRight,
                POVFacing.Down  => _value == (int)POVFacing.Down  || _value == (int)POVFacing.DownLeft  || _value == (int)POVFacing.DownRight,
                POVFacing.Left  => _value == (int)POVFacing.Left  || _value == (int)POVFacing.UpLeft    || _value == (int)POVFacing.DownLeft,
                _               => false
            };
        }

        public enum FriendlyOffsetName
        {
            X    = 0,
            Y    = 4,
            Z    = 8,
            RX   = 12,
            RY   = 16,
            RZ   = 20,
            S1   = 24,
            S2   = 28,
            PoV1 = 32,
            PoV2 = 36,
            PoV3 = 40,
            PoV4 = 44,
            B1   = 48,
            B2   = 49,
            B3   = 50,
            B4   = 51,
            B5   = 52,
            B6   = 53,
            B7   = 54,
            B8   = 55,
            B9   = 56,
            B10  = 57,
            B11  = 58,
            B12  = 59,
            B13  = 60,
            B14  = 61,
            B15  = 62,
            B16  = 63,
            B17  = 64,
            B18  = 65,
            B19  = 66,
            B20  = 67,
            B21  = 68,
            B22  = 69,
            B23  = 70,
            B24  = 71,
            B25  = 72,
            B26  = 73,
            B27  = 74,
            B28  = 75,
            B29  = 76,
            B30  = 77,
            B31  = 78,
            B32  = 79,
            B33  = 80,
            B34  = 81,
            B35  = 82,
            B36  = 83,
            B37  = 84,
            B38  = 85,
            B39  = 86,
            B40  = 87,
            B41  = 88,
            B42  = 89,
            B43  = 90,
            B44  = 91,
            B45  = 92,
            B46  = 93,
            B47  = 94,
            B48  = 95,
            B49  = 96,
            B50  = 97,
            B51  = 98,
            B52  = 99,
            B53  = 100,
            B54  = 101,
            B55  = 102,
            B56  = 103,
            B57  = 104,
            B58  = 105,
            B59  = 106,
            B60  = 107,
            B61  = 108,
            B62  = 109,
            B63  = 110,
            B64  = 111,
            B65  = 112,
            B66  = 113,
            B67  = 114,
            B68  = 115,
            B69  = 116,
            B70  = 117,
            B71  = 118,
            B72  = 119,
            B73  = 120,
            B74  = 121,
            B75  = 122,
            B76  = 123,
            B77  = 124,
            B78  = 125,
            B79  = 126,
            B80  = 127,
            B81  = 128,
            B82  = 129,
            B83  = 130,
            B84  = 131,
            B85  = 132,
            B86  = 133,
            B87  = 134,
            B88  = 135,
            B89  = 136,
            B90  = 137,
            B91  = 138,
            B92  = 139,
            B93  = 140,
            B94  = 141,
            B95  = 142,
            B96  = 143,
            B97  = 144,
            B98  = 145,
            B99  = 146,
            B100 = 147,
            B101 = 148,
            B102 = 149,
            B103 = 150,
            B104 = 151,
            B105 = 152,
            B106 = 153,
            B107 = 154,
            B108 = 155,
            B109 = 156,
            B110 = 157,
            B111 = 158,
            B112 = 159,
            B113 = 160,
            B114 = 161,
            B115 = 162,
            B116 = 163,
            B117 = 164,
            B118 = 165,
            B119 = 166,
            B120 = 167,
            B121 = 168,
            B122 = 169,
            B123 = 170,
            B124 = 171,
            B125 = 172,
            B126 = 173,
            B127 = 174,
            B128 = 175,
        }
    }
}