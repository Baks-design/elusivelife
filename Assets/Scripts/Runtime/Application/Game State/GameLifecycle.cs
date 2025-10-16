using System;
using Cysharp.Threading.Tasks;
using ElusiveLife.Runtime.Utils.Helpers;
using VContainer.Unity;

namespace ElusiveLife.Runtime.Application.Game_State
{
    public class GameLifecycle : IGameLifecycle, ITickable
    {
        public GameState CurrentState { get; private set; } = GameState.Initializing;

        public event Action<GameState> OnStateChanged;

        public async UniTask Initialize()
        {
            Logging.Log("Game Initialized");
            await StartGameFlow();
        }

        public void Tick() { }

        private async UniTask StartGameFlow() => await ChangeState(GameState.Playing);

        public async UniTask ChangeState(GameState newState)
        {
            if (CurrentState == newState) return;

            // Exit current state
            await ExitState(CurrentState);
            
            // Change state
            var previousState = CurrentState;
            CurrentState = newState;
            
            // Enter new state
            await EnterState(newState);
            
            OnStateChanged?.Invoke(newState);
            Logging.Log($"GameState changed: {previousState} -> {newState}");
        }

        private UniTask ExitState(GameState state) => state switch
        {
            GameState.MainMenu => ExitMainMenu(),
            GameState.Playing => ExitPlaying(),
            GameState.Paused => ExitPaused(),
            _ => UniTask.CompletedTask
        };

        private UniTask EnterState(GameState state) => state switch
        {
            GameState.MainMenu => EnterMainMenu(),
            GameState.Playing => EnterPlaying(),
            GameState.Paused => EnterPaused(),
            _ => UniTask.CompletedTask
        };

        private UniTask EnterMainMenu() => UniTask.CompletedTask;
        private UniTask ExitMainMenu() => UniTask.CompletedTask;
        private UniTask EnterPlaying() => UniTask.CompletedTask;
        private UniTask ExitPlaying() => UniTask.CompletedTask;
        private UniTask EnterPaused() => UniTask.CompletedTask;
        private UniTask ExitPaused() => UniTask.CompletedTask;
    }
}