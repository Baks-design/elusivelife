using System;
using Cysharp.Threading.Tasks;
using ElusiveLife.Runtime.Application.Input.Interfaces;
using ElusiveLife.Runtime.Application.Persistence;
using ElusiveLife.Runtime.Utils.Helpers;
using VContainer;
using VContainer.Unity;

namespace ElusiveLife.Runtime.Application.Game_State
{
    public class GameLifecycle : IGameLifecycle, IInitializable, ITickable
    {
        private readonly IInputSystemManager _inputSystemManager;
        private readonly ISaveManager _saveManager;

        public GameState CurrentState { get; private set; } = GameState.Initializing;

        public event Action<GameState> OnStateChanged;

        [Inject]
        public GameLifecycle(
            IInputSystemManager inputSystemManager,
            ISaveManager saveManager) 
        {
            _inputSystemManager = inputSystemManager;
            _saveManager = saveManager;
        }

        public void Initialize()
        {
            Logging.Log("Game Initialized");
            StartGameFlow().Forget();
        } 

        public void Tick() { }

        private async UniTask StartGameFlow()
        {
            // Load the default save slot
            await _saveManager.LoadGameAsync("default");
            await ChangeState(GameState.Playing);
        }

        public async UniTask ChangeState(GameState newState)
        {
            if (CurrentState == newState) return;

            // Auto-save when leaving certain states
            if (ShouldAutoSave(CurrentState))
                await _saveManager.SaveGameAsync(_saveManager.CurrentSlot);

            await ExitState(CurrentState);

            var previousState = CurrentState;
            CurrentState = newState;

            await EnterState(newState);

            OnStateChanged?.Invoke(newState);
            Logging.Log($"GameState changed: {previousState} -> {newState}");
        }

        private bool ShouldAutoSave(GameState state)
            => state == GameState.Playing || state == GameState.GameOver;

        private UniTask ExitState(GameState state) 
        => state switch
        {
            GameState.MainMenu => ExitMainMenu(),
            GameState.Playing => ExitPlaying(),
            GameState.Paused => ExitPaused(),
            _ => UniTask.CompletedTask
        };

        private UniTask EnterState(GameState state) 
        => state switch
        {
            GameState.MainMenu => EnterMainMenu(),
            GameState.Playing => EnterPlaying(),
            GameState.Paused => EnterPaused(),
            _ => UniTask.CompletedTask
        };

        private UniTask EnterMainMenu()
        {
            GameSystem.IsShowCursor(true);
            GameSystem.IsTimeActive(false);
            _inputSystemManager.SwitchToUiInput();
            return UniTask.CompletedTask;
        }

        private UniTask ExitMainMenu() => UniTask.CompletedTask;

        private UniTask EnterPlaying()
        {
            GameSystem.IsShowCursor(false);
            GameSystem.IsTimeActive(true);
            _inputSystemManager.SwitchToPlayerInput();
            return UniTask.CompletedTask;
        }

        private UniTask ExitPlaying() => UniTask.CompletedTask;

        private UniTask EnterPaused()
        {
            GameSystem.IsShowCursor(true);
            GameSystem.IsTimeActive(false);
            _inputSystemManager.SwitchToUiInput();
            return UniTask.CompletedTask;
        }

        private UniTask ExitPaused() => UniTask.CompletedTask;
    }
}