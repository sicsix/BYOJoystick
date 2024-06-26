using BYOJoystick.Bindings;

namespace BYOJoystick.Controls
{
    public class CPresetButton : IControl
    {
        protected readonly MFDPortalPresetButton PortalPresetButton;
        protected readonly MFDPortalManager      MFDPortalManager;

        public CPresetButton(MFDPortalPresetButton portalPresetButton, MFDPortalManager mfdPortalManager)
        {
            MFDPortalManager   = mfdPortalManager;
            PortalPresetButton = portalPresetButton;
        }

        public void PostUpdate()
        {
        }

        public static void Save(CPresetButton c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.PortalPresetButton.SavePreset();
        }

        public static void Load(CPresetButton c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                c.MFDPortalManager.PlayInputSound();
                c.PortalPresetButton.LoadPreset();
            }
        }
    }
}