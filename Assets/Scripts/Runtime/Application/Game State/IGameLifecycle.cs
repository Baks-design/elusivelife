using System;
using Cysharp.Threading.Tasks;

namespace ElusiveLife.Runtime.Application.Game_State
{
    public interface IGameLifecycle
    {
        GameState CurrentState { get; }

        event Action<GameState> OnStateChanged;

        void Initialize();
        UniTask ChangeState(GameState newState);
    }
}