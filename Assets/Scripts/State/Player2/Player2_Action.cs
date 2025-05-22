using UnityEngine;
using State = StateMachine<Player2>.State;

public partial class Player2
{
    public class PhaseAction : State
    {
        private CalcSprite calcSprite = new CalcSprite();

        protected override void OnEnter(State prevState)
        {
            int[][] putPosition = calcSprite.ConvertJuggedArray(GameManager.I.selectAddress);
            int idx = Random.Range(0, putPosition.Length);
            GameManager.I.selectAddress = string.Empty;

            string getSprite = calcSprite.GetSpritesPosition(putPosition[0][0], putPosition[0][1], GameManager.I.fieldState, yourSprite, enemySprite);
            int[][] getAddress = calcSprite.ConvertJuggedArray(getSprite);

            // 駒を置く
            GameManager.I.fieldState[putPosition[idx][0]][putPosition[idx][1]] = yourSprite;
            for (int i = 0; i < getAddress.Length; i++)
            {
                GameManager.I.fieldState[getAddress[i][0]][getAddress[i][1]] = yourSprite;
            }
        }

        protected override void OnUpdate()
        {
            stateMachine.Dispatch((int)Phase.Update);
        }
    }
}
