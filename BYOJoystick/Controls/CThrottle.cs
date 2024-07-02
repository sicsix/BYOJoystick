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

        protected          float                     ThrottleValue;
        protected          float                     PreviousThrottleValue;
        protected          float                     ThrottleDeadzone;
        protected          Vector3                   ThumbstickVector;
        protected          Vector3                   DigitalThumbstickVector;
        protected          bool                      ThumbstickWasZero;
        protected          bool                      TriggerPressed;
        protected          bool                      ThumbstickPressed;
        protected          bool                      MenuButtonPressed;
        protected readonly Func<VRThrottle, bool>    GetRemoteOnly;
        protected readonly Func<VRThrottle, float>   GetGateAnimThrottle;
        protected readonly Action<VRThrottle, float> SetGateAnimThrottle;
        protected readonly Func<VRThrottle, bool>    GetBelowGate;
        protected readonly Action<VRThrottle, bool>  SetBelowGate;
        protected readonly Func<VRThrottle, float>   GetLastClickT;
        protected readonly Action<VRThrottle, float> SetLastClickT;
        protected readonly Action<VRThrottle, float> SetThrottle;

        protected readonly DigitalToAxisSmoothed ThrottleSmoothed = new DigitalToAxisSmoothed(0.25f, 1f, 2f);

        public CThrottle(VRInteractable interactable, VRThrottle throttle, bool isMulticrew)
        {
            IsMP                = VTOLMPUtils.IsMultiplayer();
            IsMulticrew         = isMulticrew;
            Interactable        = interactable;
            Throttle            = throttle;
            GetGateAnimThrottle = CompiledExpressions.CreateFieldGetter<VRThrottle, float>("gateAnimThrottle");
            SetGateAnimThrottle = CompiledExpressions.CreateFieldSetter<VRThrottle, float>("gateAnimThrottle");
            GetBelowGate        = CompiledExpressions.CreateFieldGetter<VRThrottle, bool>("belowGate");
            SetBelowGate        = CompiledExpressions.CreateFieldSetter<VRThrottle, bool>("belowGate");
            GetLastClickT       = CompiledExpressions.CreateFieldGetter<VRThrottle, float>("lastClickT");
            SetLastClickT       = CompiledExpressions.CreateFieldSetter<VRThrottle, float>("lastClickT");
            SetThrottle         = CompiledExpressions.CreateFieldSetter<VRThrottle, float>("throttle");

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
                SetThrottleValue(ThrottleValue + ThrottleSmoothed.Delta, true);
            else if (ThrottleValue != PreviousThrottleValue)
                SetThrottleValue(ThrottleValue, false);

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

        protected void SetThrottleValue(float throttle, bool isButtonInput)
        {
            ThrottleValue = throttle;
            throttle      = Mathf.Clamp(throttle, Throttle.minThrottle, Mathf.Clamp01(Throttle.throttleLimiter));
            bool sendEvents = Throttle.sendEvents;
            Throttle.sendEvents = true;

            if (!IsMP || !IsMulticrew)
            {
                UpdateThrottle(throttle);
                PreviousThrottleValue = ThrottleValue;
            }
            else
            {
                float delta = Mathf.Abs(ThrottleValue - PreviousThrottleValue);
                if (isButtonInput || (delta > Mathf.Max(0.005f, ThrottleDeadzone) && !GrabHandler.IsGrabbed) || (GrabHandler.IsGrabbed && delta > ThrottleDeadzone / 2f))
                {
                    PreviousThrottleValue = ThrottleValue;
                    GrabHandler.GrabThrottle(1.5f);
                }

                if (SyncWrapper == null || SyncWrapper.TryInteractTimed(true, 0.5f))
                {
                    if (GrabHandler.IsGrabbed && !GetRemoteOnly(Throttle))
                        UpdateThrottle(throttle);
                }
            }

            Throttle.sendEvents = sendEvents;
        }

        private void UpdateThrottle(float throttle)
        {
            if (Throttle.abGate)
            {
                bool throttleUpdated = false;
                if (throttle > Throttle.abGateThreshold && throttle < 1f - Throttle.abPostGateWidth)
                {
                    float gateAnimThrottle = Mathf.Lerp(GetGateAnimThrottle(Throttle), Mathf.Lerp(Throttle.abGateThreshold, throttle, 0.25f), 25f * Time.deltaTime);
                    SetGateAnimThrottle(Throttle, gateAnimThrottle);
                    throttle = Throttle.abGateThreshold;
                    Throttle.UpdateThrottle(throttle);
                    Throttle.UpdateThrottleAnim(gateAnimThrottle);
                    SetThrottle(Throttle, throttle);
                    throttleUpdated = true;
                }
                else if (throttle >= 1f - Throttle.abPostGateWidth)
                {
                    throttle = 1f;
                    float gateAnimThrottle = Mathf.Lerp(GetGateAnimThrottle(Throttle), 1f, 25f * Time.deltaTime);
                    SetGateAnimThrottle(Throttle, gateAnimThrottle);
                    Throttle.UpdateThrottleAnim(gateAnimThrottle);
                    Throttle.UpdateThrottle(throttle);
                    SetThrottle(Throttle, throttle);
                    throttleUpdated = true;
                }
                else
                    SetGateAnimThrottle(Throttle, throttle);

                if (throttle > Throttle.abGateThreshold + 0.01f)
                {
                    if (GetBelowGate(Throttle))
                    {
                        SetBelowGate(Throttle, false);
                        if (Throttle.gateAudioSource != null)
                            Throttle.gateAudioSource.PlayOneShot(Throttle.gateABSound);
                    }
                }
                else if (!GetBelowGate(Throttle))
                {
                    SetBelowGate(Throttle, true);
                    if (Throttle.gateAudioSource != null)
                        Throttle.gateAudioSource.PlayOneShot(Throttle.gateMilSound);
                }

                if (throttleUpdated)
                    return;
            }

            if (Throttle.audioSource != null && Mathf.Abs(throttle - GetLastClickT(Throttle)) > Throttle.throttleClickInterval)
            {
                SetLastClickT(Throttle, throttle);
                Throttle.audioSource.volume = throttle;
                Throttle.audioSource.PlayOneShot(Throttle.throttleClickSound);
            }

            Throttle.UpdateThrottleAnim(throttle);
            Throttle.UpdateThrottle(throttle);
            SetThrottle(Throttle, throttle);
        }

        public static void Set(CThrottle c, Binding binding, int state)
        {
            c.ThrottleValue    = ((JoystickBinding)binding).GetAsFloat();
            c.ThrottleDeadzone = ((JoystickBinding)binding).Deadzone;
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
            c.ThumbstickVector.y = ((JoystickBinding)binding).GetAsFloatCentered();
        }

        public static void SetThumbstickY(CThrottle c, Binding binding, int state)
        {
            c.ThumbstickVector.y = ((JoystickBinding)binding).GetAsFloatCentered();
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