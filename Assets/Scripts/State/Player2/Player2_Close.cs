using UnityEngine;
using State = StateMachine<Player2>.State;

public partial class Player2
{
    public class PhaseClose : State
    {
        protected override void OnUpdate()
        {
            if (Owner.phaseAnim.IsGameEnd())
            {
                stateMachine.Dispatch((int)Phase.GameEnd);
            }
            else
            {
                GameManager.I.currTurn = GameManager.TURN_YOUR;
                stateMachine.Dispatch((int)Phase.Idle);
            }
        }
    }
}
