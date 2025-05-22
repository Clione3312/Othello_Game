using UnityEngine;
using State = StateMachine<Player1>.State;

public partial class Player1
{
    public class PhaseGameEnd : State
    {
        protected override void OnUpdate()
        {
            Owner.phaseAnim.ShowResult();
        }

    }
}

