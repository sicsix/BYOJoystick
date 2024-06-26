using System.Collections;
using BYOJoystick.Bindings;
using UnityEngine;

namespace BYOJoystick.Controls
{
    public class CDoor : IControl
    {
        protected readonly VRInteractable Interactable;
        protected readonly VRDoor         Door;
        protected          bool           IsOpening;
        protected          bool           IsClosing;
        protected          IEnumerator    CurrentCoroutine;

        private const float TotalOperateTime = 1.25f;

        public CDoor(VRInteractable interactable, VRDoor door)
        {
            Interactable = interactable;
            Door         = door;
        }

        public void PostUpdate()
        {
        }

        public static void Open(CDoor c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                if (c.CurrentCoroutine != null)
                    c.Door.StopCoroutine(c.CurrentCoroutine);
                c.CurrentCoroutine = c.OpenDoor();
                c.Door.StartCoroutine(c.CurrentCoroutine);
            }
        }

        public static void Close(CDoor c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                if (c.CurrentCoroutine != null)
                    c.Door.StopCoroutine(c.CurrentCoroutine);
                c.CurrentCoroutine = c.CloseDoor();
                c.Door.StartCoroutine(c.CurrentCoroutine);
            }
        }

        public static void Toggle(CDoor c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                if (c.IsOpening)
                {
                    c.Door.StopCoroutine(c.CurrentCoroutine);
                    c.CurrentCoroutine = c.CloseDoor();
                    c.Door.StartCoroutine(c.CurrentCoroutine);
                }
                else if (c.IsClosing)
                {
                    c.Door.StopCoroutine(c.CurrentCoroutine);
                    c.CurrentCoroutine = c.OpenDoor();
                    c.Door.StartCoroutine(c.CurrentCoroutine);
                }
                else
                {
                    c.CurrentCoroutine = c.Door.currentAngle > 0 ? c.CloseDoor() : c.OpenDoor();
                    c.Door.StartCoroutine(c.CurrentCoroutine);
                }
            }
        }

        private IEnumerator OpenDoor()
        {
            IsOpening = true;
            float state = Door.currentAngle / Door.maxDoorAngle;
            while (state < 1)
            {
                state += Time.deltaTime * 1 / TotalOperateTime;
                state =  Mathf.Clamp01(state);
                Door.RemoteSetState(state);
                yield return null;
            }

            IsOpening = false;
        }

        private IEnumerator CloseDoor()
        {
            IsClosing = true;
            float state = Door.currentAngle / Door.maxDoorAngle;
            while (state > 0)
            {
                state -= Time.deltaTime * 1 / TotalOperateTime;
                state =  Mathf.Clamp01(state);
                Door.RemoteSetState(state);
                yield return null;
            }

            IsClosing = false;
        }
    }
}