using BYOJoystick.Controls;
using BYOJoystick.Managers.Base;
using VTOLVR.Multiplayer;
using MFDButtons = MFD.MFDButtons;

namespace BYOJoystick.Managers
{
    public class AV42CManager : Manager
    {
        public override string GameName    => "AV-42C";
        public override string ShortName   => "AV42C";
        public override bool   IsMulticrew => false;

        private static string SideJoystick   => "Local/vtol4adjustableJoystick";
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

            FlightAxis("Throttle", "Throttle", ThrottleTilt, CThrottle.Set);
            FlightButton("Throttle Increase", "Throttle", ThrottleTilt, CThrottle.Increase);
            FlightButton("Throttle Decrease", "Throttle", ThrottleTilt, CThrottle.Decrease);

            FlightAxis("Tilt", "Throttle", ThrottleTilt, CThrottleTilt.SetTiltTarget);
            FlightButton("Tilt Up", "Throttle", ThrottleTilt, CThrottleTilt.TiltUp);
            FlightButton("Tilt Down", "Throttle", ThrottleTilt, CThrottleTilt.TiltDown);

            FlightAxis("Brakes/Airbrakes Axis", "Throttle", ThrottleTilt, CThrottle.Trigger);
            FlightButton("Brakes/Airbrakes", "Throttle", ThrottleTilt, CThrottle.Trigger);

            FlightButton("Flaps Cycle", "Flaps", ByManifest<VRLever, CLever>, CLever.Cycle, i: 15);
            FlightButton("Flaps Increase", "Flaps", ByManifest<VRLever, CLever>, CLever.Next, i: 15);
            FlightButton("Flaps Decrease", "Flaps", ByManifest<VRLever, CLever>, CLever.Prev, i: 15);
            FlightButton("Flaps 1", "Flaps", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 15);
            FlightButton("Flaps 2", "Flaps", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 15);
            FlightButton("Flaps 3", "Flaps", ByManifest<VRLever, CLever>, CLever.Set, 2, i: 15);

            FlightButton("Landing Gear Toggle", "Landing Gear", ByManifest<VRLever, CLever>, CLever.Cycle, i: 14);
            FlightButton("Landing Gear Up", "Landing Gear", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 14);
            FlightButton("Landing Gear Down", "Landing Gear", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 14);

            FlightButton("Brake Lock Toggle", "Brake Locks", ByManifest<VRLever, CLever>, CLever.Cycle, i: 25);
            FlightButton("Brake Lock On", "Brake Locks", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 25);
            FlightButton("Brake Lock Off", "Brake Locks", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 25);

            FlightButton("Launch Bar Toggle", "Launch Bar", ByManifest<VRLever, CLever>, CLever.Cycle, i: 17);
            FlightButton("Launch Bar Extend", "Launch Bar", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 17);
            FlightButton("Launch Bar Retract", "Launch Bar", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 17);

            FlightButton("Arrestor Hook Toggle", "Arrestor Hook", ByManifest<VRLever, CLever>, CLever.Cycle, i: 16);
            FlightButton("Arrestor Hook Extend", "Arrestor Hook", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 16);
            FlightButton("Arrestor Hook Retract", "Arrestor Hook", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 16);

            FlightButton("RCS Mode Cycle", "RCS Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 5);
            FlightButton("RCS Mode Prev", "RCS Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Prev, i: 5);
            FlightButton("RCS Mode Next", "RCS Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Next, i: 5);
            FlightButton("RCS Mode On", "RCS Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 5);
            FlightButton("RCS Mode Auto", "RCS Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 5);
            FlightButton("RCS Mode Off", "RCS Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 2, i: 5);

            AddPostUpdateControl("Joystick");
            AddPostUpdateControl("Throttle");
        }

        protected override void CreateAssistControls()
        {
            AssistButton("Master Toggle", "Toggle Flight Assist", ByManifest<VRLever, CLever>, CLever.Cycle, i: 28);
            AssistButton("Master On", "Toggle Flight Assist", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 28);
            AssistButton("Master Off", "Toggle Flight Assist", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 28);

            AssistButton("SAS Roll Toggle", "Toggle Roll Assist", ByManifest<VRLever, CLever>, CLever.Cycle, i: 31);
            AssistButton("SAS Roll On", "Toggle Roll Assist", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 31);
            AssistButton("SAS Roll Off", "Toggle Roll Assist", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 31);

            AssistButton("SAS Yaw Toggle", "Toggle Yaw Assist", ByManifest<VRLever, CLever>, CLever.Cycle, i: 30);
            AssistButton("SAS Yaw On", "Toggle Yaw Assist", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 30);
            AssistButton("SAS Yaw Off", "Toggle Yaw Assist", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 30);

            AssistButton("SAS Pitch Toggle", "Toggle Pitch Assist", ByManifest<VRLever, CLever>, CLever.Cycle, i: 29);
            AssistButton("SAS Pitch On", "Toggle Pitch Assist", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 29);
            AssistButton("SAS Pitch Off", "Toggle Pitch Assist", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 29);

            AssistButton("Auto Pitch Trim Toggle", "Toggle Pitch Trim", ByManifest<VRLever, CLever>, CLever.Cycle, i: 32);
            AssistButton("Auto Pitch Trim On", "Toggle Pitch Trim", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 32);
            AssistButton("Auto Pitch Trim Off", "Toggle Pitch Trim", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 32);

            AssistButton("G-Limiter Toggle", "Toggle G-Limiter", ByManifest<VRLever, CLever>, CLever.Cycle, i: 33);
            AssistButton("G-Limiter On", "Toggle G-Limiter", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 33);
            AssistButton("G-Limiter Off", "Toggle G-Limiter", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 33);
        }

        protected override void CreateNavigationControls()
        {
            NavButton("A/P Nav Mode", "Navigation Mode", ByManifest<VRButton, CButton>, CButton.Use, i: 19);
            NavButton("A/P Hvr Mode", "Hover Mode", ByManifest<VRButton, CButton>, CButton.Use, i: 17);
            NavButton("A/P Hdg Hold", "Heading Hold", ByManifest<VRButton, CButton>, CButton.Use, i: 15);
            NavButton("A/P Alt Hold", "Altitude Hold", ByManifest<VRButton, CButton>, CButton.Use, i: 16);
            NavButton("A/P Off", "All AP Off", ByManifest<VRButton, CButton>, CButton.Use, i: 18);

            NavButton("Altitude Mode Toggle", "Toggle Altitude Mode", ByManifest<VRButton, CButton>, CButton.Use, i: 14);
            NavButton("Clear Waypoint", "Clear Waypoint", ByManifest<VRButton, CButton>, CButton.Use, i: 12);
        }

        protected override void CreateSystemsControls()
        {
            SystemsButton("Clear Cautions", "Clear Cautions", ByManifest<VRButton, CButton>, CButton.Use, i: 13);

            SystemsButton("Master Arm Toggle", "Switch Cover (Master Arm)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Cycle, c: false, i: 19);
            SystemsButton("Master Arm On", "Switch Cover (Master Arm)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Set, 1, c: false, i: 19);
            SystemsButton("Master Arm Off", "Switch Cover (Master Arm)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Set, 0, c: false, i: 19);

            SystemsButton("Fire Weapon", "Joystick", Joysticks, CJoystick.Trigger, i: 0);
            SystemsButton("Cycle Weapons", "Joystick", Joysticks, CJoystick.MenuButton, i: 0);

            SystemsButton("Fire Countermeasures", "Throttle", ThrottleTilt, CThrottle.MenuButton);

            SystemsButton("Flares Toggle", "Toggle Flares", ByManifest<VRLever, CLever>, CLever.Cycle, i: 27);
            SystemsButton("Flares On", "Toggle Flares", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 27);
            SystemsButton("Flares Off", "Toggle Flares", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 27);

            SystemsButton("Chaff Toggle", "Toggle Chaff", ByManifest<VRLever, CLever>, CLever.Cycle, i: 26);
            SystemsButton("Chaff On", "Toggle Chaff", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 26);
            SystemsButton("Chaff Off", "Toggle Chaff", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 26);

            SystemsButton("Engine Left Toggle", "Switch Cover (Engine Left)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Cycle, c: false, i: 9);
            SystemsButton("Engine Left On", "Switch Cover (Engine Left)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Set, 1, c: false, i: 9);
            SystemsButton("Engine Left Off", "Switch Cover (Engine Left)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Set, 0, c: false, i: 9);
            SystemsButton("Engine Right Toggle", "Switch Cover (Engine Right)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Cycle, c: false, i: 11);
            SystemsButton("Engine Right On", "Switch Cover (Engine Right)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Set, 1, c: false, i: 11);
            SystemsButton("Engine Right Off", "Switch Cover (Engine Right)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Set, 0, c: false, i: 11);

            SystemsButton("Main Battery Toggle", "Main Battery", ByManifest<VRLever, CLever>, CLever.Cycle, i: 21);
            SystemsButton("Main Battery On", "Main Battery", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 21);
            SystemsButton("Main Battery Off", "Main Battery", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 21);

            SystemsButton("APU Toggle", "Auxilliary Power", ByManifest<VRLever, CLever>, CLever.Cycle, i: 12);
            SystemsButton("APU On", "Auxilliary Power", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 12);
            SystemsButton("APU Off", "Auxilliary Power", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 12);

            SystemsButton("RWR Mode Cycle", "RWR Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 6);
            SystemsButton("RWR Mode Prev", "RWR Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Prev, i: 6);
            SystemsButton("RWR Mode Next", "RWR Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Next, i: 6);
            SystemsButton("RWR Mode On", "RWR Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 6);
            SystemsButton("RWR Mode Mute", "RWR Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 6);
            SystemsButton("RWR Mode Off", "RWR Mode", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 2, i: 6);
        }

        protected override void CreateHUDControls()
        {
            HUDButton("Helmet Visor Toggle", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.ToggleVisor);
            HUDButton("Helmet Visor Open", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.OpenVisor);
            HUDButton("Helmet Visor Closed", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.CloseVisor);
            HUDButton("Helmet NV Toggle", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.ToggleNightVision);
            HUDButton("Helmet NV On", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.EnableNightVision);
            HUDButton("Helmet NV Off", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.DisableNightVision);

            HUDButton("HMCS Power Toggle", "HMCS Power", ByManifest<VRLever, CLever>, CLever.Cycle, i: 7);
            HUDButton("HMCS Power On", "HMCS Power", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 7);
            HUDButton("HMCS Power Off", "HMCS Power", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 7);

            HUDButton("HUD Power Toggle", "HUD Power", ByManifest<VRLever, CLever>, CLever.Cycle, i: 6);
            HUDButton("HUD Power On", "HUD Power", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 6);
            HUDButton("HUD Power Off", "HUD Power", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 6);

            HUDAxis("HUD Tint", "HUD Tint", ByName<VRTwistKnob, CKnob>, CKnob.Set, i: 1);
            HUDButton("HUD Tint Increase", "HUD Tint", ByName<VRTwistKnob, CKnob>, CKnob.Increase, i: 1);
            HUDButton("HUD Tint Decrease", "HUD Tint", ByName<VRTwistKnob, CKnob>, CKnob.Decrease, i: 1);

            HUDAxis("HUD Brightness", "HUD Brightness", ByName<VRTwistKnob, CKnob>, CKnob.Set, i: 2);
            HUDButton("HUD Brightness Increase", "HUD Brightness", ByName<VRTwistKnob, CKnob>, CKnob.Increase, i: 2);
            HUDButton("HUD Brightness Decrease", "HUD Brightness", ByName<VRTwistKnob, CKnob>, CKnob.Decrease, i: 2);

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
            DisplayButton("MMFD Left RWR Page", "RWR", ByManifest<VRButton, CButton>, CButton.Use, i: 84);

            DisplayButton("MMFD Right Toggle", "MMFD Right", MMFD, CMMFD.PowerToggle, i: 1);
            DisplayButton("MMFD Right On", "MMFD Right", MMFD, CMMFD.PowerOn, i: 1);
            DisplayButton("MMFD Right Off", "MMFD Right", MMFD, CMMFD.PowerOff, i: 1);
            DisplayButton("MMFD Right Info Page", "Fuel Info", ByManifest<VRButton, CButton>, CButton.Use, i: 87);
            DisplayButton("MMFD Right Fuel Page", "Fuel", ByManifest<VRButton, CButton>, CButton.Use, i: 88);

            DisplayAxis("MFD Brightness", "MFD Brightness", ByName<VRTwistKnob, CKnob>, CKnob.Set, i: 3);
            DisplayButton("MFD Brightness Increase", "MFD Brightness", ByName<VRTwistKnob, CKnob>, CKnob.Increase, i: 3);
            DisplayButton("MFD Brightness Decrease", "MFD Brightness", ByName<VRTwistKnob, CKnob>, CKnob.Decrease, i: 3);

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

            DisplayButton("MFD Center Toggle", "MFD Center", MFD, CMFD.PowerToggle, i: 1);
            DisplayButton("MFD Center On", "MFD Center", MFD, CMFD.PowerOn, i: 1);
            DisplayButton("MFD Center Off", "MFD Center", MFD, CMFD.PowerOff, i: 1);
            DisplayButton("MFD Center L1", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.L1, i: 1);
            DisplayButton("MFD Center L2", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.L2, i: 1);
            DisplayButton("MFD Center L3", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.L3, i: 1);
            DisplayButton("MFD Center L4", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.L4, i: 1);
            DisplayButton("MFD Center L5", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.L5, i: 1);
            DisplayButton("MFD Center R1", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.R1, i: 1);
            DisplayButton("MFD Center R2", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.R2, i: 1);
            DisplayButton("MFD Center R3", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.R3, i: 1);
            DisplayButton("MFD Center R4", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.R4, i: 1);
            DisplayButton("MFD Center R5", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.R5, i: 1);
            DisplayButton("MFD Center T1", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.T1Home, i: 1);
            DisplayButton("MFD Center T2", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.T2, i: 1);
            DisplayButton("MFD Center T3", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.T3, i: 1);
            DisplayButton("MFD Center T4", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.T4, i: 1);
            DisplayButton("MFD Center T5", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.T5, i: 1);
            DisplayButton("MFD Center B1", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.B1, i: 1);
            DisplayButton("MFD Center B2", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.B2, i: 1);
            DisplayButton("MFD Center B3", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.B3, i: 1);
            DisplayButton("MFD Center B4", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.B4, i: 1);
            DisplayButton("MFD Center B5", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.B5, i: 1);

            DisplayButton("MFD Right Toggle", "MFD Right", MFD, CMFD.PowerToggle, i: 2);
            DisplayButton("MFD Right On", "MFD Right", MFD, CMFD.PowerOn, i: 2);
            DisplayButton("MFD Right Off", "MFD Right", MFD, CMFD.PowerOff, i: 2);
            DisplayButton("MFD Right L1", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.L1, i: 2);
            DisplayButton("MFD Right L2", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.L2, i: 2);
            DisplayButton("MFD Right L3", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.L3, i: 2);
            DisplayButton("MFD Right L4", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.L4, i: 2);
            DisplayButton("MFD Right L5", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.L5, i: 2);
            DisplayButton("MFD Right R1", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.R1, i: 2);
            DisplayButton("MFD Right R2", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.R2, i: 2);
            DisplayButton("MFD Right R3", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.R3, i: 2);
            DisplayButton("MFD Right R4", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.R4, i: 2);
            DisplayButton("MFD Right R5", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.R5, i: 2);
            DisplayButton("MFD Right T1", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.T1Home, i: 2);
            DisplayButton("MFD Right T2", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.T2, i: 2);
            DisplayButton("MFD Right T3", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.T3, i: 2);
            DisplayButton("MFD Right T4", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.T4, i: 2);
            DisplayButton("MFD Right T5", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.T5, i: 2);
            DisplayButton("MFD Right B1", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.B1, i: 2);
            DisplayButton("MFD Right B2", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.B2, i: 2);
            DisplayButton("MFD Right B3", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.B3, i: 2);
            DisplayButton("MFD Right B4", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.B4, i: 2);
            DisplayButton("MFD Right B5", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.B5, i: 2);

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
            MusicButton("Music Play/Pause", "Play/Pause", ByManifest<VRButton, CButton>, CButton.Use, i: 91);
            MusicButton("Music Next", "Next Song", ByManifest<VRButton, CButton>, CButton.Use, i: 90);
            MusicButton("Music Previous", "Prev Song", ByManifest<VRButton, CButton>, CButton.Use, i: 89);
            MusicAxis("Music Volume", "Radio Volume", ByName<VRTwistKnob, CKnob>, CKnob.Set, i: 7);
            MusicButton("Music Volume Up", "Radio Volume", ByName<VRTwistKnob, CKnob>, CKnob.Increase, i: 7);
            MusicButton("Music Volume Down", "Radio Volume", ByName<VRTwistKnob, CKnob>, CKnob.Decrease, i: 7);

            AddPostUpdateControl("Radio Volume");
        }

        protected override void CreateLightsControls()
        {
            LightsButton("Interior Lights Toggle", "Cockpit Lights", ByManifest<VRLever, CLever>, CLever.Cycle, i: 0);
            LightsButton("Interior Lights On", "Cockpit Lights", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 0);
            LightsButton("Interior Lights Off", "Cockpit Lights", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 0);

            LightsButton("Bay Lights Toggle", "Bay Lights", ByManifest<VRLever, CLever>, CLever.Cycle, i: 1);
            LightsButton("Bay Lights On", "Bay Lights", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 1);
            LightsButton("Bay Lights Off", "Bay Lights", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 1);

            LightsButton("Instrument Lights Toggle", "Instrument Lights", ByManifest<VRLever, CLever>, CLever.Cycle, i: 2);
            LightsButton("Instrument Lights On", "Instrument Lights", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 2);
            LightsButton("Instrument Lights Off", "Instrument Lights", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 2);
            LightsAxis("Instrument Brightness", "Instrument Brightness", ByName<VRTwistKnob, CKnob>, CKnob.Set, i: 0);
            LightsButton("Instrument Brightness Increase", "Instrument Brightness", ByName<VRTwistKnob, CKnob>, CKnob.Increase, i: 0);
            LightsButton("Instrument Brightness Decrease", "Instrument Brightness", ByName<VRTwistKnob, CKnob>, CKnob.Decrease, i: 0);

            LightsButton("Nav Lights Toggle", "Navigation Lights", ByManifest<VRLever, CLever>, CLever.Cycle, i: 3);
            LightsButton("Nav Lights On", "Navigation Lights", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 3);
            LightsButton("Nav Lights Off", "Navigation Lights", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 3);

            LightsButton("Strobe Lights Toggle", "Strobe Lights", ByManifest<VRLever, CLever>, CLever.Cycle, i: 4);
            LightsButton("Strobe Lights On", "Strobe Lights", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 4);
            LightsButton("Strobe Lights Off", "Strobe Lights", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 4);

            LightsButton("Landing Lights Toggle", "Landing Lights", ByManifest<VRLever, CLever>, CLever.Cycle, i: 5);
            LightsButton("Landing Lights On", "Landing Lights", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 5);
            LightsButton("Landing Lights Off", "Landing Lights", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 5);

            AddPostUpdateControl("Instrument Brightness");
        }

        protected override void CreateMiscControls()
        {
            MiscButton("Doors Toggle", "Side Doors", ByManifest<VRLever, CLever>, CLever.Cycle, i: 24);
            MiscButton("Doors Open", "Side Doors", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 24);
            MiscButton("Doors Close", "Side Doors", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 24);

            MiscButton("Raise Seat", "Raise Seat", ByManifest<VRButton, CButton>, CButton.Use, i: 7);
            MiscButton("Lower Seat", "Lower Seat", ByManifest<VRButton, CButton>, CButton.Use, i: 8);
            MiscButton("Eject", "Eject", ByType<EjectHandle, CEject>, CEject.Pull);

            MiscButton("Fuel Port Toggle", "Fuel Port", ByManifest<VRLever, CLever>, CLever.Cycle, i: 34);
            MiscButton("Fuel Port Open", "Fuel Port", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 34);
            MiscButton("Fuel Port Close", "Fuel Port", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 34);

            MiscButton("Fuel Dump Toggle", "Switch Cover (Fuel Dump)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Cycle, c: false, i: 23);
            MiscButton("Fuel Dump On", "Switch Cover (Fuel Dump)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Set, 1, c: false, i: 23);
            MiscButton("Fuel Dump Off", "Switch Cover (Fuel Dump)", ByManifest<VRLever, CLeverCovered>, CLeverCovered.Set, 0, c: false, i: 23);

            MiscButton("Jettison Execute", "Switch Cover", ByManifest<VRLever, CButtonCovered>, CButtonCovered.Use, c: false, i: 20);
            MiscButton("Jettison Mark All", "JettisonAll", ByManifest<VRButton, CButton>, CButton.Use, i: 21);
            MiscButton("Jettison Mark Empty", "Jettison Empty", ByManifest<VRButton, CButton>, CButton.Use, i: 22);
            MiscButton("Jettison Clear Marks", "Clear Jettison Marks", ByManifest<VRButton, CButton>, CButton.Use, i: 23);
        }
    }
}