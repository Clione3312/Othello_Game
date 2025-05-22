using UnityEngine;
using State = StateMachine<Player1>.State;

public partial class Player1
{
    public class PhaseIdle : State
    {
        protected override void OnUpdate()
        {
            if (GameManager.I.currTurn == GameManager.TURN_YOUR) {
                stateMachine.Dispatch((int)Phase.Check);
            }
        }
    }
}
