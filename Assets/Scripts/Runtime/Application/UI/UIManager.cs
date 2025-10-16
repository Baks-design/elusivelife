using Cysharp.Threading.Tasks;
using ElusiveLife.Runtime.Application.Game_State;
using ElusiveLife.Runtime.Utils.Helpers;
using VContainer;

namespace ElusiveLife.Runtime.Application.UI
{
    public class UIManager : IUIManager
    {
        private readonly IGameLifecycle _gameLifecycle;

        [Inject]
        public UIManager(IGameLifecycle gameLifecycle)
        {
            _gameLifecycle = gameLifecycle;
            _gameLifecycle.OnStateChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState state) => HandleStateChange(state).Forget();

        private async UniTask HandleStateChange(GameState state)
        {
            switch (state)
            {
                case GameState.MainMenu:
                    await HideGameHUD();
                    await ShowMainMenu();
                    break;
                case GameState.Playing:
                    await HideMainMenu();
                    await ShowGameHUD();
                    break;
            }
        }

        public UniTask ShowMainMenu()
        {
            Logging.Log("Showing Main Menu");
            return UniTask.CompletedTask;
        }

        public UniTask HideMainMenu()
        {
            Logging.Log("Hiding Main Menu");
            return UniTask.CompletedTask;
        }

        public UniTask ShowGameHUD()
        {
            Logging.Log("Showing Game HUD");
            return UniTask.CompletedTask;
        }

        public UniTask HideGameHUD()
        {
            Logging.Log("Hiding Game HUD");
            return UniTask.CompletedTask;
        }
    }
}