using System;
using System.Collections.Generic;

namespace ElusiveLife.Runtime.Utils.StateMachine
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IGameState> _states;
        private IGameState _currentState;

        public Type CurrentStateType { get; private set; }

        public event Action<Type, Type> OnStateChanged;

        public GameStateMachine() => _states = new Dictionary<Type, IGameState>();

        public void RegisterState<T>(IGameState state) where T : IGameState => _states[typeof(T)] = state;

        public void ChangeState<T>() where T : IGameState
        {
            var newStateType = typeof(T);

            if (_states.TryGetValue(newStateType, out var newState))
            {
                var previousStateType = CurrentStateType;

                _currentState?.Exit();
                _currentState = newState;
                CurrentStateType = newStateType;
                _currentState.Enter();

                OnStateChanged?.Invoke(previousStateType, newStateType);
            }
            else
                throw new ArgumentException($"State {newStateType.Name} is not registered");
        }

        public void Update() => _currentState?.Update();

        public bool IsInState<T>() where T : IGameState => CurrentStateType == typeof(T);

        public T GetState<T>() where T : class, IGameState
        {
            if (_states.TryGetValue(typeof(T), out IGameState state))
                return state as T;
            return null;
        }
    }
}