using BYOJoystick.Bindings;

namespace BYOJoystick.Controls
{
    public class CUFD : IControl
    {
        protected readonly MultiObjectSwitcher Switcher;
        protected readonly ObjectPowerUnit     PowerUnit;
        protected readonly CButton             PowerButton;
        protected readonly CButton             FuelPageButton;
        protected readonly CButton             APPageButton;
        protected readonly CButton             StatusPageButton;
        protected readonly CButton             RadioPageButton;

        private CButton _nextButtonPressed;
        private CButton _prevButtonPressed;

        public CUFD(MultiObjectSwitcher switcher,
                    ObjectPowerUnit     powerUnit,
                    CButton             powerButton,
                    CButton             fuelPageButton,
                    CButton             apPageButton,
                    CButton             statusPageButton,
                    CButton             radioPageButton)
        {
            Switcher         = switcher;
            PowerUnit        = powerUnit;
            PowerButton      = powerButton;
            FuelPageButton   = fuelPageButton;
            APPageButton     = apPageButton;
            StatusPageButton = statusPageButton;
            RadioPageButton  = radioPageButton;
        }

        public void PostUpdate()
        {
        }

        public static void PowerToggle(CUFD c, Binding binding, int state)
        {
            CButton.Use(c.PowerButton, binding, state);
        }

        public static void PowerOn(CUFD c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                if (!c.PowerUnit.isPowered)
                    CButton.Use(c.PowerButton, binding, state);
            }
            else
                CButton.Use(c.PowerButton, binding, state);
        }

        public static void PowerOff(CUFD c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                if (c.PowerUnit.isPowered)
                    CButton.Use(c.PowerButton, binding, state);
            }
            else
                CButton.Use(c.PowerButton, binding, state);
        }

        public static void Fuel(CUFD c, Binding binding, int state)
        {
            CButton.Use(c.FuelPageButton, binding, state);
        }

        public static void AP(CUFD c, Binding binding, int state)
        {
            CButton.Use(c.APPageButton, binding, state);
        }

        public static void Status(CUFD c, Binding binding, int state)
        {
            CButton.Use(c.StatusPageButton, binding, state);
        }

        public static void Radio(CUFD c, Binding binding, int state)
        {
            CButton.Use(c.RadioPageButton, binding, state);
        }

        public static void Next(CUFD c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                if (c.Switcher.currIdx == 0)
                    c._nextButtonPressed = c.APPageButton;
                else if (c.Switcher.currIdx == 1)
                    c._nextButtonPressed = c.StatusPageButton;
                else if (c.Switcher.currIdx == 2)
                    c._nextButtonPressed = c.RadioPageButton;
                else if (c.Switcher.currIdx == 3)
                    c._nextButtonPressed = c.FuelPageButton;
                CButton.Use(c._nextButtonPressed, binding, state);
            }
            else if (c._nextButtonPressed != null)
            {
                CButton.Use(c._nextButtonPressed, binding, state);
                c._nextButtonPressed = null;
            }
        }

        public static void Previous(CUFD c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                if (c.Switcher.currIdx == 0)
                    c._prevButtonPressed = c.RadioPageButton;
                else if (c.Switcher.currIdx == 1)
                    c._prevButtonPressed = c.FuelPageButton;
                else if (c.Switcher.currIdx == 2)
                    c._prevButtonPressed = c.APPageButton;
                else if (c.Switcher.currIdx == 3)
                    c._prevButtonPressed = c.StatusPageButton;
                CButton.Use(c._prevButtonPressed, binding, state);
            }
            else if (c._prevButtonPressed != null)
            {
                CButton.Use(c._prevButtonPressed, binding, state);
                c._prevButtonPressed = null;
            }
        }
    }
}