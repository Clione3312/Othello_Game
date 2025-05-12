using DG.Tweening;
using TMPro;
using UnityEngine;

public class SpriteCount : MonoBehaviour
{
    Sequence seq;

    public void CalcSpriteCount(){
        GameManager.I.prevYourScore = GameManager.I.yourScore;
        GameManager.I.prevEnemyScore  = GameManager.I.enemyScore;

        GameManager.I.yourScore = 0;
        GameManager.I.enemyScore = 0;

        for (int y = 0; y < GameManager.BOARD_SIZE; y++) {
            for (int x = 0; x < GameManager.BOARD_SIZE; x++) {
                if (GameManager.I.fieldState[y][x] == GameManager.I.yourSprite) GameManager.I.yourScore += 1;
                if (GameManager.I.fieldState[y][x] == GameManager.I.enemySprite) GameManager.I.enemyScore += 1;
            }
        }

    }
}
