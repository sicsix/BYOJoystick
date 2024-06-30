using System;
using BYOJoystick.Bindings;

namespace BYOJoystick.Controls
{
    public class CLeverCovered : IControl
    {
        protected readonly CLever SwitchCover;
        protected readonly CLever Lever;


        public CLeverCovered(VRInteractable interactable, VRSwitchCover switchCover)
        {
            SwitchCover = new CLever(interactable, interactable.GetComponent<VRLever>());
            var lever = switchCover.coveredSwitch.GetComponent<VRLever>();
            if (lever != null)
                Lever = new CLever(switchCover.coveredSwitch, lever);
            else
                throw new InvalidOperationException("SwitchCover must have a covered switch");
        }

        public void PostUpdate()
        {
        }

        public static void Cycle(CLeverCovered c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                c.SwitchCover.SetLeverValue(0);
                CLever.Cycle(c.Lever, binding, state);
                if (c.Lever.CurrentState == 0)
                    c.SwitchCover.SetLeverValue(1);
            }
        }

        public static void Next(CLeverCovered c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                c.SwitchCover.SetLeverValue(0);
                CLever.Next(c.Lever, binding, state);
                if (c.Lever.CurrentState == 0)
                    c.SwitchCover.SetLeverValue(1);
            }
        }

        public static void Prev(CLeverCovered c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                c.SwitchCover.SetLeverValue(0);
                CLever.Prev(c.Lever, binding, state);
                if (c.Lever.CurrentState == 0)
                    c.SwitchCover.SetLeverValue(1);
            }
        }

        public static void Set(CLeverCovered c, Binding binding, int state)
        {
            if (c.Lever.CurrentState == state)
                return;
            c.SwitchCover.SetLeverValue(0);
            CLever.Set(c.Lever, binding, state);
            if (c.Lever.CurrentState == 0)
                c.SwitchCover.SetLeverValue(1);
        }
    }
}