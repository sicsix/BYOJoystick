using BYOJoystick.Bindings;
using BYOJoystick.Controls.Converters;

namespace BYOJoystick.Controls
{
    public class CSpdAdjust : IControl
    {
        protected readonly APKnobSpeedDialAdjust SpeedKnob;

        protected readonly DigitalToAxisSmoothed AxisSmoothed = new DigitalToAxisSmoothed(4f / 150f, 110f / 150f, 3f);

        public CSpdAdjust(APKnobSpeedDialAdjust speedKnob)
        {
            SpeedKnob = speedKnob;
        }

        public void PostUpdate()
        {
            if (AxisSmoothed.Calculate())
                SpeedKnob.OnTwistDelta(-AxisSmoothed.Delta);
        }

        public static void Increase(CSpdAdjust c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.AxisSmoothed.IncreaseButtonDown();
            else
                c.AxisSmoothed.IncreaseButtonUp();
        }

        public static void Decrease(CSpdAdjust c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.AxisSmoothed.DecreaseButtonDown();
            else
                c.AxisSmoothed.DecreaseButtonUp();
        }
    }
}