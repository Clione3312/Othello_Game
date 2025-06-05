using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

using State = StateMachine<Player>.State;

public class Player : MonoBehaviour
{
    private StateMachine<Player> _stateMachine;

    public enum SystemPhase : int
    {
        GameStart,  // ゲーム開始
        Battle,     // バトル開始
        GameEnd     // ゲーム終了
    }

    public enum TurnPhase : int
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

    public int yourSprite;
    public int enemySprite;

    public const int TURN_YOUR = 1;
    public const int TURN_ENEMY = 2;

    #region Cut In
    [Header("Cut In")]
    [SerializeField] public CanvasGroup turnCutIn;
    [SerializeField] public TextMeshProUGUI textCutIn;
    #endregion

    #region Score Text
    [Header("Score Text")]
    [SerializeField] public TextMeshProUGUI textYourScore;
    [SerializeField] public TextMeshProUGUI textEnemyScore;
    #endregion

    public Sequence seq;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _stateMachine = new StateMachine<Player>(this);
        _stateMachine.AddTransition<PhaseIdle, PhaseCheck>((int)TurnPhase.Check);
        _stateMachine.AddTransition<PhaseCheck, PhaseStart>((int)TurnPhase.Start);
        _stateMachine.AddTransition<PhaseStart, PhaseCalc>((int)TurnPhase.Calc);
        _stateMachine.AddTransition<PhaseCalc, PhaseAction>((int)TurnPhase.Action);
        _stateMachine.AddTransition<PhaseAction, PhaseUpdate>((int)TurnPhase.Update);

        _stateMachine.AddTransition<PhaseClose, PhaseIdle>((int)TurnPhase.Idle);
        _stateMachine.AddAnyTransition<PhaseClose>((int)TurnPhase.Close);
        _stateMachine.AddAnyTransition<PhaseEnd>((int)TurnPhase.GameEnd);

        _stateMachine.Start<PhaseIdle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.I.MainPhase() == (int)Player.SystemPhase.Battle)
        {
            _stateMachine.Update();
        }
    }
}

internal class PhaseIdle : State
{
    protected override void OnUpdate()
    {
        Owner.yourSprite = GameManager.I.Turn() == Player.TURN_YOUR ? GameManager.I.YourSprite() : GameManager.I.EnemySprite();
        Owner.enemySprite = GameManager.I.Turn() == Player.TURN_YOUR ? GameManager.I.EnemySprite() : GameManager.I.YourSprite();

        StateMachine.Dispatch((int)Player.TurnPhase.Check);

    }
}

internal class PhaseCheck : State
{
    private CalcSprite calcSprite = new CalcSprite();
    private string putPos = string.Empty;

    protected override void OnEnter(State prevState)
    {
        putPos = calcSprite.GetPutPosition(GameManager.I.FieldState(), Owner.yourSprite, Owner.enemySprite);
    }

    protected override void OnUpdate()
    {
        if (putPos != string.Empty)
        {
            StateMachine.Dispatch((int)Player.TurnPhase.Start);
        }
        else
        {
            StateMachine.Dispatch((int)Player.TurnPhase.Close);
        }
    }
}

internal class PhaseStart : State
{
    private const string TURN_NAME_DEF = "{0} の ターン";


    protected override void OnEnter(State prevState)
    {
        Owner.seq = DOTween.Sequence();

        Owner.seq
            .OnStart(() =>
            {
                Owner.textCutIn.text = string.Format(TURN_NAME_DEF, GameManager.I.Turn() == Player.TURN_YOUR ? "あなた" : "あいて");
                Owner.turnCutIn.alpha = 0;
                Owner.textCutIn.transform.localPosition = new Vector3(1920, 0, 0);
                Owner.turnCutIn.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            })
            .Append(Owner.turnCutIn.DOFade(1, 0.1f))
            .Join(Owner.textCutIn.transform.DOLocalMoveX(0, 1.0f))
            .Join(Owner.turnCutIn.transform.DOLocalRotate(new Vector3(0, 0, 0), 1.0f))
            .AppendInterval(1.0f)
            .Append(Owner.turnCutIn.DOFade(0, 0.9f))
            .Join(Owner.textCutIn.transform.DOLocalMoveX(-1920, 1.0f))
            .Join(Owner.turnCutIn.transform.DOLocalRotate(new Vector3(90, 0, 0), 1.0f));

        Owner.seq.SetAutoKill(true).Play();

    }

    protected override async void OnUpdate()
    {
        await Owner.seq.AsyncWaitForCompletion();
        StateMachine.Dispatch((int)Player.TurnPhase.Calc);
    }
}

internal class PhaseCalc : State
{
    private Difficulity.Difficulity_Public difficulity = new Difficulity.Difficulity_Public();
    string putPos = string.Empty;

    protected override void OnEnter(State prevState)
    {
        GameManager.I.SelectAddress(string.Empty);

        if (GameManager.I.Turn() == Player.TURN_ENEMY)
        {
            putPos = string.Empty;
            switch (GameManager.I.EnemyType())
            {
                case (int)GameManager.Enemy.Computer:
                    putPos = difficulity.GetPutPositionString(Owner.yourSprite, Owner.enemySprite).ToString();

                    break;
                case (int)GameManager.Enemy.Web:
                    break;
            }
            GameManager.I.SelectAddress(putPos);
        }
    }

    protected override void OnUpdate()
    {
        if (GameManager.I.SelectAddress() != string.Empty) StateMachine.Dispatch((int)Player.TurnPhase.Action);
    }
}

internal class PhaseAction : State
{
    private CalcSprite calcSprite = new CalcSprite();

    protected override async void OnEnter(State prevState)
    {
        // 駒を置く位置を取得
        int[][] putPosArray = calcSprite.ConvertJuggedArray(GameManager.I.SelectAddress());
        int idx = UnityEngine.Random.Range(0, putPosArray.Length);
        int posY = putPosArray[idx][0];
        int posX = putPosArray[idx][1];

        GameManager.I.SelectAddress(string.Empty);

        // 獲得できる駒の位置を取得
        string getSprite = calcSprite.GetSpritesPosition(posY, posX, GameManager.I.FieldState(), Owner.yourSprite, Owner.enemySprite);
        int[][] getAddress = calcSprite.ConvertJuggedArray(getSprite);

        // 駒を置く
        GameManager.I.FieldState(posY, posX, Owner.yourSprite);
        GameManager.I.FieldSprite(posY, posX).ChangeState(Owner.yourSprite);

        // 駒を取る
        for (int i = 0; i < getAddress.Length; i++)
        {
            posY = getAddress[i][0];
            posX = getAddress[i][1];
            GameManager.I.FieldState(posY, posX, Owner.yourSprite);
            GameManager.I.FieldSprite(posY, posX).ChangeState(Owner.yourSprite);
        }

        await UniTask.Delay(1000  * getAddress.Length);
    }

    protected override void OnUpdate()
    {
        StateMachine.Dispatch((int)Player.TurnPhase.Update);
    }
}

internal class PhaseUpdate : State
{
    protected override void OnEnter(State prevState)
    {
        GameManager.I.CountScore();
    }

    protected override void OnUpdate()
    {
        StateMachine.Dispatch((int)Player.TurnPhase.Close);
    }
}

internal class PhaseClose : State
{

    protected override void OnEnter(State prevState)
    {
        GameManager.I.TurnChange();
    }

    protected override void OnUpdate()
    {
        if (GameManager.I.MainPhase() == (int)Player.SystemPhase.GameEnd)
        {
            StateMachine.Dispatch((int)Player.TurnPhase.GameEnd);
        }
        else
        {
            StateMachine.Dispatch((int)Player.TurnPhase.Idle);
        }
    }
}

internal class PhaseEnd : State
{

    protected override void OnEnter(State prevState)
    {
        Owner.enabled = false;
    }
}