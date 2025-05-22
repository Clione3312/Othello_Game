using UnityEngine;
using State = StateMachine<Player1>.State;


public partial class Player1 : MonoBehaviour
{
    [SerializeField] private PhaseAnim phaseAnim;

    public StateMachine<Player1> stateMachine;
    private enum Phase : int
    {
        Idle,       // 待機
        Check,      // 確認
        Start,      // ターン開始
        Calc,       // 計算(手動)
        Action,     // 結果
        Update,     // 反映
        Close,      // ターン終了
        GameEnd     // ゲーム終了
    }
    public static int yourSprite;
    public static int enemySprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        yourSprite = GameManager.I.yourSprite;
        enemySprite = GameManager.I.enemySprite;

        stateMachine = new StateMachine<Player1>(this);

        stateMachine.AddTransition<PhaseIdle, PhaseCheck>((int)Phase.Check);
        stateMachine.AddTransition<PhaseCheck, PhaseStart>((int)Phase.Start);
        stateMachine.AddTransition<PhaseStart, PhaseCalc>((int)Phase.Calc);
        stateMachine.AddTransition<PhaseCalc, PhaseAction>((int)Phase.Action);
        stateMachine.AddTransition<PhaseAction, PhaseUpdate>((int)Phase.Update);
        stateMachine.AddTransition<PhaseClose, PhaseIdle>((int)Phase.Idle);

        stateMachine.AddAnyTransition<PhaseClose>((int)Phase.Close);
        stateMachine.AddAnyTransition<PhaseGameEnd>((int)Phase.GameEnd);

        stateMachine.Start<PhaseIdle>();
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }
}
