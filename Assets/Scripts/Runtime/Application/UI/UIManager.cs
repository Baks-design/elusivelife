using Cysharp.Threading.Tasks;
using ElusiveLife.Runtime.Application.Game_State;
using ElusiveLife.Runtime.Utils.Helpers;
using VContainer;

namespace ElusiveLife.Runtime.Application.UI
{
    public class UIManager : IUIManager
    {
        private readonly IGameLifecycle _gameLifecycle;
        private readonly SlotSelectionUI _slotSelectionUI;
        private bool _isSlotSelectionVisible = false;

        [Inject]
        public UIManager(IGameLifecycle gameLifecycle, SlotSelectionUI slotSelectionUI)
        {
            _gameLifecycle = gameLifecycle;
            _slotSelectionUI = slotSelectionUI;

            _gameLifecycle.OnStateChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState state) => HandleStateChange(state).Forget();

        private async UniTask HandleStateChange(GameState state)
        {
            switch (state)
            {
                case GameState.MainMenu:
                    await HideGameHUD();
                    await HideSlotSelection();
                    await ShowMainMenu();
                    break;
                case GameState.Playing:
                    await HideMainMenu();
                    await HideSlotSelection();
                    await ShowGameHUD();
                    break;
            }
        }

        public UniTask ShowMainMenu()
        {
            Logging.Log("Showing Main Menu");
            // Actual UI implementation would show main menu panel
            return UniTask.CompletedTask;
        }

        public UniTask HideMainMenu()
        {
            Logging.Log("Hiding Main Menu");
            // Actual UI implementation would hide main menu panel
            return UniTask.CompletedTask;
        }

        public UniTask ShowGameHUD()
        {
            Logging.Log("Showing Game HUD");
            // Actual UI implementation would show game HUD
            return UniTask.CompletedTask;
        }

        public UniTask HideGameHUD()
        {
            Logging.Log("Hiding Game HUD");
            // Actual UI implementation would hide game HUD
            return UniTask.CompletedTask;
        }

        public UniTask ShowSlotSelection()
        {
            _slotSelectionUI.ShowSlotSelection();
            return UniTask.CompletedTask;
        }

        public UniTask HideSlotSelection()
        {
            _slotSelectionUI.HideSlotSelection();
            return UniTask.CompletedTask;
        }

        public void OnSlotSelectionShown()
        {
            _isSlotSelectionVisible = true;
            Logging.Log("Slot selection shown");
        }

        public void OnSlotSelectionHidden()
        {
            _isSlotSelectionVisible = false;
            Logging.Log("Slot selection hidden");
        }

        public void UpdateScore(int score)
        {
            Logging.Log($"Score Updated: {score}");
            // Update score display in HUD
        }

        ~UIManager()
        {
            // Clean up events
            _gameLifecycle.OnStateChanged -= OnGameStateChanged;
        }
    }
}