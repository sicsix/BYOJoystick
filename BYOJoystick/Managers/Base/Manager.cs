using System;
using System.Collections.Generic;
using BYOJoystick.Actions;
using BYOJoystick.Controls;
using UnityEngine;

namespace BYOJoystick.Managers.Base
{
    public abstract partial class Manager
    {
        public abstract string GameName    { get; }
        public abstract string ShortName   { get; }
        public abstract bool   IsMulticrew { get; }
        public virtual  bool   IsSeatA     { get; }

        public Dictionary<ActionCategory, List<ControlAction>> ControlActionsByCategory { get; } = new Dictionary<ActionCategory, List<ControlAction>>();

        public Dictionary<string, ControlAction> ControlActions { get; } = new Dictionary<string, ControlAction>();

        protected readonly Dictionary<string, IControl> Controls = new Dictionary<string, IControl>();
        protected          GameObject                   Vehicle;
        protected          VehicleControlManifest       VehicleControlManifest;
        protected          VRInteractable[]             Interactables;
        protected          MFDManager                   MFDManager;
        protected          MFDPortalManager[]           MFDPortalManagers = Array.Empty<MFDPortalManager>();
        protected          MultiPortalSOISwitcher       PortalSOISwitcher;
        private readonly   List<string>                 _postUpdateControlNames = new List<string>();
        private readonly   List<IControl>               _postUpdateControls     = new List<IControl>();


        public void Clear()
        {
            Controls.Clear();
            _postUpdateControls.Clear();
            foreach (var action in ControlActions.Values)
            {
                action.Clear();
            }
        }

        public void Initialise()
        {
            Plugin.Log($"Initialising manager for {GameName}...");

            ControlActionsByCategory.Clear();
            foreach (ActionCategory category in Enum.GetValues(typeof(ActionCategory)))
            {
                ControlActionsByCategory.Add(category, new List<ControlAction>());
            }

            ModifierButton();
            CreateFlightControls();
            CreateAssistControls();
            CreateNavigationControls();
            CreateSystemsControls();
            CreateHUDControls();
            CreateDisplayControls();
            CreateRadioControls();
            CreateMusicControls();
            CreateLightsControls();
            StaticButton("Reset VR Position", ActionCategory.Misc, ResetVRPosition);
            CreateMiscControls();

            Plugin.Log($"Setup {ControlActions.Values.Count} control actions for {GameName}.");
        }

        protected abstract void PreMapping();

        protected abstract void CreateFlightControls();
        protected abstract void CreateAssistControls();
        protected abstract void CreateNavigationControls();
        protected abstract void CreateSystemsControls();
        protected abstract void CreateHUDControls();
        protected abstract void CreateDisplayControls();
        protected abstract void CreateRadioControls();
        protected abstract void CreateMusicControls();
        protected abstract void CreateLightsControls();
        protected abstract void CreateMiscControls();

        protected void AddPostUpdateControl(string controlName)
        {
            _postUpdateControlNames.Add(controlName);
        }

        public void RunPostUpdateControls()
        {
            for (int i = 0; i < _postUpdateControls.Count; i++)
            {
                _postUpdateControls[i].PostUpdate();
            }
        }

        public ControlAction GetAction(string name)
        {
            if (!ControlActions.TryGetValue(name, out var action))
                throw new InvalidOperationException($"Action {name} not found.");
            return action;
        }
    }
}