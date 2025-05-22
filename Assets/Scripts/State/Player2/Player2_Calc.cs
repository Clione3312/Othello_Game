using Cysharp.Threading.Tasks;
using UnityEngine;
using State = StateMachine<Player2>.State;

public partial class Player2
{
    public class PhaseCalc : State
    {
        public Difficulity.Difficulity_Public difficulity = new Difficulity.Difficulity_Public();

        protected override void OnEnter(State prevState)
        {
            GameManager.I.selectAddress = string.Empty;
            GameManager.I.selectAddress = difficulity.GetPutPositionString(yourSprite, enemySprite);
        }

        protected override void OnUpdate()
        {
            if (GameManager.I.selectAddress != string.Empty) {
                stateMachine.Dispatch((int)Phase.Action);
            }
        }
    }
}
