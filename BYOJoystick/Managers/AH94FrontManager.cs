using System;
using BYOJoystick.Controls;
using BYOJoystick.Managers.Base;
using VTOLVR.DLC.Rotorcraft;
using VTOLVR.Multiplayer;
using MFDButtons = MFD.MFDButtons;

namespace BYOJoystick.Managers
{
    public class AH94FrontManager : Manager
    {
        public override string GameName    => "AH-94 Front";
        public override string ShortName   => "AH94Front";
        public override bool   IsMulticrew => true;
        public override bool   IsSeatA     => false;

        private static string Dash           => "PassengerOnlyObjects/DashCanvas/Front";
        private static string UpFrontDisplay => "PassengerOnlyObjects/DashCanvas/Front/UpfrontDisplay";
        private static string Cont           => "PassengerOnlyObjects/localCockpit/controls";
        private static string SideJoystick   => "PassengerOnlyObjects/localCockpit/controls/vtol4adjustableJoystick_front";
        private static string CenterJoystick => "PassengerOnlyObjects/localCockpit/controls/centerJoystickBase_front";
        private static string EjectHandle    => "PassengerOnlyObjects/bailBase_Front";

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

            FlightAxis("Power", "Power Lever (Front)", ByManifest<VRThrottle, CThrottle>, CThrottle.Set, i: 1);
            FlightButton("Power Increase", "Power Lever (Front)", ByManifest<VRThrottle, CThrottle>, CThrottle.Decrease, i: 1);
            FlightButton("Power Decrease", "Power Lever (Front)", ByManifest<VRThrottle, CThrottle>, CThrottle.Increase, i: 1);

            FlightAxis("Collective", "Collective (Front)", ByManifest<VRThrottle, CThrottle>, CThrottle.Set, i: 0);
            FlightButton("Collective Up", "Collective (Front)", ByManifest<VRThrottle, CThrottle>, CThrottle.Increase, i: 0);
            FlightButton("Collective Down", "Collective (Front)", ByManifest<VRThrottle, CThrottle>, CThrottle.Decrease, i: 0);

            FlightButton("Trim Pitch Up", "Joystick", Joysticks, CJoystick.ThumbstickUp);
            FlightButton("Trim Pitch Down", "Joystick", Joysticks, CJoystick.ThumbstickDown);
            FlightButton("Trim Roll Left", "Joystick", Joysticks, CJoystick.ThumbstickLeft);
            FlightButton("Trim Roll Right", "Joystick", Joysticks, CJoystick.ThumbstickRight);
            FlightButton("Trim Yaw Left", "Coll Functions", ByType<AH94CollectiveFunctions, CCollFuncs>, CCollFuncs.TrimYawLeft, r: Cont);
            FlightButton("Trim Yaw Right", "Coll Functions", ByType<AH94CollectiveFunctions, CCollFuncs>, CCollFuncs.TrimYawRight, r: Cont);

            FlightButton("Landing Gear Toggle", "Landing Gear (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 0);
            FlightButton("Landing Gear Up", "Landing Gear (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 0);
            FlightButton("Landing Gear Down", "Landing Gear (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 0);

            AddPostUpdateControl("Joystick");
            AddPostUpdateControl("Power Lever (Front)");
            AddPostUpdateControl("Collective (Front)");
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
            NavButton("A/P Nav Mode", "Nav AP (Front)", ByName<VRInteractable, CInteractable>, CInteractable.Use);
            NavButton("A/P Hvr Mode", "Hover AP (Front)", ByName<VRInteractable, CInteractable>, CInteractable.Use);
            NavButton("A/P Hdg Hold", "Heading AP (Front)", ByName<VRInteractable, CInteractable>, CInteractable.Use);
            NavButton("A/P Alt Hold", "Altitude AP (Front)", ByName<VRInteractable, CInteractable>, CInteractable.Use);
            NavButton("A/P Off", "AP Off (Front)", ByName<VRInteractable, CInteractable>, CInteractable.Use);

            NavButton("A/P Alt Increase", "AP Alt + (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 77);
            NavButton("A/P Alt Decrease", "AP Alt - (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 78);

            NavButton("A/P Hdg Right", "AP Heading Right (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 75);
            NavButton("A/P Hdg Left", "AP Heading Left (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 76);

            NavButton("Clear Waypoint", "Clear Waypoint", ByManifest<VRButton, CButton>, CButton.Use, i: 97);
        }

        protected override void CreateSystemsControls()
        {
            SystemsButton("Clear Cautions", "Master Caution (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 0);

            SystemsButton("Master Arm Toggle", "Master Arm (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 69);
            SystemsButton("Master Safe Toggle", "Master Safe (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 70);

            SystemsButton("Fire Weapon", "Cyclic (Front)", Joysticks, CJoystick.Trigger);
            SystemsButton("Cycle Weapons", "Cyclic (Front)", Joysticks, CJoystick.MenuButton);

            SystemsButton("Articulate Auto Enable", "Coll Functions", ByType<AH94CollectiveFunctions, CCollFuncs>, CCollFuncs.ArticAuto, r: Cont);

            SystemsAxisC("Articulate Tilt", "Coll Functions", ByType<AH94CollectiveFunctions, CCollFuncs>, CCollFuncs.SetArtic, r: Cont);
            SystemsButton("Articulate Up", "Coll Functions", ByType<AH94CollectiveFunctions, CCollFuncs>, CCollFuncs.ArticUp, r: Cont);
            SystemsButton("Articulate Down", "Coll Functions", ByType<AH94CollectiveFunctions, CCollFuncs>, CCollFuncs.ArticDown, r: Cont);

            SystemsButton("Fire Countermeasures", "Coll Functions", ByType<AH94CollectiveFunctions, CCollFuncs>, CCollFuncs.FireCM, r: Cont);
            SystemsButton("Flares Toggle", "Toggle Flares (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 158);
            SystemsButton("Chaff Toggle", "Toggle Chaff (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 159);

            SystemsButton("Engine Left On", "Starter Left (Front)", ByManifest<VRLever, CLever>, CLever.Set, 2, i: 3);
            SystemsButton("Engine Left Off", "Starter Left (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 3);

            SystemsButton("Engine Right On", "Starter Right (Front)", ByManifest<VRLever, CLever>, CLever.Set, 2, i: 4);
            SystemsButton("Engine Right Off", "Starter Right (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 4);

            SystemsButton("Main Battery On", "Main Battery (Front)", ByManifest<VRLever, CLever>, CLever.Set, 2, i: 2);
            SystemsButton("Main Battery Off", "Main Battery (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 2);

            SystemsButton("APU On", "Auxilliary Power (Front)", ByManifest<VRLever, CLever>, CLever.Set, 2, i: 1);
            SystemsButton("APU Off", "Auxilliary Power (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 1);
        }

        protected override void CreateHUDControls()
        {
            HUDButton("Helmet Visor Toggle", "Helmet", HelmetController, CHelmet.ToggleVisor);
            HUDButton("Helmet Visor Open", "Helmet", HelmetController, CHelmet.OpenVisor);
            HUDButton("Helmet Visor Closed", "Helmet", HelmetController, CHelmet.CloseVisor);
            HUDButton("Helmet NV Toggle", "Helmet", HelmetController, CHelmet.ToggleNightVision);
            HUDButton("Helmet NV On", "Helmet", HelmetController, CHelmet.EnableNightVision);
            HUDButton("Helmet NV Off", "Helmet", HelmetController, CHelmet.DisableNightVision);

            HUDButton("HMD Power Toggle", "HMD Power (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 3);
            HUDButton("HMD Power On", "HMD Power (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 3);
            HUDButton("HMD Power Off", "HMD Power (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 3);

            HUDAxis("HMD Brightness", "HMD Brightness (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 4);
            HUDButton("HMD Brightness Increase", "HMD Brightness (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 4);
            HUDButton("HMD Brightness Decrease", "HMD Brightness (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 4);

            AddPostUpdateControl("HMD Brightness (Front)");
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

            DisplayButton("RWR Power Toggle", "RWR Power (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 10);
            DisplayButton("RWR Power On", "RWR Power (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 10);
            DisplayButton("RWR Power Off", "RWR Power (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 10);

            DisplayButton("UFD Power Toggle", "UFD", UFD, CUFD.PowerToggle);
            DisplayButton("UFD Power On", "UFD", UFD, CUFD.PowerOn);
            DisplayButton("UFD Power Off", "UFD", UFD, CUFD.PowerOff);

            DisplayButton("UFD Next Page", "UFD", UFD, CUFD.Next);
            DisplayButton("UFD Prev Page", "UFD", UFD, CUFD.Previous);
            DisplayButton("UFD Fuel Page", "UFD", UFD, CUFD.Fuel);
            DisplayButton("UFD Autopilot Page", "UFD", UFD, CUFD.AP);
            DisplayButton("UFD Status Page", "UFD", UFD, CUFD.Status);

            DisplayAxis("MFD Brightness", "MFD Brightness (Front)", ByName<VRTwistKnob, CKnob>, CKnob.Set);
            DisplayButton("MFD Brightness Increase", "MFD Brightness (Front)", ByName<VRTwistKnob, CKnob>, CKnob.Increase);
            DisplayButton("MFD Brightness Decrease", "MFD Brightness (Front)", ByName<VRTwistKnob, CKnob>, CKnob.Decrease);

            DisplayButton("MFD Swap", "Swap MFDs (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 61);

            DisplayButton("MFD Left Toggle", "MFDGunner2 Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 11);
            DisplayButton("MFD Left On", "MFDGunner2 Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 11);
            DisplayButton("MFD Left Off", "MFDGunner2 Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 11);
            DisplayButton("MFD Left L1", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.L1, i: 3);
            DisplayButton("MFD Left L2", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.L2, i: 3);
            DisplayButton("MFD Left L3", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.L3, i: 3);
            DisplayButton("MFD Left L4", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.L4, i: 3);
            DisplayButton("MFD Left L5", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.L5, i: 3);
            DisplayButton("MFD Left R1", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.R1, i: 3);
            DisplayButton("MFD Left R2", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.R2, i: 3);
            DisplayButton("MFD Left R3", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.R3, i: 3);
            DisplayButton("MFD Left R4", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.R4, i: 3);
            DisplayButton("MFD Left R5", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.R5, i: 3);
            DisplayButton("MFD Left T1", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.T1Home, i: 3);
            DisplayButton("MFD Left T2", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.T2, i: 3);
            DisplayButton("MFD Left T3", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.T3, i: 3);
            DisplayButton("MFD Left T4", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.T4, i: 3);
            DisplayButton("MFD Left T5", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.T5, i: 3);
            DisplayButton("MFD Left B1", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.B1, i: 3);
            DisplayButton("MFD Left B2", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.B2, i: 3);
            DisplayButton("MFD Left B3", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.B3, i: 3);
            DisplayButton("MFD Left B4", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.B4, i: 3);
            DisplayButton("MFD Left B5", "MFD Left", MFD, CMFD.Press, (int)MFDButtons.B5, i: 3);

            DisplayButton("MFD Center Toggle", "MFDGunner1 Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 12);
            DisplayButton("MFD Center On", "MFDGunner1 Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 12);
            DisplayButton("MFD Center Off", "MFDGunner1 Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 12);
            DisplayButton("MFD Center L1", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.L1, i: 2);
            DisplayButton("MFD Center L2", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.L2, i: 2);
            DisplayButton("MFD Center L3", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.L3, i: 2);
            DisplayButton("MFD Center L4", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.L4, i: 2);
            DisplayButton("MFD Center R1", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.R1, i: 2);
            DisplayButton("MFD Center R2", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.R2, i: 2);
            DisplayButton("MFD Center R3", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.R3, i: 2);
            DisplayButton("MFD Center R4", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.R4, i: 2);
            DisplayButton("MFD Center T1", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.T1Home, i: 2);
            DisplayButton("MFD Center T2", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.T2, i: 2);
            DisplayButton("MFD Center T3", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.T3, i: 2);
            DisplayButton("MFD Center T4", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.T4, i: 2);
            DisplayButton("MFD Center B1", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.B1, i: 2);
            DisplayButton("MFD Center B2", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.B2, i: 2);
            DisplayButton("MFD Center B3", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.B3, i: 2);
            DisplayButton("MFD Center B4", "MFD Center", MFD, CMFD.Press, (int)MFDButtons.B4, i: 2);

            DisplayButton("MFD Right Toggle", "MFDGunner3 Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 13);
            DisplayButton("MFD Right On", "MFDGunner3 Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 13);
            DisplayButton("MFD Right Off", "MFDGunner3 Power", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 13);
            DisplayButton("MFD Right L1", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.L1, i: 4);
            DisplayButton("MFD Right L2", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.L2, i: 4);
            DisplayButton("MFD Right L3", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.L3, i: 4);
            DisplayButton("MFD Right L4", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.L4, i: 4);
            DisplayButton("MFD Right L5", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.L5, i: 4);
            DisplayButton("MFD Right R1", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.R1, i: 4);
            DisplayButton("MFD Right R2", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.R2, i: 4);
            DisplayButton("MFD Right R3", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.R3, i: 4);
            DisplayButton("MFD Right R4", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.R4, i: 4);
            DisplayButton("MFD Right R5", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.R5, i: 4);
            DisplayButton("MFD Right T1", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.T1Home, i: 4);
            DisplayButton("MFD Right T2", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.T2, i: 4);
            DisplayButton("MFD Right T3", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.T3, i: 4);
            DisplayButton("MFD Right T4", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.T4, i: 4);
            DisplayButton("MFD Right T5", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.T5, i: 4);
            DisplayButton("MFD Right B1", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.B1, i: 4);
            DisplayButton("MFD Right B2", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.B2, i: 4);
            DisplayButton("MFD Right B3", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.B3, i: 4);
            DisplayButton("MFD Right B4", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.B4, i: 4);
            DisplayButton("MFD Right B5", "MFD Right", MFD, CMFD.Press, (int)MFDButtons.B5, i: 4);

            AddPostUpdateControl("MFD Brightness (Front)");
            AddPostUpdateControl("SOI");
        }

        protected override void CreateRadioControls()
        {
            RadioButton("Radio Transmit", "Radio", ByType<CockpitTeamRadioManager, CRadio>, CRadio.Transmit, r: Dash, n: true);

            RadioButton("Radio Channel Cycle", "Radio Channel (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 1);
            RadioButton("Radio Channel Team", "Radio Channel (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 1);
            RadioButton("Radio Channel Global", "Radio Channel (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 1);

            RadioButton("Radio Mode Cycle", "Radio Mode (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 0);
            RadioButton("Radio Mode Next", "Radio Mode (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Next, i: 0);
            RadioButton("Radio Mode Prev", "Radio Mode (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Prev, i: 0);
            RadioButton("Radio Mode Hot Mic", "Radio Mode (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 0);
            RadioButton("Radio Mode PTT", "Radio Mode (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 0);
            RadioButton("Radio Mode Off", "Radio Mode (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 2, i: 0);

            RadioAxis("Radio Volume", "Comm Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 1);
            RadioButton("Radio Volume Increase", "Comm Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 1);
            RadioButton("Radio Volume Decrease", "Comm Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 1);

            RadioAxis("Radio Team Volume", "Team Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 2, n: true);
            RadioButton("Radio Team Volume Increase", "Team Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 2, n: true);
            RadioButton("Radio Team Volume Decrease", "Team Radio Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 2, n: true);

            RadioButton("Intercom Toggle", "Intra Comms (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Cycle, i: 2, n: true);
            RadioButton("Intercom On", "Intra Comms (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 0, i: 2, n: true);
            RadioButton("Intercom Off", "Intra Comms (Front)", ByManifest<VRTwistKnobInt, CKnobInt>, CKnobInt.Set, 1, i: 2, n: true);

            RadioAxis("Intercom Volume", "Intra Comm Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 3, n: true);
            RadioButton("Intercom Volume Increase", "Intra Comm Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 3, n: true);
            RadioButton("Intercom Volume Decrease", "Intra Comm Volume (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 3, n: true);

            AddPostUpdateControl("Comm Radio Volume (Front)");
            AddPostUpdateControl("Team Radio Volume (Front)");
            AddPostUpdateControl("Intra Comm Volume (Front)");
        }

        protected override void CreateMusicControls()
        {
        }

        protected override void CreateLightsControls()
        {
            LightsButton("Interior Lights Toggle", "Interior Lights (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 5);
            LightsButton("Interior Lights On", "Interior Lights (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 5);
            LightsButton("Interior Lights Off", "Interior Lights (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 5);

            LightsButton("Instrument Lights Toggle", "Instrument Lights (Front)", ByManifest<VRLever, CLever>, CLever.Cycle, i: 6, n: true);
            LightsButton("Instrument Lights On", "Instrument Lights (Front)", ByManifest<VRLever, CLever>, CLever.Set, 1, i: 6, n: true);
            LightsButton("Instrument Lights Off", "Instrument Lights (Front)", ByManifest<VRLever, CLever>, CLever.Set, 0, i: 6, n: true);
            LightsAxis("Instrument Brightness", "Instrument Brightness (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Set, i: 0, n: true);
            LightsButton("Instrument Brightness Increase", "Instrument Brightness (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Increase, i: 0, n: true);
            LightsButton("Instrument Brightness Decrease", "Instrument Brightness (Front)", ByManifest<VRTwistKnob, CKnob>, CKnob.Decrease, i: 0, n: true);

            AddPostUpdateControl("Instrument Brightness (Front)");
        }

        protected override void CreateMiscControls()
        {
            MiscButton("Door Left Toggle", "Door Handle (Front L)", ByManifest<VRDoor, CDoor>, CDoor.Toggle, i: 4);
            MiscButton("Door Left Open", "Door Handle (Front L)", ByManifest<VRDoor, CDoor>, CDoor.Open, i: 4);
            MiscButton("Door Left Close", "Door Handle (Front L)", ByManifest<VRDoor, CDoor>, CDoor.Close, i: 4);

            MiscButton("Door Right Toggle", "Door Handle (Front R)", ByManifest<VRDoor, CDoor>, CDoor.Toggle, i: 6);
            MiscButton("Door Right Open", "Door Handle (Front R)", ByManifest<VRDoor, CDoor>, CDoor.Open, i: 6);
            MiscButton("Door Right Close", "Door Handle (Front R)", ByManifest<VRDoor, CDoor>, CDoor.Close, i: 6);

            MiscButton("Switch Seat", "Switch Seat (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 157, n: true);
            MiscButton("Raise Seat", "Raise Seat", ByName<VRButton, CButton>, CButton.Use, r: Dash);
            MiscButton("Lower Seat", "Lower Seat", ByName<VRButton, CButton>, CButton.Use, r: Dash);
            MiscButton("Eject", "Eject", ByType<EjectHandle, CEject>, CEject.Pull, r: EjectHandle);

            MiscButton("Jettison Execute", "Jettison (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 68);
            MiscButton("Jettison L TIP", "Mark Jett L TIP (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 66);
            MiscButton("Jettison L OUT", "Mark Jett L OUT (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 62);
            MiscButton("Jettison L IN", "Mark Jett L IN (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 63);
            MiscButton("Jettison R IN", "Mark Jett R IN (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 64);
            MiscButton("Jettison R OUT", "Mark Jett R OUT (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 65);
            MiscButton("Jettison R TIP", "Mark Jett R TIP (Front)", ByManifest<VRButton, CButton>, CButton.Use, i: 67);
        }

        private CUFD UFD(string name, string root, bool nullable, bool checkName, int idx)
        {
            if (TryGetExistingControl<CUFD>(name, out var existingControl))
                return existingControl;
            var switcher = GetGameObject(Dash).GetComponentInChildren<MultiObjectSwitcher>();
            if (switcher == null)
                throw new InvalidOperationException("Could not find UFD switcher");

            var powerUnit = GetGameObject(UpFrontDisplay).GetComponentInChildren<ObjectPowerUnit>();
            if (powerUnit == null)
                throw new InvalidOperationException("Could not find UFD power unit");

            var powerButton = ByManifest<VRButton, CButton>("UFD Power (Front)", null, false, true, 73);
            if (powerButton == null)
                throw new InvalidOperationException("Could not find UFD power button");

            var fuelPageButton = ByManifest<VRButton, CButton>("UFD Fuel (Front)", null, false, true, 71);
            if (fuelPageButton == null)
                throw new InvalidOperationException("Could not find UFD fuel page button");

            var apPageButton = ByManifest<VRButton, CButton>("UFD Autopilot (Front)", null, false, true, 72);
            if (apPageButton == null)
                throw new InvalidOperationException("Could not find UFD autopilot page button");

            var statusPageButton = ByManifest<VRButton, CButton>("UFD Status (Front)", null, false, true, 74);
            if (statusPageButton == null)
                throw new InvalidOperationException("Could not find UFD status page button");

            var cufd = new CUFD(switcher, powerUnit, powerButton, fuelPageButton, apPageButton, statusPageButton);
            Controls.Add(name, cufd);
            return cufd;
        }
    }
}