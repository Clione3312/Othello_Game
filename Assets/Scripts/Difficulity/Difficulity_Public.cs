using UnityEngine;

public class Difficulity_Public
{
    private static Difficulity_Public _instance= new Difficulity_Public();
    public static Difficulity_Public I { get; private set; } = _instance;

    private int ySprite;
    private int eSprite;
    private string posString;

    public string GetPutPositionString(int ySp, int eSp) {

        ySprite = ySp;
        eSprite = eSp;

        switch (GameManager.I.difficulity)
        {
            case GameManager.Difficulity.Easy:
                posString = Difficulity_Easy.I.GetSpritePosition(GameManager.I.fieldState, ySprite, eSprite);
                break;
            case GameManager.Difficulity.Normal:
                posString = Difficulity_Normal.I.GetSpritePosition(GameManager.I.fieldState, ySprite, eSprite);
                break;
            case GameManager.Difficulity.Hard:
                posString = string.Empty;
                break;
            case GameManager.Difficulity.VeryHard:
                posString = string.Empty;
                break;
            case GameManager.Difficulity.Impossible:
                posString = string.Empty;
                break;
        }

        return posString;
    }

    public int[][] CreateCloneField(int[][] filed) {
        int[][] clone = new int[][]{new int[GameManager.BOARD_SIZE], new int[GameManager.BOARD_SIZE], new int[GameManager.BOARD_SIZE], new int[GameManager.BOARD_SIZE], new int[GameManager.BOARD_SIZE], new int[GameManager.BOARD_SIZE], new int[GameManager.BOARD_SIZE], new int[GameManager.BOARD_SIZE]};

        for (int y = 0; y < GameManager.BOARD_SIZE; y++) {
            for (int x = 0; x < GameManager.BOARD_SIZE; x++) clone[y][x] = filed[y][x];
        }

        return clone;
    }
}
