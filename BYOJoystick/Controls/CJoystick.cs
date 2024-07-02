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
        public static readonly List<CJoystick> Instances = new List<CJoystick>();

        protected readonly bool IsMP;
        protected readonly bool IsMulticrew;
        protected readonly bool HasJoystickAxes;

        protected readonly VRJoystick              SideJoystick;
        protected readonly InteractableSyncWrapper SideJoystickSyncWrapper;
        protected readonly JoystickGrabHandler     SideJoystickGrabHandler;

        protected readonly bool                    HasCenterJoystick;
        protected readonly VRJoystick              CenterJoystick;
        protected readonly InteractableSyncWrapper CenterJoystickSyncWrapper;
        protected readonly JoystickGrabHandler     CenterJoystickGrabHandler;

        protected          Vector3                JoystickVector;
        protected          Vector3                PreviousJoystickVector;
        protected          float                  PitchDeadzone;
        protected          float                  RollDeadzone;
        protected          float                  YawDeadzone;
        protected          Vector3                ThumbstickVector;
        protected          Vector3                DigitalThumbstickVector;
        protected          bool                   ThumbstickWasZero;
        protected          bool                   TriggerPressed;
        protected          bool                   ThumbstickPressed;
        protected          bool                   MenuButtonPressed;
        protected readonly Func<VRJoystick, bool> GetRemoteOnly;


        protected readonly DigitalToAxisSmoothed ThumbstickXSmoothed = new DigitalToAxisSmoothed(0.25f, 1f, 1f);
        protected readonly DigitalToAxisSmoothed ThumbstickYSmoothed = new DigitalToAxisSmoothed(0.25f, 1f, 1f);

        public CJoystick(VRJoystick sideJoystick, VRJoystick centerJoystick, bool isMulticrew, bool hasJoystickAxes)
        {
            Instances.Add(this);
            IsMP                                  = VTOLMPUtils.IsMultiplayer();
            IsMulticrew                           = isMulticrew;
            SideJoystick                          = sideJoystick;
            CenterJoystick                        = centerJoystick;
            HasCenterJoystick                     = centerJoystick != null;
            HasJoystickAxes                       = hasJoystickAxes;
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

            if (HasCenterJoystick)
            {
                var centerJoystickInteractable = centerJoystick.GetComponent<VRInteractable>();
                CenterJoystickSyncWrapper = InteractableSyncWrapper.Create(centerJoystickInteractable);
                CenterJoystickGrabHandler = JoystickGrabHandler.Create(centerJoystick, connectedJoysticks, muvs, centerJoystickInteractable);
            }
        }

        [HarmonyPatch(typeof(ConnectedJoystickSync), nameof(ConnectedJoystickSync.RPC_ForceRelease))]
        class Patch
        {
            static void Prefix(int excludeIdx)
            {
                // TODO BUG This should fix the override control on T-55 but never gets called for some reason
                for (int i = 0; i < Instances.Count; i++)
                {
                    var sideJoystickGrabHandler = Instances[i].SideJoystickGrabHandler;
                    var sideJoystickSyncWrapper = Instances[i].SideJoystickSyncWrapper;
                    if (sideJoystickGrabHandler != null)
                    {
                        if (sideJoystickGrabHandler.ControlIndex == excludeIdx)
                            continue;
                        
                        if (sideJoystickGrabHandler.IsGrabbed)
                            sideJoystickGrabHandler.ReleaseStick();
                        sideJoystickSyncWrapper?.StopInteracting(true);
                    }

                    var centerJoystickGrabHandler = Instances[i].CenterJoystickGrabHandler;
                    var centerJoystickSyncWrapper = Instances[i].CenterJoystickSyncWrapper;
                    if (centerJoystickGrabHandler != null)
                    {
                        if (centerJoystickGrabHandler.ControlIndex == excludeIdx)
                            continue;
                        
                        if (centerJoystickGrabHandler.IsGrabbed)
                            centerJoystickGrabHandler.ReleaseStick();
                        centerJoystickSyncWrapper.StopInteracting(true);
                    }
                }
            }
        }


        public void PostUpdate()
        {
            bool isSideJoystickActive = SideJoystick.gameObject.activeInHierarchy;
            var  joystick             = isSideJoystickActive || !HasCenterJoystick ? SideJoystick : CenterJoystick;

            if (HasJoystickAxes)
            {
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

                    float pMag = Mathf.Abs(JoystickVector.x);
                    float yMag = Mathf.Abs(JoystickVector.y);
                    float rMag = Mathf.Abs(JoystickVector.z);

                    if (!grabHandler.IsGrabbed && (pMag > Mathf.Max(0.005f, PitchDeadzone) || yMag > Mathf.Max(0.005f, YawDeadzone) || rMag > Mathf.Max(0.005f, RollDeadzone))
                     || (grabHandler.IsGrabbed
                      && (pMag > Mathf.Max(0.005f, PitchDeadzone / 2f) || yMag > Mathf.Max(0.005f, YawDeadzone / 2f) || rMag > Mathf.Max(0.005f, RollDeadzone / 2f))))
                    {
                        Plugin.Log($"Grabbing stick X: {JoystickVector.x}, Y: {JoystickVector.y}, Z: {JoystickVector.z}");
                        Plugin.Log($"Deadzones: Pitch: {PitchDeadzone}, Yaw: {YawDeadzone}, Roll: {RollDeadzone}");
                        grabHandler.GrabStick(1.5f);
                    }

                    if (JoystickVector != PreviousJoystickVector)
                    {
                        PreviousJoystickVector = JoystickVector;

                        if (syncWrapper == null || syncWrapper.TryInteractTimed(true, 0.5f))
                        {
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
            c.JoystickVector.x = -((JoystickBinding)binding).GetAsFloatCenteredNoDeadzone();
            c.PitchDeadzone    = ((JoystickBinding)binding).Deadzone;
        }

        public static void SetYaw(CJoystick c, Binding binding, int state)
        {
            c.JoystickVector.y = ((JoystickBinding)binding).GetAsFloatCenteredNoDeadzone();
            c.RollDeadzone     = ((JoystickBinding)binding).Deadzone;
        }

        public static void SetRoll(CJoystick c, Binding binding, int state)
        {
            c.JoystickVector.z = -((JoystickBinding)binding).GetAsFloatCenteredNoDeadzone();
            c.YawDeadzone      = ((JoystickBinding)binding).Deadzone;
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
            c.ThumbstickVector.x = ((JoystickBinding)binding).GetAsFloatCentered();
        }

        public static void SetThumbstickY(CJoystick c, Binding binding, int state)
        {
            c.ThumbstickVector.y = ((JoystickBinding)binding).GetAsFloatCentered();
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