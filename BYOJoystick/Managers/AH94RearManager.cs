using System;
using BYOJoystick.Controls;
using BYOJoystick.Managers.Base;
using VTOLVR.DLC.Rotorcraft;
using VTOLVR.Multiplayer;
using MFDButtons = MFD.MFDButtons;

namespace BYOJoystick.Managers
{
    public class AH94RearManager : Manager
    {
        public override string GameName    => "AH-94 Rear";
        public override string ShortName   => "AH94Rear";
        public override bool   IsMulticrew => true;
        public override bool   IsSeatA     => true;

        private static string Dash           => "PassengerOnlyObjects/DashCanvas/Rear";
        private static string UpFrontDisplay => "PassengerOnlyObjects/DashCanvas/Rear/UpfrontDisplay_rear";
        private static string Cont           => "PassengerOnlyObjects/localCockpit/controls.001";
        private static string SideJoystick   => "PassengerOnlyObjects/localCockpit/controls.001/vtol4adjustableJoystick_rear";
        private static string CenterJoystick => "PassengerOnlyObjects/localCockpit/controls.001/centerJoystickBase_rear";
        private static string EjectHandle    => "PassengerOnlyObjects/bailBase_rear";

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

            FlightAxis("Power", "Power Lever (Rear)", ByManifest<VRThrottle, CThrottle>, CThrottle.Set, i: 0);
            FlightButton("Power Increase", "Power Lever (Rear)", ByManifest<VRThrottle, CThrottle>, CThrottle.Decrease, i: 0);
            FlightButton("Power Decrease", "Power Lever (Rear)", ByManifest<VRThrottle, CThrottle>, CThrottle.Increase, i: 0);

            FlightAxis("Collective", "Flight Collective (Rear)", ByManifest<VRThrottle, CThrottle>, CThrottle.Set, i: 2);
            FlightButton("Collective Up", "Flight Collective (Rear)", ByManifest<VRThrottle, CThrottle>, CThrottle.Increase, i: 2);
            FlightButton("Collective Down", "Flight Collective (Rear)", ByManifest<VRThrottle, CThrottle>, CThrottle.Decrease, i: 2);

            FlightButton("Trim Pitch Up", "Joystick", Joysticks, CJoystick.ThumbstickUp);
            FlightButton("Trim Pitch Down", "Joystick", Joysticks, CJoystick.ThumbstickDown);
            FlightButton("Trim Roll Left", "Joystick", Joysticks, CJoystick.ThumbstickLeft);
            FlightButton("Trim Roll Right", "Joystick", Joysticks, CJoystick.ThumbstickRight);
            FlightButton("Trim Yaw Left", "Coll Functions", ByType<AH94CollectiveFunctions, CCollFuncs>, CCollFuncs.TrimYawLeft, r: Cont);
            FlightButton("Trim Yaw Right", "Coll Functions", ByType<AH94CollectiveFunctions, CCollFuncs>, CCollFuncs.TrimYawRight, r: Cont);

            FlightButton("Landing Gear Toggle", "Landing Gear (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 13);
            FlightButton("Landing Gear Up", "Landing Gear (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 13);
            FlightButton("Landing Gear Down", "Landing Gear (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 13);

            FlightButton("Brake Lock Toggle", "Parking Brake", ByManifest<VRLever, CLever>, CLever.Cycle, i: 12);
            FlightButton("Brake Lock On", "Parking Brake", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 12);
            FlightButton("Brake Lock Off", "Parking Brake", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 12);

            FlightButton("Rotor Brake Toggle", "Rotor Brake", ByManifest<VRLever, CLever>, CLever.Cycle, i: 11);
            FlightButton("Rotor Brake On", "Rotor Brake", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 11);
            FlightButton("Rotor Brake Off", "Rotor Brake", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 11);

            FlightButton("Rotor Fold Toggle", "Rotor Fold", ByManifest<VRLever, CLever>, CLever.Cycle, i: 10);
            FlightButton("Rotor Fold Up", "Rotor Fold", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 10);
            FlightButton("Rotor Fold Down", "Rotor Fold", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 10);

            AddPostUpdateControl("Joystick");
            AddPostUpdateControl("Power Lever (Rear)");
            AddPostUpdateControl("Flight Collective (Rear)");
            AddPostUpdateControl("Coll Functions");
        }

        protected override void CreateAssistControls()
        {
            AssistAxis("SAS Level", "SAS Level (Rear)", ByName<VRTwistKnob, CKnob>, CKnob.Set, r: Dash);
            AssistButton("SAS Level Increase", "SAS Level (Rear)", ByName<VRTwistKnob, CKnob>, CKnob.Increase, r: Dash);
            AssistButton("SAS Level Decrease", "SAS Level (Rear)", ByName<VRTwistKnob, CKnob>, CKnob.Decrease, r: Dash);

            AddPostUpdateControl("SAS Level (Rear)");
        }

        protected override void CreateNavigationControls()
        {
            NavButton("A/P Nav Mode", "Nav AP (Rear)", ByName<VRInteractable, CInteractable>, CInteractable.Use);
            NavButton("A/P Hvr Mode", "Hover AP (Rear)", ByName<VRInteractable, CInteractable>, CInteractable.Use);
            NavButton("A/P Hdg Hold", "Heading AP (Rear)", ByName<VRInteractable, CInteractable>, CInteractable.Use);
            NavButton("A/P Alt Hold", "Altitude AP (Rear)", ByName<VRInteractable, CInteractable>, CInteractable.Use);
            NavButton("A/P Off", "AP Off (Rear)", ByName<VRInteractable, CInteractable>, CInteractable.Use);

            NavButton("A/P Alt Increase", "AP Alt + (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 95);
            NavButton("A/P Alt Decrease", "AP Alt - (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 96);

            NavButton("A/P Hdg Right", "Heading Right (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 93);
            NavButton("A/P Hdg Left", "Heading Left (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 94);

            NavButton("Clear Waypoint", "Clear Waypoint", ByManifest<VRButton, CButton>, CButton.Use, i: 97);
        }

        protected override void CreateSystemsControls()
        {
            SystemsButton("Clear Cautions", "Master Caution (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 99);

            SystemsButton("Master Arm Toggle", "Master Arm (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 140);
            SystemsButton("Master Safe Toggle", "Master Safe (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 141);

            SystemsButton("Fire Weapon", "Cyclic (Rear)", Joysticks, CJoystick.Trigger);
            SystemsButton("Cycle Weapons", "Cyclic (Rear)", Joysticks, CJoystick.MenuButton);

            SystemsButton("Articulate Auto Enable", "Coll Functions", ByType<AH94CollectiveFunctions, CCollFuncs>, CCollFuncs.ArticAuto, r: Cont);

            SystemsAxisC("Articulate Tilt", "Coll Functions", ByType<AH94CollectiveFunctions, CCollFuncs>, CCollFuncs.SetArtic, r: Cont);
            SystemsButton("Articulate Up", "Coll Functions", ByType<AH94CollectiveFunctions, CCollFuncs>, CCollFuncs.ArticUp, r: Cont);
            SystemsButton("Articulate Down", "Coll Functions", ByType<AH94CollectiveFunctions, CCollFuncs>, CCollFuncs.ArticDown, r: Cont);

            SystemsButton("Fire Countermeasures", "Coll Functions", ByType<AH94CollectiveFunctions, CCollFuncs>, CCollFuncs.FireCM, r: Cont);
            SystemsButton("Flares Toggle", "Toggle Flares (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 161);
            SystemsButton("Chaff Toggle", "Toggle Chaff (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 160);

            SystemsButton("Engine Left On", "Starter Left (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 2, i: 16);
            SystemsButton("Engine Left Off", "Starter Left (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 16);

            SystemsButton("Engine Right On", "Starter Right (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 2, i: 17);
            SystemsButton("Engine Right Off", "Starter Right (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 17);

            SystemsButton("Main Battery On", "Main Battery (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 2, i: 14);
            SystemsButton("Main Battery Off", "Main Battery (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 14);

            SystemsButton("APU On", "Auxilliary Power (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 2, i: 15);
            SystemsButton("APU Off", "Auxilliary Power (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 15);
        }

        protected override void CreateHUDControls()
        {
            HUDButton("Helmet Visor Toggle", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.ToggleVisor);
            HUDButton("Helmet Visor Open", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.OpenVisor);
            HUDButton("Helmet Visor Closed", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.CloseVisor);
            HUDButton("Helmet NV Toggle", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.ToggleNightVision);
            HUDButton("Helmet NV On", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.EnableNightVision);
            HUDButton("Helmet NV Off", "Helmet", ByType<HelmetController, CHelmet>, CHelmet.DisableNightVision);

            HUDButton("HMD Power Toggle", "HMD Power (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 6);
            HUDButton("HMD Power On", "HMD Power (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 6);
            HUDButton("HMD Power Off", "HMD Power (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 6);

            HUDAxis("HMD Brightness", "HMD Brightness (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 6);
            HUDButton("HMD Brightness Increase", "HMD Brightness (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 6);
            HUDButton("HMD Brightness Decrease", "HMD Brightness (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 6);

            AddPostUpdateControl("HMD Brightness (Rear)");
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

            DisplayButton("SOI Radar Elev Up", "SOI", SOI, CSOI.RadarElevUp);
            DisplayButton("SOI Radar Elev Down", "SOI", SOI, CSOI.RadarElevDown);

            DisplayButton("ADI Power Toggle", "ADI Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 4);
            DisplayButton("ADI Power On", "ADI Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 4);
            DisplayButton("ADI Power Off", "ADI Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 4);

            DisplayButton("RWR Power Toggle", "RWR Power (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 5);
            DisplayButton("RWR Power On", "RWR Power (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 5);
            DisplayButton("RWR Power Off", "RWR Power (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 5);

            DisplayButton("UFD Power Toggle", "UFD", UFD, CUFD.PowerToggle);
            DisplayButton("UFD Power On", "UFD", UFD, CUFD.PowerOn);
            DisplayButton("UFD Power Off", "UFD", UFD, CUFD.PowerOff);

            DisplayButton("UFD Next Page", "UFD", UFD, CUFD.Next);
            DisplayButton("UFD Prev Page", "UFD", UFD, CUFD.Previous);
            DisplayButton("UFD Fuel Page", "UFD", UFD, CUFD.Fuel);
            DisplayButton("UFD Autopilot Page", "UFD", UFD, CUFD.AP);
            DisplayButton("UFD Status Page", "UFD", UFD, CUFD.Status);

            DisplayAxis("MFD Brightness", "MFD Brightness (Rear)", ByName<VRTwistKnob, CKnob>, CKnob.Set);
            DisplayButton("MFD Brightness Increase", "MFD Brightness (Rear)", ByName<VRTwistKnob, CKnob>, CKnob.Increase);
            DisplayButton("MFD Brightness Decrease", "MFD Brightness (Rear)", ByName<VRTwistKnob, CKnob>, CKnob.Decrease);

            DisplayButton("MFD Swap", "Swap MFDs (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 98);

            DisplayButton("MFD Left Toggle", "MFD1 Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 14);
            DisplayButton("MFD Left On", "MFD1 Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 14);
            DisplayButton("MFD Left Off", "MFD1 Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 14);
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

            DisplayButton("MFD Right Toggle", "MFD2 Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 15);
            DisplayButton("MFD Right On", "MFD2 Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 15);
            DisplayButton("MFD Right Off", "MFD2 Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 15);
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

            AddPostUpdateControl("MFD Brightness (Rear)");
            AddPostUpdateControl("SOI");
        }

        protected override void CreateRadioControls()
        {
            RadioButton("Radio Transmit", "Radio", ByType<CockpitTeamRadioManager, CRadio>, CRadio.Transmit, r: Dash, n: true);

            RadioButton("Radio Channel Cycle", "Radio Channel (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 8);
            RadioButton("Radio Channel Team", "Radio Channel (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 8);
            RadioButton("Radio Channel Global", "Radio Channel (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 8);

            RadioButton("Radio Mode Cycle", "Radio Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 7);
            RadioButton("Radio Mode Next", "Radio Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Next, i: 7);
            RadioButton("Radio Mode Prev", "Radio Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Prev, i: 7);
            RadioButton("Radio Mode Hot Mic", "Radio Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 7);
            RadioButton("Radio Mode PTT", "Radio Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 7);
            RadioButton("Radio Mode Off", "Radio Mode (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 2, i: 7);

            RadioAxis("Radio Volume", "Comm Radio Volume (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 8);
            RadioButton("Radio Volume Increase", "Comm Radio Volume (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 8);
            RadioButton("Radio Volume Decrease", "Comm Radio Volume (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 8);

            RadioAxis("Radio Team Volume", "Team Radio Volume (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 9, n: true);
            RadioButton("Radio Team Volume Increase", "Team Radio Volume (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 9, n: true);
            RadioButton("Radio Team Volume Decrease", "Team Radio Volume (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 9, n: true);

            RadioButton("Intercom Toggle", "Intra Comms (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 9, n: true);
            RadioButton("Intercom On", "Intra Comms (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 9, n: true);
            RadioButton("Intercom Off", "Intra Comms (Rear)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 9, n: true);

            RadioAxis("Intercom Volume", "Intra Comm Volume (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 10, n: true);
            RadioButton("Intercom Volume Increase", "Intra Comm Volume (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 10, n: true);
            RadioButton("Intercom Volume Decrease", "Intra Comm Volume (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 10, n: true);

            AddPostUpdateControl("Comm Radio Volume (Rear)");
            AddPostUpdateControl("Team Radio Volume (Rear)");
            AddPostUpdateControl("Intra Comm Volume (Rear)");
        }

        protected override void CreateMusicControls()
        {
            MusicButton("Music Play/Pause", "Play/Pause", ByManifest<VRButton, CButton>, CButton.Use, i: 81);
            MusicButton("Music Next", "Next Song", ByManifest<VRButton, CButton>, CButton.Use, i: 80);
            MusicButton("Music Previous", "Prev Song", ByManifest<VRButton, CButton>, CButton.Use, i: 79);
            MusicAxis("Music Volume", "MP3 Radio Volume", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 5);
            MusicButton("Music Volume Up", "MP3 Radio Volume", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 5);
            MusicButton("Music Volume Down", "MP3 Radio Volume", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 5);

            AddPostUpdateControl("MP3 Radio Volume");
        }

        protected override void CreateLightsControls()
        {
            LightsButton("Interior Lights Toggle", "Interior Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 18);
            LightsButton("Interior Lights On", "Interior Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 18);
            LightsButton("Interior Lights Off", "Interior Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 18);

            LightsButton("Instrument Lights Toggle", "Instrument Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 19);
            LightsButton("Instrument Lights On", "Instrument Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 19);
            LightsButton("Instrument Lights Off", "Instrument Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 19);
            LightsAxis("Instrument Brightness", "Instrument Brightness (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 7);
            LightsButton("Instrument Brightness Increase", "Instrument Brightness (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 7);
            LightsButton("Instrument Brightness Decrease", "Instrument Brightness (Rear)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 7);

            LightsButton("Nav Lights Toggle", "Nav Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 22);
            LightsButton("Nav Lights On", "Nav Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 22);
            LightsButton("Nav Lights Off", "Nav Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 22);

            LightsButton("Strobe Lights Toggle", "Strobe Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 21);
            LightsButton("Strobe Lights On", "Strobe Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 21);
            LightsButton("Strobe Lights Off", "Strobe Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 21);

            LightsButton("Landing Lights Toggle", "Landing Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 20);
            LightsButton("Landing Lights On", "Landing Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 20);
            LightsButton("Landing Lights Off", "Landing Lights (Rear)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 20);

            AddPostUpdateControl("Instrument Brightness (Rear)");
        }

        protected override void CreateMiscControls()
        {
            MiscButton("Door Left Toggle", "Door Handle (Rear L)", ByManifest<VRDoor, CDoor>, CDoor.Toggle, i: 1);
            MiscButton("Door Left Open", "Door Handle (Rear L)", ByManifest<VRDoor, CDoor>, CDoor.Open, i: 1);
            MiscButton("Door Left Close", "Door Handle (Rear L)", ByManifest<VRDoor, CDoor>, CDoor.Close, i: 1);

            MiscButton("Door Right Toggle", "Door Handle (Rear R)", ByManifest<VRDoor, CDoor>, CDoor.Toggle, i: 3);
            MiscButton("Door Right Open", "Door Handle (Rear R)", ByManifest<VRDoor, CDoor>, CDoor.Open, i: 3);
            MiscButton("Door Right Close", "Door Handle (Rear R)", ByManifest<VRDoor, CDoor>, CDoor.Close, i: 3);

            MiscButton("Switch Seat", "Switch Seat (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 156, n: true);
            MiscButton("Raise Seat", "Raise Seat", ByName<VRButton, CButton>, CButton.Use, r: Dash);
            MiscButton("Lower Seat", "Lower Seat", ByName<VRButton, CButton>, CButton.Use, r: Dash);
            MiscButton("Eject", "Eject", ByType<EjectHandle, CEject>, CEject.Pull, r: EjectHandle);

            MiscButton("Jettison Execute", "Jettison (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 88);
            MiscButton("Jettison L TIP", "Mark Jett L TIP (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 86);
            MiscButton("Jettison L OUT", "Mark Jett L OUT (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 82);
            MiscButton("Jettison L IN", "Mark Jett L IN (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 83);
            MiscButton("Jettison R IN", "Mark Jett R IN (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 84);
            MiscButton("Jettison R OUT", "Mark Jett R OUT (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 85);
            MiscButton("Jettison R TIP", "Mark Jett R TIP (Rear)", ByManifest<VRButton, CButton>, CButton.Use, i: 87);
        }

        private CUFD UFD(string name, string root, bool nullable, int idx)
        {
            if (TryGetExistingControl<CUFD>(name, out var existingControl))
                return existingControl;
            var switcher = GetGameObject(Dash).GetComponentInChildren<MultiObjectSwitcher>();
            if (switcher == null)
                throw new InvalidOperationException("Could not find UFD switcher");

            var powerUnit = GetGameObject(UpFrontDisplay).GetComponentInChildren<ObjectPowerUnit>();
            if (powerUnit == null)
                throw new InvalidOperationException("Could not find UFD power unit");

            var powerButton = ByManifest<VRButton, CButton>("UFD Power (Rear)", null, false, 91);
            if (powerButton == null)
                throw new InvalidOperationException("Could not find UFD power button");

            var fuelPageButton = ByManifest<VRButton, CButton>("UFD Fuel (Rear)", null, false, 89);
            if (fuelPageButton == null)
                throw new InvalidOperationException("Could not find UFD fuel page button");

            var apPageButton = ByManifest<VRButton, CButton>("UFD Autopilot (Rear)", null, false, 90);
            if (apPageButton == null)
                throw new InvalidOperationException("Could not find UFD autopilot page button");

            var statusPageButton = ByManifest<VRButton, CButton>("UFD Status (Rear)", null, false, 92);
            if (statusPageButton == null)
                throw new InvalidOperationException("Could not find UFD status page button");

            var cufd = new CUFD(switcher, powerUnit, powerButton, fuelPageButton, apPageButton, statusPageButton);
            Controls.Add(name, cufd);
            return cufd;
        }
    }
}