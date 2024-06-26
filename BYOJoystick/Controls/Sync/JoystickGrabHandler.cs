using System;
using System.Collections;
using VTOLVR.Multiplayer;

namespace BYOJoystick.Controls.Sync
{
    public class JoystickGrabHandler
    {
        public bool IsGrabbed { get; private set; }

        private readonly VRInteractable                  _interactable;
        private readonly VRJoystick                      _joystick;
        private readonly ConnectedJoysticks              _jSync;
        private readonly MultiUserVehicleSync            _muvs;
        private readonly int                             _ctrlIdx;
        private readonly Action<ConnectedJoysticks, int> _onLocalGrabbedStick;
        private readonly Action<ConnectedJoysticks, int> _onLocalReleasedStick;

        private JoystickGrabHandler(VRJoystick           joystick,
                                    ConnectedJoysticks   jSync,
                                    int                  ctrlIdx,
                                    MultiUserVehicleSync muvs,
                                    VRInteractable       interactable)
        {
            _interactable         = interactable;
            _joystick             = joystick;
            _jSync                = jSync;
            _muvs                 = muvs;
            _ctrlIdx              = ctrlIdx;
            _onLocalGrabbedStick  = CompiledExpressions.CreateEventInvoker<ConnectedJoysticks>("OnLocalGrabbedStick");
            _onLocalReleasedStick = CompiledExpressions.CreateEventInvoker<ConnectedJoysticks>("OnLocalReleasedStick");
        }

        public static JoystickGrabHandler Create(VRJoystick joystick, ConnectedJoysticks jSync, MultiUserVehicleSync muvs, VRInteractable interactable)
        {
            int ctrlIdx = jSync.joysticks.IndexOf(joystick);
            if (ctrlIdx == -1)
                Plugin.Log($"Joystick {interactable.GetControlReferenceName()} not found in ConnectedJoysticks");
            else
                Plugin.Log($"Creating JoystickGrabHandler for {interactable.GetControlReferenceName()}");

            return ctrlIdx == -1 ? null : new JoystickGrabHandler(joystick, jSync, ctrlIdx, muvs, interactable);
        }

        public void GrabStick()
        {
            IsGrabbed = true;
            _jSync.OnGrabbedStick(_ctrlIdx);
            _muvs.OnControlInteract(_interactable);
            _onLocalGrabbedStick.Invoke(_jSync, _ctrlIdx);
            _interactable.StartCoroutine(GrabbedEnumerator());
        }

        private IEnumerator GrabbedEnumerator()
        {
            while (IsGrabbed)
            {
                _muvs.OnControlInteracting();
                yield return null;
            }
        }

        public void ReleaseStick()
        {
            IsGrabbed = false;
            _jSync.OnReleasedStick(_ctrlIdx);
            _muvs.OnControlStopInteract(_interactable);
            _onLocalReleasedStick.Invoke(_jSync, _ctrlIdx);
        }
    }
}