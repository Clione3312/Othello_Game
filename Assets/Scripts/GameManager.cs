using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 盤面サイズ
    /// </summary>
    public const int BOARD_SIZE = 8;

    public const float INST_OFFSET = 3.5f;
    public const float INST_OFFSET_Y = 0.4f;

    /// <summary>
    /// ゲーム難易度
    /// </summary>
    public enum Difficulity {
        Easy,
        Normal,
        Hard,
        VeryHard,
        Impossible
    }
    public Difficulity difficulity = Difficulity.Easy;

    /// <summary>
    /// 盤面上の石の種類
    /// </summary>
    public const int SPRITE_NONE = 0;
    public const int SPRITE_BLACK = 1;
    public const int SPRITE_WHITE = 2;

    /// <summary>
    /// 盤面上の石の状態
    /// </summary>
    public int[][] fieldState{ get; private set; } = new int[][]{new int[BOARD_SIZE], new int[BOARD_SIZE], new int[BOARD_SIZE], new int[BOARD_SIZE], new int[BOARD_SIZE], new int[BOARD_SIZE], new int[BOARD_SIZE], new int[BOARD_SIZE]};
    public Sprite[][] fieldSprite{ get; private set; } = new Sprite[][]{new Sprite[BOARD_SIZE], new Sprite[BOARD_SIZE], new Sprite[BOARD_SIZE], new Sprite[BOARD_SIZE], new Sprite[BOARD_SIZE], new Sprite[BOARD_SIZE], new Sprite[BOARD_SIZE], new Sprite[BOARD_SIZE]};
    public Selecter[][] fieldSelecter{ get; private set; } = new Selecter[][]{new Selecter[BOARD_SIZE], new Selecter[BOARD_SIZE], new Selecter[BOARD_SIZE], new Selecter[BOARD_SIZE], new Selecter[BOARD_SIZE], new Selecter[BOARD_SIZE], new Selecter[BOARD_SIZE], new Selecter[BOARD_SIZE]};

    /// <summary>
    /// 自分/相手の石の種類
    /// </summary>
    public int yourSprite = SPRITE_BLACK;
    public int enemySprite = SPRITE_WHITE;

    /// <summary>
    /// 自分/相手のスコア
    /// </summary>
    public int prevYourScore = 2;
    public int prevEnemyScore = 2;
    public int yourScore = 2;
    public int enemyScore = 2;

    /// <summary>
    /// 自分/相手のターン
    /// </summary>
    public const int TURN_YOUR = 0;
    public const int TURN_ENEMY = 1;
    public int currTurn = 0;

    public bool isGuide = true;

}
