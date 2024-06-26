using System.Collections;
using UnityEngine;

namespace BYOJoystick.Controls.Sync
{
    public class InteractableSyncWrapper
    {
        public bool IsInteracting { get; private set; }

        private readonly VRInteractable     _interactable;
        private readonly VRInteractableSync _iSync;
        private          float              _interactTimer;
        private          float              _interactTime;

        private InteractableSyncWrapper(VRInteractableSync iSync, VRInteractable interactable)
        {
            _iSync        = iSync;
            _interactable = interactable;
        }

        public static InteractableSyncWrapper Create(VRInteractable interactable)
        {
            var iSync = interactable.GetComponent<VRInteractableSync>();
            if (iSync != null)
                Plugin.Log($"Creating InteractableSyncWrapper for {interactable.name}");
            return iSync == null ? null : new InteractableSyncWrapper(iSync, interactable);
        }

        public bool TryInteractTimed(bool isRightCon, float time)
        {
            if (IsInteracting)
            {
                SetTimer(time);
                return true;
            }

            if (!TryInteracting(isRightCon))
            {
                _interactTime = 0;
                return false;
            }

            StartCoroutine(time);
            return true;
        }

        public bool TryInteracting(bool isRightCon)
        {
            ulong exclusiveUser = _iSync.GetCurrentExclusiveUser();
            if (_iSync.exclusive && exclusiveUser != 0L && exclusiveUser != BDSteamClient.mySteamID)
            {
                IsInteracting = false;
                return false;
            }

            _iSync.SendInteractRPC(isRightCon, true);

            if (!_iSync.exclusive || !_iSync.isMine)
            {
                IsInteracting = true;
                return true;
            }

            if (exclusiveUser == 0L)
            {
                _iSync.SendRPC("SetExclusiveUser", exclusiveUser = BDSteamClient.mySteamID);
                _iSync.SetExclusiveUser(BDSteamClient.mySteamID);
                IsInteracting = true;
                return true;
            }

            if (exclusiveUser != BDSteamClient.mySteamID)
            {
                IsInteracting = false;
                return false;
            }

            IsInteracting = true;
            return true;
        }

        public void StopInteracting(bool isRightCon)
        {
            IsInteracting = false;
            ulong exclusiveUser = _iSync.GetCurrentExclusiveUser();
            _iSync.SendInteractRPC(isRightCon, false);
            if (!_iSync.exclusive || exclusiveUser != BDSteamClient.mySteamID)
                return;

            _iSync.SendRPC("SetExclusiveUser", 0);
            _iSync.SetExclusiveUser(0);
        }

        private void SetTimer(float time)
        {
            _interactTime  = time;
            _interactTimer = 0;
        }

        private void StartCoroutine(float time)
        {
            SetTimer(time);
            _iSync.StartCoroutine(InteractingCoroutine());
        }

        private IEnumerator InteractingCoroutine()
        {
            while (true)
            {
                _interactTimer += Time.deltaTime;
                if (_interactTimer > _interactTime || !IsInteracting)
                {
                    StopInteracting(false);
                    _interactTimer = 0;
                    yield break;
                }

                yield return null;
            }
        }
    }
}