using Cysharp.Threading.Tasks;
using ElusiveLife.Runtime.Application.Persistence;
using ElusiveLife.Runtime.Application.UI;
using ElusiveLife.Runtime.Utils.Helpers;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace ElusiveLife.Runtime.Application.Game_State
{
    public class SlotSelectionUI : MonoBehaviour
    {
        [SerializeField] private GameObject slotSelectionPanel;
        [SerializeField] private Transform slotListContainer;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button newSlotButton;
        [SerializeField] private GameObject slotButtonPrefab;
        
        [Inject] private readonly ISaveManager _saveManager;
        [Inject] private readonly IUIManager _uiManager;
        [Inject] private readonly IGameLifecycle _gameLifecycle;

        private void Start()
        {
            // Register button events
            closeButton.onClick.AddListener(OnCloseClicked);
            newSlotButton.onClick.AddListener(OnNewSlotClicked);
            
            // Hide panel initially
            slotSelectionPanel.SetActive(false);
        }

        public async void ShowSlotSelection()
        {
            await RefreshSlotList();
            slotSelectionPanel.SetActive(true);
            
            // Notify UI manager
            _uiManager.OnSlotSelectionShown();
        }

        public void HideSlotSelection()
        {
            slotSelectionPanel.SetActive(false);
            _uiManager.OnSlotSelectionHidden();
        }

        private async UniTask RefreshSlotList()
        {
            // Clear existing slot buttons
            foreach (Transform child in slotListContainer)
                Destroy(child.gameObject);

            // Create buttons for each save slot
            foreach (var slotName in _saveManager.SaveSlots)
            {
                var slotButtonObj = Instantiate(slotButtonPrefab, slotListContainer);
                if (slotButtonObj.TryGetComponent<SlotButton>(out var slotButton))
                {
                    // Load slot data to display info
                    var exists = _saveManager.SaveExists(slotName);
                    var slotInfo = exists ? await GetSlotInfo(slotName) : "Empty Slot";
                    
                    slotButton.Setup(slotName, slotInfo, exists, this);
                }
            }
        }

        private async UniTask<string> GetSlotInfo(string slotName)
        {
            var tempService = new DataService($"save_{slotName}.json");
            await tempService.LoadGameAsync();
            
            var data = tempService.PlayerData;
            return $"{data.PlayerName}";
        }

        public async void OnSlotSelected(string slotName)
        {
            await _saveManager.SwitchSlotAsync(slotName);
            await _saveManager.LoadGameAsync(slotName);
            
            Logging.Log($"Loaded slot: {slotName}");
            
            // Show loading screen or transition
            await _uiManager.HideMainMenu();
            await _gameLifecycle.ChangeState(GameState.Playing);
            
            HideSlotSelection();
        }

        public async void OnDeleteSlot(string slotName)
        {
            await _saveManager.DeleteSaveAsync(slotName);
            await RefreshSlotList();
            Logging.Log($"Deleted slot: {slotName}");
        }

        private async void OnNewSlotClicked()
        {
            var newSlotName = $"Slot_{_saveManager.SaveSlots.Count + 1}";
            await _saveManager.SwitchSlotAsync(newSlotName);
            
            // Initialize new slot with default data
            await _saveManager.SaveGameAsync(newSlotName);
            
            await RefreshSlotList();
            Logging.Log($"Created new slot: {newSlotName}");
        }

        private void OnCloseClicked() => HideSlotSelection();

        private void OnDisable()
        {
            // Clean up event listeners
            closeButton.onClick.RemoveAllListeners();
            newSlotButton.onClick.RemoveAllListeners();
        }
    }
}