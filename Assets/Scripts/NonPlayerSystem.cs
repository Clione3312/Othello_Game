using UnityEngine;
using DG.Tweening;

public class NonPlayerSystem : MonoBehaviour
{
    private bool isJob = false;
    private int ySprite;
    private int eSprite;

    // Update is called once per frame
    void Update()
    {
        if (!isJob && GameManager.I.currTurn == GameManager.TURN_ENEMY) {
            PutSprite();
        }
    }

    private void PutSprite() {
        ySprite = GameManager.I.enemySprite;
        eSprite = GameManager.I.yourSprite;

        isJob = true;

        // 獲得できる駒の位置を取得する
        string putString = Difficulity_Public.I.GetPutPositionString(ySprite, eSprite);

        // 置ける位置が無い場合、相手のターンに切り替える
        if (putString == string.Empty) {
            putString = GetSpriteCheck.I.PutSpritePosition(GameManager.I.fieldState, eSprite, ySprite);
            if (putString != string.Empty) {
                GameManager.I.currTurn = GameManager.TURN_YOUR;
            }
            isJob = false;
            return;
        }

        // 獲得できる駒の位置をランダムに取得する
        string[] pos  = putString.Split(' ');
        string posStr = pos[Random.Range(0, pos.Length)];
        int posY = int.Parse(posStr.Split(',')[0]);
        int posX = int.Parse(posStr.Split(',')[1]);

        // 獲得できる駒の位置を取得する
        string getSprite = GetSpriteCheck.I.GetSpritePosition(posY, posX, GameManager.I.fieldState, ySprite, eSprite);

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

        // 獲得できる駒の位置を取得する
        putString = GetSpriteCheck.I.PutSpritePosition(GameManager.I.fieldState, eSprite, ySprite);

        // ターンを切り替える
        GameManager.I.currTurn = putString != string.Empty ? GameManager.TURN_YOUR : GameManager.TURN_ENEMY;

        isJob = false;
    }
}
