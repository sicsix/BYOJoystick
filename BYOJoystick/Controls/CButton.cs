using System;
using BYOJoystick.Bindings;
using BYOJoystick.Controls.Sync;
using UnityEngine;
using VTOLVR.Multiplayer;

namespace BYOJoystick.Controls
{
    public class CButton : IControl
    {
        protected readonly VRInteractable               Interactable;
        protected readonly InteractableSyncWrapper      SyncWrapper;
        protected readonly bool                         IsMP;
        protected readonly VRButton                     Button;
        protected          bool                         Pressed;
        protected readonly Action<VRInteractable, bool> SetInteracting;
        protected readonly Action<VRInteractable, int>  SetInteractedOnFrame;

        public CButton(VRInteractable interactable, VRButton button)
        {
            Interactable         = interactable;
            IsMP                 = VTOLMPUtils.IsMultiplayer();
            SyncWrapper          = IsMP ? InteractableSyncWrapper.Create(interactable) : null;
            Button               = button;
            Pressed              = false;
            SetInteracting       = CompiledExpressions.CreateFieldSetter<VRInteractable, bool>("interacting");
            SetInteractedOnFrame = CompiledExpressions.CreateFieldSetter<VRInteractable, int>("interactedOnFrame");
        }

        public void PostUpdate()
        {
        }

        public static void Use(CButton c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                if (c.Pressed || !c.Interactable.enabled || !c.Interactable.gameObject.activeInHierarchy)
                    return;

                if (c.IsMP)
                {
                    if (c.SyncWrapper != null && !c.SyncWrapper.TryInteracting(false))
                        return;
                }

                c.Pressed = true;
                c.SetInteracting(c.Interactable, true);
                c.SetInteractedOnFrame(c.Interactable, Time.frameCount);
                c.Button.Vrint_OnStartInteraction(null);
                c.Interactable.OnInteract?.Invoke();

                if (c.Interactable.OnInteracting == null)
                    return;
                c.Interactable.StartCoroutine(c.Interactable.WhileInteractingRoutine());
            }
            else
            {
                if (!c.Pressed)
                    return;

                if (c.IsMP && c.SyncWrapper != null && c.SyncWrapper.IsInteracting)
                    c.SyncWrapper.StopInteracting(false);

                c.Pressed = false;
                c.Interactable.StopInteraction();
                c.Button.Vrint_OnStopInteraction(null);
            }
        }
    }
}