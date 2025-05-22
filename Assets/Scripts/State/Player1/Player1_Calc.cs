using UnityEngine;
using State = StateMachine<Player1>.State;

public partial class Player1
{
    public class PhaseCalc : State
    {
        protected override void OnEnter(State prevState)
        {
            GameManager.I.selectAddress = string.Empty;
        }

        protected override void OnUpdate()
        {
            if (GameManager.I.selectAddress != string.Empty) {
                stateMachine.Dispatch((int)Phase.Action);
            }
        }
    }
}
