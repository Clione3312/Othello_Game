using UnityEngine;
using State = StateMachine<Player2>.State;

public partial class Player2
{
    public class PhaseGameEnd : State
    {
        protected override void OnEnter(State prevState)
        {
            Owner.phaseAnim.ShowResult();
        }

    }
}

