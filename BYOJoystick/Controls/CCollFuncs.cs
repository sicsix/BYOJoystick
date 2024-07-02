using BYOJoystick.Bindings;
using BYOJoystick.Controls.Converters;
using UnityEngine;
using VTOLVR.DLC.Rotorcraft;
using VTOLVR.Multiplayer;

namespace BYOJoystick.Controls
{
    public class CCollFuncs : IControl
    {
        protected readonly AH94CollectiveFunctions CollFuncs;
        protected readonly bool                    IsMP;
        protected          bool                    FiringCM;
        protected          float                   ArticulateValue;
        protected          float                   DigitalArticulateValue;
        protected          float                   TimeSetArtiAuto;

        protected readonly DigitalToAxisSmoothed ArticulateSmoothed = new DigitalToAxisSmoothed(0.25f, 1f, 1f);
        protected readonly DigitalToAxisSmoothed TrimYawSmoothed    = new DigitalToAxisSmoothed(0.25f, 1f, 1f);

        public CCollFuncs(AH94CollectiveFunctions collFuncs)
        {
            CollFuncs = collFuncs;
            IsMP      = VTOLMPUtils.IsMultiplayer();
        }

        public void PostUpdate()
        {
            if (TrimYawSmoothed.Calculate())
                CollFuncs.tailRotor.ShifitTrimYaw(new Vector3(TrimYawSmoothed.Value, 0, 0));

            if (ArticulateSmoothed.Calculate())
                DigitalArticulateValue = ArticulateSmoothed.Value;

            if (Mathf.Abs(ArticulateValue) < 0.03f)
                ArticulateValue = 0;

            if (DigitalArticulateValue != 0)
                UpdateArticulateTilt(DigitalArticulateValue);
            else if (ArticulateValue != 0)
                UpdateArticulateTilt(ArticulateValue);

            DigitalArticulateValue = 0;
        }

        public static void FireCM(CCollFuncs c, Binding binding, int state)
        {
            if (!c.FiringCM && binding.GetAsBool())
            {
                if (c.IsMP && !c.CollFuncs.muvs.netEntity.isMine)
                    c.CollFuncs.muvs.RemoteStartCM();
                else
                    c.CollFuncs.cmm.FireCM();
                c.FiringCM = true;
            }
            else if (c.FiringCM && !binding.GetAsBool())
            {
                if (c.IsMP && !c.CollFuncs.muvs.netEntity.isMine)
                    c.CollFuncs.muvs.RemoteStopCM();
                else
                    c.CollFuncs.cmm.StopFireCM();
                c.FiringCM = false;
            }
        }

        private void UpdateArticulateTilt(float value)
        {
            if (IsMP && !CollFuncs.muvs.IsLocalWeaponController() && Time.time - TimeSetArtiAuto > 0.5f)
                return;
            if (CollFuncs.ahpSync != null && !CollFuncs.ahpSync.isMine)
                CollFuncs.ahpSync.RemoteInput(0f - value);
            else
                CollFuncs.artiHardpoint.Tilt(0f - value, Time.deltaTime);
        }

        public static void ArticAuto(CCollFuncs c, Binding binding, int state)
        {
            if (c.IsMP && !c.CollFuncs.muvs.IsLocalWeaponController())
                return;

            if (c.CollFuncs.ahpSync != null && !c.CollFuncs.ahpSync.isMine)
                c.CollFuncs.ahpSync.RemoteSetAuto();
            else
                c.CollFuncs.artiHardpoint.autoMode = true;

            c.TimeSetArtiAuto = Time.time;
        }

        public static void SetArtic(CCollFuncs c, Binding binding, int state)
        {
            c.ArticulateValue = ((JoystickBinding)binding).GetAsFloatCentered();
        }

        public static void ArticUp(CCollFuncs c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.ArticulateSmoothed.IncreaseButtonDown();
            else
                c.ArticulateSmoothed.IncreaseButtonUp();
        }

        public static void ArticDown(CCollFuncs c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.ArticulateSmoothed.DecreaseButtonDown();
            else
                c.ArticulateSmoothed.DecreaseButtonUp();
        }

        public static void TrimYawLeft(CCollFuncs c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.TrimYawSmoothed.DecreaseButtonDown();
            else
                c.TrimYawSmoothed.DecreaseButtonUp();
        }

        public static void TrimYawRight(CCollFuncs c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.TrimYawSmoothed.IncreaseButtonDown();
            else
                c.TrimYawSmoothed.IncreaseButtonUp();
        }
    }
}