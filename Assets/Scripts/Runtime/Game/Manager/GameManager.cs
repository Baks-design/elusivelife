using System;
using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.StateMachine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Manager
{
    public class GameManager : StatefulEntity, IGameServices //TODO: Finishing
    {
        public void StartGame()
        {
            InitStateMachine();
            SetupStateMachine();
        }

        private void SetupStateMachine()
        {
            var gameplayState = new GameplayState(this);
            var pauseState = new PauseState(this);

            At<Func<bool>>(gameplayState, pauseState, () => false);
            At<Func<bool>>(pauseState, gameplayState, () => true);

            StateMachine.SetState(gameplayState);
        }

        public void UpdateGame() => UpdateStateMachine();

        public void EndGame()
        {
        }
    }
}