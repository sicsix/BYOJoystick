using System;
using BYOJoystick.Controls;
using BYOJoystick.Managers.Base;
using VTOLVR.DLC.EW;
using VTOLVR.Multiplayer;

namespace BYOJoystick.Managers
{
    public class EF24GRearManager : Manager
    {
        public override string GameName    => "EF-24G Rear";
        public override string ShortName   => "EF24GRear";
        public override bool   IsMulticrew => true;
        public override bool   IsSeatA     => false;

        private static string Cockpit       => "PassengerOnlyObjs/RearCockpit";
        private static string Seat          => "RearSeatParent";
        private static string RightJoystick => "PassengerOnlyObjs/RearCockpit/ewoStickTilt/joyInteractable_rearRight";

        private CJoystick RightStick(string name, string root, bool nullable, int idx)
        {
            return GetJoysticksByPaths(name, RightJoystick, null);
        }

        protected override void PreMapping()
        {
            MFDPortalManagers = GetGameObject(Cockpit).GetComponentsInChildren<MFDPortalManager>(true);
            if (MFDPortalManagers[0] == null)
                throw new InvalidOperationException("MFDPortalManager not found");
            PortalSOISwitcher = GetGameObject(Cockpit).GetComponentInChildren<MultiPortalSOISwitcher>(true);
        }

        protected override void CreateFlightControls()
        {
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

            NavButton("A/P Alt Increase", "Alt +", ByManifest<VRButton, CButton>, CButton.Use, i: 66);
            NavButton("A/P Alt Decrease", "Alt -", ByManifest<VRButton, CButton>, CButton.Use, i: 67);

            NavButton("A/P Hdg Right", "Heading Right", ByManifest<VRButton, CButton>, CButton.Use, i: 68);
            NavButton("A/P Hdg Left", "Heading Left", ByManifest<VRButton, CButton>, CButton.Use, i: 69);

            NavButton("A/P Crs Right", "Course Right", ByManifest<VRButton, CButton>, CButton.Use, i: 41);
            NavButton("A/P Crs Left", "Course Left", ByManifest<VRButton, CButton>, CButton.Use, i: 42);

            NavButton("Altitude Mode Toggle", "Toggle Altitude Mode", ByType<VehicleMaster, CVehicleMaster>, CVehicleMaster.ToggleAltMode);
            NavButton("Clear Waypoint", "Clear Waypoint", ByManifest<VRButton, CButton>, CButton.Use, i: 61);
        }

        protected override void CreateSystemsControls()
        {
            SystemsButton("Clear Cautions", "Dismiss", ByName<VRButton, CButton>, CButton.Use, r: Cockpit);

            SystemsButton("Master Arm Toggle", "Master Arm (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 73);
            SystemsButton("Master Safe Toggle", "Master Safe (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 74);

            SystemsButton("Master Mode Toggle", "Master Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 12);
            SystemsButton("Master Mode WPN", "Master Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 12);
            SystemsButton("Master Mode EW", "Master Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, 1, i: 12);

            SystemsButton("Arming Mode Toggle", "EF24Hotas", EF24Hotas, CEF24Hotas.ToggleArmingMode);
            SystemsButton("Arming Mode AA", "EF24Hotas", EF24Hotas, CEF24Hotas.ArmingAA);
            SystemsButton("Arming Mode AG", "EF24Hotas", EF24Hotas, CEF24Hotas.ArmingAG);

            SystemsButton("Fire Weapon", "Joystick", RightStick, CJoystick.Trigger);
            SystemsButton("Cycle Weapons", "Joystick", RightStick, CJoystick.MenuButton);

            SystemsButton("Tx Cycle EM Band", "EF24Hotas", EF24Hotas, CEF24Hotas.CycleEMBand);
            SystemsButton("Tx Cycle Mode", "EF24Hotas", EF24Hotas, CEF24Hotas.CycleTxMode);
            SystemsButton("Tx Next", "EF24Hotas", EF24Hotas, CEF24Hotas.NextTx);
            SystemsButton("Tx Prev", "EF24Hotas", EF24Hotas, CEF24Hotas.PrevTx);
            SystemsButton("Tx Power Increase", "EF24Hotas", EF24Hotas, CEF24Hotas.IncreaseTxPower);
            SystemsButton("Tx Power Decrease", "EF24Hotas", EF24Hotas, CEF24Hotas.DecreaseTxPower);

            SystemsButton("Radar Power Toggle", "Radar Power (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 10);
            SystemsButton("Radar Power On", "Radar Power (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 10);
            SystemsButton("Radar Power Off", "Radar Power (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 10);

            SystemsButton("RWR Mode Cycle", "RWR Mode", ByType<DashRWR, CDashRWR>, CDashRWR.Cycle);
            SystemsButton("RWR Mode Prev", "RWR Mode", ByType<DashRWR, CDashRWR>, CDashRWR.Prev);
            SystemsButton("RWR Mode Next", "RWR Mode", ByType<DashRWR, CDashRWR>, CDashRWR.Next);
            SystemsButton("RWR Mode On", "RWR Mode", ByType<DashRWR, CDashRWR>, CDashRWR.On);
            SystemsButton("RWR Mode Mute", "RWR Mode", ByType<DashRWR, CDashRWR>, CDashRWR.Silent);
            SystemsButton("RWR Mode Off", "RWR Mode", ByType<DashRWR, CDashRWR>, CDashRWR.Off);

            AddPostUpdateControl("Joystick");
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

            HUDButton("HMCS Power Toggle", "HMCS Power (Rear)", ByName<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle);
            HUDButton("HMCS Power On", "HMCS Power (Rear)", ByName<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1);
            HUDButton("HMCS Power Off", "HMCS Power (Rear)", ByName<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0);
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

            DisplayButton("TSD Slew TGP/EOTS", "Slew TGP", TSDInteractable, CInteractable.Use);
            DisplayButton("TSD GPS-S", "GPS Send", TSDInteractable, CInteractable.Use);

            DisplayAxis("MFD Brightness", "MFD Brightness (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 8);
            DisplayButton("MFD Brightness Increase", "MFD Brightness (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 8);
            DisplayButton("MFD Brightness Decrease", "MFD Brightness (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 8);

            DisplayButton("MFD Power Toggle", "Display Power (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 9);
            DisplayButton("MFD Power On", "Display Power (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 9);
            DisplayButton("MFD Power Off", "Display Power (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 9);

            DisplayButton("MFD Load Layout 1", "Preset 1", ByName<MFDPortalPresetButton, CPresetButton>, CPresetButton.Load, i: 62, r: Cockpit);
            DisplayButton("MFD Save Layout 1", "Preset 1", ByName<MFDPortalPresetButton, CPresetButton>, CPresetButton.Save, i: 62, r: Cockpit);
            DisplayButton("MFD Load Layout 2", "Preset 2", ByName<MFDPortalPresetButton, CPresetButton>, CPresetButton.Load, i: 63, r: Cockpit);
            DisplayButton("MFD Save Layout 2", "Preset 2", ByName<MFDPortalPresetButton, CPresetButton>, CPresetButton.Save, i: 63, r: Cockpit);
            DisplayButton("MFD Load Layout 3", "Preset 3", ByName<MFDPortalPresetButton, CPresetButton>, CPresetButton.Load, i: 64, r: Cockpit);
            DisplayButton("MFD Save Layout 3", "Preset 3", ByName<MFDPortalPresetButton, CPresetButton>, CPresetButton.Save, i: 64, r: Cockpit);
            DisplayButton("MFD Load Layout 4", "Preset 4", ByName<MFDPortalPresetButton, CPresetButton>, CPresetButton.Load, i: 65, r: Cockpit);
            DisplayButton("MFD Save Layout 4", "Preset 4", ByName<MFDPortalPresetButton, CPresetButton>, CPresetButton.Save, i: 65, r: Cockpit);

            AddPostUpdateControl("MFD Brightness (Rear)");
            AddPostUpdateControl("SOI");
        }

        protected override void CreateRadioControls()
        {
            RadioButton("Radio Transmit", "Radio", ByType<CockpitTeamRadioManager, CRadio>, CRadio.Transmit, r: Cockpit);

            RadioButton("Radio Channel Cycle", "Radio Channel (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 14);
            RadioButton("Radio Channel Team", "Radio Channel (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 14);
            RadioButton("Radio Channel Global", "Radio Channel (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 14);

            RadioButton("Radio Mode Cycle", "Radio Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 13);
            RadioButton("Radio Mode Next", "Radio Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Next, i: 13);
            RadioButton("Radio Mode Prev", "Radio Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Prev, i: 13);
            RadioButton("Radio Mode Hot Mic", "Radio Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 13);
            RadioButton("Radio Mode PTT", "Radio Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 13);
            RadioButton("Radio Mode Off", "Radio Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 2, i: 13);

            RadioAxis("Radio Volume", "Command Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 10);
            RadioButton("Radio Volume Increase", "Command Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 10);
            RadioButton("Radio Volume Decrease", "Command Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 10);

            RadioAxis("Radio Team Volume", "Team Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 11, n: true);
            RadioButton("Radio Team Volume Increase", "Team Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 11, n: true);
            RadioButton("Radio Team Volume Decrease", "Team Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 11, n: true);

            RadioButton("Intercom Toggle", "Intercom (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 15, n: true);
            RadioButton("Intercom On", "Intercom (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 15, n: true);
            RadioButton("Intercom Off", "Intercom (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 15, n: true);

            RadioAxis("Intercom Volume", "Intercom Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 12, n: true);
            RadioButton("Intercom Volume Increase", "Intercom Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 12, n: true);
            RadioButton("Intercom Volume Decrease", "Intercom Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 12, n: true);

            AddPostUpdateControl("Command Radio Volume (Front)");
            AddPostUpdateControl("Team Radio Volume (Front)");
            AddPostUpdateControl("Intercom Volume (Front)");
        }

        protected override void CreateMusicControls()
        {
        }

        protected override void CreateLightsControls()
        {
            LightsButton("Interior Lights Toggle", "Interior Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 21);
            LightsButton("Interior Lights On", "Interior Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 21);
            LightsButton("Interior Lights Off", "Interior Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 21);

            LightsAxis("Instrument Brightness", "Instrument Illumination (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 9);
            LightsButton("Instrument Brightness Increase", "Instrument Illumination (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 9);
            LightsButton("Instrument Brightness Decrease", "Instrument Illumination (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 9);

            LightsButton("Formation Lights Toggle", "Formation Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 26);
            LightsButton("Formation Lights On", "Formation Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 26);
            LightsButton("Formation Lights Off", "Formation Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 26);

            LightsButton("Nav Lights Toggle", "Nav Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 25);
            LightsButton("Nav Lights On", "Nav Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 25);
            LightsButton("Nav Lights Off", "Nav Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 25);

            LightsButton("Strobe Lights Toggle", "Strobe Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 24);
            LightsButton("Strobe Lights On", "Strobe Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 24);
            LightsButton("Strobe Lights Off", "Strobe Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 24);

            LightsButton("Landing Lights Toggle", "Landing Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 23);
            LightsButton("Landing Lights On", "Landing Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 23);
            LightsButton("Landing Lights Off", "Landing Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 23);

            AddPostUpdateControl("Instrument Illumination (Rear)");
        }

        protected override void CreateMiscControls()
        {
            MiscButton("Canopy Toggle", "Canopy Control (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 27);
            MiscButton("Canopy Open", "Canopy Control (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 27);
            MiscButton("Canopy Close", "Canopy Control (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 27);

            MiscButton("Switch Seat", "Switch Seat (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 72, n: true);
            MiscButton("Raise Seat", "Seat Up", ByManifest<VRButton, CButton>, CButton.Use, i: 87);
            MiscButton("Lower Seat", "Seat Down", ByManifest<VRButton, CButton>, CButton.Use, i: 88);
            MiscButton("Eject", "Eject", ByType<EjectHandle, CEject>, CEject.Pull, r: Seat);

            MiscButton("Jettison Execute", "Jettison Switch", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Push, i: 11);
            MiscButton("Jettison Cycle", "Jettison Switch", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 11);
            MiscButton("Jettison Next", "Jettison Switch", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Next, i: 11);
            MiscButton("Jettison Prev", "Jettison Switch", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Prev, i: 11);
            MiscButton("Jettison Mark All", "Jettison Switch", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 11);
            MiscButton("Jettison Mark Ext", "Jettison Switch", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 11);
            MiscButton("Jettison Mark Sel", "Jettison Switch", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 2, i: 11);
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