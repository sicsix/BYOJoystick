using System;
using System.Collections;
using UnityEngine;
using VTOLVR.Multiplayer;

namespace BYOJoystick.Controls.Sync
{
    public class ThrottleGrabHandler
    {
        public bool IsGrabbed { get; private set; }

        private readonly VRInteractable                  _interactable;
        private readonly VRThrottle                      _throttle;
        private readonly ConnectedThrottles              _tSync;
        private readonly int                             _ctrlIdx;
        private readonly MultiUserVehicleSync            _muvs;
        private readonly Action<ConnectedThrottles, int> _onLocalGrabbedThrottle;
        private readonly Action<ConnectedThrottles, int> _onLocalReleasedThrottle;
        private          float                           _grabTimer;
        private          float                           _grabTime;

        private ThrottleGrabHandler(VRThrottle           throttle,
                                    ConnectedThrottles   tSync,
                                    int                  ctrlIdx,
                                    MultiUserVehicleSync muvs,
                                    VRInteractable       interactable)
        {
            _interactable            = interactable;
            _throttle                = throttle;
            _tSync                   = tSync;
            _ctrlIdx                 = ctrlIdx;
            _muvs                    = muvs;
            _onLocalGrabbedThrottle  = CompiledExpressions.CreateEventInvoker<ConnectedThrottles>("OnLocalGrabbedThrottle");
            _onLocalReleasedThrottle = CompiledExpressions.CreateEventInvoker<ConnectedThrottles>("OnLocalReleasedThrottle");
        }

        public static ThrottleGrabHandler Create(VRThrottle throttle, ConnectedThrottles tSync, MultiUserVehicleSync muvs, VRInteractable interactable)
        {
            if (tSync == null)
            {
                Plugin.Log("ConnectedThrottles is null");
                return null;
            }

            int ctrlIdx = tSync.throttles.IndexOf(throttle);
            if (ctrlIdx == -1)
                Plugin.Log($"Throttle {interactable.GetControlReferenceName()} not found in ConnectedThrottles");
            else
                Plugin.Log($"Creating ThrottleGrabHandler for {interactable.GetControlReferenceName()}");

            return ctrlIdx == -1 ? null : new ThrottleGrabHandler(throttle, tSync, ctrlIdx, muvs, interactable);
        }

        public void GrabThrottle(float time)
        {
            _grabTime  = time;
            _grabTimer = 0;
            if (IsGrabbed)
                return;

            IsGrabbed = true;
            _tSync.OnGrabbedThrottle(_ctrlIdx);
            _muvs.OnControlInteract(_interactable);
            _onLocalGrabbedThrottle.Invoke(_tSync, _ctrlIdx);
            _interactable.StartCoroutine(GrabbedEnumerator());
        }

        private IEnumerator GrabbedEnumerator()
        {
            while (true)
            {
                _grabTimer += Time.deltaTime;
                if (_grabTimer > _grabTime || !IsGrabbed)
                {
                    ReleaseThrottle();
                    yield break;
                }

                _muvs.OnControlInteracting();
                yield return null;
            }
        }

        private void ReleaseThrottle()
        {
            IsGrabbed = false;
            _tSync.OnReleasedThrottle(_ctrlIdx);
            _muvs.OnControlStopInteract(_interactable);
            _onLocalReleasedThrottle.Invoke(_tSync, _ctrlIdx);
        }
    }
}