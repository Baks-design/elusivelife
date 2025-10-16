using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Helpers;
using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.StateMachine;
using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Manager
{
    public class GameplayState : IState
    {
        private readonly GameManager _gameManager;

        public GameplayState(GameManager gameManager) => _gameManager = gameManager;

        public void OnEnter()
        {
            Time.timeScale = 1f;
            GameSystem.SetCursor(false);
        }
    }
}