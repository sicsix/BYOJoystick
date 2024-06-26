using System;
using BYOJoystick.Bindings;
using BYOJoystick.Controls.Sync;
using VTOLVR.Multiplayer;

namespace BYOJoystick.Controls
{
    public class CKnobInt : IControl
    {
        protected readonly VRInteractable          Interactable;
        protected readonly InteractableSyncWrapper SyncWrapper;
        protected readonly bool                    IsMP;
        protected readonly VRTwistKnobInt          TwistKnobInt;

        public CKnobInt(VRInteractable interactable, VRTwistKnobInt twistKnobInt)
        {
            Interactable = interactable;
            IsMP         = VTOLMPUtils.IsMultiplayer();
            SyncWrapper  = IsMP ? InteractableSyncWrapper.Create(interactable) : null;
            TwistKnobInt = twistKnobInt;
        }

        public void PostUpdate()
        {
        }

        private void SetKnobValue(int value)
        {
            if (IsMP && SyncWrapper != null)
            {
                if (!SyncWrapper.TryInteractTimed(false, 1f))
                    return;
            }

            TwistKnobInt.RemoteSetState(value);
        }

        public static void Set(CKnobInt c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.SetKnobValue(state);
        }

        public static void Next(CKnobInt c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.SetKnobValue(Math.Min(c.TwistKnobInt.currentState + 1, c.TwistKnobInt.states - 1));
        }

        public static void Prev(CKnobInt c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.SetKnobValue(Math.Max(c.TwistKnobInt.currentState - 1, 0));
        }

        public static void Cycle(CKnobInt c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.SetKnobValue((c.TwistKnobInt.currentState + 1) % c.TwistKnobInt.states);
        }

        public static void Push(CKnobInt c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.TwistKnobInt.onPushButton?.Invoke();
        }
    }
}