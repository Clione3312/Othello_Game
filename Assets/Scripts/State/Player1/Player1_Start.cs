using UnityEngine;
using State = StateMachine<Player1>.State;

public partial class Player1
{
    public class PhaseStart : State
    {
        private const string TRUN_TEXT = "あなた の ターン";

        protected override void OnEnter(State prevState)
        {
            Owner.phaseAnim.TurnStartCutIn(TRUN_TEXT);
        }

        protected override void OnUpdate()
        {
            stateMachine.Dispatch((int)Phase.Calc);
        }
    }
}
