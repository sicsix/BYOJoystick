using System;
using BYOJoystick.Bindings;
using BYOJoystick.Controls.Sync;
using UnityEngine;
using VTOLVR.Multiplayer;

namespace BYOJoystick.Controls
{
    public class CInteractable : IControl
    {
        protected readonly VRInteractable               Interactable;
        protected readonly InteractableSyncWrapper      SyncWrapper;
        protected readonly bool                         IsMP;
        protected          bool                         Pressed;
        protected readonly Action<VRInteractable, bool> SetInteracting;
        protected readonly Action<VRInteractable, int>  SetInteractedOnFrame;

        public CInteractable(VRInteractable interactable)
        {
            Interactable         = interactable;
            IsMP                 = VTOLMPUtils.IsMultiplayer();
            SyncWrapper          = IsMP ? InteractableSyncWrapper.Create(interactable) : null;
            Pressed              = false;
            SetInteracting       = CompiledExpressions.CreatePropertySetter<VRInteractable, bool>("interacting");
            SetInteractedOnFrame = CompiledExpressions.CreateFieldSetter<VRInteractable, int>("interactedOnFrame");
        }

        public void PostUpdate()
        {
        }

        public static void Use(CInteractable c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                if (c.Pressed || !c.Interactable.enabled)
                    return;

                if (c.IsMP)
                {
                    if (c.SyncWrapper != null && !c.SyncWrapper.TryInteracting(false))
                        return;
                }

                c.Pressed = true;
                c.SetInteracting(c.Interactable, true);
                c.SetInteractedOnFrame(c.Interactable, Time.frameCount);
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
            }
        }
    }
}