using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;

using UniRx;

using State = StateMachine<TitleState>.State;
using System.Buffers;

public class TitleState : MonoBehaviour
{
    private StateMachine<TitleState> _stateMachine;

    private PhaseTitle phaseTitle = new PhaseTitle();

    #region Fade
    [Header("Fade")]
    [SerializeField] public FadeSystem fade;
    #endregion

    #region Title
    [Header("Start")]
    [SerializeField] public CanvasGroup titlePanel;
    [SerializeField] public CanvasGroup titleLogo;
    [SerializeField] public CanvasGroup titleText;
    #endregion

    #region Menu
    [Header("Menu Panel")]
    [SerializeField] public CanvasGroup menuPanel;
    [SerializeField] public CanvasGroup menuObject;
    #endregion

    public Sequence sequence;

    public bool isStart = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private async void Start()
    {
        isStart = false;

        _stateMachine = new StateMachine<TitleState>(this);

        _stateMachine.AddAnyTransition<PhaseMenu>((int)GameManager.Title.Menu);
        _stateMachine.AddTransition<PhaseMenu, PhaseSetting>((int)GameManager.Title.Setting);
        _stateMachine.AddTransition<PhaseMenu, PhaseOption>((int)GameManager.Title.Option);
        _stateMachine.AddTransition<PhaseMenu, PhaseExit>((int)GameManager.Title.Exit);

        await fade.FadeIn();
        switch (GameManager.I.titleMode)
        {
            case GameManager.Title.Title:
                _stateMachine.Start<PhaseTitle>();
                break;
            case GameManager.Title.Menu:
                _stateMachine.Start<PhaseMenu>();
                break;
            case GameManager.Title.Setting:
                _stateMachine.Start<PhaseSetting>();
                break;
        }
    }

    public async void GameStart()
    {
        await titleText.DOFade(0, 1.0f).Play().AsyncWaitForCompletion();
        await titleLogo.DOFade(0, 1.0f).Play().AsyncWaitForCompletion();
        titlePanel.gameObject.SetActive(false);
        isStart = true;
    }


    // Update is called once per frame
    private void Update()
    {
        _stateMachine.Update();
    }
}

internal class PhaseTitle : State
{
    protected override async void OnEnter(State prevState)
    {
        await ShowTitle();
    }

    private async UniTask ShowTitle()
    {
        Owner.titlePanel.gameObject.SetActive(true);

        await Owner.titleLogo.DOFade(1, 5f).Play().AsyncWaitForCompletion();
        await UniTask.WaitForSeconds(1.0f);
        await Owner.titleText.DOFade(1, 2.0f).SetLoops(-1, LoopType.Yoyo).Play().AsyncWaitForCompletion();
    }


    protected override void OnUpdate()
    {
        if (Owner.isStart)
        {
            Owner.isStart = false;
            StateMachine.Dispatch((int)GameManager.Title.Menu);
        }
    }
}

internal class PhaseMenu : State
{
    protected override async void OnEnter(State prevState)
    {
        await ShowMenu();
    }

    private async UniTask ShowMenu()
    {
        Owner.menuPanel.gameObject.SetActive(true);

        await Owner.menuObject.DOFade(1, 3f).Play().AsyncWaitForCompletion();
    }
}

internal class PhaseSetting : State
{
}

internal class PhaseOption : State
{
}

internal class PhaseExit : State
{
}