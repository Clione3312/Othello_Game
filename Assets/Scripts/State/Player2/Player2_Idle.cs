using UnityEngine;
using State = StateMachine<Player2>.State;

public partial class Player2
{
    public class PhaseIdle : State
    {
        protected override void OnUpdate()
        {
            if (GameManager.I.currTurn == GameManager.TURN_ENEMY) {
                stateMachine.Dispatch((int)Phase.Check);
            }
        }
    }
}
