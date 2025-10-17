using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ElusiveLife.Runtime.Application.Game_State
{
    public class SlotButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI slotNameText;
        [SerializeField] private TextMeshProUGUI slotInfoText;
        [SerializeField] private Button selectButton;
        [SerializeField] private Button deleteButton;
        [SerializeField] private GameObject emptyIndicator;
        private string _slotName;
        private SlotSelectionUI _parentUI;

        public void Setup(string slotName, string slotInfo, bool slotExists, SlotSelectionUI parentUI)
        {
            _slotName = slotName;
            _parentUI = parentUI;

            slotNameText.text = slotName;
            slotInfoText.text = slotInfo;
            emptyIndicator.SetActive(!slotExists);
            deleteButton.gameObject.SetActive(slotExists);

            // Set up button events
            selectButton.onClick.AddListener(OnSelectClicked);
            deleteButton.onClick.AddListener(OnDeleteClicked);
        }

        private void OnSelectClicked() => _parentUI.OnSlotSelected(_slotName);

        private void OnDeleteClicked() => _parentUI.OnDeleteSlot(_slotName);

        private void OnDisable()
        {
            selectButton.onClick.RemoveAllListeners();
            deleteButton.onClick.RemoveAllListeners();
        }
    }
}