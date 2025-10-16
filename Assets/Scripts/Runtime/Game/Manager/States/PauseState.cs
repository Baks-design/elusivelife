using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Helpers;
using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.StateMachine;
using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Manager
{
    public class PauseState : IState
    {
        private readonly GameManager _gameManager;

        public PauseState(GameManager gameManager) => _gameManager = gameManager;

        public void OnEnter()
        {
            Time.timeScale = 0f;
            GameSystem.SetCursor(true);
        }
    }
}