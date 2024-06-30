using System;
using System.Collections.Generic;
using BYOJoystick.Bindings;
using BYOJoystick.Controls.Converters;
using BYOJoystick.Controls.Sync;
using Harmony;
using UnityEngine;
using VTOLVR.Multiplayer;

namespace BYOJoystick.Controls
{
    public class CJoystick : IControl
    {
        public static List<CJoystick> Instances = new List<CJoystick>();

        protected readonly bool IsMP;
        protected readonly bool IsMulticrew;

        protected readonly VRJoystick              SideJoystick;
        protected readonly InteractableSyncWrapper SideJoystickSyncWrapper;
        protected readonly JoystickGrabHandler     SideJoystickGrabHandler;

        protected readonly bool                    HasCenterJoystick;
        protected readonly VRJoystick              CenterJoystick;
        protected readonly InteractableSyncWrapper CenterJoystickSyncWrapper;
        protected readonly JoystickGrabHandler     CenterJoystickGrabHandler;

        protected          Vector3                JoystickVector;
        protected          Vector3                PreviousJoystickVector;
        protected          Vector3                ThumbstickVector;
        protected          Vector3                DigitalThumbstickVector;
        protected          bool                   ThumbstickWasZero;
        protected          bool                   TriggerPressed;
        protected          bool                   ThumbstickPressed;
        protected          bool                   MenuButtonPressed;
        protected readonly Func<VRJoystick, bool> GetRemoteOnly;
        protected          float                  ReleaseTimer;
        private const      float                  ReleaseTime = 1.5f;


        protected readonly DigitalToAxisSmoothed ThumbstickXSmoothed = new DigitalToAxisSmoothed(0.25f, 1f, 1f);
        protected readonly DigitalToAxisSmoothed ThumbstickYSmoothed = new DigitalToAxisSmoothed(0.25f, 1f, 1f);

        public CJoystick(VRJoystick sideJoystick, VRJoystick centerJoystick, bool isMulticrew)
        {
            Instances.Add(this);
            IsMP                                  = VTOLMPUtils.IsMultiplayer();
            IsMulticrew                           = isMulticrew;
            SideJoystick                          = sideJoystick;
            CenterJoystick                        = centerJoystick;
            HasCenterJoystick                     = centerJoystick != null;
            SideJoystick.returnToZeroWhenReleased = false;
            if (HasCenterJoystick)
                CenterJoystick.returnToZeroWhenReleased = false;

            if (!IsMP || !IsMulticrew)
                return;
            GetRemoteOnly = CompiledExpressions.CreateFieldGetter<VRJoystick, bool>("remoteOnly");

            var connectedJoysticks = VTOLAPI.GetPlayersVehicleGameObject().GetComponentInChildren<ConnectedJoysticks>();
            var muvs               = VTOLAPI.GetPlayersVehicleGameObject().GetComponent<MultiUserVehicleSync>();

            var sideJoystickInteractable = sideJoystick.GetComponent<VRInteractable>();
            SideJoystickSyncWrapper = InteractableSyncWrapper.Create(sideJoystickInteractable);
            SideJoystickGrabHandler = JoystickGrabHandler.Create(sideJoystick, connectedJoysticks, muvs, sideJoystickInteractable);

            if (!HasCenterJoystick)
                return;
            var centerJoystickInteractable = centerJoystick.GetComponent<VRInteractable>();
            CenterJoystickSyncWrapper = InteractableSyncWrapper.Create(centerJoystickInteractable);
            CenterJoystickGrabHandler = JoystickGrabHandler.Create(centerJoystick, connectedJoysticks, muvs, centerJoystickInteractable);
        }

        [HarmonyPatch(typeof(ConnectedJoystickSync), nameof(ConnectedJoystickSync.RPC_ForceRelease))]
        class Patch
        {
            static void Prefix(int excludeIdx)
            {
                Plugin.Log("Executing prefix patch for ConnectedJoystickSync.RPC_ForceRelease");
                for (int i = 0; i < Instances.Count; i++)
                {
                    var side = Instances[i].SideJoystickGrabHandler;
                    if (side != null)
                    {
                        if (side.ControlIndex != excludeIdx && side.IsGrabbed)
                            side.ReleaseStick();
                    }

                    var center = Instances[i].CenterJoystickGrabHandler;
                    if (center != null)
                    {
                        if (center.ControlIndex != excludeIdx && center.IsGrabbed)
                            center.ReleaseStick();
                    }
                }
            }
        }


        public void PostUpdate()
        {
            bool isSideJoystickActive = SideJoystick.gameObject.activeInHierarchy;
            var  joystick             = isSideJoystickActive || !HasCenterJoystick ? SideJoystick : CenterJoystick;

            if (!IsMP || !IsMulticrew)
            {
                if (JoystickVector != PreviousJoystickVector)
                {
                    PreviousJoystickVector = JoystickVector;
                    joystick.RemoteSetStick(JoystickVector);
                    joystick.OnSetStick?.Invoke(JoystickVector);
                    joystick.OnSetSteer?.Invoke(JoystickVector.y);
                }
            }
            else
            {
                var syncWrapper = isSideJoystickActive ? SideJoystickSyncWrapper : CenterJoystickSyncWrapper;
                var grabHandler = isSideJoystickActive ? SideJoystickGrabHandler : CenterJoystickGrabHandler;

                float magnitude = Mathf.Max(new Vector2(JoystickVector.x, JoystickVector.z).magnitude, Mathf.Abs(JoystickVector.y));

                if (grabHandler.IsGrabbed)
                {
                    if (magnitude > 0.03f)
                        ReleaseTimer = 0f;
                    else
                    {
                        ReleaseTimer += Time.deltaTime;
                        if (ReleaseTimer > ReleaseTime)
                            grabHandler.ReleaseStick();
                    }
                }
                else
                {
                    if (magnitude > 0.15f)
                    {
                        grabHandler.GrabStick();
                        ReleaseTimer = 0f;
                    }
                }

                if (JoystickVector != PreviousJoystickVector)
                {
                    PreviousJoystickVector = JoystickVector;

                    syncWrapper?.TryInteractTimed(true, 0.5f);

                    if (grabHandler.IsGrabbed && !GetRemoteOnly(joystick))
                    {
                        joystick.RemoteSetStick(JoystickVector);
                        if (joystick.sendEvents)
                        {
                            joystick.OnSetStick?.Invoke(JoystickVector);
                            joystick.OnSetSteer?.Invoke(JoystickVector.y);
                        }
                    }
                }
            }

            if (ThumbstickXSmoothed.Calculate())
                DigitalThumbstickVector.x = ThumbstickXSmoothed.Value;

            if (ThumbstickYSmoothed.Calculate())
                DigitalThumbstickVector.y = ThumbstickYSmoothed.Value;

            if (DigitalThumbstickVector != Vector3.zero)
            {
                ThumbstickWasZero = false;
                SideJoystick.OnSetThumbstick?.Invoke(DigitalThumbstickVector);
            }
            else if (ThumbstickVector != Vector3.zero)
            {
                ThumbstickWasZero = false;
                SideJoystick.OnSetThumbstick?.Invoke(ThumbstickVector);
            }
            else if (!ThumbstickWasZero)
            {
                ThumbstickWasZero = true;
                SideJoystick.OnSetThumbstick?.Invoke(Vector3.zero);
                SideJoystick.OnResetThumbstick?.Invoke();
            }

            DigitalThumbstickVector = Vector3.zero;
        }

        public static void SetPitch(CJoystick c, Binding binding, int state)
        {
            c.JoystickVector.x = -binding.GetAsFloatCentered();
        }

        public static void SetYaw(CJoystick c, Binding binding, int state)
        {
            c.JoystickVector.y = binding.GetAsFloatCentered();
        }

        public static void SetRoll(CJoystick c, Binding binding, int state)
        {
            c.JoystickVector.z = -binding.GetAsFloatCentered();
        }

        public static void MenuButton(CJoystick c, Binding binding, int state)
        {
            if (!c.MenuButtonPressed && binding.GetAsBool())
            {
                c.MenuButtonPressed = true;
                c.SideJoystick.OnMenuButtonDown?.Invoke();
            }

            if (c.MenuButtonPressed && !binding.GetAsBool())
            {
                c.MenuButtonPressed = false;
                c.SideJoystick.OnMenuButtonUp?.Invoke();
            }
        }

        public static void Trigger(CJoystick c, Binding binding, int state)
        {
            if (!c.TriggerPressed && binding.GetAsBool())
            {
                c.TriggerPressed = true;
                c.SideJoystick.OnTriggerDown?.Invoke();
            }

            if (c.TriggerPressed && !binding.GetAsBool())
            {
                c.TriggerPressed = false;
                c.SideJoystick.OnTriggerUp?.Invoke();
            }

            c.SideJoystick.OnTriggerAxis?.Invoke(binding.GetAsBool() ? 1f : 0f);
        }

        public static void ThumbstickButton(CJoystick c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.SideJoystick.OnThumbstickButton?.Invoke();

            if (!c.ThumbstickPressed && binding.GetAsBool())
            {
                c.ThumbstickPressed = true;
                c.SideJoystick.OnThumbstickButtonDown?.Invoke();
            }

            if (c.ThumbstickPressed && !binding.GetAsBool())
            {
                c.ThumbstickPressed = false;
                c.SideJoystick.OnThumbstickButtonUp?.Invoke();
            }
        }

        public static void SetThumbstickX(CJoystick c, Binding binding, int state)
        {
            c.ThumbstickVector.x = binding.GetAsFloatCentered();
        }

        public static void SetThumbstickY(CJoystick c, Binding binding, int state)
        {
            c.ThumbstickVector.y = binding.GetAsFloatCentered();
        }

        public static void ThumbstickUp(CJoystick c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.ThumbstickYSmoothed.IncreaseButtonDown();
            else
                c.ThumbstickYSmoothed.IncreaseButtonUp();
        }

        public static void ThumbstickDown(CJoystick c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.ThumbstickYSmoothed.DecreaseButtonDown();
            else
                c.ThumbstickYSmoothed.DecreaseButtonUp();
        }

        public static void ThumbstickLeft(CJoystick c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.ThumbstickXSmoothed.DecreaseButtonDown();
            else
                c.ThumbstickXSmoothed.DecreaseButtonUp();
        }

        public static void ThumbstickRight(CJoystick c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.ThumbstickXSmoothed.IncreaseButtonDown();
            else
                c.ThumbstickXSmoothed.IncreaseButtonUp();
        }
    }
}