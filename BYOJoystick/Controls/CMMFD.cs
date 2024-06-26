using System;
using System.Linq;
using BYOJoystick.Bindings;

namespace BYOJoystick.Controls
{
    public class CMMFD : IControl
    {
        protected readonly MFD     MMFD;
        protected readonly CButton PowerButton;

        public CMMFD(MFD mmfd)
        {
            MMFD = mmfd;
            var powerInteractable = mmfd.transform.gameObject.GetComponentsInChildren<VRInteractable>().FirstOrDefault(o => o.GetControlReferenceName().Contains("Power"));
            if (powerInteractable == null)
                throw new InvalidOperationException("Power interactable not found.");
            var powerButton = powerInteractable.GetComponent<VRButton>();
            if (powerButton == null)
                throw new InvalidOperationException("Power button not found.");
            PowerButton = new CButton(powerInteractable, powerButton);
        }

        public void PostUpdate()
        {
        }

        public static void PowerToggle(CMMFD c, Binding binding, int state)
        {
            CButton.Use(c.PowerButton, binding, -1);
        }

        public static void PowerOn(CMMFD c, Binding binding, int state)
        {
            if (!c.MMFD.powerOn && binding.GetAsBool())
                c.MMFD.TurnOn();
        }

        public static void PowerOff(CMMFD c, Binding binding, int state)
        {
            if (c.MMFD.powerOn && binding.GetAsBool())
                c.MMFD.TurnOff();
        }
    }
}