using System.Collections.Generic;
using BYOJoystick.Actions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BYOJoystick.UI.Scripts
{
    public class CategoryActions : MonoBehaviour
    {
        public TextMeshProUGUI CategoryHeader;
        public GameObject      ActionsList;
        public GameObject      ActionHeaderPrefab;

        public void Initialise(ActionCategory category, List<ControlAction> actions)
        {
            CategoryHeader.text = category.ToString();

            foreach (Transform child in ActionsList.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (var action in actions)
            {
                var actionHeader = Instantiate(ActionHeaderPrefab, ActionsList.transform);
                actionHeader.GetComponentInChildren<TextMeshProUGUI>().text = action.Name;
            }

            var categoryActionsSize = GetComponent<RectTransform>().sizeDelta;
            var categoryHeaderSize  = CategoryHeader.transform.parent.GetComponent<RectTransform>().sizeDelta;
            var actionListSize      = ActionsList.GetComponent<RectTransform>().sizeDelta;
            var actionHeaderSize    = ActionHeaderPrefab.GetComponent<RectTransform>().sizeDelta;

            float actionsListHeight = actionHeaderSize.y * actions.Count + ActionsList.GetComponent<VerticalLayoutGroup>().spacing * (actions.Count - 1);

            float thisHeight = categoryHeaderSize.y + actionsListHeight + GetComponent<VerticalLayoutGroup>().spacing + 2;

            ActionsList.GetComponent<RectTransform>().sizeDelta = new Vector2(actionListSize.x, actionsListHeight);
            GetComponent<RectTransform>().sizeDelta             = new Vector2(categoryActionsSize.x, thisHeight);
        }
    }
}