using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BYOJoystick.UI.Scripts
{
    public class VehicleSelector : MonoBehaviour
    {
        public string          GameName;
        public string          ShortName;
        public TextMeshProUGUI SelectText;
        public GameObject      SaveLoadPanel;
        public Button          SelectButton;
        public Button          SaveButton;
        public Button          LoadButton;
        public GameObject      CopyFromPanel;
        public Button          CopyFromButton;

        public void Initialise(string         gameName,
                               string         shortName,
                               Action<string> onSelect,
                               Action<string> onSave,
                               Action<string> onLoad,
                               Action<string> onCopyFrom)
        {
            GameName        = gameName;
            ShortName       = shortName;
            SelectText.text = gameName;

            SelectButton.onClick.AddListener(() =>
            {
                onSelect(ShortName);
            });

            SaveButton.onClick.AddListener(() =>
            {
                onSave(ShortName);
            });

            LoadButton.onClick.AddListener(() =>
            {
                onLoad(ShortName);
            });

            CopyFromButton.onClick.AddListener(() =>
            {
                onCopyFrom(ShortName);
            });
        }

        public void SetSelected(bool selected)
        {
            SaveLoadPanel.SetActive(selected);
            CopyFromPanel.SetActive(!selected);
        }
    }
}