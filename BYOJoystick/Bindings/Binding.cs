using BYOJoystick.Actions;

namespace BYOJoystick.Bindings
{
    public abstract class Binding
    {
        public string Target { get; set; }

        protected ControlAction Action;

        public void SetAction(ControlAction action)
        {
            Action = action;
        }

        public abstract float GetAsFloat();
        public abstract float GetAsFloatCentered();

        public abstract bool GetAsBool();

        public abstract string GetDisplayString();
    }
}