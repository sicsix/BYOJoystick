using System;
using System.Linq;
using BYOJoystick.Bindings;
using BYOJoystick.Controls.Converters;
using BYOJoystick.Controls.Sync;
using UnityEngine;
using VTOLVR.Multiplayer;

namespace BYOJoystick.Controls
{
    public class CThrottle : IControl
    {
        protected readonly bool IsMP;
        protected readonly bool IsMulticrew;

        protected readonly VRInteractable          Interactable;
        protected readonly VRThrottle              Throttle;
        protected readonly InteractableSyncWrapper SyncWrapper;
        protected readonly ThrottleGrabHandler     GrabHandler;

        protected          float                  ThrottleValue;
        protected          float                  PreviousThrottleValue;
        protected          Vector3                ThumbstickVector;
        protected          Vector3                DigitalThumbstickVector;
        protected          bool                   ThumbstickWasZero;
        protected          bool                   TriggerPressed;
        protected          bool                   ThumbstickPressed;
        protected          bool                   MenuButtonPressed;
        protected readonly Func<VRThrottle, bool> GetRemoteOnly;

        protected readonly DigitalToAxisSmoothed ThrottleSmoothed = new DigitalToAxisSmoothed(0.25f, 1f, 2f);

        public CThrottle(VRInteractable interactable, VRThrottle throttle, bool isMulticrew)
        {
            IsMP         = VTOLMPUtils.IsMultiplayer();
            IsMulticrew  = isMulticrew;
            Interactable = interactable;
            Throttle     = throttle;

            if (!IsMP || !IsMulticrew)
                return;
            GetRemoteOnly = CompiledExpressions.CreateFieldGetter<VRThrottle, bool>("remoteOnly");
            var throttleInteractable = throttle.GetComponent<VRInteractable>();
            SyncWrapper = InteractableSyncWrapper.Create(Interactable);
            var connectedThrottles = VTOLAPI.GetPlayersVehicleGameObject().GetComponentsInChildren<ConnectedThrottles>().FirstOrDefault(o => o.throttles.Contains(throttle));
            var muvs               = VTOLAPI.GetPlayersVehicleGameObject().GetComponent<MultiUserVehicleSync>();
            GrabHandler = ThrottleGrabHandler.Create(Throttle, connectedThrottles, muvs, throttleInteractable);
        }

        public virtual void PostUpdate()
        {
            if (ThrottleSmoothed.Calculate())
                SetThrottleValue(Throttle.currentThrottle + ThrottleSmoothed.Delta);
            else
                SetThrottleValue(ThrottleValue);

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

        protected void SetThrottleValue(float value)
        {
            if (!IsMP || !IsMulticrew)
            {
                Throttle.RemoteSetThrottleForceEvents(Mathf.Clamp01(value));
                PreviousThrottleValue = ThrottleValue;
                ThrottleValue         = value;
            }
            else
            {
                float delta = Mathf.Abs(value - PreviousThrottleValue);
                if (delta < 0.01f)
                    return;

                if ((delta > 0.075f && !GrabHandler.IsGrabbed) || (delta > 0.01f && GrabHandler.IsGrabbed))
                    GrabHandler.GrabThrottle(0.5f);

                if ((SyncWrapper == null || SyncWrapper.TryInteractTimed(false, 0.5f)) && !GetRemoteOnly(Throttle))
                {
                    PreviousThrottleValue = ThrottleValue;
                    Throttle.RemoteSetThrottle(Mathf.Clamp01(value));
                    ThrottleValue = value;
                }
            }
        }

        public static void Set(CThrottle c, Binding binding, int state)
        {
            c.ThrottleValue = binding.GetAsFloat();
        }

        public static void Increase(CThrottle c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.ThrottleSmoothed.IncreaseButtonDown();
            else
                c.ThrottleSmoothed.IncreaseButtonUp();
        }

        public static void Decrease(CThrottle c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.ThrottleSmoothed.DecreaseButtonDown();
            else
                c.ThrottleSmoothed.DecreaseButtonUp();
        }

        public static void MenuButton(CThrottle c, Binding binding, int state)
        {
            if (!c.MenuButtonPressed && binding.GetAsBool())
            {
                c.MenuButtonPressed = true;
                c.Throttle.OnMenuButtonDown?.Invoke();
            }

            if (c.MenuButtonPressed && !binding.GetAsBool())
            {
                c.MenuButtonPressed = false;
                c.Throttle.OnMenuButtonUp?.Invoke();
            }
        }

        public static void Trigger(CThrottle c, Binding binding, int state)
        {
            if (!c.TriggerPressed && binding.GetAsBool())
            {
                c.TriggerPressed = true;
                c.Throttle.OnTriggerDown?.Invoke();
            }

            if (c.TriggerPressed && !binding.GetAsBool())
            {
                c.TriggerPressed = false;
                c.Throttle.OnTriggerUp?.Invoke();
            }

            c.Throttle.OnTriggerAxis?.Invoke(binding.GetAsBool() ? 1f : 0f);
        }

        public static void ThumbstickButton(CThrottle c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.Throttle.OnStickPressed?.Invoke();

            if (!c.ThumbstickPressed && binding.GetAsBool())
            {
                c.ThumbstickPressed = true;
                c.Throttle.OnStickPressDown?.Invoke();
            }

            if (c.ThumbstickPressed && !binding.GetAsBool())
            {
                c.ThumbstickPressed = false;
                c.Throttle.OnStickPressUp?.Invoke();
            }
        }

        public static void SetThumbstickX(CThrottle c, Binding binding, int state)
        {
            c.ThumbstickVector.y = binding.GetAsFloatCentered();
        }

        public static void SetThumbstickY(CThrottle c, Binding binding, int state)
        {
            c.ThumbstickVector.y = binding.GetAsFloatCentered();
        }

        public static void ThumbstickUp(CThrottle c, Binding binding, int state)
        {
            c.DigitalThumbstickVector.y = binding.GetAsBool() ? 1f : 0f;
        }

        public static void ThumbstickDown(CThrottle c, Binding binding, int state)
        {
            c.DigitalThumbstickVector.y = binding.GetAsBool() ? -1f : 0f;
        }

        public static void ThumbstickLeft(CThrottle c, Binding binding, int state)
        {
            c.DigitalThumbstickVector.x = binding.GetAsBool() ? -1f : 0f;
        }

        public static void ThumbstickRight(CThrottle c, Binding binding, int state)
        {
            c.DigitalThumbstickVector.x = binding.GetAsBool() ? 1f : 0f;
        }
    }
}