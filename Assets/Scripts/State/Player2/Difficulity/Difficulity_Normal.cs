using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Difficulity_Normal
{
    // private static Difficulity_Normal _instance = new Difficulity_Normal();
    // public static Difficulity_Normal I { get; private set; } = _instance;

    private CalcSprite calcSprite = new CalcSprite();
    private Difficulity.Difficulity_Public difficulity = new Difficulity.Difficulity_Public();

    private int[][] fieldState;
    private int yourSprite, enemySprite;

    private const int DEEP_LEVEL = 31;
    private int[,] SCORE_WEIGHT = new int[GameManager.BOARD_SIZE, GameManager.BOARD_SIZE] {
        { 2714, 147, 69, -18, -18, 69, 147, 2714 },
        { 147, -577, -186, -153, -153, -186, -577, 147 },
        { 69, -186, -379, -122, -122, -379, -186, 69 },
        { -18, -153, -122, -169, -169, -122, -153, -18 },
        { -18, -153, -122, -169, -169, -122, -153, -18 },
        { 69, -186, -379, -122, -122, -379, -186, 69 },
        { 147, -577, -186, -153, -153, -186, -577, 147 },
        { 2714, 147, 69, -18, -18, 69, 147, 2714 }
    };


    public async UniTask<string> GetSpritePosition(int[][] field, int ySprite, int eSprite)
    {
        float maxScore = 10000.0f;
        string bestPos = string.Empty;

        fieldState = field;
        yourSprite = ySprite;
        enemySprite = eSprite;

        // 置ける位置を取得する
        string posStr = calcSprite.GetPutPosition(fieldState, yourSprite, enemySprite);

        // 置ける場所がない場合、空白を返す
        if (posStr == string.Empty) return string.Empty;

        foreach (string item in posStr.Split(' '))
        {
            int posY = int.Parse(item.Split(',')[0]);
            int posX = int.Parse(item.Split(',')[1]);

            // 盤面をコピー
            int[][] cloneField = difficulity.CreateCloneField(fieldState);

            // 特定の位置から、獲得できる駒位置を取得する
            string getSprite = calcSprite.GetSpritesPosition(posY, posX, cloneField, yourSprite, enemySprite);
            int[][] getPosArray = calcSprite.ConvertJuggedArray(getSprite);

            // 獲得できる駒を変える
            cloneField[posY][posX] = yourSprite;
            for (int i = 0; i < getPosArray.Length; i++)
            {
                cloneField[getPosArray[i][0]][getPosArray[i][1]] = yourSprite;
            }

            float currScore = await NegaMaxMethod(cloneField, DEEP_LEVEL - 1, enemySprite, yourSprite);
            if (currScore <= maxScore)
            {
                if (currScore < maxScore)
                {
                    maxScore = currScore;
                    bestPos = string.Empty;
                }
                string addPos = string.Format("{0},{1}", posY, posX);
                bestPos = (bestPos != string.Empty ? bestPos + " " : string.Empty) + addPos;
            }
        }

        return bestPos;
    }

    private async UniTask<float> NegaMaxMethod(int[][] field, int limit, int ySprite, int eSprite)
    {
        float maxScore = 10000.0f;

        // 両サイドの
        string yPutPos = calcSprite.GetPutPosition(field, ySprite, eSprite);
        string ePutPos = calcSprite.GetPutPosition(field, eSprite, ySprite);

        // 置ける場所がない場合、評価値を返す
        if (limit == 0 || (yPutPos == string.Empty && ePutPos == string.Empty))
        {
            return GetSpriteScore(field);
        }

        // 置ける場所がある場合、獲得できる駒を変える
        if (yPutPos != string.Empty)
        {

            foreach (string item in yPutPos.Split(' '))
            {
                int posY = int.Parse(item.Split(',')[0]);
                int posX = int.Parse(item.Split(',')[1]);

                // 盤面をコピー
                int[][] cloneField = difficulity.CreateCloneField(field);

                // 特定の位置から、獲得できる駒位置を取得する
                string getSprite = calcSprite.GetSpritesPosition(posY, posX, cloneField, ySprite, eSprite);
                int[][] getPosArray = calcSprite.ConvertJuggedArray(getSprite);

                // 獲得できる駒を変える
                cloneField[posY][posX] = ySprite;
                for (int i = 0; i < getPosArray.Length; i++)
                {
                    cloneField[getPosArray[i][0]][getPosArray[i][1]] = ySprite;
                }

                // 次のターンの処理を実行する
                float currScore = await NegaMaxMethod(cloneField, limit - 1, eSprite, ySprite);
                currScore *= -1;
                if (currScore < maxScore)
                {
                    maxScore = currScore;
                }
                else if (currScore > maxScore)
                {
                    // 置ける場所がない場合、評価値を返す
                    return maxScore;
                }
            }

        }
        else
        {
            // 次のターンの処理を実行する
            float currScore = await NegaMaxMethod(field, limit - 1, eSprite, ySprite);
            currScore *= -1;
            if (currScore < maxScore)
            {
                maxScore = currScore;
            }
            else if (currScore > maxScore)
            {
                // 置ける場所がない場合、評価値を返す
                return maxScore;
            }
        }

        return maxScore;
    }

    private float GetSpriteScore(int[][] field)
    {
        int yScore = 0;
        int eScore = 0;

        for (int y = 0; y < GameManager.BOARD_SIZE; y++)
        {
            for (int x = 0; x < GameManager.BOARD_SIZE; x++)
            {
                if (field[y][x] == yourSprite) { yScore += SCORE_WEIGHT[y, x]; continue; }
                if (field[y][x] == enemySprite) { eScore += SCORE_WEIGHT[y, x]; continue; }
            }
        }

        return yScore - eScore;
    }
}
