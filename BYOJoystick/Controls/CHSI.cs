using BYOJoystick.Bindings;
using BYOJoystick.Controls.Converters;

namespace BYOJoystick.Controls
{
    public class CHSI : IControl
    {
        protected readonly DashHSI DashHsi;

        protected readonly DigitalToAxisSmoothed CourseSmoothed  = new DigitalToAxisSmoothed(2f / 80f, 60f / 80f, 3f);
        protected readonly DigitalToAxisSmoothed HeadingSmoothed = new DigitalToAxisSmoothed(2f / 80f, 60f / 80f, 3f);

        public CHSI(DashHSI dashHsi)
        {
            DashHsi = dashHsi;
        }

        public void PostUpdate()
        {
            if (CourseSmoothed.Calculate())
                DashHsi.OnTwistCrsKnob(-CourseSmoothed.Delta);

            if (HeadingSmoothed.Calculate())
                DashHsi.OnTwistHdgKnob(-HeadingSmoothed.Delta);
        }

        public static void HeadingRight(CHSI c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.HeadingSmoothed.IncreaseButtonDown();
            else
                c.HeadingSmoothed.IncreaseButtonUp();
        }

        public static void HeadingLeft(CHSI c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.HeadingSmoothed.DecreaseButtonDown();
            else
                c.HeadingSmoothed.DecreaseButtonUp();
        }

        public static void CourseRight(CHSI c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.CourseSmoothed.IncreaseButtonDown();
            else
                c.CourseSmoothed.IncreaseButtonUp();
        }

        public static void CourseLeft(CHSI c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.CourseSmoothed.DecreaseButtonDown();
            else
                c.CourseSmoothed.DecreaseButtonUp();
        }
    }
}