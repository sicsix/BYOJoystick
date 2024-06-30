using BYOJoystick.Controls;
using BYOJoystick.Managers.Base;
using VTOLVR.Multiplayer;
using MFDButtons = MFD.MFDButtons;

namespace BYOJoystick.Managers
{
    public class T55RearManager : Manager
    {
        public override string GameName    => "T-55 Rear";
        public override string ShortName   => "T55Rear";
        public override bool   IsMulticrew => true;
        public override bool   IsSeatA     => false;

        private static string Dash           => "PassengerOnlyObjs/DashCanvasRear";
        private static string Seat           => "RearSeatParent";
        private static string Switcher       => "PassengerOnlyObjs/PilotSwitcher (1)";
        private static string Throttle       => "PassengerOnlyObjs/ThrottleParent Rear";
        private static string MMFDManager    => "PassengerOnlyObjs/DashCanvasRear/Dash/MiniMFDLeft";
        private static string SideJoystick   => "PassengerOnlyObjs/SideStickObjects (Rear)";
        private static string CenterJoystick => "PassengerOnlyObjs/CenterStickObjects (Rear)";

        private CJoystick Joysticks(string name, string root, bool nullable, bool checkName, int idx)
        {
            return GetJoysticksByPaths(name, SideJoystick, CenterJoystick);
        }

        protected override void PreMapping()
        {
        }

        protected override void CreateFlightControls()
        {
            FlightAxisC("Joystick Pitch", "Joystick (Rear)", Joysticks, CJoystick.SetPitch);
            FlightAxisC("Joystick Yaw", "Joystick (Rear)", Joysticks, CJoystick.SetYaw);
            FlightAxisC("Joystick Roll", "Joystick (Rear)", Joysticks, CJoystick.SetRoll);

            FlightAxis("Throttle", "Throttle", ByType<VRThrottle, CThrottle>, CThrottle.Set, r: Throttle);
            FlightButton("Throttle Increase", "Throttle", ByType<VRThrottle, CThrottle>, CThrottle.Increase, r: Throttle);
            FlightButton("Throttle Decrease", "Throttle", ByType<VRThrottle, CThrottle>, CThrottle.Decrease, r: Throttle);

            FlightAxis("Brakes/Airbrakes Axis", "Throttle", ByType<VRThrottle, CThrottle>, CThrottle.Trigger, r: Throttle);
            FlightButton("Brakes/Airbrakes", "Throttle", ByType<VRThrottle, CThrottle>, CThrottle.Trigger, r: Throttle);

            FlightButton("Flaps Cycle", "Flaps (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 44);
            FlightButton("Flaps Increase", "Flaps (Rear)", ByManifest<VRLever, CLever>, CLever.Next, i: 44);
            FlightButton("Flaps Decrease", "Flaps (Rear)", ByManifest<VRLever, CLever>, CLever.Prev, i: 44);
            FlightButton("Flaps 1", "Flaps (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 44);
            FlightButton("Flaps 2", "Flaps (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 44);
            FlightButton("Flaps 3", "Flaps (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 2, i: 44);

            FlightButton("Landing Gear Toggle", "Landing Gear (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 43);
            FlightButton("Landing Gear Up", "Landing Gear (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 43);
            FlightButton("Landing Gear Down", "Landing Gear (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 43);

            FlightButton("Brake Lock Toggle", "Brake Locks (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 57);
            FlightButton("Brake Lock On", "Brake Locks (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 57);
            FlightButton("Brake Lock Off", "Brake Locks (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 57);

            FlightButton("Wing Fold Toggle", "Wing Fold (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 56);
            FlightButton("Wing Fold Up", "Wing Fold (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 56);
            FlightButton("Wing Fold Down", "Wing Fold (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 56);

            FlightButton("Launch Bar Toggle", "Launch Bar (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 46);
            FlightButton("Launch Bar Extend", "Launch Bar (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 46);
            FlightButton("Launch Bar Retract", "Launch Bar (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 46);

            FlightButton("Arrestor Hook Toggle", "Arrestor Hook (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 45);
            FlightButton("Arrestor Hook Extend", "Arrestor Hook (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 45);
            FlightButton("Arrestor Hook Retract", "Arrestor Hook (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 45);

            FlightButton("Catapult T/O Trim Toggle", "Catapult T/O Trim (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 61);
            FlightButton("Catapult T/O Trim On", "Catapult T/O Trim (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 61);
            FlightButton("Catapult T/O Trim Off", "Catapult T/O Trim (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 61);

            FlightButton("Control Override Mode Cycle", "Control Override Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 9, n: true);
            FlightButton("Control Override Mode Prev", "Control Override Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Prev, i: 9, n: true);
            FlightButton("Control Override Mode Next", "Control Override Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Next, i: 9, n: true);
            FlightButton("Control Override Mode Lock", "Control Override Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 9, n: true);
            FlightButton("Control Override Mode Norm", "Control Override Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 9, n: true);
            FlightButton("Control Override Mode Ovrd", "Control Override Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 2, i: 9, n: true);

            AddPostUpdateControl("Joystick (Rear)");
            AddPostUpdateControl("Throttle");
        }

        protected override void CreateAssistControls()
        {
            AssistButton("Master Toggle", "Toggle Flight Assist (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 62);
            AssistButton("Master On", "Toggle Flight Assist (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 62);
            AssistButton("Master Off", "Toggle Flight Assist (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 62);

            AssistButton("SAS Roll Toggle", "Toggle Roll Assist (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 65);
            AssistButton("SAS Roll On", "Toggle Roll Assist (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 65);
            AssistButton("SAS Roll Off", "Toggle Roll Assist (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 65);

            AssistButton("SAS Yaw Toggle", "Toggle Yaw Assist (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 64);
            AssistButton("SAS Yaw On", "Toggle Yaw Assist (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 64);
            AssistButton("SAS Yaw Off", "Toggle Yaw Assist (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 64);

            AssistButton("SAS Pitch Toggle", "Toggle Pitch Assist (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 63);
            AssistButton("SAS Pitch On", "Toggle Pitch Assist (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 63);
            AssistButton("SAS Pitch Off", "Toggle Pitch Assist (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 63);

            AssistButton("G-Limiter Toggle", "Toggle G-Limiter (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 66);
            AssistButton("G-Limiter On", "Toggle G-Limiter (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 66);
            AssistButton("G-Limiter Off", "Toggle G-Limiter (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 66);
        }

        protected override void CreateNavigationControls()
        {
            NavButton("A/P Nav Mode", "Navigation Mode (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 64);
            NavButton("A/P Spd Hold", "Airspeed Hold (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 66);
            NavButton("A/P Hdg Hold", "Heading Hold (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 68);
            NavButton("A/P Alt Hold", "Altitude Hold (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 67);
            NavButton("A/P Off", "All AP Off (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 65);

            NavButton("A/P Alt Increase", "Adjust AP Altitude (Rear)", ByType<APKnobAltAdjust, CAltAdjust>, CAltAdjust.Increase, r: Dash);
            NavButton("A/P Alt Decrease", "Adjust AP Altitude (Rear)", ByType<APKnobAltAdjust, CAltAdjust>, CAltAdjust.Decrease, r: Dash);

            NavButton("A/P Hdg Right", "AP Heading Set (Rear)", ByType<DashHSI, CHSI>, CHSI.HeadingRight, r: Dash);
            NavButton("A/P Hdg Left", "AP Heading Set (Rear)", ByType<DashHSI, CHSI>, CHSI.HeadingLeft, r: Dash);

            NavButton("A/P Spd Increase", "Auto Speed Set (Rear)", ByType<APKnobSpeedDialAdjust, CSpdAdjust>, CSpdAdjust.Increase, r: Dash);
            NavButton("A/P Spd Decrease", "Auto Speed Set (Rear)", ByType<APKnobSpeedDialAdjust, CSpdAdjust>, CSpdAdjust.Decrease, r: Dash);

            NavButton("A/P Crs Right", "Course Set (Rear)", ByType<DashHSI, CHSI>, CHSI.CourseRight, r: Dash);
            NavButton("A/P Crs Left", "Course Set (Rear)", ByType<DashHSI, CHSI>, CHSI.CourseLeft, r: Dash);

            NavButton("Altitude Mode Toggle", "Toggle Altitude Mode (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 62);
            NavButton("Clear Waypoint", "Clear Waypoint (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 63);

            AddPostUpdateControl("Adjust AP Altitude (Rear)");
            AddPostUpdateControl("AP Heading Set (Rear)");
            AddPostUpdateControl("Auto Speed Set (Rear)");
            AddPostUpdateControl("Course Set (Rear)");
        }

        protected override void CreateSystemsControls()
        {
            SystemsButton("Clear Cautions", "Clear Cautions (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 61);

            SystemsButton("Master Arm Toggle", "Master Arm (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 119);
            SystemsButton("Master Safe Toggle", "Master Safe (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 120);

            SystemsButton("Fire Weapon", "Joystick (Rear)", Joysticks, CJoystick.Trigger);
            SystemsButton("Cycle Weapons", "Joystick (Rear)", Joysticks, CJoystick.MenuButton);
            SystemsButton("Fire Countermeasures", "Throttle", ByType<VRThrottle, CThrottle>, CThrottle.MenuButton, r: Throttle);

            SystemsButton("Flares Toggle", "Toggle Flares (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 68);
            SystemsButton("Flares On", "Toggle Flares (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 68);
            SystemsButton("Flares Off", "Toggle Flares (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 68);

            SystemsButton("Chaff Toggle", "Toggle Chaff (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 67);
            SystemsButton("Chaff On", "Toggle Chaff (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 67);
            SystemsButton("Chaff Off", "Toggle Chaff (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 67);

            SystemsButton("Engine Toggle", "Switch Cover (Engine) (Rear)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Cycle, i: 38);
            SystemsButton("Engine On", "Switch Cover (Engine) (Rear)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Set, 1, i: 38);
            SystemsButton("Engine Off", "Switch Cover (Engine) (Rear)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Set, 0, i: 38);

            SystemsButton("Main Battery Toggle", "Main Battery (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 41);
            SystemsButton("Main Battery On", "Main Battery (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 41);
            SystemsButton("Main Battery Off", "Main Battery (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 41);

            SystemsButton("APU Toggle", "Auxilliary Power (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 39);
            SystemsButton("APU On", "Auxilliary Power (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 39);
            SystemsButton("APU Off", "Auxilliary Power (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 39);

            SystemsButton("Radar Power Toggle", "Radar Power (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 12);
            SystemsButton("Radar Power On", "Radar Power (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 12);
            SystemsButton("Radar Power Off", "Radar Power (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 12);

            SystemsButton("RWR Mode Cycle", "RWR Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 13);
            SystemsButton("RWR Mode Prev", "RWR Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Prev, i: 13);
            SystemsButton("RWR Mode Next", "RWR Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Next, i: 13);
            SystemsButton("RWR Mode On", "RWR Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 13);
            SystemsButton("RWR Mode Mute", "RWR Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 13);
            SystemsButton("RWR Mode Off", "RWR Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 2, i: 13);
        }

        protected override void CreateHUDControls()
        {
            HUDButton("Helmet Visor Toggle", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.ToggleVisor, r: Seat);
            HUDButton("Helmet Visor Open", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.OpenVisor, r: Seat);
            HUDButton("Helmet Visor Closed", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.CloseVisor, r: Seat);
            HUDButton("Helmet NV Toggle", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.ToggleNightVision, r: Seat);
            HUDButton("Helmet NV On", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.EnableNightVision, r: Seat);
            HUDButton("Helmet NV Off", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.DisableNightVision, r: Seat);

            HUDButton("HMCS Power Toggle", "HMCS Power (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 36);
            HUDButton("HMCS Power On", "HMCS Power (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 36);
            HUDButton("HMCS Power Off", "HMCS Power (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 36);

            HUDButton("HUD Power Toggle", "HUD Power (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 35);
            HUDButton("HUD Power On", "HUD Power (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 35);
            HUDButton("HUD Power Off", "HUD Power (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 35);

            HUDAxis("HUD Tint", "HUD Tint (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 12);
            HUDButton("HUD Tint Increase", "HUD Tint (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 12);
            HUDButton("HUD Tint Decrease", "HUD Tint (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 12);

            HUDAxis("HUD Brightness", "HUD Brightness (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 13);
            HUDButton("HUD Brightness Increase", "HUD Brightness (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 13);
            HUDButton("HUD Brightness Decrease", "HUD Brightness (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 13);

            AddPostUpdateControl("HUD Tint (Rear)");
            AddPostUpdateControl("HUD Brightness (Rear)");
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
            DisplayButton("MMFD Left RWR Page", "RWR (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 74);

            DisplayButton("MMFD Right Toggle", "MMFD Right", MMFD, CMMFD.PowerToggle, i: 1, r: MMFDManager);
            DisplayButton("MMFD Right On", "MMFD Right", MMFD, CMMFD.PowerOn, i: 1, r: MMFDManager);
            DisplayButton("MMFD Right Off", "MMFD Right", MMFD, CMMFD.PowerOff, i: 1, r: MMFDManager);
            DisplayButton("MMFD Right Info Page", "Info (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 78);
            DisplayButton("MMFD Right Fuel Page", "Fuel (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 77);

            DisplayButton("MFD Mirror Toggle", "MFD Mirror", ByName<VRLever, CLever>, CLever.Cycle, i: 55, n: true);
            DisplayButton("MFD Mirror On", "MFD Mirror", ByName<VRLever, CLever>, CLever.Set, 1, i: 55, n: true);
            DisplayButton("MFD Mirror Off", "MFD Mirror", ByName<VRLever, CLever>, CLever.Set, 0, i: 55, n: true);

            DisplayAxis("MFD Brightness", "MFD Brightness (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 15);
            DisplayButton("MFD Brightness Increase", "MFD Brightness (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 15);
            DisplayButton("MFD Brightness Decrease", "MFD Brightness (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 15);

            DisplayButton("MFD Swap", "Swap MFDs (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 121);

            DisplayButton("MFD Left Toggle", "MFD Left", MFD, CMFD.PowerToggle, i: 2);
            DisplayButton("MFD Left On", "MFD Left", MFD, CMFD.PowerOn, i: 2);
            DisplayButton("MFD Left Off", "MFD Left", MFD, CMFD.PowerOff, i: 2);
            DisplayButton("MFD Left L1", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.L1, i: 2);
            DisplayButton("MFD Left L2", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.L2, i: 2);
            DisplayButton("MFD Left L3", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.L3, i: 2);
            DisplayButton("MFD Left L4", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.L4, i: 2);
            DisplayButton("MFD Left L5", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.L5, i: 2);
            DisplayButton("MFD Left R1", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.R1, i: 2);
            DisplayButton("MFD Left R2", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.R2, i: 2);
            DisplayButton("MFD Left R3", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.R3, i: 2);
            DisplayButton("MFD Left R4", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.R4, i: 2);
            DisplayButton("MFD Left R5", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.R5, i: 2);
            DisplayButton("MFD Left T1", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.T1Home, i: 2);
            DisplayButton("MFD Left T2", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.T2, i: 2);
            DisplayButton("MFD Left T3", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.T3, i: 2);
            DisplayButton("MFD Left T4", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.T4, i: 2);
            DisplayButton("MFD Left T5", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.T5, i: 2);
            DisplayButton("MFD Left B1", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.B1, i: 2);
            DisplayButton("MFD Left B2", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.B2, i: 2);
            DisplayButton("MFD Left B3", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.B3, i: 2);
            DisplayButton("MFD Left B4", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.B4, i: 2);
            DisplayButton("MFD Left B5", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.B5, i: 2);

            DisplayButton("MFD Right Toggle", "MFD Right", MFD, CMFD.PowerToggle, i: 3);
            DisplayButton("MFD Right On", "MFD Right", MFD, CMFD.PowerOn, i: 3);
            DisplayButton("MFD Right Off", "MFD Right", MFD, CMFD.PowerOff, i: 3);
            DisplayButton("MFD Right L1", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.L1, i: 3);
            DisplayButton("MFD Right L2", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.L2, i: 3);
            DisplayButton("MFD Right L3", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.L3, i: 3);
            DisplayButton("MFD Right L4", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.L4, i: 3);
            DisplayButton("MFD Right L5", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.L5, i: 3);
            DisplayButton("MFD Right R1", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.R1, i: 3);
            DisplayButton("MFD Right R2", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.R2, i: 3);
            DisplayButton("MFD Right R3", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.R3, i: 3);
            DisplayButton("MFD Right R4", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.R4, i: 3);
            DisplayButton("MFD Right R5", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.R5, i: 3);
            DisplayButton("MFD Right T1", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.T1Home, i: 3);
            DisplayButton("MFD Right T2", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.T2, i: 3);
            DisplayButton("MFD Right T3", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.T3, i: 3);
            DisplayButton("MFD Right T4", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.T4, i: 3);
            DisplayButton("MFD Right T5", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.T5, i: 3);
            DisplayButton("MFD Right B1", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.B1, i: 3);
            DisplayButton("MFD Right B2", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.B2, i: 3);
            DisplayButton("MFD Right B3", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.B3, i: 3);
            DisplayButton("MFD Right B4", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.B4, i: 3);
            DisplayButton("MFD Right B5", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.B5, i: 3);

            AddPostUpdateControl("MFD Brightness (Rear)");
            AddPostUpdateControl("SOI");
        }

        protected override void CreateRadioControls()
        {
            RadioButton("Radio Transmit", "Radio", ByType<CockpitTeamRadioManager, CRadio>, CRadio.Transmit, r: Dash);

            RadioButton("Radio Channel Cycle", "Radio Channel (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 15);
            RadioButton("Radio Channel Team", "Radio Channel (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 15);
            RadioButton("Radio Channel Global", "Radio Channel (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 15);

            RadioButton("Radio Mode Cycle", "Radio Mode (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 14);
            RadioButton("Radio Mode Next", "Radio Mode (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Next, i: 14);
            RadioButton("Radio Mode Prev", "Radio Mode (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Prev, i: 14);
            RadioButton("Radio Mode Hot Mic", "Radio Mode (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 14);
            RadioButton("Radio Mode PTT", "Radio Mode (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 14);
            RadioButton("Radio Mode Off", "Radio Mode (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 2, i: 14);

            RadioAxis("Radio Volume", "Comm Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 20);
            RadioButton("Radio Volume Increase", "Comm Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 20);
            RadioButton("Radio Volume Decrease", "Comm Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 20);

            RadioAxis("Radio Team Volume", "Team Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 21, n: true);
            RadioButton("Radio Team Volume Increase", "Team Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 21, n: true);
            RadioButton("Radio Team Volume Decrease", "Team Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 21, n: true);

            RadioButton("Intercom Toggle", "Intra Comms (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 16, n: true);
            RadioButton("Intercom On", "Intra Comms (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 16, n: true);
            RadioButton("Intercom Off", "Intra Comms (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 16, n: true);

            RadioAxis("Intercom Volume", "Intra Comm Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 22, n: true);
            RadioButton("Intercom Volume Increase", "Intra Comm Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 22, n: true);
            RadioButton("Intercom Volume Decrease", "Intra Comm Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 22, n: true);

            AddPostUpdateControl("Comm Radio Volume (Front)");
            AddPostUpdateControl("Team Radio Volume (Front)");
            AddPostUpdateControl("Intra Comm Volume (Front)");
        }

        protected override void CreateMusicControls()
        {
        }

        protected override void CreateLightsControls()
        {
            LightsButton("Interior Lights Toggle", "Interior Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 48);
            LightsButton("Interior Lights On", "Interior Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 48);
            LightsButton("Interior Lights Off", "Interior Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 48);

            LightsButton("Instrument Lights Toggle", "Instrument Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 49);
            LightsButton("Instrument Lights On", "Instrument Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 49);
            LightsButton("Instrument Lights Off", "Instrument Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 49);
            LightsAxis("Instrument Brightness", "Instrument Brightness (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 14);
            LightsButton("Instrument Brightness Increase", "Instrument Brightness (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 14);
            LightsButton("Instrument Brightness Decrease", "Instrument Brightness (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 14);

            LightsButton("Nav Lights Toggle", "Nav Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 52);
            LightsButton("Nav Lights On", "Nav Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 52);
            LightsButton("Nav Lights Off", "Nav Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 52);

            LightsButton("Strobe Lights Toggle", "Strobe Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 51);
            LightsButton("Strobe Lights On", "Strobe Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 51);
            LightsButton("Strobe Lights Off", "Strobe Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 51);

            LightsButton("Landing Lights Toggle", "Landing Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 50);
            LightsButton("Landing Lights On", "Landing Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 50);
            LightsButton("Landing Lights Off", "Landing Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 50);

            AddPostUpdateControl("Instrument Brightness (Rear)");
        }

        protected override void CreateMiscControls()
        {
            MiscButton("Canopy Toggle", "Canopy (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 42);
            MiscButton("Canopy Open", "Canopy (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 42);
            MiscButton("Canopy Close", "Canopy (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 42);

            MiscButton("Switch Seat", "Switch Seat (Rear)", ByType<VRButton, CButton>, CButton.Use, n: true, r: Switcher);
            MiscButton("Raise Seat", "Raise Seat", ByName<VRButton, CButton>, CButton.Use, r: Dash);
            MiscButton("Lower Seat", "Lower Seat", ByName<VRButton, CButton>, CButton.Use, r: Dash);
            MiscButton("Eject", "Eject", ByType<EjectHandle, CEject>, CEject.Pull, r: Seat);

            MiscButton("Fuel Port Toggle", "Fuel Port (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 58);
            MiscButton("Fuel Port Open", "Fuel Port (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 58);
            MiscButton("Fuel Port Close", "Fuel Port (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 58);

            MiscButton("Fuel Dump Toggle", "Switch Cover (Fuel Dump) (Rear)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Cycle, i: 60);
            MiscButton("Fuel Dump On", "Switch Cover (Fuel Dump) (Rear)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Set, 1, i: 60);
            MiscButton("Fuel Dump Off", "Switch Cover (Fuel Dump) (Rear)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Set, 0, i: 60);

            MiscButton("Jettison Execute", "Switch Cover (Jettison) (Rear)", ByManifest<VRLever, CButtonCovered>, CButtonCovered.Use, i: 47);
            MiscButton("Jettison Mark All", "Jettison All (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 70);
            MiscButton("Jettison Mark Empty", "Jettison Empty (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 71);
            MiscButton("Jettison Mark Ext. Tanks", "Jettison Ext Tanks (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 73);
            MiscButton("Jettison Clear Marks", "Clear Jettison Marks (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 72);
        }
    }
}