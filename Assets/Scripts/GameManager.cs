using Unity.VisualScripting;
using UnityEngine;
using UniRx;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public static GameManager I { get; private set; } = _instance;

    /// <summary>
    /// タイトル画面
    /// </summary>
    #region TitleMenu
    public enum Title
    {
        Title,
        Menu,
        Setting,
        Option,
        Exit,
        Start
    }
    public Title titleMode = Title.Title;
    #endregion

    /// <summary>
    /// 盤面サイズ
    /// </summary>
    public const int BOARD_SIZE = 8;

    public const float INST_OFFSET = 3.5f;
    public const float INST_OFFSET_Y = 0.4f;

    /// <summary>
    /// フェーズ：メイン
    /// </summary>
    #region MainPhase
    private enum SystemPhase : int
    {
        GameStart,  // ゲーム開始
        Setting,    // 盤面準備
        Battle,     // バトル開始
        GameEnd     // ゲーム終了
    }
    private SystemPhase mainPhase = SystemPhase.GameStart;
    public void MainPhase(int phase)
    {
        if (phase < 0)
        {
            this.mainPhase = SystemPhase.GameStart;
        }
        else if (phase > System.Enum.GetValues(typeof(SystemPhase)).Length - 1)
        {
            this.mainPhase = SystemPhase.GameEnd;
        }
        else
        {
            this.mainPhase = (SystemPhase)phase;
        }
    }
    public int MainPhase() { return (int)mainPhase; }
    #endregion

    /// <summary>
    /// フェーズ：プレイヤー
    /// </summary>
    #region PlayerPhase
    private enum TurnPhase : int
    {
        Idle,       // 待機
        Check,      // 確認
        Start,      // ターン開始
        Calc, // 計算(手動)
        // CPUCalc,    // 計算(手動)
        // WebCalc,    // 計算(手動)
        Action,     // 結果
        Update,     // 反映
        Close,      // ターン終了
        GameEnd     // ゲーム終了
    }
    private TurnPhase playerPhase;
    public void PlayerPhase(int phase)
    {
        if (phase < 0)
        {
            this.playerPhase = TurnPhase.Idle;
        }
        else if (phase > System.Enum.GetValues(typeof(TurnPhase)).Length - 1)
        {
            this.playerPhase = TurnPhase.GameEnd;
        }
        else
        {
            this.playerPhase = (TurnPhase)phase;
        }
    }
    public int PlayerPhase() { return (int)playerPhase; }
    #endregion

    /// <summary>
    /// 相手のタイプ
    /// </summary>
    #region EnemyType
    public enum Enemy : int
    {
        Computer,
        Web
    }
    private Enemy enemyType;
    public void EnemyType(int type)
    {
        if (type < 0)
        {
            this.enemyType = Enemy.Computer;
        }
        else if (type > System.Enum.GetValues(typeof(Enemy)).Length - 1)
        {
            this.enemyType = Enemy.Web;
        }
        else
        {
            this.enemyType = (Enemy)type;
        }
    }
    public int EnemyType() { return (int)enemyType; }
    #endregion

    /// <summary>
    /// 相手の難易度
    /// </summary>
    #region Difficulity
    public enum EnemyDifficulity
    {
        Easy,
        Normal,
        Hard,
        VeryHard,
        Impossible
    }
    private EnemyDifficulity difficulity = EnemyDifficulity.Easy;
    public void Difficulity(int difficulity)
    {
        if (difficulity < 0)
        {
            this.difficulity = EnemyDifficulity.Easy;
        }
        else if (difficulity > System.Enum.GetValues(typeof(EnemyDifficulity)).Length - 1)
        {
            this.difficulity = EnemyDifficulity.Impossible;
        }
        else
        {
            this.difficulity = (EnemyDifficulity)difficulity;
        }
    }
    public int Difficulity() { return (int)difficulity; }
    #endregion

    /// <summary>
    /// 盤面上の石の種類
    /// </summary>
    public const int SPRITE_NONE = 0;
    public const int SPRITE_BLACK = 1;
    public const int SPRITE_WHITE = 2;

    /// <summary>
    /// 盤面上の石の状態
    /// </summary>
    private int[][] fieldState = new int[][] { new int[BOARD_SIZE], new int[BOARD_SIZE], new int[BOARD_SIZE], new int[BOARD_SIZE], new int[BOARD_SIZE], new int[BOARD_SIZE], new int[BOARD_SIZE], new int[BOARD_SIZE] };
    public int[][] FieldState() { return fieldState; }
    public int FieldState(int posY, int posX) { return fieldState[posY][posX]; }
    public void FieldState(int posY, int posX, int state) { fieldState[posY][posX] = state; }

    private Sprite[][] fieldSprite = new Sprite[][] { new Sprite[BOARD_SIZE], new Sprite[BOARD_SIZE], new Sprite[BOARD_SIZE], new Sprite[BOARD_SIZE], new Sprite[BOARD_SIZE], new Sprite[BOARD_SIZE], new Sprite[BOARD_SIZE], new Sprite[BOARD_SIZE] };
    public Sprite[][] FieldSprite() { return fieldSprite; }
    public Sprite FieldSprite(int posY, int posX) { return fieldSprite[posY][posX]; }
    public void FieldSprite(int posY, int posX, Sprite sprite) { fieldSprite[posY][posX] = sprite; }

    private Selecter[][] fieldSelecter = new Selecter[][] { new Selecter[BOARD_SIZE], new Selecter[BOARD_SIZE], new Selecter[BOARD_SIZE], new Selecter[BOARD_SIZE], new Selecter[BOARD_SIZE], new Selecter[BOARD_SIZE], new Selecter[BOARD_SIZE], new Selecter[BOARD_SIZE] };
    public Selecter[][] FieldSelecter() { return fieldSelecter; }
    public Selecter FieldSelecter(int posY, int posX) { return fieldSelecter[posY][posX]; }
    public void FieldSelecter(int posY, int posX, Selecter selecter) { fieldSelecter[posY][posX] = selecter; }


    /// <summary>
    /// 自分/相手の石の種類
    /// </summary>
    #region PlayerSprite
    public enum SpriteType
    {
        Black,
        White
    }
    private int yourSprite = SPRITE_BLACK;
    private int enemySprite = SPRITE_WHITE;
    public void YourSprite(int sprite) { yourSprite = sprite; }
    public int YourSprite() { return yourSprite; }
    public void EnemySprite(int sprite) { enemySprite = sprite; }
    public int EnemySprite() { return enemySprite; }
    #endregion

    /// <summary>
    /// 自分/相手のスコア
    /// </summary>
    public ReactiveProperty<int> prevYourScore = new ReactiveProperty<int>(2);
    public ReactiveProperty<int> prevEnemyScore = new ReactiveProperty<int>(2);
    public ReactiveProperty<int> yourScore = new ReactiveProperty<int>(2);
    public ReactiveProperty<int> enemyScore = new ReactiveProperty<int>(2);
    public void CountScore()
    {
        prevYourScore.Value = yourScore.Value;
        prevEnemyScore.Value = enemyScore.Value;

        yourScore.Value = 0;
        enemyScore.Value = 0;

        for (int posY = 0; posY < BOARD_SIZE; posY++)
        {
            for (int posX = 0; posX < BOARD_SIZE; posX++)
            {
                if (fieldState[posY][posX] == yourSprite) yourScore.Value++;//自分の石を数える
                if (fieldState[posY][posX] == enemySprite) enemyScore.Value++;//相手の石を数える
            }
        }
    }
    public int PrevYourScore() { return prevYourScore.Value; }
    public int PrevEnemyScore() { return prevEnemyScore.Value; }
    public int YourScore() { return yourScore.Value; }
    public int EnemyScore() { return enemyScore.Value; }

    /// <summary>
    /// 自分/相手のターン
    /// </summary>
    public const int TURN_IDLE = 0;
    public const int TURN_YOUR = 1;
    public const int TURN_ENEMY = 2;
    private int currTurn = TURN_IDLE;
    public void Turn(int turn)
    {
        switch (turn)
        {
            case TURN_YOUR: currTurn = TURN_YOUR; break;
            case TURN_ENEMY: currTurn = TURN_ENEMY; break;
        }
    }
    public int Turn() { return currTurn; }
    public void TurnChange()
    {
        switch (currTurn)
        {
            case TURN_YOUR: currTurn = TURN_ENEMY; break;
            case TURN_ENEMY: currTurn = TURN_YOUR; break;
        }
    }


    /// <summary>
    /// ガイド表示
    /// </summary>
    private bool isGuide = true;
    public void IsGuide(bool value) { isGuide = value; }
    public bool IsGuide() { return isGuide; }

    /// <summary>
    /// 選択した駒の種類
    /// </summary>
    private int selectSprite = SPRITE_BLACK;
    public void SelectSprite(int value) { selectSprite = value; }
    public int SelectSprite() { return selectSprite; }

    /// <summary>
    /// 選択した駒の位置
    /// </summary>
    private string selectAddress = string.Empty;
    public void SelectAddress(string value) { selectAddress = value; }
    public string SelectAddress() { return selectAddress; }

    /// <summary>
    /// 選択した持ち時間
    /// </summary>
    public enum TimeType : int
    {
        shortTime,
        normalTime,
        semiLongTime,
        longTime,
        veryLongTime
    }
    private int selectTime = (int)TimeType.normalTime;
    public void SelectTime(int value) { selectTime = value; }
    public int SelectTime() { return selectTime; }


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            I = _instance;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}
