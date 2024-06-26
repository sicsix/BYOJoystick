using BYOJoystick.Bindings;

namespace BYOJoystick.Controls
{
    public class CVehicleMaster : IControl
    {
        protected readonly VehicleMaster    VehicleMaster;
        protected readonly MFDPortalManager MFDPortalManager;

        public CVehicleMaster(VehicleMaster vehicleMaster, MFDPortalManager mfdPortalManager)
        {
            VehicleMaster    = vehicleMaster;
            MFDPortalManager = mfdPortalManager;
        }

        public void PostUpdate()
        {
        }

        public static void ToggleAltMode(CVehicleMaster c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                if (c.MFDPortalManager != null)
                    c.MFDPortalManager.PlayInputSound();
                c.VehicleMaster.ToggleRadarAltMode();
            }
        }
    }
}