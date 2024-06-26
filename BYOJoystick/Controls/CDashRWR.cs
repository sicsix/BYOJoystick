using BYOJoystick.Bindings;
using UnityEngine;

namespace BYOJoystick.Controls
{
    public class CDashRWR : IControl
    {
        protected readonly DashRWR          DashRWR;
        protected readonly MFDPortalManager MFDPortalManager;

        public CDashRWR(DashRWR dashRWR, MFDPortalManager mfdPortalManager)
        {
            MFDPortalManager = mfdPortalManager;
            DashRWR          = dashRWR;
        }

        public void PostUpdate()
        {
        }

        public static void Cycle(CDashRWR c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                c.MFDPortalManager.PlayInputSound();
                c.DashRWR.SetMasterMode((int)c.DashRWR.mode + 1 % 3);
            }
        }

        public static void Next(CDashRWR c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                c.MFDPortalManager.PlayInputSound();
                c.DashRWR.SetMasterMode(Mathf.Clamp((int)c.DashRWR.mode + 1, 0, 2));
            }
        }

        public static void Prev(CDashRWR c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                c.MFDPortalManager.PlayInputSound();
                c.DashRWR.SetMasterMode(Mathf.Clamp((int)c.DashRWR.mode - 1, 0, 2));
            }
        }

        public static void On(CDashRWR c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                c.MFDPortalManager.PlayInputSound();
                c.DashRWR.SetMasterMode(0);
            }
        }

        public static void Silent(CDashRWR c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                c.MFDPortalManager.PlayInputSound();
                c.DashRWR.SetMasterMode(1);
            }
        }

        public static void Off(CDashRWR c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                c.MFDPortalManager.PlayInputSound();
                c.DashRWR.SetMasterMode(2);
            }
        }
    }
}