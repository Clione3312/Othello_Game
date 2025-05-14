using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class JudgeSystem : MonoBehaviour
{
    [SerializeField, Header("Result Panel")]
    private CanvasGroup resultPanel;
    [SerializeField] private CanvasGroup resultObject;
    [SerializeField] private CanvasGroup resultImage;
    [SerializeField] private CanvasGroup resultButtons;

    [SerializeField, Header("TextArea")]
    private TextMeshProUGUI yScoreText;
    [SerializeField]private TextMeshProUGUI eScoreText;

    [SerializeField, Header("TextArea")]
    private Image resultImg;
    [SerializeField]private UnityEngine.Sprite winImg;
    [SerializeField]private UnityEngine.Sprite loseImg;
    [SerializeField]private UnityEngine.Sprite DrawImg;

    private bool isJob = false;

    private Sequence seq;
    int yourCount = 0;
    int enemyCount = 0;

    // Update is called once per frame
    void Update()
    {
        if(!isJob && CheckGameEnd() && !resultPanel.gameObject.activeSelf){
            isJob = true;

            // ゲーム終了処理
            GameEnd();

            // ゲーム終了フラグをリセット
            isJob = false;
        }
    }

    /// <summary>
    /// ゲーム終了条件を確認する
    /// </summary>
    /// <returns></returns>
    private bool CheckGameEnd(){
        yourCount = GameManager.I.yourScore;
        enemyCount = GameManager.I.enemyScore;

        // ゲーム続行条件
        // 1. 盤面が埋まっていない
        // 2. どちらも駒が残っている
        if ((yourCount + enemyCount != Math.Pow(GameManager.BOARD_SIZE, 2))) return false;
        if ((yourCount == 0 || enemyCount == 0)) return false;

        string yPutString = GetSpriteCheck.I.PutSpritePosition(GameManager.I.fieldState, GameManager.I.yourSprite, GameManager.I.enemySprite);
        string ePutString = GetSpriteCheck.I.PutSpritePosition(GameManager.I.fieldState, GameManager.I.enemySprite, GameManager.I.yourSprite);

        // ゲーム続行条件
        // 3.置ける場所が残っている
        if ((yPutString != string.Empty || ePutString != string.Empty)) return false;

        return true;
    }

    private void GameEnd() {
        seq = DOTween.Sequence();

        resultPanel.gameObject.SetActive(true);
        resultObject.gameObject.SetActive(true);
        resultImage.gameObject.SetActive(true);
        resultButtons.gameObject.SetActive(true);

        seq
            .Append(resultObject.DOFade(1f, 1.5f))
            .Append(yScoreText.DOCounter(0, GameManager.I.yourScore, 2.0f))
            .Join(eScoreText.DOCounter(0, GameManager.I.enemyScore, 2.0f))
            .AppendInterval(1.5f)
            .Append(resultObject.transform.DOLocalMoveY(200f, 1.0f))
            .AppendInterval(1.5f)
            .Play();

        if (yourCount > enemyCount) {
            seq.OnStart(() => {resultImg.sprite = winImg;});
        } else if (yourCount < enemyCount) {
            seq.OnStart(() => {resultImg.sprite = loseImg;});
        } else {
            seq.OnStart(() => {resultImg.sprite = DrawImg;});
        }
        seq
            .Append(resultImage.DOFade(1f, 1.0f))
            .AppendInterval(1.0f)
            .Play();

        seq
            .Append(resultButtons.DOFade(1f, 1.0f))
            .AppendInterval(1.0f)
            .Play();
    }
}
