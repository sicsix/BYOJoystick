using BYOJoystick.Bindings;
using BYOJoystick.Controls.Converters;
using UnityEngine;

namespace BYOJoystick.Controls
{
    public class CSweep : IControl
    {
        protected readonly AeroGeometryLever Lever;

        protected float LeverValue;
        protected float PreviousLeverValue;

        protected readonly DigitalToAxisSmoothed LeverSmoothed = new DigitalToAxisSmoothed(0.5f, 1f, 1f);


        public CSweep(AeroGeometryLever lever)
        {
            Lever = lever;
        }

        public void PostUpdate()
        {
            if (LeverSmoothed.Calculate())
                SetLeverValue(1 - Lever.CurrentOutput + LeverSmoothed.Delta);
            else if (LeverValue != PreviousLeverValue)
                SetLeverValue(LeverValue);
        }

        private void SetLeverValue(float value)
        {
            LeverValue         = Mathf.Clamp01(value);
            PreviousLeverValue = LeverValue;
            Lever.RemoteSetManual(1f - LeverValue);
        }

        public static void Set(CSweep c, Binding binding, int state)
        {
            c.LeverValue = binding.GetAsFloat();
        }

        public static void Forward(CSweep c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.LeverSmoothed.IncreaseButtonDown();
            else
                c.LeverSmoothed.IncreaseButtonUp();
        }

        public static void Backward(CSweep c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.LeverSmoothed.DecreaseButtonDown();
            else
                c.LeverSmoothed.DecreaseButtonUp();
        }

        public static void ToggleAuto(CSweep c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                if (c.Lever.CurrentAuto)
                    c.Lever.RemoteSetManual(0f);
                else
                    c.Lever.RemoteSetAuto();
            }
        }

        public static void AutoOn(CSweep c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.Lever.RemoteSetAuto();
        }

        public static void AutoOff(CSweep c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.Lever.RemoteSetManual(0f);
        }
    }
}