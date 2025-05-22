using UnityEngine;
using State = StateMachine<Player2>.State;

public partial class Player2
{
    public class PhaseUpdate : State
    {
        protected override void OnEnter(State prevState)
        {
            GameManager.I.yourScore = 0;
            GameManager.I.enemyScore = 0;

            for (int posY = 0; posY < GameManager.BOARD_SIZE; posY++)
            {
                for (int posX = 0; posX < GameManager.BOARD_SIZE; posX++)
                {
                    if (GameManager.I.fieldState[posY][posX] == GameManager.I.yourSprite) GameManager.I.yourScore++;
                    else if (GameManager.I.fieldState[posY][posX] == GameManager.I.enemySprite) GameManager.I.enemyScore++;

                    GameManager.I.fieldSprite[posY][posX].SetState(GameManager.I.fieldState[posY][posX]);
                    GameManager.I.fieldSelecter[posY][posX].SetState(1);
                }
            }

            Owner.phaseAnim.UpdateScoreText();
        }

        protected override void OnUpdate()
        {
            stateMachine.Dispatch((int)Phase.Close);
        }
    }
}

