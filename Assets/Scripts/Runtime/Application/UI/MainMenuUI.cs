using ElusiveLife.Runtime.Application.Game_State;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace ElusiveLife.Runtime.Application.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button loadButton;
        [SerializeField] private Button quitButton;
        
        [Inject] private readonly IUIManager _uiManager;
        [Inject] private readonly IGameLifecycle _gameLifecycle;

        private void OnEnable()
        {
            startButton.onClick.AddListener(OnStartClicked);
            loadButton.onClick.AddListener(OnLoadClicked);
            quitButton.onClick.AddListener(OnQuitClicked);
        }

        private async void OnStartClicked() => await _gameLifecycle.ChangeState(GameState.Playing);

        private async void OnLoadClicked() => await _uiManager.ShowSlotSelection();

        private void OnQuitClicked() => UnityEngine.Application.Quit();

        private void OnDisable()
        {
            startButton.onClick.RemoveAllListeners();
            loadButton.onClick.RemoveAllListeners();
            quitButton.onClick.RemoveAllListeners();
        }
    }
}