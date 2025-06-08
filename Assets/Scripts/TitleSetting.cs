using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;
using UniRx;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class TitleSetting : MonoBehaviour
{
    #region Setting
    [Header("Setting Panel")]
    [SerializeField] private CanvasGroup settingPanel;
    #endregion

    #region Header
    [Header("Header Text")]
    [SerializeField] private TextMeshProUGUI headerText;
    #endregion

    #region Contents
    [Header("Select List")]
    [SerializeField] private TextMeshProUGUI difficulityLabel;
    [SerializeField] private TextMeshProUGUI spriteLabel;
    [SerializeField] private TextMeshProUGUI timeLabel;
    [SerializeField] private Button selectButton;
    #endregion

    #region Button
    [Header("first Object")]
    [SerializeField] private TextMeshProUGUI firstObject;
    #endregion

    const string HEADER_TEXT = "Setting";

    // リスト参照
    private ReactiveProperty<int> difficulityNo = new ReactiveProperty<int>(0);
    private ReactiveProperty<int> spriteNo = new ReactiveProperty<int>(0);
    private ReactiveProperty<int> timeNo = new ReactiveProperty<int>(2);

    private void Start()
    {
        difficulityNo.Subscribe(_ => UpdateDifficulity());
        spriteNo.Subscribe(_ => UpdateSprite());
        timeNo.Subscribe(_ => UpdateTime());
    }

    public async UniTask ShowSetting()
    {
        settingPanel.gameObject.SetActive(true);

        headerText.text = HEADER_TEXT;

        EventSystem.current.firstSelectedGameObject = firstObject.gameObject;
        EventSystem.current.SetSelectedGameObject(firstObject.gameObject);

        await UniTask.WaitForSeconds(0.5f);
        await settingPanel.DOFade(1, 3f).Play().AsyncWaitForCompletion();
        await UniTask.WaitForSeconds(0.5f);
    }

    public async UniTask HideSetting()
    {
        await settingPanel.DOFade(1, 1f).Play().AsyncWaitForCompletion();

        settingPanel.gameObject.SetActive(false);
    }

    public void GameStart()
    {
        GameManager.I.titleMode = GameManager.Title.Start;
    }

    public void CancelSetting()
    {
        GameManager.I.titleMode = GameManager.Title.Menu;
    }

    public void NextDifficulity()
    {
        difficulityNo.Value++;

        if (difficulityNo.Value > System.Enum.GetValues(typeof(GameManager.EnemyDifficulity)).Length - 1)
        {
            difficulityNo.Value = 0;
        }
    }

    public void PrevDifficulity()
    {
        difficulityNo.Value--;

        if (difficulityNo.Value < 0)
        {
            difficulityNo.Value = System.Enum.GetValues(typeof(GameManager.EnemyDifficulity)).Length - 1;
        }
    }
    private void UpdateDifficulity()
    {
        GameManager.EnemyDifficulity difficulity = (GameManager.EnemyDifficulity)System.Enum.ToObject(typeof(GameManager.EnemyDifficulity), difficulityNo.Value);
        string typeName = string.Empty;

        switch (difficulity)
        {
            case GameManager.EnemyDifficulity.Easy:
                typeName = "かんたん";
                break;
            case GameManager.EnemyDifficulity.Normal:
                typeName = "ふつう";
                break;
            case GameManager.EnemyDifficulity.Hard:
                typeName = "つよい";
                break;
            case GameManager.EnemyDifficulity.VeryHard:
                typeName = "すごい";
                break;
            case GameManager.EnemyDifficulity.Impossible:
                typeName = "やばい";
                break;
        }

        difficulityLabel.text = typeName;
    }

    public void NextSprite()
    {
        spriteNo.Value++;

        if (spriteNo.Value > System.Enum.GetValues(typeof(GameManager.SpriteType)).Length - 1)
        {
            spriteNo.Value = 0;
        }
    }

    public void PrevSprite()
    {
        spriteNo.Value--;

        if (spriteNo.Value < 0)
        {
            spriteNo.Value = System.Enum.GetValues(typeof(GameManager.SpriteType)).Length - 1;
        }
    }
    private void UpdateSprite()
    {
        GameManager.SpriteType sprite = (GameManager.SpriteType)System.Enum.ToObject(typeof(GameManager.SpriteType), spriteNo.Value);
        string typeName = string.Empty;

        switch (sprite)
        {
            case GameManager.SpriteType.Black:
                typeName = "先手";
                break;
            case GameManager.SpriteType.White:
                typeName = "後手";
                break;
        }

        spriteLabel.text = typeName;
    }

    public void NextTime()
    {
        timeNo.Value++;

        if (timeNo.Value > System.Enum.GetValues(typeof(GameManager.TimeType)).Length - 1)
        {
            timeNo.Value = 0;
        }
    }

    public void PrevTime()
    {
        timeNo.Value--;

        if (timeNo.Value < 0)
        {
            timeNo.Value = System.Enum.GetValues(typeof(GameManager.TimeType)).Length - 1;
        }
    }
    private void UpdateTime()
    {
        GameManager.TimeType time = (GameManager.TimeType)System.Enum.ToObject(typeof(GameManager.TimeType), timeNo.Value);
        int typeTime = 0;

        switch (time)
        {
            case GameManager.TimeType.shortTime:
                typeTime = 30;
                break;
            case GameManager.TimeType.normalTime:
                typeTime = 45;
                break;
            case GameManager.TimeType.semiLongTime:
                typeTime = 60;
                break;
            case GameManager.TimeType.longTime:
                typeTime = 90;
                break;
            case GameManager.TimeType.veryLongTime:
                typeTime = 120;
                break;
        }
        timeLabel.text = typeTime.ToString();
    }
}
