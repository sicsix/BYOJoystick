using BYOJoystick.Bindings;
using BYOJoystick.Controls.Converters;
using BYOJoystick.Controls.Sync;
using VTOLVR.Multiplayer;

namespace BYOJoystick.Controls
{
    public class CKnob : IControl
    {
        protected readonly VRInteractable          Interactable;
        protected readonly InteractableSyncWrapper SyncWrapper;
        protected readonly bool                    IsMP;
        protected readonly VRTwistKnob             TwistKnob;


        protected readonly DigitalToAxisSmoothed AxisSmoothed = new DigitalToAxisSmoothed(1 / 3f);

        public CKnob(VRInteractable interactable, VRTwistKnob twistKnob)
        {
            Interactable = interactable;
            IsMP         = VTOLMPUtils.IsMultiplayer();
            SyncWrapper  = IsMP ? InteractableSyncWrapper.Create(interactable) : null;
            TwistKnob    = twistKnob;
        }

        public void PostUpdate()
        {
            if (AxisSmoothed.Calculate())
                SetKnobValue(TwistKnob.currentValue + AxisSmoothed.Delta);
        }

        private void SetKnobValue(float value)
        {
            if (IsMP && SyncWrapper != null)
            {
                if (!SyncWrapper.TryInteractTimed(false, 1f))
                    return;
            }

            TwistKnob.SetKnobValue(value);
        }


        public static void Set(CKnob c, Binding binding, int state)
        {
            c.SetKnobValue(((JoystickBinding)binding).GetAsFloat());
        }

        public static void Increase(CKnob c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.AxisSmoothed.IncreaseButtonDown();
            else
                c.AxisSmoothed.IncreaseButtonUp();
        }

        public static void Decrease(CKnob c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.AxisSmoothed.DecreaseButtonDown();
            else
                c.AxisSmoothed.DecreaseButtonUp();
        }
    }
}