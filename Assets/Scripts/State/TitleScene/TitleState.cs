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

    #region Public
    [Header("Base Panel")]
    [SerializeField] public TitleBase titleBase;
    #endregion

    #region Title
    [Header("Title Panel")]
    [SerializeField] public TitleStart titleStart;
    #endregion

    #region Menu
    [Header("Menu Panel")]
    [SerializeField] public TitleMenu titleMenu;
    #endregion

    #region Setting
    [Header("Setting Panel")]
    [SerializeField] public TitleSetting titleSetting;
    #endregion

    public Sequence sequence;

    public bool isStart = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private async void Start()
    {
        isStart = false;

        _stateMachine = new StateMachine<TitleState>(this);

        _stateMachine.AddAnyTransition<PhaseMenu>((int)GameManager.Title.Menu);
        _stateMachine.AddTransition<PhaseMenu, PhaseTitle>((int)GameManager.Title.Title);
        _stateMachine.AddTransition<PhaseMenu, PhaseSetting>((int)GameManager.Title.Setting);
        _stateMachine.AddTransition<PhaseMenu, PhaseOption>((int)GameManager.Title.Option);
        _stateMachine.AddTransition<PhaseMenu, PhaseExit>((int)GameManager.Title.Exit);
        _stateMachine.AddTransition<PhaseSetting, PhaseGameStart>((int)GameManager.Title.Start);

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
        GameManager.I.titleMode = GameManager.Title.Title;
        await Owner.titleBase.UpdateBgPanel();
        await Owner.titleStart.ShowTitle();
    }

    protected override async void OnUpdate()
    {
        if (GameManager.I.titleMode == GameManager.Title.Menu)
        {
            await Owner.titleStart.HideTitle();
            StateMachine.Dispatch((int)GameManager.Title.Menu);
        }
    }
}

internal class PhaseMenu : State
{
    protected override async void OnEnter(State prevState)
    {
        await Owner.titleBase.UpdateBgPanel();
        await Owner.titleMenu.ShowMenu();
    }

    protected override async void OnUpdate()
    {
        switch (GameManager.I.titleMode)
        {
            case GameManager.Title.Title:
                await Owner.titleMenu.HideMenu();
                StateMachine.Dispatch((int)GameManager.Title.Title);
                break;
            case GameManager.Title.Setting:
                await Owner.titleMenu.HideMenu();
                StateMachine.Dispatch((int)GameManager.Title.Setting);
                break;
            case GameManager.Title.Option:
                await Owner.titleMenu.HideMenu();
                StateMachine.Dispatch((int)GameManager.Title.Option);
                break;
            case GameManager.Title.Exit:
                // await Owner.titleMenu.HideMenu();
                StateMachine.Dispatch((int)GameManager.Title.Exit);
                break;
        }
    }
}

internal class PhaseSetting : State
{
    protected override async void OnEnter(State prevState)
    {
        await Owner.titleBase.UpdateBgPanel();
        await Owner.titleSetting.ShowSetting();
    }

    protected override async void OnUpdate()
    {
        switch (GameManager.I.titleMode)
        {
            case GameManager.Title.Menu:
                await Owner.titleSetting.HideSetting();
                StateMachine.Dispatch((int)GameManager.Title.Menu);
                break;
            case GameManager.Title.Start:
                await Owner.titleSetting.HideSetting();
                StateMachine.Dispatch((int)GameManager.Title.Start);
                break;
        }
    }
}

internal class PhaseOption : State
{
    // protected override async void OnEnter(State prevState)
    // {
    //     await Owner.titleBase.UpdateBgPanel();
    //     await Owner.titleMenu.ShowOption();
    // }

    // protected override async void OnUpdate()
    // {
    //     switch (GameManager.I.titleMode)
    //     {
    //         case GameManager.Title.Menu:
    //             await Owner.titleMenu.HideOption();
    //             StateMachine.Dispatch((int)GameManager.Title.Menu);
    //             break;
    //     }
    // }
}

internal class PhaseExit : State
{
    // protected override async void OnEnter(State prevState)
    // {
    //     await Owner.titleMenu.ShowExit();
    // }

    // protected override async void OnUpdate()
    // {
    //     switch (GameManager.I.titleMode)
    //     {
    //         case GameManager.Title.Menu:
    //             await Owner.titleMenu.HideOption();
    //             StateMachine.Dispatch((int)GameManager.Title.Menu);
    //             break;
    //     }
    // }
}

internal class PhaseGameStart : State
{
    // protected override async void OnEnter(State prevState)
    // {
    //     await Owner.titleMenu.ShowExit();
    // }

    // protected override async void OnUpdate()
    // {
    //     switch (GameManager.I.titleMode)
    //     {
    //         case GameManager.Title.Menu:
    //             await Owner.titleMenu.HideOption();
    //             StateMachine.Dispatch((int)GameManager.Title.Menu);
    //             break;
    //     }
    // }
}