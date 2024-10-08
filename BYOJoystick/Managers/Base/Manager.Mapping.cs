﻿using System;
using BYOJoystick.Bindings;
using BYOJoystick.Controls;
using UnityEngine;
using VTOLVR.DLC.Rotorcraft;
using VTOLVR.Multiplayer;

namespace BYOJoystick.Managers.Base
{
    public abstract partial class Manager
    {
        public void MapControls(GameObject vehicle)
        {
            Plugin.Log($"Mapping controls for {GameName}...");
            if (vehicle != null)
                Vehicle = vehicle;
            else if (Vehicle == null)
                throw new InvalidOperationException("Vehicle not set.");

            VehicleControlManifest = Vehicle.GetComponent<VehicleControlManifest>();
            if (VehicleControlManifest == null)
                throw new InvalidOperationException("VehicleControlManifest not found.");

            CJoystick.Instances.Clear();

            Interactables = Vehicle.GetComponentsInChildren<VRInteractable>(true);
            Plugin.Log($"Found {Interactables.Length} interactables.");

            MFDManager = VehicleControlManifest.mfdManager;
            if (VehicleControlManifest.mfdPortalManager != null)
                MFDPortalManagers = new[] { VehicleControlManifest.mfdPortalManager };

            PreMapping();

            Plugin.Log($"Mapping {ControlActions.Count} control actions...");
            foreach (var controlAction in ControlActions.Values)
            {
                controlAction.Map();
            }

            Plugin.Log($"Adding {_postUpdateControlNames.Count} post update controls...");
            foreach (string controlName in _postUpdateControlNames)
            {
                if (Controls.TryGetValue(controlName, out var control))
                    _postUpdateControls.Add(control);
                else
                    Plugin.Log($"WARNING - Post update control {controlName} not found, this may be caused by being in single or multiplayer mode.");
            }

            Plugin.Log($"Controls mapped for {GameName}.");
        }

        protected U ByName<T, U>(string name, string root, bool nullable, bool checkName, int idx) where T : MonoBehaviour where U : class, IControl
        {
            if (TryGetExistingControl<U>(name, out var existingControl))
                return existingControl;
            var rootObject = root == null ? Vehicle : GetGameObject(root, nullable);
            if (rootObject == null)
                return null;
            var interactables = rootObject == Vehicle ? Interactables : rootObject.GetComponentsInChildren<VRInteractable>(true);
            var interactable  = FindInteractable(name, interactables, nullable);
            if (interactable == null)
                return null;
            var component = GetComponent<T>(interactable);
            return ToControl<T, U>(name, interactable, component);
        }

        protected U ByType<T, U>(string name, string root, bool nullable, bool checkName, int idx) where T : MonoBehaviour where U : class, IControl
        {
            if (TryGetExistingControl<U>(name, out var existingControl))
                return existingControl;
            var rootObject = root == null ? Vehicle : GetGameObject(root, nullable);
            if (rootObject == null)
                return null;
            var component = FindComponent<T>(rootObject, nullable);
            if (component == null)
                return null;
            var interactable = component.GetComponent<VRInteractable>();
            return ToControl<T, U>(name, interactable, component);
        }

        private VRThrottle GetManifestThrottle(string name, int idx, bool checkName)
        {
            if (idx < 0)
                return VehicleControlManifest.throttle;

            if (idx < VehicleControlManifest.powerLevers.Length)
            {
                var powerLever = VehicleControlManifest.powerLevers[idx];
                if (powerLever != null)
                {
                    if (!checkName || GetInteractable(powerLever).GetControlReferenceName() == name)
                        return powerLever;
                }
            }

            if (idx < VehicleControlManifest.collectives.Length)
            {
                var collective = VehicleControlManifest.collectives[idx];
                if (collective != null)
                {
                    if (!checkName || GetInteractable(collective).GetControlReferenceName() == name)
                        return collective;
                }
            }

            throw new InvalidOperationException($"Throttle {name} not found.");
        }

        private T GetManifestControl<T>(string name, int idx, bool nullable, T[] controls) where T : MonoBehaviour
        {
            if (idx < 0 || idx >= controls.Length)
                throw new InvalidOperationException($"{nameof(T)} index {idx} for {name} out of range.");
            var component = controls[idx];

            if (component == null)
            {
                if (nullable)
                    return null;
                throw new InvalidOperationException($"{nameof(T)} {name} not found.");
            }

            return component;
        }

        protected U ByManifest<T, U>(string name, string root, bool nullable, bool checkName, int idx) where T : MonoBehaviour where U : class, IControl
        {
            if (TryGetExistingControl<U>(name, out var existingControl))
                return existingControl;

            T component;
            if (typeof(T) == typeof(VRButton))
                component = GetManifestControl(name, idx, nullable, VehicleControlManifest.buttons) as T;
            else if (typeof(T) == typeof(VRLever))
                component = GetManifestControl(name, idx, nullable, VehicleControlManifest.levers) as T;
            else if (typeof(T) == typeof(VRTwistKnob))
                component = GetManifestControl(name, idx, nullable, VehicleControlManifest.twistKnobs) as T;
            else if (typeof(T) == typeof(VRTwistKnobInt))
                component = GetManifestControl(name, idx, nullable, VehicleControlManifest.twistKnobInts) as T;
            else if (typeof(T) == typeof(VRDoor))
                component = GetManifestControl(name, idx, nullable, VehicleControlManifest.doors) as T;
            else if (typeof(T) == typeof(VRThrottle))
                component = GetManifestThrottle(name, idx, checkName) as T;
            else if (typeof(T) == typeof(VRJoystick))
                component = GetManifestControl(name, idx, nullable, VehicleControlManifest.joysticks) as T;
            else
                throw new InvalidOperationException($"Unsupported control type {typeof(T).Name}.");

            if (component == null)
            {
                if (nullable)
                    return null;
                throw new InvalidOperationException($"{nameof(T)} {name} not found.");
            }

            var interactable = GetInteractable(component);

            if (checkName && interactable.GetControlReferenceName() != name)
                throw new InvalidOperationException($"Interactable {interactable.GetControlReferenceName()} does not match {name}.");

            return ToControl<T, U>(name, interactable, component);
        }

        protected bool TryGetExistingControl<U>(string name, out U control) where U : class, IControl
        {
            if (Controls.TryGetValue(name, out var foundControl))
            {
                if (!(foundControl is U value))
                    throw new InvalidOperationException($"Control {name} is not of type {typeof(U).Name}");
                control = value;
                return true;
            }

            control = null;
            return false;
        }

        protected GameObject GetGameObject(string name, bool canBeNull = false)
        {
            var transform = Vehicle.transform.Find(name);
            if (transform == null)
            {
                if (canBeNull)
                    return null;
                throw new InvalidOperationException($"GameObject {name} not found");
            }

            return transform.gameObject;
        }

        protected static VRInteractable FindInteractable(string name, VRInteractable[] interactables, bool canBeNull = false)
        {
            VRInteractable interactable = null;
            for (int i = 0; i < interactables.Length; i++)
            {
                if (interactables[i].GetControlReferenceName() != name)
                    continue;
                interactable = interactables[i];
                break;
            }

            if (interactable == null && !canBeNull)
                throw new InvalidOperationException($"Interactable {name} not found.");
            return interactable;
        }

        protected static T FindComponent<T>(GameObject root, bool canBeNull = false) where T : MonoBehaviour
        {
            var component = root.GetComponentInChildren<T>(true);
            if (component == null && !canBeNull)
                throw new InvalidOperationException($"Component {typeof(T).Name} not found.");
            return component;
        }

        protected static VRInteractable GetInteractable<T>(T component) where T : MonoBehaviour
        {
            var interactable = component.GetComponent<VRInteractable>();
            if (interactable == null)
                throw new InvalidOperationException($"Component {typeof(T).Name} requires VRInteractable component.");

            return interactable;
        }

        protected static T GetComponent<T>(VRInteractable interactable) where T : MonoBehaviour
        {
            var component = interactable.GetComponent<T>();
            if (component == null)
                throw new InvalidOperationException($"Interactable {interactable.GetControlReferenceName()} requires {typeof(T).Name} component.");

            return component;
        }

        protected U ToControl<T, U>(string name, VRInteractable interactable, T component) where T : MonoBehaviour where U : class, IControl
        {
            IControl control;
            switch (component)
            {
                case VRInteractable vrInteractable:
                    control = new CInteractable(vrInteractable);
                    break;
                case VRLever lever:
                    var switchCover = interactable.GetComponent<VRSwitchCover>();
                    if (switchCover == null)
                        control = new CLever(interactable, lever);
                    else if (switchCover.coveredSwitch.GetComponent<VRButton>() != null)
                        control = new CButtonCovered(interactable, switchCover);
                    else
                        control = new CLeverCovered(interactable, switchCover);
                    break;
                case VRTwistKnob twistKnob:
                    control = new CKnob(interactable, twistKnob);
                    break;
                case VRTwistKnobInt twistKnobInt:
                    control = new CKnobInt(interactable, twistKnobInt);
                    break;
                case VRButton button:
                    control = new CButton(interactable, button);
                    break;
                case VRThrottle throttle:
                    control = new CThrottle(Vehicle, interactable, throttle, IsMulticrew);
                    break;
                case VRDoor door:
                    control = new CDoor(interactable, door);
                    break;
                case MFDPortalPresetButton presetButton:
                    control = new CPresetButton(presetButton, MFDPortalManagers.Length > 0 ? MFDPortalManagers[0] : null);
                    break;
                case APKnobAltAdjust altKnob:
                    control = new CAltAdjust(altKnob);
                    break;
                case APKnobSpeedDialAdjust speedKnob:
                    control = new CSpdAdjust(speedKnob);
                    break;
                case DashHSI dashHsi:
                    control = new CHSI(dashHsi);
                    break;
                case HelmetController helmet:
                    control = new CHelmet(helmet);
                    break;
                case MFD mfd:
                    control = typeof(U) == typeof(CMFD) ? (IControl)new CMFD(mfd) : new CMMFD(mfd);
                    break;
                case CockpitTeamRadioManager radio:
                    control = new CRadio(radio);
                    break;
                case EjectHandle ejectHandle:
                    control = new CEject(ejectHandle);
                    break;
                case AH94CollectiveFunctions collective:
                    control = new CCollFuncs(collective);
                    break;
                case VehicleMaster vehicleMaster:
                    control = new CVehicleMaster(vehicleMaster, MFDPortalManagers.Length > 0 ? MFDPortalManagers[0] : null);
                    break;
                case CMSConfigUI cmsConfig:
                    control = new CCMS(cmsConfig, MFDPortalManagers.Length > 0 ? MFDPortalManagers[0] : null);
                    break;
                case DashRWR dashRWR:
                    control = new CDashRWR(dashRWR, MFDPortalManagers.Length > 0 ? MFDPortalManagers[0] : null);
                    break;
                case MFDRadarUI radarUI:
                    control = new CRadarPower(radarUI, MFDPortalManagers.Length > 0 ? MFDPortalManagers[0] : null);
                    break;
                case AeroGeometryLever aeroLever:
                    control = new CSweep(aeroLever);
                    break;
                default:
                    throw new InvalidOperationException($"Control {typeof(T).Name} not supported");
            }

            Controls.Add(name, control);

            return (U)control;
        }

        private MFD GetMFD(int index)
        {
            var mfdManager = VehicleControlManifest.mfdManager;
            if (mfdManager == null)
                throw new InvalidOperationException("MFD Manager not found.");
            if (mfdManager.mfds.Count <= index)
                throw new InvalidOperationException($"MFD{index} not found.");
            var mfd = mfdManager.mfds[index];
            if (mfd == null)
                throw new InvalidOperationException($"MFD{index} not found.");
            return mfd;
        }

        protected CMFD MFD(string name, string root, bool nullable, bool checkName, int idx)
        {
            if (TryGetExistingControl<CMFD>(name, out var existingControl))
                return existingControl;
            return ToControl<MFD, CMFD>(name, null, GetMFD(idx));
        }

        private MFD GetMMFDByManifest(int idx)
        {
            var mmfdManager = VehicleControlManifest.miniMfdManager;
            if (mmfdManager == null)
                throw new InvalidOperationException("MMFD Manager not found.");
            if (mmfdManager.mfds.Count <= idx)
                throw new InvalidOperationException($"MMFD{idx} not found.");
            var mmfd = mmfdManager.mfds[idx];
            if (mmfd == null)
                throw new InvalidOperationException($"MMFD{idx} not found.");
            return mmfd;
        }

        private MFD GetMMFDByPath(string root, int idx)
        {
            var mmfdManager = GetGameObject(root).GetComponentInChildren<MFDManager>(true);
            if (mmfdManager == null)
                throw new InvalidOperationException("MMFD Manager not found.");
            if (mmfdManager.mfds.Count <= idx)
                throw new InvalidOperationException($"MMFD{idx} not found.");
            var mmfd = mmfdManager.mfds[idx];
            if (mmfd == null)
                throw new InvalidOperationException($"MMFD{idx} not found.");
            return mmfd;
        }

        protected CMMFD MMFD(string name, string root, bool nullable, bool checkName, int idx)
        {
            if (TryGetExistingControl<CMMFD>(name, out var existingControl))
                return existingControl;

            return ToControl<MFD, CMMFD>(name, null, root != null ? GetMMFDByPath(root, idx) : GetMMFDByManifest(idx));
        }

        protected CJoystick GetJoysticksByPaths(string name, string sideStickRootPath, string centerStickRootPath)
        {
            if (TryGetExistingControl<CJoystick>(name, out var existingControl))
                return existingControl;
            var sideStickRoot   = GetGameObject(sideStickRootPath);
            var centerStickRoot = centerStickRootPath != null ? GetGameObject(centerStickRootPath) : null;
            var sideStick       = FindComponent<VRJoystick>(sideStickRoot);
            var centerStick     = centerStickRootPath != null ? FindComponent<VRJoystick>(centerStickRoot) : null;
            var control         = new CJoystick(Vehicle, sideStick, centerStick, IsMulticrew, true);
            Controls.Add(name, control);
            return control;
        }

        protected CSOI SOI(string name, string root, bool nullable, bool checkName, int idx)
        {
            if (TryGetExistingControl<CSOI>(name, out var existingControl))
                return existingControl;
            if (MFDManager == null && MFDPortalManagers.Length == 0)
                throw new InvalidOperationException("Neither MFD Manager or MFD Portal Manager were found.");

            var              tedacFunctions = FindComponent<AH94TEDACFunctions>(Vehicle, true);
            TargetingMFDPage tgpPage;
            MFDRadarUI       radarPage;
            DashMapDisplay   mapPage;
            if (tedacFunctions != null)
            {
                tgpPage   = tedacFunctions.tgpPage;
                radarPage = tedacFunctions.radarUI;
                mapPage   = tedacFunctions.mapPage;
            }
            else
            {
                tgpPage   = FindComponent<TargetingMFDPage>(Vehicle);
                radarPage = FindComponent<MFDRadarUI>(Vehicle, true);
                mapPage   = FindComponent<DashMapDisplay>(Vehicle);
            }

            var tsdPage     = FindComponent<MFDPTacticalSituationDisplay>(Vehicle, true);
            var soiSwitcher = PortalSOISwitcher != null ? null : FindComponent<ThrottleSOISwitcher>(Vehicle);

            var control = new CSOI(MFDManager, MFDPortalManagers, tgpPage, radarPage, mapPage, tsdPage, soiSwitcher, PortalSOISwitcher);
            Controls.Add(name, control);
            return control;
        }

        protected CThrottleTilt ThrottleTilt(string name, string root, bool nullable, bool checkName, int idx)
        {
            if (TryGetExistingControl<CThrottleTilt>(name, out var existingControl))
                return existingControl;

            var throttle       = FindComponent<VRThrottle>(Vehicle);
            var interactable   = GetInteractable(throttle);
            var tiltController = FindComponent<TiltController>(Vehicle);
            var control        = new CThrottleTilt(Vehicle, interactable, throttle, tiltController, IsMulticrew);
            Controls.Add(name, control);
            return control;
        }

        protected CInteractable TSDInteractable(string name, string root, bool nullable, bool checkName, int idx)
        {
            if (TryGetExistingControl<CInteractable>(name, out var existingControl))
                return existingControl;
            var tsd           = FindComponent<MFDPTacticalSituationDisplay>(Vehicle);
            var interactable  = FindInteractable(name, tsd.GetComponentsInChildren<VRInteractable>(true));
            var cInteractable = new CInteractable(interactable);
            Controls.Add(name, cInteractable);
            return cInteractable;
        }

        protected CHelmet HelmetController(string name, string root, bool nullable, bool checkName, int idx)
        {
            if (TryGetExistingControl<CHelmet>(name, out var existingControl))
                return existingControl;

            HelmetController helmetController = null;

            var playerModelSyncs = Vehicle.GetComponentsInChildren<PlayerModelSync>(true);
            for (int i = 0; i < playerModelSyncs.Length; i++)
            {
                var playerModelSync = playerModelSyncs[i];
                if (playerModelSync.netEntity.isMine)
                {
                    helmetController = playerModelSync.localHelmet;
                    if (helmetController == null)
                        throw new InvalidOperationException("HelmetController not found on PlayerModelSync.");
                    Plugin.Log("Found HelmetController on PlayerModelSync.");
                    break;
                }
            }

            if (helmetController == null)
            {
                helmetController = FindComponent<HelmetController>(Vehicle);
                Plugin.Log("Found HelmetController as child of Vehicle.");
            }

            var control = new CHelmet(helmetController);
            Controls.Add(name, control);
            return control;
        }

        protected static void ResetVRPosition(IControl control, Binding binding, int state)
        {
            if (binding.GetAsBool())
                VRHead.ReCenter();
        }
    }
}