using BYOJoystick.Bindings;

namespace BYOJoystick.Controls
{
    public class CJettison : IControl
    {
        protected readonly JettisonKnobSwitch JettisonKnobSwitch;

        public CJettison(JettisonKnobSwitch jettisonKnobSwitch)
        {
            JettisonKnobSwitch = jettisonKnobSwitch;
        }

        public void PostUpdate()
        {
        }

        public static void Jettison(CJettison c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.JettisonKnobSwitch.JettisonButton();
        }
    }
}