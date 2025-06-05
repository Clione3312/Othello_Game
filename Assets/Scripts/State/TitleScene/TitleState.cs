using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;

using State = StateMachine<TitleState>.State;
using System.Threading.Tasks;
using UnityEngine.UI;

public class TitleState : MonoBehaviour
{
    private StateMachine<TitleState> _stateMachine;

    public enum Title : int
    {
        Title,
        Menu,
        Setting,
        Option,
        Exit
    }

    #region Title
    [Header("Fade")]
    [SerializeField] private FadeSystem fade;
    public FadeSystem Fade => fade;
    #endregion

    #region Title
    [Header("Start")]
    [SerializeField] private RawImage titleLogo;
    public RawImage TitleLogo => titleLogo;
    [SerializeField] private TextMeshProUGUI titleText;
    public TextMeshProUGUI TitleText => titleText;
    #endregion



    public Sequence sequence;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {

        _stateMachine = new StateMachine<TitleState>(this);

        _stateMachine.AddAnyTransition<PhaseMenu>((int)Title.Menu);
        _stateMachine.AddTransition<PhaseMenu, PhaseSetting>((int)Title.Setting);
        _stateMachine.AddTransition<PhaseMenu, PhaseOption>((int)Title.Option);
        _stateMachine.AddTransition<PhaseMenu, PhaseExit>((int)Title.Exit);

        if (GameManager.I.TitleMode() == (int)Title.Title) {
            _stateMachine.Start<PhaseTitle>();
        }


        // switch ((int)GameManager.I.TitleMode())
        // {
        //     case (int)Title.Title:
        //         _stateMachine.Start<PhaseTitle>();
        //         break;
        //     case (int)Title.Menu:
        //         _stateMachine.Start<PhaseMenu>();
        //         break;
        //     case (int)Title.Setting:
        //         _stateMachine.Start<PhaseSetting>();
        //         break;
        // }
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
        await Owner.Fade.FadeIn();

        await ShowTitle();
    }

    private async UniTask ShowTitle()
    {
        Owner.sequence = DOTween.Sequence();

        await Owner.sequence
            .Append(Owner.TitleLogo.DOFade(1, 1.0f))
            .AppendInterval(1.0f);

        await Owner.sequence.Play().AsyncWaitForCompletion();

        await UniTask.WaitUntil(() => Owner.sequence.IsPlaying());

        await Owner.TitleText.DOFade(1, 1.0f).SetLoops(-1, LoopType.Yoyo).Play().AsyncWaitForCompletion();
    }

    protected override async void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            await FadeTitle();

            StateMachine.Dispatch((int)TitleState.Title.Menu);
        }
    }

    private async UniTask FadeTitle()
    {
        await Owner.TitleText.DOFade(0, 1.0f).Play().AsyncWaitForCompletion();
        await UniTask.WaitForSeconds(1.0f);
        await Owner.TitleLogo.DOFade(0, 1.0f).Play().AsyncWaitForCompletion();

    }
}

internal class PhaseMenu : State
{
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