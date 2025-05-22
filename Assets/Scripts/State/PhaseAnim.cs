using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System;

public class PhaseAnim : MonoBehaviour
{
    private CalcSprite calcSprite = new CalcSprite();

    #region Score Text
    [Header("Score Text")]
    [SerializeField] private TextMeshProUGUI guYourScore;
    public TextMeshProUGUI GUYourScore => guYourScore;
    [SerializeField] private TextMeshProUGUI guEnemyScore;
    public TextMeshProUGUI GUEnemyScore => guEnemyScore;
    #endregion

    #region Cut In
    [Header("Cut In")]
    [SerializeField] private CanvasGroup turnCutIn;
    public CanvasGroup TurnCutIn => turnCutIn;
    [SerializeField] private TextMeshProUGUI textCutIn;
    public TextMeshProUGUI TextCutIn => textCutIn;
    #endregion

    #region Result Panel
    [Header("result Panel")]
    [SerializeField] private CanvasGroup resultPanel;
    public CanvasGroup ResultPanel => resultPanel;
    [SerializeField] private CanvasGroup resultObject;
    public CanvasGroup ResultObject => resultObject;
    [SerializeField] private CanvasGroup resultImage;
    public CanvasGroup ResultImage => resultImage;
    [SerializeField] private CanvasGroup resultButtons;
    public CanvasGroup ResultButtons => resultButtons;

    [Header("Result Score")]
    [SerializeField] private TextMeshProUGUI textYourScore;
    public TextMeshProUGUI TextYourScore => textYourScore;
    [SerializeField] private TextMeshProUGUI textEnemyScore;
    public TextMeshProUGUI TextEnemyScore => textEnemyScore;

    [Header("Result Image")]
    [SerializeField] private Image resultImg;
    public Image ResultImg => resultImg;
    [SerializeField] private UnityEngine.Sprite winSprite;
    public UnityEngine.Sprite WinSprite => winSprite;
    [SerializeField] private UnityEngine.Sprite loseSprite;
    public UnityEngine.Sprite LoseSprite => loseSprite;
    [SerializeField] private UnityEngine.Sprite drawSprite;
    public UnityEngine.Sprite DrawSprite => drawSprite;
    #endregion

    private Sequence seq;


    public void TurnStartCutIn(string strText)
    {
        seq = DOTween.Sequence();

        seq
            .OnStart(() =>
            {
                textCutIn.text = strText;
                turnCutIn.alpha = 0;
                textCutIn.transform.localPosition = new Vector3(1920, 0, 0);
                turnCutIn.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            })
            .Append(turnCutIn.DOFade(1, 0.1f))
            .Join(textCutIn.transform.DOLocalMoveX(0, 1.0f))
            .Join(turnCutIn.transform.DOLocalRotate(new Vector3(0, 0, 0), 1.0f))
            .AppendInterval(1.0f)
            .Append(turnCutIn.DOFade(0, 0.9f))
            .Join(textCutIn.transform.DOLocalMoveX(-1920, 1.0f))
            .Join(turnCutIn.transform.DOLocalRotate(new Vector3(90, 0, 0), 1.0f))
            .Play();
    }

    public void UpdateScoreText()
    {
        seq = DOTween.Sequence();

        int prevYourScore = int.Parse(guYourScore.text);
        int prevEnemyScore = int.Parse(guEnemyScore.text);

        seq
            .Append(guYourScore.DOCounter(prevYourScore, GameManager.I.yourScore, 1.0f))
            .Join(guEnemyScore.DOCounter(prevEnemyScore, GameManager.I.enemyScore, 1.0f))
            .Play();
    }

    public void ShowResult()
    {
        if (resultPanel.gameObject.activeSelf) return;

        int yourScore = GameManager.I.yourScore;
        int enemyScore = GameManager.I.enemyScore;

        seq = DOTween.Sequence();

        seq
            .OnStart(() =>
            {
                ResultPanel.gameObject.SetActive(true);
                ResultPanel.alpha = 0;
            })
            .Append(ResultPanel.DOFade(1, 0.3f))
            .Append(ResultObject.DOFade(1, 1.0f))
            .Append(TextYourScore.DOCounter(0, yourScore, 1.0f))
            .Append(TextEnemyScore.DOCounter(0, enemyScore, 1.0f))
            .AppendInterval(0.5f)
            .Append(ResultObject.transform.DOLocalMoveY(200f, 1.0f))
            .Play();

        if (yourScore > enemyScore) resultImg.sprite = winSprite;
        else if (yourScore < enemyScore) resultImg.sprite = loseSprite;
        else resultImg.sprite = drawSprite;

        seq
            .Append(ResultImage.DOFade(1, 0.1f))
            .Join(ResultImage.transform.DOShakePosition(1.0f, new Vector3(0, 20, 0), 10, 90))
            .Play();

        seq
            .Append(ResultButtons.DOFade(1, 0.3f))
            .Play();

    }

    public bool IsGameEnd()
    {
        int yourSprite = GameManager.I.yourSprite;
        int enemySprite = GameManager.I.enemySprite;

        string yPutStr = calcSprite.GetPutPosition(GameManager.I.fieldState, yourSprite, enemySprite);
        string ePutStr = calcSprite.GetPutPosition(GameManager.I.fieldState, enemySprite, yourSprite);

        bool flg1 = yourSprite + enemySprite == Math.Pow(GameManager.BOARD_SIZE, 2);
        bool flg2 = yourSprite == 0 || enemySprite == 0;
        bool flg3 = yPutStr == string.Empty && ePutStr == string.Empty;

        return flg1 || flg2 || flg3;
    }
}
