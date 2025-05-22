using UnityEngine;
using State = StateMachine<Player1>.State;

public partial class Player1
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
                GameManager.I.currTurn = GameManager.TURN_ENEMY;
                stateMachine.Dispatch((int)Phase.Idle);
            }
        }
    }
}
