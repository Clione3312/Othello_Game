using UnityEngine;
using State = StateMachine<Player2>.State;

public partial class Player2
{
    public class PhaseStart : State
    {
        private const string TRUN_TEXT = "あいて の ターン";

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
