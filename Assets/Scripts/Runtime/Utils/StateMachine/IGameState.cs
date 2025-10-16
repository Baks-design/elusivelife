namespace ElusiveLife.Runtime.Utils.StateMachine
{
    public interface IGameState
    {
        void Enter();
        void Exit();
        void Update();
    }
}