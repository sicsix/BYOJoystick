using BYOJoystick.Controls;
using BYOJoystick.Managers.Base;
using VTOLVR.Multiplayer;
using MFDButtons = MFD.MFDButtons;

namespace BYOJoystick.Managers
{
    public class FA26BManager : Manager
    {
        public override string GameName    => "F/A-26B";
        public override string ShortName   => "FA26B";
        public override bool   IsMulticrew => false;

        private static string SideJoystick   => "Local/SideStickObjects";
        private static string CenterJoystick => "Local/CenterStickObjects";

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

            FlightAxis("Throttle", "Throttle", ByManifest<VRThrottle, CThrottle>, CThrottle.Set);
            FlightButton("Throttle Increase", "Throttle", ByManifest<VRThrottle, CThrottle>, CThrottle.Increase);
            FlightButton("Throttle Decrease", "Throttle", ByManifest<VRThrottle, CThrottle>, CThrottle.Decrease);

            FlightAxis("Brakes/Airbrakes Axis", "Throttle", ByManifest<VRThrottle, CThrottle>, CThrottle.Trigger);
            FlightButton("Brakes/Airbrakes", "Throttle", ByManifest<VRThrottle, CThrottle>, CThrottle.Trigger);

            FlightButton("Flaps Cycle", "Flaps", ByManifest<VRLever, CLever>, CLever.Cycle, i: 11);
            FlightButton("Flaps Increase", "Flaps", ByManifest<VRLever, CLever>, CLever.Next, i: 11);
            FlightButton("Flaps Decrease", "Flaps", ByManifest<VRLever, CLever>, CLever.Prev, i: 11);
            FlightButton("Flaps 1", "Flaps", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 11);
            FlightButton("Flaps 2", "Flaps", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 11);
            FlightButton("Flaps 3", "Flaps", ByManifest<VRLever, CLever>, CLever.Set, 2, i: 11);

            FlightButton("Landing Gear Toggle", "Landing Gear", ByManifest<VRLever, CLever>, CLever.Cycle, i: 10);
            FlightButton("Landing Gear Up", "Landing Gear", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 10);
            FlightButton("Landing Gear Down", "Landing Gear", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 10);

            FlightButton("Brake Lock Toggle", "Brake Locks", ByManifest<VRLever, CLever>, CLever.Cycle, i: 18);
            FlightButton("Brake Lock On", "Brake Locks", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 18);
            FlightButton("Brake Lock Off", "Brake Locks", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 18);

            FlightButton("Wing Fold Toggle", "Wing Fold", ByManifest<VRLever, CLever>, CLever.Cycle, i: 17);
            FlightButton("Wing Fold Up", "Wing Fold", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 17);
            FlightButton("Wing Fold Down", "Wing Fold", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 17);

            FlightButton("Launch Bar Toggle", "Launch Bar", ByManifest<VRLever, CLever>, CLever.Cycle, i: 13);
            FlightButton("Launch Bar Extend", "Launch Bar", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 13);
            FlightButton("Launch Bar Retract", "Launch Bar", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 13);

            FlightButton("Arrestor Hook Toggle", "Arrestor Hook", ByManifest<VRLever, CLever>, CLever.Cycle, i: 12);
            FlightButton("Arrestor Hook Extend", "Arrestor Hook", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 12);
            FlightButton("Arrestor Hook Retract", "Arrestor Hook", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 12);

            FlightButton("Catapult T/O Trim Toggle", "Catapult T/O Trim", ByManifest<VRLever, CLever>, CLever.Cycle, i: 27);
            FlightButton("Catapult T/O Trim On", "Catapult T/O Trim", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 27);
            FlightButton("Catapult T/O Trim Off", "Catapult T/O Trim", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 27);

            AddPostUpdateControl("Joystick");
            AddPostUpdateControl("Throttle");
        }

        protected override void CreateAssistControls()
        {
            AssistButton("Master Toggle", "Toggle Flight Assist", ByManifest<VRLever, CLever>, CLever.Cycle, i: 21);
            AssistButton("Master On", "Toggle Flight Assist", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 21);
            AssistButton("Master Off", "Toggle Flight Assist", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 21);

            AssistButton("SAS Roll Toggle", "Toggle Roll Assist", ByManifest<VRLever, CLever>, CLever.Cycle, i: 24);
            AssistButton("SAS Roll On", "Toggle Roll Assist", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 24);
            AssistButton("SAS Roll Off", "Toggle Roll Assist", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 24);

            AssistButton("SAS Yaw Toggle", "Toggle Yaw Assist", ByManifest<VRLever, CLever>, CLever.Cycle, i: 23);
            AssistButton("SAS Yaw On", "Toggle Yaw Assist", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 23);
            AssistButton("SAS Yaw Off", "Toggle Yaw Assist", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 23);

            AssistButton("SAS Pitch Toggle", "Toggle Pitch Assist", ByManifest<VRLever, CLever>, CLever.Cycle, i: 22);
            AssistButton("SAS Pitch On", "Toggle Pitch Assist", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 22);
            AssistButton("SAS Pitch Off", "Toggle Pitch Assist", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 22);

            AssistButton("G-Limiter Toggle", "Toggle G-Limiter", ByManifest<VRLever, CLever>, CLever.Cycle, i: 26);
            AssistButton("G-Limiter On", "Toggle G-Limiter", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 26);
            AssistButton("G-Limiter Off", "Toggle G-Limiter", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 26);
        }

        protected override void CreateNavigationControls()
        {
            NavButton("A/P Nav Mode", "Navigation Mode", ByManifest<VRButton, CButton>, CButton.Use, i: 7);
            NavButton("A/P Spd Hold", "Airspeed Hold", ByManifest<VRButton, CButton>, CButton.Use, i: 5);
            NavButton("A/P Hdg Hold", "Heading Hold", ByManifest<VRButton, CButton>, CButton.Use, i: 3);
            NavButton("A/P Alt Hold", "Altitude Hold", ByManifest<VRButton, CButton>, CButton.Use, i: 4);
            NavButton("A/P Off", "All AP Off", ByManifest<VRButton, CButton>, CButton.Use, i: 6);

            NavButton("A/P Alt Increase", "Adjust AP Altitude", ByType<APKnobAltAdjust, CAltAdjust>, CAltAdjust.Increase);
            NavButton("A/P Alt Decrease", "Adjust AP Altitude", ByType<APKnobAltAdjust, CAltAdjust>, CAltAdjust.Decrease);

            NavButton("A/P Hdg Right", "AP Heading Set", ByType<DashHSI, CHSI>, CHSI.HeadingRight);
            NavButton("A/P Hdg Left", "AP Heading Set", ByType<DashHSI, CHSI>, CHSI.HeadingLeft);

            NavButton("A/P Spd Increase", "Auto Speed Set", ByType<APKnobSpeedDialAdjust, CSpdAdjust>, CSpdAdjust.Increase);
            NavButton("A/P Spd Decrease", "Auto Speed Set", ByType<APKnobSpeedDialAdjust, CSpdAdjust>, CSpdAdjust.Decrease);

            NavButton("A/P Crs Right", "Course Set", ByType<DashHSI, CHSI>, CHSI.CourseRight);
            NavButton("A/P Crs left", "Course Set", ByType<DashHSI, CHSI>, CHSI.CourseLeft);

            NavButton("Altitude Mode Toggle", "Toggle Altitude Mode", ByManifest<VRButton, CButton>, CButton.Use, i: 1);
            NavButton("Clear Waypoint", "Clear Waypoint", ByManifest<VRButton, CButton>, CButton.Use, i: 2);

            AddPostUpdateControl("Adjust AP Altitude");
            AddPostUpdateControl("AP Heading Set");
            AddPostUpdateControl("Auto Speed Set");
            AddPostUpdateControl("Course Set");
        }

        protected override void CreateSystemsControls()
        {
            SystemsButton("Clear Cautions", "Clear Cautions", ByManifest<VRButton, CButton>, CButton.Use, i: 0);

            SystemsButton("Master Arm Toggle", "Switch Cover (Master Arm)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Cycle, i: 16);
            SystemsButton("Master Arm On", "Switch Cover (Master Arm)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Set, 1, i: 16);
            SystemsButton("Master Arm Off", "Switch Cover (Master Arm)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Set, 0, i: 16);

            SystemsButton("Fire Weapon", "Joystick", Joysticks, CJoystick.Trigger, i: 0);
            SystemsButton("Cycle Weapons", "Joystick", Joysticks, CJoystick.MenuButton, i: 0);
            SystemsButton("Fire Countermeasures", "Throttle", ByManifest<VRThrottle, CThrottle>, CThrottle.MenuButton);

            SystemsButton("Flares Toggle", "Toggle Flares", ByManifest<VRLever, CLever>, CLever.Cycle, i: 20);
            SystemsButton("Flares On", "Toggle Flares", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 20);
            SystemsButton("Flares Off", "Toggle Flares", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 20);

            SystemsButton("Chaff Toggle", "Toggle Chaff", ByManifest<VRLever, CLever>, CLever.Cycle, i: 19);
            SystemsButton("Chaff On", "Toggle Chaff", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 19);
            SystemsButton("Chaff Off", "Toggle Chaff", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 19);

            SystemsButton("Engine Left Toggle", "Switch Cover (Left Engine)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Cycle, i: 3);
            SystemsButton("Engine Left On", "Switch Cover (Left Engine)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Set, 1, i: 3);
            SystemsButton("Engine Left Off", "Switch Cover (Left Engine)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Set, 0, i: 3);
            SystemsButton("Engine Right Toggle", "Switch Cover (Right Engine)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Cycle, i: 5);
            SystemsButton("Engine Right On", "Switch Cover (Right Engine)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Set, 1, i: 5);
            SystemsButton("Engine Right Off", "Switch Cover (Right Engine)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Set, 0, i: 5);

            SystemsButton("Main Battery Toggle", "Main Battery", ByManifest<VRLever, CLever>, CLever.Cycle, i: 8);
            SystemsButton("Main Battery On", "Main Battery", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 8);
            SystemsButton("Main Battery Off", "Main Battery", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 8);

            SystemsButton("APU Toggle", "Auxilliary Power", ByManifest<VRLever, CLever>, CLever.Cycle, i: 6);
            SystemsButton("APU On", "Auxilliary Power", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 6);
            SystemsButton("APU Off", "Auxilliary Power", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 6);

            SystemsButton("Radar Power Toggle", "Radar Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 4);
            SystemsButton("Radar Power On", "Radar Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 4);
            SystemsButton("Radar Power Off", "Radar Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 4);

            SystemsButton("RWR Mode Cycle", "RWR Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 3);
            SystemsButton("RWR Mode Prev", "RWR Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Prev, i: 3);
            SystemsButton("RWR Mode Next", "RWR Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Next, i: 3);
            SystemsButton("RWR Mode On", "RWR Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 3);
            SystemsButton("RWR Mode Mute", "RWR Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 3);
            SystemsButton("RWR Mode Off", "RWR Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 2, i: 3);
        }

        protected override void CreateHUDControls()
        {
            HUDButton("Helmet Visor Toggle", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.ToggleVisor);
            HUDButton("Helmet Visor Open", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.OpenVisor);
            HUDButton("Helmet Visor Closed", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.CloseVisor);
            HUDButton("Helmet NV Toggle", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.ToggleNightVision);
            HUDButton("Helmet NV On", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.EnableNightVision);
            HUDButton("Helmet NV Off", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.DisableNightVision);

            HUDButton("HMCS Power Toggle", "HMCS Power", ByManifest<VRLever, CLever>, CLever.Cycle, i: 1);
            HUDButton("HMCS Power On", "HMCS Power", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 1);
            HUDButton("HMCS Power Off", "HMCS Power", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 1);

            HUDButton("HUD Power Toggle", "HUD Power", ByManifest<VRLever, CLever>, CLever.Cycle, i: 0);
            HUDButton("HUD Power On", "HUD Power", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 0);
            HUDButton("HUD Power Off", "HUD Power", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 0);

            HUDAxis("HUD Tint", "HUD Tint", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 0);
            HUDButton("HUD Tint Increase", "HUD Tint", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 0);
            HUDButton("HUD Tint Decrease", "HUD Tint", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 0);

            HUDAxis("HUD Brightness", "HUD Brightness", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 1);
            HUDButton("HUD Brightness Increase", "HUD Brightness", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 1);
            HUDButton("HUD Brightness Decrease", "HUD Brightness", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 1);

            HUDButton("HUD Declutter Cycle", "HUD Declutter", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 0);
            HUDButton("HUD Declutter Increase", "HUD Declutter", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Next, i: 0);
            HUDButton("HUD Declutter Decrease", "HUD Declutter", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Prev, i: 0);
            HUDButton("HUD Declutter 1", "HUD Declutter", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 0);
            HUDButton("HUD Declutter 2", "HUD Declutter", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 0);
            HUDButton("HUD Declutter 3", "HUD Declutter", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 2, i: 0);

            AddPostUpdateControl("HUD Tint");
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

            DisplayButton("MMFD Left Toggle", "MMFD Left", MMFD, CMMFD.PowerToggle, i: 0);
            DisplayButton("MMFD Left On", "MMFD Left", MMFD, CMMFD.PowerOn, i: 0);
            DisplayButton("MMFD Left Off", "MMFD Left", MMFD, CMMFD.PowerOff, i: 0);
            DisplayButton("MMFD Left RWR Page", "RWR", ByManifest<VRButton, CButton>, CButton.Use, i: 16);

            DisplayButton("MMFD Right Toggle", "MMFD Right", MMFD, CMMFD.PowerToggle, i: 1);
            DisplayButton("MMFD Right On", "MMFD Right", MMFD, CMMFD.PowerOn, i: 1);
            DisplayButton("MMFD Right Off", "MMFD Right", MMFD, CMMFD.PowerOff, i: 1);
            DisplayButton("MMFD Right Info Page", "Info", ByManifest<VRButton, CButton>, CButton.Use, i: 20);
            DisplayButton("MMFD Right Fuel Page", "Fuel", ByManifest<VRButton, CButton>, CButton.Use, i: 19);

            DisplayAxis("MFD Brightness", "MFD Brightness", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 2);
            DisplayButton("MFD Brightness Increase", "MFD Brightness", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 2);
            DisplayButton("MFD Brightness Decrease", "MFD Brightness", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 2);

            DisplayButton("MFD Swap", "Swap MFDs", ByManifest<VRButton, CButton>, CButton.Use, i: 8);

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

            AddPostUpdateControl("MFD Brightness");
            AddPostUpdateControl("SOI");
        }

        protected override void CreateRadioControls()
        {
            RadioButton("Radio Transmit", "Radio", ByType<CockpitTeamRadioManager, CRadio>, CRadio.Transmit);

            RadioButton("Radio Channel Cycle", "Radio Channel", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 6);
            RadioButton("Radio Channel Team", "Radio Channel", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 6);
            RadioButton("Radio Channel Global", "Radio Channel", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 6);

            RadioButton("Radio Mode Cycle", "Radio Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 5);
            RadioButton("Radio Mode Next", "Radio Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Next, i: 5);
            RadioButton("Radio Mode Prev", "Radio Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Prev, i: 5);
            RadioButton("Radio Mode Hot Mic", "Radio Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 5);
            RadioButton("Radio Mode PTT", "Radio Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 5);
            RadioButton("Radio Mode Off", "Radio Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 2, i: 5);

            RadioAxis("Radio Volume", "Comm Radio Volume", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 7);
            RadioButton("Radio Volume Increase", "Comm Radio Volume", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 7);
            RadioButton("Radio Volume Decrease", "Comm Radio Volume", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 7);

            RadioAxis("Radio Team Volume", "Team Radio Volume", ByName<VRTwistKnob, CKnob>, CKnob.Set, n: true);
            RadioButton("Radio Team Volume Increase", "Team Radio Volume", ByName<VRTwistKnob, CKnob>, CKnob.Increase, n: true);
            RadioButton("Radio Team Volume Decrease", "Team Radio Volume", ByName<VRTwistKnob, CKnob>, CKnob.Decrease, n: true);

            AddPostUpdateControl("Comm Radio Volume");
            AddPostUpdateControl("Team Radio Volume");
        }

        protected override void CreateMusicControls()
        {
            MusicButton("Music Play/Pause", "Play/Pause", ByManifest<VRButton, CButton>, CButton.Use, i: 63);
            MusicButton("Music Next", "Next Song", ByManifest<VRButton, CButton>, CButton.Use, i: 62);
            MusicButton("Music Previous", "Prev Song", ByManifest<VRButton, CButton>, CButton.Use, i: 61);
            MusicAxis("Music Volume", "Radio Volume", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 8);
            MusicButton("Music Volume Up", "Radio Volume", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 8);
            MusicButton("Music Volume Down", "Radio Volume", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 8);

            AddPostUpdateControl("Radio Volume");
        }

        protected override void CreateLightsControls()
        {
            LightsButton("Interior Lights Toggle", "Interior Lights", ByManifest<VRLever, CLever>, CLever.Cycle, i: 31);
            LightsButton("Interior Lights On", "Interior Lights", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 31);
            LightsButton("Interior Lights Off", "Interior Lights", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 31);

            LightsButton("Instrument Lights Toggle", "Instrument Lights", ByManifest<VRLever, CLever>, CLever.Cycle, i: 32);
            LightsButton("Instrument Lights On", "Instrument Lights", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 32);
            LightsButton("Instrument Lights Off", "Instrument Lights", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 32);
            LightsAxis("Instrument Brightness", "Instrument Brightness", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 9);
            LightsButton("Instrument Brightness Increase", "Instrument Brightness", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 9);
            LightsButton("Instrument Brightness Decrease", "Instrument Brightness", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 9);

            LightsButton("Nav Lights Toggle", "Nav Lights", ByManifest<VRLever, CLever>, CLever.Cycle, i: 35);
            LightsButton("Nav Lights On", "Nav Lights", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 35);
            LightsButton("Nav Lights Off", "Nav Lights", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 35);

            LightsButton("Strobe Lights Toggle", "Strobe Lights", ByManifest<VRLever, CLever>, CLever.Cycle, i: 34);
            LightsButton("Strobe Lights On", "Strobe Lights", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 34);
            LightsButton("Strobe Lights Off", "Strobe Lights", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 34);

            LightsButton("Landing Lights Toggle", "Landing Lights", ByManifest<VRLever, CLever>, CLever.Cycle, i: 33);
            LightsButton("Landing Lights On", "Landing Lights", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 33);
            LightsButton("Landing Lights Off", "Landing Lights", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 33);

            AddPostUpdateControl("Instrument Brightness");
        }

        protected override void CreateMiscControls()
        {
            MiscButton("Canopy Toggle", "Canopy", ByManifest<VRLever, CLever>, CLever.Cycle, i: 9);
            MiscButton("Canopy Open", "Canopy", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 9);
            MiscButton("Canopy Close", "Canopy", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 9);

            MiscButton("Raise Seat", "Raise Seat", ByManifest<VRButton, CButton>, CButton.Use, i: 14);
            MiscButton("Lower Seat", "Lower Seat", ByManifest<VRButton, CButton>, CButton.Use, i: 15);
            MiscButton("Eject", "Eject", ByType<EjectHandle, CEject>, CEject.Pull);

            MiscButton("Fuel Port Toggle", "Fuel Port", ByManifest<VRLever, CLever>, CLever.Cycle, i: 28);
            MiscButton("Fuel Port Open", "Fuel Port", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 28);
            MiscButton("Fuel Port Close", "Fuel Port", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 28);

            MiscButton("Fuel Dump Toggle", "Switch Cover (Fuel Dump)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Cycle, i: 30);
            MiscButton("Fuel Dump On", "Switch Cover (Fuel Dump)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Set, 1, i: 30);
            MiscButton("Fuel Dump Off", "Switch Cover (Fuel Dump)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Set, 0, i: 30);

            MiscButton("Jettison Execute", "Switch Cover (Jettison)", ByManifest<VRLever, CButtonCovered>, CButtonCovered.Use, i: 14);
            MiscButton("Jettison Mark All", "Jettison All", ByManifest<VRButton, CButton>, CButton.Use, i: 10);
            MiscButton("Jettison Mark Empty", "Jettison Empty", ByManifest<VRButton, CButton>, CButton.Use, i: 11);
            MiscButton("Jettison Mark Ext. Tanks", "Jettison Ext Tanks", ByManifest<VRButton, CButton>, CButton.Use, i: 13);
            MiscButton("Jettison Clear Marks", "Clear Jettison Marks", ByManifest<VRButton, CButton>, CButton.Use, i: 12);
        }

        private CJoystick Joysticks(string name, VRInteractable[] interactables, int index)
        {
            return GetJoysticksByPaths(name, "Local/SideStickObjects", "Local/CenterStickObjects");
        }
    }
}