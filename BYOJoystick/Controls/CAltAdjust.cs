using BYOJoystick.Bindings;
using BYOJoystick.Controls.Converters;

namespace BYOJoystick.Controls
{
    public class CAltAdjust : IControl
    {
        protected readonly APKnobAltAdjust AltKnob;

        protected readonly DigitalToAxisSmoothed AxisSmoothed = new DigitalToAxisSmoothed(25f / 1000f, 800f / 1000f, 3f);

        public CAltAdjust(APKnobAltAdjust altKnob)
        {
            AltKnob = altKnob;
        }

        public void PostUpdate()
        {
            if (AxisSmoothed.Calculate())
                AltKnob.OnTwistDelta(-AxisSmoothed.Delta);
        }

        public static void Increase(CAltAdjust c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.AxisSmoothed.IncreaseButtonDown();
            else
                c.AxisSmoothed.IncreaseButtonUp();
        }

        public static void Decrease(CAltAdjust c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.AxisSmoothed.DecreaseButtonDown();
            else
                c.AxisSmoothed.DecreaseButtonUp();
        }
    }
}