using BYOJoystick.Bindings;

namespace BYOJoystick.Controls
{
    public class CRadarPower : IControl
    {
        protected readonly MFDRadarUI       MFDRadarUI;
        protected readonly MFDPortalManager MFDPortalManager;

        public CRadarPower(MFDRadarUI mfdRadarUI, MFDPortalManager mfdPortalManager)
        {
            MFDRadarUI       = mfdRadarUI;
            MFDPortalManager = mfdPortalManager;
        }

        public void PostUpdate()
        {
        }

        public static void Toggle(CRadarPower c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                c.MFDPortalManager.PlayInputSound();
                c.MFDRadarUI.ToggleRadarPower();
            }
        }

        public static void On(CRadarPower c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                c.MFDPortalManager.PlayInputSound();
                c.MFDRadarUI.SetRadarPower(1);
            }
        }

        public static void Off(CRadarPower c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                c.MFDPortalManager.PlayInputSound();
                c.MFDRadarUI.SetRadarPower(0);
            }
        }
    }
}