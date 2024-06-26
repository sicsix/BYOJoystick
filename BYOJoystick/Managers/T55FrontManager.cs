using BYOJoystick.Controls;
using BYOJoystick.Managers.Base;
using VTOLVR.Multiplayer;
using MFDButtons = MFD.MFDButtons;

namespace BYOJoystick.Managers
{
    public class T55FrontManager : Manager
    {
        public override string GameName    => "T-55 Front";
        public override string ShortName   => "T55Front";
        public override bool   IsMulticrew => true;
        public override bool   IsSeatA     => true;

        private static string Dash           => "PassengerOnlyObjs/DashCanvasFront";
        private static string Seat           => "FrontSeatParent";
        private static string Switcher       => "PassengerOnlyObjs/PilotSwitcher";
        private static string MMFDManager    => "PassengerOnlyObjs/DashCanvasFront/Dash/MiniMFDLeft";
        private static string SideJoystick   => "PassengerOnlyObjs/SideStickObjects (Front)";
        private static string CenterJoystick => "PassengerOnlyObjs/CenterStickObjects (Front)";

        private CJoystick Joysticks(string name, string root, bool nullable, int idx)
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

            FlightAxis("Throttle", "Throttle", ByManifest<VRThrottle, CThrottle>, CThrottle.Set);
            FlightButton("Throttle Increase", "Throttle", ByManifest<VRThrottle, CThrottle>, CThrottle.Increase);
            FlightButton("Throttle Decrease", "Throttle", ByManifest<VRThrottle, CThrottle>, CThrottle.Decrease);

            FlightAxis("Brakes/Airbrakes Axis", "Throttle", ByManifest<VRThrottle, CThrottle>, CThrottle.Trigger);
            FlightButton("Brakes/Airbrakes", "Throttle", ByManifest<VRThrottle, CThrottle>, CThrottle.Trigger);

            FlightButton("Flaps Cycle", "Flaps (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 9);
            FlightButton("Flaps Increase", "Flaps (Front)", ByManifest<VRLever, CLever>, CLever.Next, i: 9);
            FlightButton("Flaps Decrease", "Flaps (Front)", ByManifest<VRLever, CLever>, CLever.Prev, i: 9);
            FlightButton("Flaps 1", "Flaps (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 9);
            FlightButton("Flaps 2", "Flaps (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 9);
            FlightButton("Flaps 3", "Flaps (Front)", ByManifest<VRLever, CLever>, CLever.Set, 2, i: 9);

            FlightButton("Landing Gear Toggle", "Landing Gear (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 8);
            FlightButton("Landing Gear Up", "Landing Gear (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 8);
            FlightButton("Landing Gear Down", "Landing Gear (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 8);

            FlightButton("Brake Lock Toggle", "Brake Locks (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 22);
            FlightButton("Brake Lock On", "Brake Locks (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 22);
            FlightButton("Brake Lock Off", "Brake Locks (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 22);

            FlightButton("Wing Fold Toggle", "Wing Fold (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 21);
            FlightButton("Wing Fold Up", "Wing Fold (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 21);
            FlightButton("Wing Fold Down", "Wing Fold (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 21);

            FlightButton("Launch Bar Toggle", "Launch Bar (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 11);
            FlightButton("Launch Bar Extend", "Launch Bar (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 11);
            FlightButton("Launch Bar Retract", "Launch Bar (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 11);

            FlightButton("Arrestor Hook Toggle", "Arrestor Hook (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 10);
            FlightButton("Arrestor Hook Extend", "Arrestor Hook (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 10);
            FlightButton("Arrestor Hook Retract", "Arrestor Hook (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 10);

            FlightButton("Catapult T/O Trim Toggle", "Catapult T/O Trim (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 28);
            FlightButton("Catapult T/O Trim On", "Catapult T/O Trim (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 28);
            FlightButton("Catapult T/O Trim Off", "Catapult T/O Trim (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 28);

            AddPostUpdateControl("Joystick");
            AddPostUpdateControl("Throttle");
        }

        protected override void CreateAssistControls()
        {
        }

        protected override void CreateNavigationControls()
        {
            NavButton("A/P Nav Mode", "Navigation Mode (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 3);
            NavButton("A/P Spd Hold", "Airspeed Hold (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 5);
            NavButton("A/P Hdg Hold", "Heading Hold (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 7);
            NavButton("A/P Alt Hold", "Altitude Hold (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 6);
            NavButton("A/P Off", "All AP Off (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 4);

            NavButton("A/P Alt Increase", "Adjust AP Altitude (Front)", ByType<APKnobAltAdjust, CAltAdjust>, CAltAdjust.Increase, r: Dash);
            NavButton("A/P Alt Decrease", "Adjust AP Altitude (Front)", ByType<APKnobAltAdjust, CAltAdjust>, CAltAdjust.Decrease, r: Dash);

            NavButton("A/P Hdg Right", "AP Heading Set (Front)", ByType<DashHSI, CHSI>, CHSI.HeadingRight, r: Dash);
            NavButton("A/P Hdg Left", "AP Heading Set (Front)", ByType<DashHSI, CHSI>, CHSI.HeadingLeft, r: Dash);

            NavButton("A/P Spd Increase", "Auto Speed Set (Front)", ByType<APKnobSpeedDialAdjust, CSpdAdjust>, CSpdAdjust.Increase, r: Dash);
            NavButton("A/P Spd Decrease", "Auto Speed Set (Front)", ByType<APKnobSpeedDialAdjust, CSpdAdjust>, CSpdAdjust.Decrease, r: Dash);

            NavButton("A/P Crs Right", "Course Set (Front)", ByType<DashHSI, CHSI>, CHSI.CourseRight, r: Dash);
            NavButton("A/P Crs Left", "Course Set (Front)", ByType<DashHSI, CHSI>, CHSI.CourseLeft, r: Dash);

            NavButton("Altitude Mode Toggle", "Toggle Altitude Mode (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 1);
            NavButton("Clear Waypoint", "Clear Waypoint (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 2);

            AddPostUpdateControl("Adjust AP Altitude (Front)");
            AddPostUpdateControl("AP Heading Set (Front)");
            AddPostUpdateControl("Auto Speed Set (Front)");
            AddPostUpdateControl("Course Set (Front)");
        }

        protected override void CreateSystemsControls()
        {
            SystemsButton("Clear Cautions", "Clear Cautions (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 0);

            SystemsButton("Master Arm Toggle", "Master Arm (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 58);
            SystemsButton("Master Safe Toggle", "Master Safe (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 59);

            SystemsButton("Fire Weapon", "Joystick", Joysticks, CJoystick.Trigger);
            SystemsButton("Cycle Weapons", "Joystick", Joysticks, CJoystick.MenuButton);
            SystemsButton("Fire Countermeasures", "Throttle", ByManifest<VRThrottle, CThrottle>, CThrottle.MenuButton);

            SystemsButton("Flares Toggle", "Toggle Flares (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 24);
            SystemsButton("Flares On", "Toggle Flares (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 24);
            SystemsButton("Flares Off", "Toggle Flares (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 24);

            SystemsButton("Chaff Toggle", "Toggle Chaff (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 23);
            SystemsButton("Chaff On", "Toggle Chaff (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 23);
            SystemsButton("Chaff Off", "Toggle Chaff (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 23);

            SystemsButton("Engine Toggle", "Engine (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 2);
            SystemsButton("Engine On", "Engine (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 2);
            SystemsButton("Engine Off", "Engine (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 2);

            SystemsButton("Main Battery Toggle", "Main Battery", ByManifest<VRLever, CLever>, CLever.Cycle, i: 6);
            SystemsButton("Main Battery On", "Main Battery", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 6);
            SystemsButton("Main Battery Off", "Main Battery", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 6);

            SystemsButton("APU Toggle", "Auxilliary Power (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 4);
            SystemsButton("APU On", "Auxilliary Power (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 4);
            SystemsButton("APU Off", "Auxilliary Power (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 4);

            SystemsButton("Radar Power Toggle", "Radar Power (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 3);
            SystemsButton("Radar Power On", "Radar Power (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 3);
            SystemsButton("Radar Power Off", "Radar Power (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 3);

            SystemsButton("RWR Mode Cycle", "RWR Mode (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 4);
            SystemsButton("RWR Mode Prev", "RWR Mode (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Prev, i: 4);
            SystemsButton("RWR Mode Next", "RWR Mode (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Next, i: 4);
            SystemsButton("RWR Mode On", "RWR Mode (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 4);
            SystemsButton("RWR Mode Mute", "RWR Mode (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 4);
            SystemsButton("RWR Mode Off", "RWR Mode (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 2, i: 4);
        }

        protected override void CreateHUDControls()
        {
            HUDButton("Helmet Visor Toggle", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.ToggleVisor, r: Seat);
            HUDButton("Helmet Visor Open", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.OpenVisor, r: Seat);
            HUDButton("Helmet Visor Closed", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.CloseVisor, r: Seat);
            HUDButton("Helmet NV Toggle", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.ToggleNightVision, r: Seat);
            HUDButton("Helmet NV On", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.EnableNightVision, r: Seat);
            HUDButton("Helmet NV Off", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.DisableNightVision, r: Seat);

            HUDButton("HMCS Power Toggle", "HMCS Power (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 1);
            HUDButton("HMCS Power On", "HMCS Power (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 1);
            HUDButton("HMCS Power Off", "HMCS Power (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 1);

            HUDButton("HUD Power Toggle", "HUD Power (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 0);
            HUDButton("HUD Power On", "HUD Power (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 0);
            HUDButton("HUD Power Off", "HUD Power (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 0);

            HUDAxis("HUD Tint", "HUD Tint (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 0);
            HUDButton("HUD Tint Increase", "HUD Tint (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 0);
            HUDButton("HUD Tint Decrease", "HUD Tint (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 0);

            HUDAxis("HUD Brightness", "HUD Brightness (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 1);
            HUDButton("HUD Brightness Increase", "HUD Brightness (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 1);
            HUDButton("HUD Brightness Decrease", "HUD Brightness (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 1);

            AddPostUpdateControl("HUD Tint (Front)");
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

            DisplayButton("MMFD Left Toggle", "MMFD Left", MMFD, CMMFD.PowerToggle, i: 0, r: MMFDManager);
            DisplayButton("MMFD Left On", "MMFD Left", MMFD, CMMFD.PowerOn, i: 0, r: MMFDManager);
            DisplayButton("MMFD Left Off", "MMFD Left", MMFD, CMMFD.PowerOff, i: 0, r: MMFDManager);
            DisplayButton("MMFD Left RWR Page", "RWR (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 13);

            DisplayButton("MMFD Right Toggle", "MMFD Right", MMFD, CMMFD.PowerToggle, i: 1, r: MMFDManager);
            DisplayButton("MMFD Right On", "MMFD Right", MMFD, CMMFD.PowerOn, i: 1, r: MMFDManager);
            DisplayButton("MMFD Right Off", "MMFD Right", MMFD, CMMFD.PowerOff, i: 1, r: MMFDManager);
            DisplayButton("MMFD Right Info Page", "Info (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 17);
            DisplayButton("MMFD Right Fuel Page", "Fuel (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 16);

            DisplayAxis("MFD Brightness", "MFD Brightness (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 3);
            DisplayButton("MFD Brightness Increase", "MFD Brightness (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 3);
            DisplayButton("MFD Brightness Decrease", "MFD Brightness (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 3);

            DisplayButton("MFD Swap", "Swap MFDs (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 60);

            DisplayButton("MFD Left Toggle", "MFD Left", MFD, CMFD.PowerToggle, i: 0);
            DisplayButton("MFD Left On", "MFD Left", MFD, CMFD.PowerOn, i: 0);
            DisplayButton("MFD Left Off", "MFD Left", MFD, CMFD.PowerOff, i: 0);
            DisplayButton("MFD Left L1", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.L1, i: 0);
            DisplayButton("MFD Left L2", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.L2, i: 0);
            DisplayButton("MFD Left L3", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.L3, i: 0);
            DisplayButton("MFD Left L4", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.L4, i: 0);
            DisplayButton("MFD Left L5", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.L5, i: 0);
            DisplayButton("MFD Left R1", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.R1, i: 0);
            DisplayButton("MFD Left R2", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.R2, i: 0);
            DisplayButton("MFD Left R3", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.R3, i: 0);
            DisplayButton("MFD Left R4", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.R4, i: 0);
            DisplayButton("MFD Left R5", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.R5, i: 0);
            DisplayButton("MFD Left T1", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.T1Home, i: 0);
            DisplayButton("MFD Left T2", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.T2, i: 0);
            DisplayButton("MFD Left T3", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.T3, i: 0);
            DisplayButton("MFD Left T4", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.T4, i: 0);
            DisplayButton("MFD Left T5", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.T5, i: 0);
            DisplayButton("MFD Left B1", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.B1, i: 0);
            DisplayButton("MFD Left B2", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.B2, i: 0);
            DisplayButton("MFD Left B3", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.B3, i: 0);
            DisplayButton("MFD Left B4", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.B4, i: 0);
            DisplayButton("MFD Left B5", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.B5, i: 0);

            DisplayButton("MFD Right Toggle", "MFD Right", MFD, CMFD.PowerToggle, i: 1);
            DisplayButton("MFD Right On", "MFD Right", MFD, CMFD.PowerOn, i: 1);
            DisplayButton("MFD Right Off", "MFD Right", MFD, CMFD.PowerOff, i: 1);
            DisplayButton("MFD Right L1", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.L1, i: 1);
            DisplayButton("MFD Right L2", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.L2, i: 1);
            DisplayButton("MFD Right L3", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.L3, i: 1);
            DisplayButton("MFD Right L4", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.L4, i: 1);
            DisplayButton("MFD Right L5", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.L5, i: 1);
            DisplayButton("MFD Right R1", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.R1, i: 1);
            DisplayButton("MFD Right R2", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.R2, i: 1);
            DisplayButton("MFD Right R3", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.R3, i: 1);
            DisplayButton("MFD Right R4", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.R4, i: 1);
            DisplayButton("MFD Right R5", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.R5, i: 1);
            DisplayButton("MFD Right T1", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.T1Home, i: 1);
            DisplayButton("MFD Right T2", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.T2, i: 1);
            DisplayButton("MFD Right T3", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.T3, i: 1);
            DisplayButton("MFD Right T4", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.T4, i: 1);
            DisplayButton("MFD Right T5", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.T5, i: 1);
            DisplayButton("MFD Right B1", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.B1, i: 1);
            DisplayButton("MFD Right B2", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.B2, i: 1);
            DisplayButton("MFD Right B3", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.B3, i: 1);
            DisplayButton("MFD Right B4", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.B4, i: 1);
            DisplayButton("MFD Right B5", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.B5, i: 1);

            AddPostUpdateControl("MFD Brightness (Front)");
            AddPostUpdateControl("SOI");
        }

        protected override void CreateRadioControls()
        {
            RadioButton("Radio Transmit", "Radio", ByType<CockpitTeamRadioManager, CRadio>, CRadio.Transmit, r: Dash);

            RadioButton("Radio Channel Cycle", "Radio Channel (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 6);
            RadioButton("Radio Channel Team", "Radio Channel (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 6);
            RadioButton("Radio Channel Global", "Radio Channel (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 6);

            RadioButton("Radio Mode Cycle", "Radio Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 5);
            RadioButton("Radio Mode Next", "Radio Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Next, i: 5);
            RadioButton("Radio Mode Prev", "Radio Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Prev, i: 5);
            RadioButton("Radio Mode Hot Mic", "Radio Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 5);
            RadioButton("Radio Mode PTT", "Radio Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 5);
            RadioButton("Radio Mode Off", "Radio Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 2, i: 5);

            RadioAxis("Radio Volume", "Comm Radio Volume (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 8);
            RadioButton("Radio Volume Increase", "Comm Radio Volume (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 8);
            RadioButton("Radio Volume Decrease", "Comm Radio Volume (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 8);

            RadioAxis("Radio Team Volume", "Team Radio Volume (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 9, n: true);
            RadioButton("Radio Team Volume Increase", "Team Radio Volume (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 9, n: true);
            RadioButton("Radio Team Volume Decrease", "Team Radio Volume (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 9, n: true);

            RadioButton("Intercom Toggle", "Intra Comms (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 7, n: true);
            RadioButton("Intercom On", "Intra Comms (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 7, n: true);
            RadioButton("Intercom Off", "Intra Comms (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 7, n: true);

            RadioAxis("Intercom Volume", "Intra Comm Volume (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 10, n: true);
            RadioButton("Intercom Volume Increase", "Intra Comm Volume (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 10, n: true);
            RadioButton("Intercom Volume Decrease", "Intra Comm Volume (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 10, n: true);

            AddPostUpdateControl("Comm Radio Volume (Rear)");
            AddPostUpdateControl("Team Radio Volume (Rear)");
            AddPostUpdateControl("Intra Comm Volume (Rear)");
        }

        protected override void CreateMusicControls()
        {
            MusicButton("Music Play/Pause", "Play/Pause", ByName<VRButton, CButton>, CButton.Use, r: Dash);
            MusicButton("Music Next", "Next Song", ByName<VRButton, CButton>, CButton.Use, r: Dash);
            MusicButton("Music Previous", "Prev Song", ByName<VRButton, CButton>, CButton.Use, r: Dash);
            MusicAxis("Music Volume", "Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 11);
            MusicButton("Music Volume Up", "Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 11);
            MusicButton("Music Volume Down", "Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 11);

            AddPostUpdateControl("Radio Volume (Front)");
        }

        protected override void CreateLightsControls()
        {
            LightsButton("Interior Lights Toggle", "Interior Lights (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 13);
            LightsButton("Interior Lights On", "Interior Lights (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 13);
            LightsButton("Interior Lights Off", "Interior Lights (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 13);

            LightsButton("Instrument Lights Toggle", "Instrument Lights (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 14);
            LightsButton("Instrument Lights On", "Instrument Lights (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 14);
            LightsButton("Instrument Lights Off", "Instrument Lights (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 14);
            LightsAxis("Instrument Brightness", "Instrument Brightness (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 2);
            LightsButton("Instrument Brightness Increase", "Instrument Brightness (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 2);
            LightsButton("Instrument Brightness Decrease", "Instrument Brightness (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 2);

            LightsButton("Nav Lights Toggle", "Nav Lights (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 17);
            LightsButton("Nav Lights On", "Nav Lights (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 17);
            LightsButton("Nav Lights Off", "Nav Lights (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 17);

            LightsButton("Strobe Lights Toggle", "Strobe Lights", ByManifest<VRLever, CLever>, CLever.Cycle, i: 16);
            LightsButton("Strobe Lights On", "Strobe Lights", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 16);
            LightsButton("Strobe Lights Off", "Strobe Lights", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 16);

            LightsButton("Landing Lights Toggle", "Landing Lights (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 15);
            LightsButton("Landing Lights On", "Landing Lights (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 15);
            LightsButton("Landing Lights Off", "Landing Lights (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 15);

            AddPostUpdateControl("Instrument Brightness (Front)");
        }

        protected override void CreateMiscControls()
        {
            MiscButton("Canopy Toggle", "Canopy (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 7);
            MiscButton("Canopy Open", "Canopy (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 7);
            MiscButton("Canopy Close", "Canopy (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 7);

            MiscButton("Switch Seat", "Switch Seat (Rear)", ByType<VRButton, CButton>, CButton.Use, n: true, r: Switcher);
            MiscButton("Raise Seat", "Raise Seat", ByName<VRButton, CButton>, CButton.Use, r: Dash);
            MiscButton("Lower Seat", "Lower Seat", ByName<VRButton, CButton>, CButton.Use, r: Dash);
            MiscButton("Eject", "Eject", ByType<EjectHandle, CEject>, CEject.Pull, r: Seat);

            MiscButton("Fuel Port Toggle", "Fuel Port (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 25);
            MiscButton("Fuel Port Open", "Fuel Port (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 25);
            MiscButton("Fuel Port Close", "Fuel Port (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 25);

            MiscButton("Fuel Dump Toggle", "Fuel Dump Switch (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 26);
            MiscButton("Fuel Dump On", "Fuel Dump Switch (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 26);
            MiscButton("Fuel Dump Off", "Fuel Dump Switch (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 26);

            MiscButton("Jettison Execute", "Jettison (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 8);
            MiscButton("Jettison Mark All", "Jettison All (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 9);
            MiscButton("Jettison Mark Empty", "Jettison Empty (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 10);
            MiscButton("Jettison Mark Ext. Tanks", "Jettison Ext Tanks (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 12);
            MiscButton("Jettison Clear Marks", "Clear Jettison Marks (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 11);
        }
    }
}