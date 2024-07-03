using BYOJoystick.Controls;
using BYOJoystick.Managers.Base;
using VTOLVR.Multiplayer;

namespace BYOJoystick.Managers
{
    public class F45AManager : Manager
    {
        public override string GameName    => "F-45A";
        public override string ShortName   => "F45A";
        public override bool   IsMulticrew => false;

        private static string SideJoystick   => "Local/JoystickObjects/SideStickObjects";
        private static string CenterJoystick => "Local/JoystickObjects/CenterStickObjects";

        private CJoystick Joysticks(string name, string root, bool nullable, bool checkName, int idx)
        {
            return GetJoysticksByPaths(name, SideJoystick, CenterJoystick);
        }

        protected override void PreMapping()
        {
        }

        protected override void CreateFlightControls()
        {
            FlightAxisC("Joystick Pitch", "Joystick", Joysticks, CJoystick.SetPitch);
            FlightAxisC("Joystick Yaw", "Joystick", Joysticks, CJoystick.SetYaw);
            FlightAxisC("Joystick Roll", "Joystick", Joysticks, CJoystick.SetRoll);

            FlightAxis("Throttle", "Throttle", ThrottleTilt, CThrottle.Set);
            FlightButton("Throttle Increase", "Throttle", ThrottleTilt, CThrottle.Increase);
            FlightButton("Throttle Decrease", "Throttle", ThrottleTilt, CThrottle.Decrease);

            FlightAxis("Tilt", "Throttle", ThrottleTilt, CThrottleTilt.SetTiltTarget);
            FlightButton("Tilt Up", "Throttle", ThrottleTilt, CThrottleTilt.TiltUp);
            FlightButton("Tilt Down", "Throttle", ThrottleTilt, CThrottleTilt.TiltDown);

            FlightAxis("Brakes/Airbrakes Axis", "Throttle", ThrottleTilt, CThrottle.Trigger);
            FlightButton("Brakes/Airbrakes", "Throttle", ThrottleTilt, CThrottle.Trigger);

            FlightButton("Landing Gear Toggle", "Landing Gear", ByManifest<VRLever, CLever>, CLever.Cycle, i: 2);
            FlightButton("Landing Gear Up", "Landing Gear", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 2);
            FlightButton("Landing Gear Down", "Landing Gear", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 2);

            FlightButton("Brake Lock Toggle", "Parking Brake", ByManifest<VRLever, CLever>, CLever.Cycle, i: 4);
            FlightButton("Brake Lock On", "Parking Brake", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 4);
            FlightButton("Brake Lock Off", "Parking Brake", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 4);

            FlightButton("Wing Fold Toggle", "Wing Fold", ByManifest<VRLever, CLever>, CLever.Cycle, i: 3);
            FlightButton("Wing Fold Up", "Wing Fold", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 3);
            FlightButton("Wing Fold Down", "Wing Fold", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 3);

            FlightButton("Launch Bar Toggle", "Launch Bar", ByManifest<VRLever, CLever>, CLever.Cycle, i: 10);
            FlightButton("Launch Bar Extend", "Launch Bar", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 10);
            FlightButton("Launch Bar Retract", "Launch Bar", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 10);

            FlightButton("Arrestor Hook Toggle", "Arrestor Hook", ByManifest<VRLever, CLever>, CLever.Cycle, i: 12);
            FlightButton("Arrestor Hook Extend", "Arrestor Hook", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 12);
            FlightButton("Arrestor Hook Retract", "Arrestor Hook", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 12);

            AddPostUpdateControl("Joystick");
            AddPostUpdateControl("Throttle");
        }

        protected override void CreateAssistControls()
        {
        }

        protected override void CreateNavigationControls()
        {
            NavButton("A/P Nav Mode", "Nav Waypoint AP", ByName<VRInteractable, CInteractable>, CInteractable.Use);
            NavButton("A/P Hvr Mode", "Vertical Control AP Mode", ByManifest<VRButton, CButton>, CButton.Use, i: 20);
            NavButton("A/P Hdg Hold", "Heading AP", ByName<VRInteractable, CInteractable>, CInteractable.Use);
            NavButton("A/P Alt Hold", "Altitude AP", ByName<VRInteractable, CInteractable>, CInteractable.Use);
            NavButton("A/P Off", "AP Off", ByName<VRInteractable, CInteractable>, CInteractable.Use);

            NavButton("A/P Alt Increase", "Alt +", ByManifest<VRButton, CButton>, CButton.Use, i: 14);
            NavButton("A/P Alt Decrease", "Alt -", ByManifest<VRButton, CButton>, CButton.Use, i: 15);

            NavButton("A/P Hdg Right", "Heading Right", ByManifest<VRButton, CButton>, CButton.Use, i: 12);
            NavButton("A/P Hdg Left", "Heading Left", ByManifest<VRButton, CButton>, CButton.Use, i: 13);

            NavButton("Altitude Mode Toggle", "Altitude Mode", ByName<VRInteractable, CInteractable>, CInteractable.Use);
            NavButton("Clear Waypoint", "Clear Waypoint", ByManifest<VRButton, CButton>, CButton.Use, i: 24);
        }

        protected override void CreateSystemsControls()
        {
            SystemsButton("Clear Cautions", "Dismiss", ByManifest<VRButton, CButton>, CButton.Use, i: 11);

            SystemsButton("Master Arm Toggle", "Switch Cover (Master Arm)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Cycle, c: false, i: 1);
            SystemsButton("Master Arm On", "Switch Cover (Master Arm)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Set, 1, c: false, i: 1);
            SystemsButton("Master Arm Off", "Switch Cover (Master Arm)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Set, 0, c: false, i: 1);

            SystemsButton("Fire Weapon", "Joystick", Joysticks, CJoystick.Trigger, i: 0);
            SystemsButton("Cycle Weapons", "Joystick", Joysticks, CJoystick.MenuButton, i: 0);
            SystemsButton("Fire Countermeasures", "Throttle", ThrottleTilt, CThrottle.MenuButton);

            SystemsButton("Flares Toggle", "CMSConfigUI", ByType<CMSConfigUI, CCMS>, CCMS.ToggleFlares);
            SystemsButton("Flares On", "CMSConfigUI", ByType<CMSConfigUI, CCMS>, CCMS.FlaresOn);
            SystemsButton("Flares Off", "CMSConfigUI", ByType<CMSConfigUI, CCMS>, CCMS.FlaresOff);

            SystemsButton("Chaff Toggle", "CMSConfigUI", ByType<CMSConfigUI, CCMS>, CCMS.ToggleChaff);
            SystemsButton("Chaff On", "CMSConfigUI", ByType<CMSConfigUI, CCMS>, CCMS.ChaffOn);
            SystemsButton("Chaff Off", "CMSConfigUI", ByType<CMSConfigUI, CCMS>, CCMS.ChaffOff);

            SystemsButton("Engine Toggle", "Switch Cover (Engine)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Cycle, c: false, i: 14);
            SystemsButton("Engine On", "Switch Cover (Engine)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Set, 1, c: false, i: 14);
            SystemsButton("Engine Off", "Switch Cover (Engine)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Set, 0, c: false, i: 14);

            SystemsButton("Main Battery Toggle", "Main Battery", ByManifest<VRLever, CLever>, CLever.Cycle, i: 15);
            SystemsButton("Main Battery On", "Main Battery", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 15);
            SystemsButton("Main Battery Off", "Main Battery", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 15);

            SystemsButton("APU Toggle", "Auxilliary Power", ByManifest<VRLever, CLever>, CLever.Cycle, i: 16);
            SystemsButton("APU On", "Auxilliary Power", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 16);
            SystemsButton("APU Off", "Auxilliary Power", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 16);

            SystemsButton("Radar Power Toggle", "Radar Power", ByType<MFDRadarUI, CRadarPower>, CRadarPower.Toggle);
            SystemsButton("Radar Power On", "Radar Power", ByType<MFDRadarUI, CRadarPower>, CRadarPower.On, 1);
            SystemsButton("Radar Power Off", "Radar Power", ByType<MFDRadarUI, CRadarPower>, CRadarPower.Off, 0);

            SystemsButton("RWR Mode Cycle", "RWR Mode", ByType<DashRWR, CDashRWR>, CDashRWR.Cycle);
            SystemsButton("RWR Mode Prev", "RWR Mode", ByType<DashRWR, CDashRWR>, CDashRWR.Prev);
            SystemsButton("RWR Mode Next", "RWR Mode", ByType<DashRWR, CDashRWR>, CDashRWR.Next);
            SystemsButton("RWR Mode On", "RWR Mode", ByType<DashRWR, CDashRWR>, CDashRWR.On);
            SystemsButton("RWR Mode Mute", "RWR Mode", ByType<DashRWR, CDashRWR>, CDashRWR.Silent);
            SystemsButton("RWR Mode Off", "RWR Mode", ByType<DashRWR, CDashRWR>, CDashRWR.Off);
        }

        protected override void CreateHUDControls()
        {
            HUDButton("Helmet Visor Toggle", "Helmet", HelmetController, CHelmet.ToggleVisor);
            HUDButton("Helmet Visor Open", "Helmet", HelmetController, CHelmet.OpenVisor);
            HUDButton("Helmet Visor Closed", "Helmet", HelmetController, CHelmet.CloseVisor);
            HUDButton("Helmet NV Toggle", "Helmet", HelmetController, CHelmet.ToggleNightVision);
            HUDButton("Helmet NV On", "Helmet", HelmetController, CHelmet.EnableNightVision);
            HUDButton("Helmet NV Off", "Helmet", HelmetController, CHelmet.DisableNightVision);

            HUDButton("HMD Power Toggle", "HMD Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 1);
            HUDButton("HMD Power On", "HMD Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 1);
            HUDButton("HMD Power Off", "HMD Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 1);

            HUDAxis("HUD Brightness", "HUD Brightness", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 1);
            HUDButton("HUD Brightness Increase", "HUD Brightness", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 1);
            HUDButton("HUD Brightness Decrease", "HUD Brightness", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 1);

            AddPostUpdateControl("HUD Brightness");
        }

        protected override void CreateDisplayControls()
        {
            DisplayButton("SOI Slew Button", "SOI", SOI, CSOI.SlewButton);
            DisplayAxisC("SOI Slew X", "SOI", SOI, CSOI.SlewX);
            DisplayAxisC("SOI Slew Y", "SOI", SOI, CSOI.SlewY);
            DisplayButton("SOI Slew Up", "SOI", SOI, CSOI.SlewUp);
            DisplayButton("SOI Slew Right", "SOI", SOI, CSOI.SlewRight);
            DisplayButton("SOI Slew Down", "SOI", SOI, CSOI.SlewDown);
            DisplayButton("SOI Slew Left", "SOI", SOI, CSOI.SlewLeft);

            DisplayButton("SOI Next", "SOI", SOI, CSOI.Next);
            DisplayButton("SOI Prev", "SOI", SOI, CSOI.Prev);

            DisplayButton("SOI Zoom In", "SOI", SOI, CSOI.ZoomIn);
            DisplayButton("SOI Zoom Out", "SOI", SOI, CSOI.ZoomOut);

            DisplayButton("TSD Slew TGP/EOTS", "Slew EOTS", TSDInteractable, CInteractable.Use);
            DisplayButton("TSD GPS-S", "GPS Send", TSDInteractable, CInteractable.Use);

            DisplayAxis("MFD Brightness", "MFD Brightness", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 0);
            DisplayButton("MFD Brightness Increase", "MFD Brightness", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 0);
            DisplayButton("MFD Brightness Decrease", "MFD Brightness", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 0);

            DisplayButton("MFD Power Toggle", "MFD Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 0);
            DisplayButton("MFD Power On", "MFD Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 0);
            DisplayButton("MFD Power Off", "MFD Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 0);

            DisplayButton("MFD Load Layout 1", "Preset 1", ByName<MFDPortalPresetButton, CPresetButton>, CPresetButton.Load, i: 7);
            DisplayButton("MFD Save Layout 1", "Preset 1", ByName<MFDPortalPresetButton, CPresetButton>, CPresetButton.Save, i: 7);
            DisplayButton("MFD Load Layout 2", "Preset 2", ByName<MFDPortalPresetButton, CPresetButton>, CPresetButton.Load, i: 8);
            DisplayButton("MFD Save Layout 2", "Preset 2", ByName<MFDPortalPresetButton, CPresetButton>, CPresetButton.Save, i: 8);
            DisplayButton("MFD Load Layout 3", "Preset 3", ByName<MFDPortalPresetButton, CPresetButton>, CPresetButton.Load, i: 9);
            DisplayButton("MFD Save Layout 3", "Preset 3", ByName<MFDPortalPresetButton, CPresetButton>, CPresetButton.Save, i: 9);
            DisplayButton("MFD Load Layout 4", "Preset 4", ByName<MFDPortalPresetButton, CPresetButton>, CPresetButton.Load, i: 10);
            DisplayButton("MFD Save Layout 4", "Preset 4", ByName<MFDPortalPresetButton, CPresetButton>, CPresetButton.Save, i: 10);

            AddPostUpdateControl("MFD Brightness");
            AddPostUpdateControl("SOI");
        }

        protected override void CreateRadioControls()
        {
            RadioButton("Radio Transmit", "Radio", ByType<CockpitTeamRadioManager, CRadio>, CRadio.Transmit);

            RadioButton("Radio Channel Cycle", "Radio Channel", ByName<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle);
            RadioButton("Radio Channel Team", "Radio Channel", ByName<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0);
            RadioButton("Radio Channel Global", "Radio Channel", ByName<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1);

            RadioButton("Radio Mode Cycle", "Radio Mode", ByName<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle);
            RadioButton("Radio Mode Next", "Radio Mode", ByName<VRTwistKnobInt, CKnobInt>, CKnobInt.Next);
            RadioButton("Radio Mode Prev", "Radio Mode", ByName<VRTwistKnobInt, CKnobInt>, CKnobInt.Prev);
            RadioButton("Radio Mode Hot Mic", "Radio Mode", ByName<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0);
            RadioButton("Radio Mode PTT", "Radio Mode", ByName<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1);
            RadioButton("Radio Mode Off", "Radio Mode", ByName<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 2);

            RadioAxis("Radio Volume", "Comm Radio Volume", ByName<VRTwistKnob, CKnob>, CKnob.Set);
            RadioButton("Radio Volume Increase", "Comm Radio Volume", ByName<VRTwistKnob, CKnob>, CKnob.Increase);
            RadioButton("Radio Volume Decrease", "Comm Radio Volume", ByName<VRTwistKnob, CKnob>, CKnob.Decrease);

            RadioAxis("Radio Team Volume", "Team Radio Volume", ByName<VRTwistKnob, CKnob>, CKnob.Set, n: true);
            RadioButton("Radio Team Volume Increase", "Team Radio Volume", ByName<VRTwistKnob, CKnob>, CKnob.Increase, n: true);
            RadioButton("Radio Team Volume Decrease", "Team Radio Volume", ByName<VRTwistKnob, CKnob>, CKnob.Decrease, n: true);

            AddPostUpdateControl("Comm Radio Volume");
            AddPostUpdateControl("Team Radio Volume");
        }

        protected override void CreateMusicControls()
        {
            MusicButton("Music Play/Pause", "Play/Pause", ByManifest<VRButton, CButton>, CButton.Use, i: 23);
            MusicButton("Music Next", "Next Song", ByManifest<VRButton, CButton>, CButton.Use, i: 22);
            MusicButton("Music Previous", "Prev Song", ByManifest<VRButton, CButton>, CButton.Use, i: 21);
            MusicAxis("Music Volume", "Radio Volume", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 3);
            MusicButton("Music Volume Up", "Radio Volume", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 3);
            MusicButton("Music Volume Down", "Radio Volume", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 3);

            AddPostUpdateControl("Radio Volume");
        }

        protected override void CreateLightsControls()
        {
            LightsButton("Instrument Lights Toggle", "Instrument Lights", ByManifest<VRLever, CLever>, CLever.Cycle, i: 9);
            LightsButton("Instrument Lights On", "Instrument Lights", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 9);
            LightsButton("Instrument Lights Off", "Instrument Lights", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 9);
            LightsAxis("Instrument Brightness", "Instrument Brightness", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 2);
            LightsButton("Instrument Brightness Increase", "Instrument Brightness", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 2);
            LightsButton("Instrument Brightness Decrease", "Instrument Brightness", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 2);

            LightsButton("Formation Lights Toggle", "Formation Lights", ByManifest<VRLever, CLever>, CLever.Cycle, i: 8);
            LightsButton("Formation Lights On", "Formation Lights", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 8);
            LightsButton("Formation Lights Off", "Formation Lights", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 8);

            LightsButton("Nav Lights Toggle", "Nav Lights", ByManifest<VRLever, CLever>, CLever.Cycle, i: 7);
            LightsButton("Nav Lights On", "Nav Lights", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 7);
            LightsButton("Nav Lights Off", "Nav Lights", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 7);

            LightsButton("Strobe Lights Toggle", "Strobe Lights", ByManifest<VRLever, CLever>, CLever.Cycle, i: 6);
            LightsButton("Strobe Lights On", "Strobe Lights", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 6);
            LightsButton("Strobe Lights Off", "Strobe Lights", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 6);

            LightsButton("Landing Lights Toggle", "Landing Lights", ByManifest<VRLever, CLever>, CLever.Cycle, i: 5);
            LightsButton("Landing Lights On", "Landing Lights", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 5);
            LightsButton("Landing Lights Off", "Landing Lights", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 5);

            AddPostUpdateControl("Instrument Brightness");
        }

        protected override void CreateMiscControls()
        {
            MiscButton("Canopy Toggle", "Canopy Switch", ByManifest<VRLever, CLever>, CLever.Cycle, i: 18);
            MiscButton("Canopy Open", "Canopy Switch", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 18);
            MiscButton("Canopy Close", "Canopy Switch", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 18);

            MiscButton("Raise Seat", "Raise Seat", ByManifest<VRButton, CButton>, CButton.Use, i: 18);
            MiscButton("Lower Seat", "Lower Seat", ByManifest<VRButton, CButton>, CButton.Use, i: 19);
            MiscButton("Eject", "Eject", ByType<EjectHandle, CEject>, CEject.Pull);

            MiscButton("Fuel Port Toggle", "Fuel Port", ByManifest<VRLever, CLever>, CLever.Cycle, i: 11);
            MiscButton("Fuel Port Open", "Fuel Port", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 11);
            MiscButton("Fuel Port Close", "Fuel Port", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 11);

            MiscButton("Jettison Execute", "Jettison Switch", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Push, i: 2);
            MiscButton("Jettison Cycle", "Jettison Switch", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 2);
            MiscButton("Jettison Next", "Jettison Switch", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Next, i: 2);
            MiscButton("Jettison Prev", "Jettison Switch", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Prev, i: 2);
            MiscButton("Jettison Mark All", "Jettison Switch", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 2);
            MiscButton("Jettison Mark Ext", "Jettison Switch", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 2);
            MiscButton("Jettison Mark Sel", "Jettison Switch", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 2, i: 2);
        }
    }
}