using BYOJoystick.Bindings;
using BYOJoystick.Controls.Converters;
using UnityEngine;

namespace BYOJoystick.Controls
{
    public class CThrottleTilt : CThrottle
    {
        protected readonly TiltController        TiltController;
        protected readonly DigitalToAxisSmoothed TiltSmoothed = new DigitalToAxisSmoothed(0.33f, 1f, 1f);
        protected          float                 TiltTarget;
        protected          bool                  TiltTargetReached = true;

        public CThrottleTilt(VRInteractable interactable, VRThrottle throttle, TiltController tiltController, bool isMulticrew) : base(interactable, throttle, isMulticrew)
        {
            TiltController = tiltController;
        }

        public override void PostUpdate()
        {
            if (ThrottleSmoothed.Calculate())
                SetThrottleValue(Throttle.currentThrottle + ThrottleSmoothed.Delta);
            else if (Mathf.Abs(ThrottleValue              - PreviousThrottleValue) > 0.02f)
                SetThrottleValue(ThrottleValue);

            if (TiltSmoothed.Calculate())
                DigitalThumbstickVector.y = TiltSmoothed.Value;

            if (!TiltTargetReached)
            {
                float currentTilt = Mathf.Clamp01(TiltController.currentTilt / 90f);
                float diff        = Mathf.Abs(TiltTarget - currentTilt);
                if (diff < 0.001f)
                    TiltTargetReached = true;
                else
                    DigitalThumbstickVector.y = Mathf.Lerp(0.20f, 1f, Mathf.Clamp01(diff / 0.05f)) * Mathf.Sign(TiltTarget - currentTilt);
            }

            if (DigitalThumbstickVector != Vector3.zero)
            {
                ThumbstickWasZero = false;
                Throttle.OnSetThumbstick?.Invoke(DigitalThumbstickVector);
            }
            else if (ThumbstickVector != Vector3.zero)
            {
                ThumbstickWasZero = false;
                Throttle.OnSetThumbstick?.Invoke(ThumbstickVector);
            }
            else if (!ThumbstickWasZero)
            {
                ThumbstickWasZero = true;
                Throttle.OnSetThumbstick?.Invoke(Vector3.zero);
                Throttle.OnResetThumbstick?.Invoke();
            }

            DigitalThumbstickVector = Vector3.zero;
        }

        public static void SetTiltTarget(CThrottleTilt c, Binding binding, int state)
        {
            c.TiltTarget        = binding.GetAsFloat();
            c.TiltTargetReached = false;
        }

        public static void TiltUp(CThrottleTilt c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.TiltSmoothed.DecreaseButtonDown();
            else
                c.TiltSmoothed.DecreaseButtonUp();
        }

        public static void TiltDown(CThrottleTilt c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.TiltSmoothed.IncreaseButtonDown();
            else
                c.TiltSmoothed.IncreaseButtonUp();
        }
    }
}