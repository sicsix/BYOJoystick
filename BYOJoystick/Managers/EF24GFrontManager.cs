using System;
using BYOJoystick.Controls;
using BYOJoystick.Managers.Base;
using VTOLVR.DLC.EW;
using VTOLVR.Multiplayer;

namespace BYOJoystick.Managers
{
    public class EF24GFrontManager : Manager
    {
        public override string GameName    => "EF-24G Front";
        public override string ShortName   => "EF24GFront";
        public override bool   IsMulticrew => true;
        public override bool   IsSeatA     => true;

        private static string Cockpit        => "PassengerOnlyObjs/FrontCockpit";
        private static string Seat           => "FrontSeatParent";
        private static string SideJoystick   => "PassengerOnlyObjs/FrontCockpit/SideStickObjects (Front)";
        private static string CenterJoystick => "PassengerOnlyObjs/FrontCockpit/CenterStickObjects (Front)";
        private static string TSD            => "PassengerOnlyObjs/FrontCockpit/DashTransform/touchScreenArea/MFDPortals/poweredObj/-- pages --/TacticalSituationDisplay";

        private CJoystick Joysticks(string name, string root, bool nullable, int idx)
        {
            return GetJoysticksByPaths(name, SideJoystick, CenterJoystick);
        }

        protected override void PreMapping()
        {
            MFDPortalManagers = new[] { GetGameObject(Cockpit).GetComponentInChildren<MFDPortalManager>(true) };
            if (MFDPortalManagers[0] == null)
                throw new InvalidOperationException("MFDPortalManager not found");
            PortalSOISwitcher = GetGameObject(Cockpit).GetComponentInChildren<MultiPortalSOISwitcher>(true);
        }

        protected override void CreateFlightControls()
        {
            FlightAxisC("Joystick Pitch", "Joystick", Joysticks, CJoystick.SetPitch);
            FlightAxisC("Joystick Yaw", "Joystick", Joysticks, CJoystick.SetYaw);
            FlightAxisC("Joystick Roll", "Joystick", Joysticks, CJoystick.SetRoll);

            FlightAxis("Throttle", "Throttle", ByManifest<VRThrottle, CThrottle>, CThrottle.Set);
            FlightButton("Throttle Increase", "Throttle", ByManifest<VRThrottle, CThrottle>, CThrottle.Increase);
            FlightButton("Throttle Decrease", "Throttle", ByManifest<VRThrottle, CThrottle>, CThrottle.Decrease);

            FlightAxis("Sweep", "Sweep", ByType<AeroGeometryLever, CSweep>, CSweep.Set);
            FlightButton("Sweep Forward", "Sweep", ByType<AeroGeometryLever, CSweep>, CSweep.Forward);
            FlightButton("Sweep Backward", "Sweep", ByType<AeroGeometryLever, CSweep>, CSweep.Backward);

            FlightButton("Sweep Auto Toggle", "Sweep", ByType<AeroGeometryLever, CSweep>, CSweep.ToggleAuto);
            FlightButton("Sweep Auto On", "Sweep", ByType<AeroGeometryLever, CSweep>, CSweep.AutoOn);
            FlightButton("Sweep Auto Off", "Sweep", ByType<AeroGeometryLever, CSweep>, CSweep.AutoOff);

            FlightAxis("Brakes/Airbrakes Axis", "Throttle", ByManifest<VRThrottle, CThrottle>, CThrottle.Trigger);
            FlightButton("Brakes/Airbrakes", "Throttle", ByManifest<VRThrottle, CThrottle>, CThrottle.Trigger);

            FlightButton("Flaps Cycle", "Flaps (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 19);
            FlightButton("Flaps Increase", "Flaps (Front)", ByManifest<VRLever, CLever>, CLever.Next, i: 19);
            FlightButton("Flaps Decrease", "Flaps (Front)", ByManifest<VRLever, CLever>, CLever.Prev, i: 19);
            FlightButton("Flaps 1", "Flaps (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 19);
            FlightButton("Flaps 2", "Flaps (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 19);
            FlightButton("Flaps 3", "Flaps (Front)", ByManifest<VRLever, CLever>, CLever.Set, 2, i: 19);

            FlightButton("Landing Gear Toggle", "Landing Gear", ByManifest<VRLever, CLever>, CLever.Cycle, i: 20);
            FlightButton("Landing Gear Up", "Landing Gear", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 20);
            FlightButton("Landing Gear Down", "Landing Gear", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 20);

            FlightButton("Brake Lock Toggle", "Brake Locks (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 1);
            FlightButton("Brake Lock On", "Brake Locks (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 1);
            FlightButton("Brake Lock Off", "Brake Locks (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 1);

            FlightButton("Launch Bar Toggle", "Launch Bar (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 10);
            FlightButton("Launch Bar Extend", "Launch Bar (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 10);
            FlightButton("Launch Bar Retract", "Launch Bar (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 10);

            FlightButton("Arrestor Hook Toggle", "Arrestor Hook (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 11);
            FlightButton("Arrestor Hook Extend", "Arrestor Hook (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 11);
            FlightButton("Arrestor Hook Retract", "Arrestor Hook (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 11);

            FlightButton("Catapult T/O Trim Toggle", "Catapult Take Off Trim (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 12);
            FlightButton("Catapult T/O Trim On", "Catapult Take Off Trim (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 12);
            FlightButton("Catapult T/O Trim Off", "Catapult Take Off Trim (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 12);

            AddPostUpdateControl("Joystick");
            AddPostUpdateControl("Throttle");
            AddPostUpdateControl("Sweep");
        }

        protected override void CreateAssistControls()
        {
        }

        protected override void CreateNavigationControls()
        {
            NavButton("A/P Nav Mode", "Nav Waypoint AP", ByName<VRInteractable, CInteractable>, CInteractable.Use, r: Cockpit);
            NavButton("A/P Spd Hold", "Speed AP", ByName<VRInteractable, CInteractable>, CInteractable.Use, r: Cockpit);
            NavButton("A/P Hdg Hold", "Heading AP", ByName<VRInteractable, CInteractable>, CInteractable.Use, r: Cockpit);
            NavButton("A/P Alt Hold", "Altitude AP", ByName<VRInteractable, CInteractable>, CInteractable.Use, r: Cockpit);
            NavButton("A/P Off", "AP Off", ByName<VRInteractable, CInteractable>, CInteractable.Use, r: Cockpit);

            NavButton("A/P Alt Increase", "Alt +", ByManifest<VRButton, CButton>, CButton.Use, i: 13);
            NavButton("A/P Alt Decrease", "Alt -", ByManifest<VRButton, CButton>, CButton.Use, i: 14);

            NavButton("A/P Hdg Right", "Heading Right", ByManifest<VRButton, CButton>, CButton.Use, i: 15);
            NavButton("A/P Hdg Left", "Heading Left", ByManifest<VRButton, CButton>, CButton.Use, i: 16);

            NavButton("A/P Crs Right", "Course Right", ByManifest<VRButton, CButton>, CButton.Use, i: 41);
            NavButton("A/P Crs Left", "Course Left", ByManifest<VRButton, CButton>, CButton.Use, i: 42);

            NavButton("Altitude Mode Toggle", "Toggle Altitude Mode", ByType<VehicleMaster, CVehicleMaster>, CVehicleMaster.ToggleAltMode);
            NavButton("Clear Waypoint", "Clear Waypoint", ByManifest<VRButton, CButton>, CButton.Use, i: 8);
        }

        protected override void CreateSystemsControls()
        {
            SystemsButton("Clear Cautions", "Dismiss", ByManifest<VRButton, CButton>, CButton.Use, i: 7);

            SystemsButton("Master Arm Toggle", "Master Arm (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 53);
            SystemsButton("Master Safe Toggle", "Master Safe (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 54);

            SystemsButton("Master Mode Toggle", "Master Mode (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 8);
            SystemsButton("Master Mode WPN", "Master Mode (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 8);
            SystemsButton("Master Mode EW", "Master Mode (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, 1, i: 8);

            SystemsButton("Arming Mode Toggle", "EF24Hotas", EF24Hotas, CEF24Hotas.ToggleArmingMode);
            SystemsButton("Arming Mode AA", "EF24Hotas", EF24Hotas, CEF24Hotas.ArmingAA);
            SystemsButton("Arming Mode AG", "EF24Hotas", EF24Hotas, CEF24Hotas.ArmingAG);

            SystemsButton("Fire Weapon", "Joystick", Joysticks, CJoystick.Trigger);
            SystemsButton("Cycle Weapons", "Joystick", Joysticks, CJoystick.MenuButton);
            SystemsButton("Fire Countermeasures", "Throttle", ByManifest<VRThrottle, CThrottle>, CThrottle.MenuButton);

            SystemsButton("Flares Toggle", "CMSConfigUI", ByType<CMSConfigUI, CCMS>, CCMS.ToggleFlares);
            SystemsButton("Flares On", "CMSConfigUI", ByType<CMSConfigUI, CCMS>, CCMS.FlaresOn);
            SystemsButton("Flares Off", "CMSConfigUI", ByType<CMSConfigUI, CCMS>, CCMS.FlaresOff);

            SystemsButton("Chaff Toggle", "CMSConfigUI", ByType<CMSConfigUI, CCMS>, CCMS.ToggleChaff);
            SystemsButton("Chaff On", "CMSConfigUI", ByType<CMSConfigUI, CCMS>, CCMS.ChaffOn);
            SystemsButton("Chaff Off", "CMSConfigUI", ByType<CMSConfigUI, CCMS>, CCMS.ChaffOff);

            SystemsButton("Tx Cycle EM Band", "EF24Hotas", EF24Hotas, CEF24Hotas.CycleEMBand);
            SystemsButton("Tx Cycle Mode", "EF24Hotas", EF24Hotas, CEF24Hotas.CycleTxMode);
            SystemsButton("Tx Next", "EF24Hotas", EF24Hotas, CEF24Hotas.NextTx);
            SystemsButton("Tx Prev", "EF24Hotas", EF24Hotas, CEF24Hotas.PrevTx);
            SystemsButton("Tx Power Increase", "EF24Hotas", EF24Hotas, CEF24Hotas.IncreaseTxPower);
            SystemsButton("Tx Power Decrease", "EF24Hotas", EF24Hotas, CEF24Hotas.DecreaseTxPower);

            SystemsButton("Engine Left Toggle", "Engine L (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 13);
            SystemsButton("Engine Left On", "Engine L (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 13);
            SystemsButton("Engine Left Off", "Engine L (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 13);
            SystemsButton("Engine Right Toggle", "Engine R (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 15);
            SystemsButton("Engine Right On", "Engine R (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 15);
            SystemsButton("Engine Right Off", "Engine R (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 15);

            SystemsButton("Main Battery Toggle", "Main Battery", ByManifest<VRLever, CLever>, CLever.Cycle, i: 8);
            SystemsButton("Main Battery On", "Main Battery", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 8);
            SystemsButton("Main Battery Off", "Main Battery", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 8);

            SystemsButton("APU Toggle", "Auxilliary Power Unit", ByManifest<VRLever, CLever>, CLever.Cycle, i: 9);
            SystemsButton("APU On", "Auxilliary Power Unit", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 9);
            SystemsButton("APU Off", "Auxilliary Power Unit", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 9);

            SystemsButton("Radar Power Toggle", "Radar Power (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 7);
            SystemsButton("Radar Power On", "Radar Power (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 7);
            SystemsButton("Radar Power Off", "Radar Power (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 7);

            SystemsButton("RWR Mode Cycle", "RWR Mode", ByType<DashRWR, CDashRWR>, CDashRWR.Cycle);
            SystemsButton("RWR Mode Prev", "RWR Mode", ByType<DashRWR, CDashRWR>, CDashRWR.Prev);
            SystemsButton("RWR Mode Next", "RWR Mode", ByType<DashRWR, CDashRWR>, CDashRWR.Next);
            SystemsButton("RWR Mode On", "RWR Mode", ByType<DashRWR, CDashRWR>, CDashRWR.On);
            SystemsButton("RWR Mode Mute", "RWR Mode", ByType<DashRWR, CDashRWR>, CDashRWR.Silent);
            SystemsButton("RWR Mode Off", "RWR Mode", ByType<DashRWR, CDashRWR>, CDashRWR.Off);

            AddPostUpdateControl("EF24Hotas");
        }

        protected override void CreateHUDControls()
        {
            HUDButton("Helmet Visor Toggle", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.ToggleVisor, r: Seat);
            HUDButton("Helmet Visor Open", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.OpenVisor, r: Seat);
            HUDButton("Helmet Visor Closed", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.CloseVisor, r: Seat);
            HUDButton("Helmet NV Toggle", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.ToggleNightVision, r: Seat);
            HUDButton("Helmet NV On", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.EnableNightVision, r: Seat);
            HUDButton("Helmet NV Off", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.DisableNightVision, r: Seat);

            HUDButton("HUD Power Toggle", "HUD Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 0);
            HUDButton("HUD Power On", "HUD Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 0);
            HUDButton("HUD Power Off", "HUD Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 0);

            HUDAxis("HUD Tint", "HUD Tint (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 1);
            HUDButton("HUD Tint Increase", "HUD Tint (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 1);
            HUDButton("HUD Tint Decrease", "HUD Tint (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 1);

            HUDAxis("HUD Brightness", "HUD Brightness (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 0);
            HUDButton("HUD Brightness Increase", "HUD Brightness (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 0);
            HUDButton("HUD Brightness Decrease", "HUD Brightness (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 0);

            AddPostUpdateControl("HUD Brightness (Front)");
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
            
            DisplayButton("TSD Slew TGP/EOTS", "Slew TGP", TSDInteractable, CInteractable.Use, r: TSD);
            DisplayButton("TSD GPS-S", "GPS Send", TSDInteractable, CInteractable.Use, r: TSD);

            DisplayAxis("MFD Brightness", "MFD Brightness (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 5);
            DisplayButton("MFD Brightness Increase", "MFD Brightness (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 5);
            DisplayButton("MFD Brightness Decrease", "MFD Brightness (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 5);

            DisplayButton("MFD Power Toggle", "Display Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 6);
            DisplayButton("MFD Power On", "Display Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 6);
            DisplayButton("MFD Power Off", "Display Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 6);

            DisplayButton("MFD Load Layout 1", "Preset 1", ByName<MFDPortalPresetButton, CPresetButton>, CPresetButton.Load, i: 9, r: Cockpit);
            DisplayButton("MFD Save Layout 1", "Preset 1", ByName<MFDPortalPresetButton, CPresetButton>, CPresetButton.Save, i: 9, r: Cockpit);
            DisplayButton("MFD Load Layout 2", "Preset 2", ByName<MFDPortalPresetButton, CPresetButton>, CPresetButton.Load, i: 10, r: Cockpit);
            DisplayButton("MFD Save Layout 2", "Preset 2", ByName<MFDPortalPresetButton, CPresetButton>, CPresetButton.Save, i: 10, r: Cockpit);
            DisplayButton("MFD Load Layout 3", "Preset 3", ByName<MFDPortalPresetButton, CPresetButton>, CPresetButton.Load, i: 11, r: Cockpit);
            DisplayButton("MFD Save Layout 3", "Preset 3", ByName<MFDPortalPresetButton, CPresetButton>, CPresetButton.Save, i: 11, r: Cockpit);
            DisplayButton("MFD Load Layout 4", "Preset 4", ByName<MFDPortalPresetButton, CPresetButton>, CPresetButton.Load, i: 12, r: Cockpit);
            DisplayButton("MFD Save Layout 4", "Preset 4", ByName<MFDPortalPresetButton, CPresetButton>, CPresetButton.Save, i: 12, r: Cockpit);

            AddPostUpdateControl("MFD Brightness (Front)");
            AddPostUpdateControl("SOI");
        }

        protected override void CreateRadioControls()
        {
            RadioButton("Radio Transmit", "Radio", ByType<CockpitTeamRadioManager, CRadio>, CRadio.Transmit, r: Cockpit);

            RadioButton("Radio Channel Cycle", "Radio Channel (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 2);
            RadioButton("Radio Channel Team", "Radio Channel (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 2);
            RadioButton("Radio Channel Global", "Radio Channel (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 2);

            RadioButton("Radio Mode Cycle", "Radio Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 1);
            RadioButton("Radio Mode Next", "Radio Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Next, i: 1);
            RadioButton("Radio Mode Prev", "Radio Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Prev, i: 1);
            RadioButton("Radio Mode Hot Mic", "Radio Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 1);
            RadioButton("Radio Mode PTT", "Radio Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 1);
            RadioButton("Radio Mode Off", "Radio Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 2, i: 1);

            RadioAxis("Radio Volume", "Command Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 2);
            RadioButton("Radio Volume Increase", "Command Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 2);
            RadioButton("Radio Volume Decrease", "Command Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 2);

            RadioAxis("Radio Team Volume", "Team Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 3, n: true);
            RadioButton("Radio Team Volume Increase", "Team Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 3, n: true);
            RadioButton("Radio Team Volume Decrease", "Team Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 3, n: true);

            RadioButton("Intercom Toggle", "Intercom (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 3, n: true);
            RadioButton("Intercom On", "Intercom (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 3, n: true);
            RadioButton("Intercom Off", "Intercom (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 3, n: true);

            RadioAxis("Intercom Volume", "Intercom Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 4, n: true);
            RadioButton("Intercom Volume Increase", "Intercom Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 4, n: true);
            RadioButton("Intercom Volume Decrease", "Intercom Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 4, n: true);

            AddPostUpdateControl("Command Radio Volume (Front)");
            AddPostUpdateControl("Team Radio Volume (Front)");
            AddPostUpdateControl("Intercom Volume (Front)");
        }

        protected override void CreateMusicControls()
        {
            MusicButton("Music Play/Pause", "Play/Pause", ByName<VRButton, CButton>, CButton.Use);
            MusicButton("Music Next", "Next Song", ByName<VRButton, CButton>, CButton.Use);
            MusicButton("Music Previous", "Prev Song", ByName<VRButton, CButton>, CButton.Use);
            MusicAxis("Music Volume", "Radio Volume (Front)", ByName<VRTwistKnob, CKnob>, CKnob.Set);
            MusicButton("Music Volume Up", "Radio Volume (Front)", ByName<VRTwistKnob, CKnob>, CKnob.Increase);
            MusicButton("Music Volume Down", "Radio Volume (Front)", ByName<VRTwistKnob, CKnob>, CKnob.Decrease);

            AddPostUpdateControl("Radio Volume (Front)");
        }

        protected override void CreateLightsControls()
        {
            LightsButton("Interior Lights Toggle", "Interior Lights (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 2);
            LightsButton("Interior Lights On", "Interior Lights (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 2);
            LightsButton("Interior Lights Off", "Interior Lights (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 2);

            LightsAxis("Instrument Brightness", "Instrument Illumination (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 7);
            LightsButton("Instrument Brightness Increase", "Instrument Illumination (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 7);
            LightsButton("Instrument Brightness Decrease", "Instrument Illumination (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 7);

            LightsButton("Formation Lights Toggle", "Formation Lights (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 7);
            LightsButton("Formation Lights On", "Formation Lights (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 7);
            LightsButton("Formation Lights Off", "Formation Lights (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 7);

            LightsButton("Nav Lights Toggle", "Nav Lights (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 6);
            LightsButton("Nav Lights On", "Nav Lights (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 6);
            LightsButton("Nav Lights Off", "Nav Lights (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 6);

            LightsButton("Strobe Lights Toggle", "Strobe Lights", ByManifest<VRLever, CLever>, CLever.Cycle, i: 5);
            LightsButton("Strobe Lights On", "Strobe Lights", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 5);
            LightsButton("Strobe Lights Off", "Strobe Lights", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 5);

            LightsButton("Landing Lights Toggle", "Landing Lights (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 4);
            LightsButton("Landing Lights On", "Landing Lights (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 4);
            LightsButton("Landing Lights Off", "Landing Lights (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 4);

            AddPostUpdateControl("Instrument Illumination (Front)");
        }

        protected override void CreateMiscControls()
        {
            MiscButton("Canopy Toggle", "Canopy Control (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 17);
            MiscButton("Canopy Open", "Canopy Control (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 17);
            MiscButton("Canopy Close", "Canopy Control (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 17);

            MiscButton("Switch Seat", "Switch Seat (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 60, n: true);
            MiscButton("Raise Seat", "Seat Up", ByManifest<VRButton, CButton>, CButton.Use, i: 58);
            MiscButton("Lower Seat", "Seat Down", ByManifest<VRButton, CButton>, CButton.Use, i: 59);
            MiscButton("Eject", "Eject", ByType<EjectHandle, CEject>, CEject.Pull, r: Seat);

            MiscButton("Fuel Port Toggle", "Fuel Port", ByManifest<VRLever, CLever>, CLever.Cycle, i: 18);
            MiscButton("Fuel Port Open", "Fuel Port", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 18);
            MiscButton("Fuel Port Close", "Fuel Port", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 18);

            MiscButton("Jettison Execute", "Jettison Switch", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Push, i: 4);
            MiscButton("Jettison Cycle", "Jettison Switch", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 4);
            MiscButton("Jettison Next", "Jettison Switch", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Next, i: 4);
            MiscButton("Jettison Prev", "Jettison Switch", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Prev, i: 4);
            MiscButton("Jettison Mark All", "Jettison Switch", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 4);
            MiscButton("Jettison Mark Ext", "Jettison Switch", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 4);
            MiscButton("Jettison Mark Sel", "Jettison Switch", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 2, i: 4);
        }

        private CEF24Hotas EF24Hotas(string name, string root, bool nullable, int idx)
        {
            if (TryGetExistingControl<CEF24Hotas>(name, out var existingControl))
                return existingControl;
            var ef24Hotas         = FindComponent<EF24Hotas>(Vehicle);
            var setArmingAAButton = ByName<VRButton, CButton>("AA Mode", Cockpit, false, -1);
            var setArmingAGButton = ByName<VRButton, CButton>("AG Mode", Cockpit, false, -1);
            var cEF24Hotas        = new CEF24Hotas(ef24Hotas, setArmingAAButton, setArmingAGButton);
            Controls.Add(name, cEF24Hotas);
            return cEF24Hotas;
        }
        
        private CInteractable TSDInteractable(string name, string root, bool nullable, int idx)
        {
            if (TryGetExistingControl<CInteractable>(name, out var existingControl))
                return existingControl;
            var tsd           = FindComponent<MFDPTacticalSituationDisplay>(Vehicle);
            var interactable  = FindInteractable(name, tsd.GetComponentsInChildren<VRInteractable>(true));
            var cInteractable = new CInteractable(interactable);
            Controls.Add(name, cInteractable);
            return cInteractable;
        }
    }
}