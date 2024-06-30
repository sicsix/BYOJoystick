using System;
using BYOJoystick.Bindings;
using BYOJoystick.Controls.Sync;
using UnityEngine;
using VTOLVR.Multiplayer;

namespace BYOJoystick.Controls
{
    public class CLever : IControl
    {
        protected readonly VRInteractable          Interactable;
        protected readonly InteractableSyncWrapper SyncWrapper;
        protected readonly bool                    IsMP;
        protected readonly VRLever                 Lever;

        public int CurrentState => Lever.currentState;

        public CLever(VRInteractable interactable, VRLever lever)
        {
            Interactable = interactable;
            IsMP         = VTOLMPUtils.IsMultiplayer();
            SyncWrapper  = IsMP ? InteractableSyncWrapper.Create(interactable) : null;
            Lever        = lever;
        }

        public void PostUpdate()
        {
        }

        public void SetLeverValue(int value)
        {
            if (IsMP && SyncWrapper != null)
            {
                if (!SyncWrapper.TryInteractTimed(false, 1f))
                    return;
            }

            Lever.RemoteSetState(value);
        }

        public static void Set(CLever c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.SetLeverValue(Mathf.Clamp(state, 0, c.Lever.states - 1));
        }

        public static void Next(CLever c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.SetLeverValue(Math.Min(c.Lever.currentState + 1, c.Lever.states - 1));
        }

        public static void Prev(CLever c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.SetLeverValue(Math.Max(c.Lever.currentState - 1, 0));
        }

        public static void Cycle(CLever c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.SetLeverValue((c.Lever.currentState + 1) % c.Lever.states);
        }
    }
}