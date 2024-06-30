using System;
using BYOJoystick.Bindings;

namespace BYOJoystick.Controls
{
    public class CButtonCovered : IControl
    {
        protected readonly CLever  SwitchCover;
        protected readonly CButton Button;


        public CButtonCovered(VRInteractable interactable, VRSwitchCover switchCover)
        {
            SwitchCover = new CLever(interactable, interactable.GetComponent<VRLever>());
            var button = switchCover.coveredSwitch.GetComponent<VRButton>();
            if (button != null)
                Button = new CButton(switchCover.coveredSwitch, button);
            else
                throw new InvalidOperationException("SwitchCover must have a covered switch");
        }

        public void PostUpdate()
        {
        }

        public static void Use(CButtonCovered c, Binding binding, int state)
        {
            c.SwitchCover.SetLeverValue(binding.GetAsBool() ? 0 : 1);
            CButton.Use(c.Button, binding, state);
        }
    }
}