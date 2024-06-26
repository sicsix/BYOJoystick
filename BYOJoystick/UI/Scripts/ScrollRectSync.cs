using UnityEngine;
using UnityEngine.UI;

namespace BYOJoystick.UI.Scripts
{
    public class ScrollRectSync : MonoBehaviour
    {
        public ScrollRect SyncFrom;

        private void Start()
        {
            SyncFrom.onValueChanged.AddListener(OnScroll);
            enabled = false;
        }

        private void OnScroll(Vector2 position)
        {
            GetComponent<ScrollRect>().verticalNormalizedPosition = position.y;
        }
    }
}