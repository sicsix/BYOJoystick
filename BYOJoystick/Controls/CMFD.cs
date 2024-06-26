using System;
using System.Collections.Generic;
using System.Reflection;
using BYOJoystick.Bindings;

namespace BYOJoystick.Controls
{
    public class CMFD : IControl
    {
        protected readonly MFD       MFD;
        protected readonly CButton[] Buttons = new CButton[(int)MFD.MFDButtons.B5 + 1];

        public CMFD(MFD mfd)
        {
            MFD = mfd;
            var buttonCompsField = typeof(MFD).GetField("buttonComps", BindingFlags.NonPublic | BindingFlags.Instance);
            var buttonComps      = (Dictionary<MFD.MFDButtons, MFD.MFDButtonComp>)buttonCompsField.GetValue(mfd);
            foreach (var kvp in buttonComps)
            {
                var button = kvp.Value.interactable.GetComponent<VRButton>();
                if (button == null)
                    throw new InvalidOperationException($"Interactable {kvp.Value.interactable.GetControlReferenceName()} does not have a VRButton component.");

                Buttons[(int)kvp.Key] = new CButton(kvp.Value.interactable, button);
            }
        }

        public void PostUpdate()
        {
        }

        public static void PowerToggle(CMFD c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.MFD.powerKnob.RemoteSetState(c.MFD.powerOn ? 0 : 1);
        }

        public static void PowerOn(CMFD c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.MFD.powerKnob.RemoteSetState(1);
        }

        public static void PowerOff(CMFD c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.MFD.powerKnob.RemoteSetState(0);
        }

        public static void Press(CMFD c, Binding binding, int state)
        {
            CButton.Use(c.Buttons[state], binding, -1);
        }
    }
}