using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerSystem
{
    private static PlayerSystem _instance = new PlayerSystem();
    public static PlayerSystem I { get; private set; } = _instance;

    private int ySprite = 0;
    private int eSprite = 0;

    public void PutSprite(int posY, int posX) {
        ySprite = GameManager.I.yourSprite;
        eSprite = GameManager.I.enemySprite;

        // 獲得できる駒の位置を取得する
        string getSprite = GetSpriteCheck.I.GetSpritePosition(posY, posX, GameManager.I.fieldState, ySprite, eSprite);

        if (getSprite == string.Empty) return;

        // 獲得できる駒位置を、ジャグ配列に変換する
        int[][] posArray = GetSpriteCheck.I.ConvertJuggedArray(getSprite);

        // 獲得できる駒を置く
        GameManager.I.fieldState[posY][posX] = ySprite;
        foreach (int[] item in posArray) {
            GameManager.I.fieldState[item[0]][item[1]] = ySprite;
        }

        // 駒数を確認し、反映する
        MaskSpriteCount.I.CalcSpriteCount();
        DisplaySpriteCount.I.ShowScore();

        // ターンを切り替える
        GameManager.I.currTurn = GameManager.TURN_ENEMY;
    }

}
