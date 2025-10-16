using UnityEngine;

namespace ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.StateMachine
{
    /// <summary>
    /// Awake or Start can be used to declare all states and transitions.
    /// </summary>
    /// <example>
    /// <code>
    /// protected override void Awake() {
    ///     base.Awake();
    /// 
    ///     var state = new State1(this);
    ///     var anotherState = new State2(this);
    ///
    ///     At(state, anotherState, () => true);
    ///     At(state, anotherState, myFunc);
    ///     At(state, anotherState, myPredicate);
    /// 
    ///     Any(anotherState, () => true);
    ///
    ///     stateMachine.SetState(state);
    /// </code> 
    /// </example>
    public abstract class StatefulEntity
    {
        protected StateMachine StateMachine;

        protected void InitStateMachine() => StateMachine = new StateMachine();

        protected void UpdateStateMachine() => StateMachine.Update();

        protected void At<T>(IState from, IState to, T condition) => StateMachine.AddTransition(from, to, condition);

        protected void Any<T>(IState to, T condition) => StateMachine.AddAnyTransition(to, condition);
    }
}