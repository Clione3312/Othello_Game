using UnityEngine;

public class Difficulity_Easy
{
    private static Difficulity_Easy _instance = new Difficulity_Easy();
    public static Difficulity_Easy I { get; private set; } = _instance;

    public string GetSpritePosition(int[][] fieldState, int ySprite, int eSprite) {

        // 置ける位置を取得する
        string posStr = GetSpriteCheck.I.PutSpritePosition(fieldState, ySprite, eSprite);

        // 置ける場所がない場合、空白を返す
        if (posStr == string.Empty) return string.Empty;

        // 置ける位置情報を分割する
        string[] pos = posStr.Split(' ');

        return pos[Random.Range(0, pos.Length)];
    }
}
