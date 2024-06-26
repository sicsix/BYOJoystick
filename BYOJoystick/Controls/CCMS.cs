using BYOJoystick.Bindings;

namespace BYOJoystick.Controls
{
    public class CCMS : IControl
    {
        protected readonly CMSConfigUI      CMSConfigUI;
        protected readonly MFDPortalManager MFDPortalManager;

        public CCMS(CMSConfigUI cmsConfigUI, MFDPortalManager mfdPortalManager)
        {
            CMSConfigUI      = cmsConfigUI;
            MFDPortalManager = mfdPortalManager;
        }

        public void PostUpdate()
        {
        }

        public static void ToggleChaff(CCMS c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                c.MFDPortalManager.PlayInputSound();
                c.CMSConfigUI.ToggleChaff();
            }
        }

        public static void ChaffOn(CCMS c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                c.MFDPortalManager.PlayInputSound();
                c.CMSConfigUI.SetChaff(1);
            }
        }

        public static void ChaffOff(CCMS c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                c.MFDPortalManager.PlayInputSound();
                c.CMSConfigUI.SetChaff(0);
            }
        }

        public static void ToggleFlares(CCMS c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                c.MFDPortalManager.PlayInputSound();
                c.CMSConfigUI.ToggleFlares();
            }
        }

        public static void FlaresOn(CCMS c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                c.MFDPortalManager.PlayInputSound();
                c.CMSConfigUI.SetFlares(1);
            }
        }

        public static void FlaresOff(CCMS c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                c.MFDPortalManager.PlayInputSound();
                c.CMSConfigUI.SetFlares(0);
            }
        }
    }
}