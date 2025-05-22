using UnityEngine;
using State = StateMachine<Player2>.State;

public partial class Player2
{
    public class PhaseCheck : State
    {
        private CalcSprite calcSprite = new CalcSprite();
        string strPutPos = string.Empty;

        protected override void OnEnter(State prevState)
        {
            strPutPos = calcSprite.GetPutPosition(GameManager.I.fieldState, yourSprite, enemySprite);

            for (int posY = 0; posY < GameManager.BOARD_SIZE; posY++)
            {
                for (int posX = 0; posX < GameManager.BOARD_SIZE; posX++)
                {
                    GameManager.I.fieldSelecter[posY][posX].SetState(GameManager.I.fieldState[posY][posX]);
                }
            }
        }

        protected override void OnUpdate()
        {
            if (strPutPos != string.Empty)
            {
                stateMachine.Dispatch((int)Phase.Start);
            }
            else
            {
                stateMachine.Dispatch((int)Phase.Close);
            }
        }
    }
}
