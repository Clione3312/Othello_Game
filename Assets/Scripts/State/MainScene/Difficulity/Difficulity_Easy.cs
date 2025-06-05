using System.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;

public partial class Difficulity
{
    public class Difficulity_Easy
    {
        CalcSprite calcSprite = new CalcSprite();

        public string GetSpritePosition(int[][] fieldState, int ySprite, int eSprite)
        {
            // 置ける位置を取得する
            string posStr = calcSprite.GetPutPosition(fieldState, ySprite, eSprite);

            // 置ける場所がない場合、空白を返す
            if (posStr == string.Empty) return string.Empty;

            // 置ける位置情報を分割する
            string[] pos = posStr.Split(" ");

            return pos[Random.Range(0, pos.Length)];
        }
    }
}
