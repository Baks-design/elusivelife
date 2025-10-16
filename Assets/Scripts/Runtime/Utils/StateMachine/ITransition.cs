namespace ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.StateMachine
{
    public interface ITransition
    {
        IState To { get; }
        IPredicate Condition { get; }
    }
}