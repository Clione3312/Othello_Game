using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSystem : MonoBehaviour
{
    // [SerializeField, Header("Othello Board")]
    // private GameObject[] OthelloBoard;

    private enum SystemPhase : int
    {
        GameStart,  // ゲーム開始
        Battle,     // バトル開始
        GameEnd     // ゲーム終了
    }
    private CalcSprite calcSprite = new CalcSprite();

    [Header("Player Info")]
    private const int BOARD_SIZE = GameManager.BOARD_SIZE;

    private const float INST_OFFSET = GameManager.INST_OFFSET;
    private const float INST_OFFSET_Y = GameManager.INST_OFFSET_Y;

    private const int SPRITE_NONE = GameManager.SPRITE_NONE;
    private const int SPRITE_BLACK = GameManager.SPRITE_BLACK;
    private const int SPRITE_WHITE = GameManager.SPRITE_WHITE;

    private const int TURN_IDLE = GameManager.TURN_IDLE;
    private const int TURN_YOUR = GameManager.TURN_YOUR;
    private const int TURN_ENEMY = GameManager.TURN_ENEMY;

    #region Fade
    [Header("Fade")]
    [SerializeField] private FadeSystem Fade;
    #endregion

    #region Sprite
    [Header("Sprite")]
    [SerializeField] private Sprite _Sprite;
    [SerializeField] private Transform _Sprites;
    #endregion

    #region Selecter
    [Header("Selecter")]
    [SerializeField] private Selecter _Selecter;
    [SerializeField] private Transform _Selecters;
    #endregion

    #region Result Panel
    [Header("result Panel")]
    [SerializeField] internal CanvasGroup playPanel;
    [SerializeField] internal CanvasGroup resultPanel;
    [SerializeField] internal CanvasGroup resultObject;
    [SerializeField] internal CanvasGroup resultImage;
    [SerializeField] internal CanvasGroup resultButtons;

    [Header("Result Score")]
    [SerializeField] internal TextMeshProUGUI textYourScore;
    [SerializeField] internal TextMeshProUGUI textEnemyScore;

    [Header("Result Image")]
    [SerializeField] internal Image resultImg;
    [SerializeField] internal UnityEngine.Sprite winSprite;
    [SerializeField] internal UnityEngine.Sprite loseSprite;
    [SerializeField] internal UnityEngine.Sprite drawSprite;
    #endregion


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private async void Start()
    {
        GameManager.I.MainPhase((int)SystemPhase.GameStart);
        await Fade.FadeIn();

        TurnSet();

        InitializeFieldState();

        GameManager.I.MainPhase((int)SystemPhase.Battle);
    }


    private void Update()
    {
        if (GameManager.I.MainPhase() == (int)SystemPhase.Battle) {
            if (IsGameEnd())
            {
                phaseGameEnd();
            }
            else
            {
                UpdateState();
            }
        }
    }

    /// <summary>
    /// ターンのセット
    /// </summary>
    private void TurnSet()
    {
        // 駒を取得する
        GameManager.I.YourSprite(GameManager.I.SelectSprite() == SPRITE_BLACK ? SPRITE_BLACK : SPRITE_WHITE);
        GameManager.I.EnemySprite(GameManager.I.SelectSprite() == SPRITE_BLACK ? SPRITE_WHITE : SPRITE_BLACK);

        // ターンをセットする
        GameManager.I.Turn(GameManager.I.SelectSprite() == SPRITE_BLACK ? TURN_YOUR : TURN_ENEMY);
    }

    private void InitializeFieldState()
    {
        for (int posY = 0; posY < BOARD_SIZE; posY++)
        {
            for (int posX = 0; posX < BOARD_SIZE; posX++)
            {
                float offsetY = (posY >= 3 && posY <= 4 && posX >= 3 && posX <= 4) ? 0f : 1f;

                Vector3 spritePos = new Vector3(posY - INST_OFFSET, INST_OFFSET_Y + offsetY, posX - INST_OFFSET);
                Vector3 selecterPos = new Vector3(posY - INST_OFFSET, INST_OFFSET_Y, posX - INST_OFFSET);

                // オブジェクト生成
                var sprite = Instantiate(_Sprite, spritePos, Quaternion.identity, _Sprites);
                var selecter = Instantiate(_Selecter, selecterPos, Quaternion.identity, _Selecters);

                // 盤面の駒を設置
                int state = SPRITE_NONE;
                if (posY >= 3 && posY <= 4 && posX >= 3 && posX <= 4)
                {
                    state = (posY == posX) ? SPRITE_WHITE : SPRITE_BLACK;
                }
                // 盤面の駒を設置・設定
                GameManager.I.FieldState(posY, posX, state);

                // 駒の設定
                GameManager.I.FieldSprite(posY, posX, sprite);
                GameManager.I.FieldSprite(posY, posX).ChangeState(state);

                // セレクターの設定
                GameManager.I.FieldSelecter(posY, posX, selecter);
                GameManager.I.FieldSelecter(posY, posX).SetPosition(posY, posX);
                GameManager.I.FieldSelecter(posY, posX).ChangeState(state);
            }
        }
    }

    private void UpdateState()
    {
        for (int posY = 0; posY < BOARD_SIZE; posY++)
        {
            for (int posX = 0; posX < BOARD_SIZE; posX++)
            {
                int state = GameManager.I.FieldState(posY, posX);

                GameManager.I.FieldSelecter(posY, posX).ChangeState(state);
                GameManager.I.FieldSelecter(posY, posX).SetGuide(state);
            }
        }
    }

    private bool IsGameEnd()
    {
        // int yourScore = GameManager.I.yourScore;
        // int enemyScore = GameManager.I.enemyScore;

        string yPutStr = calcSprite.GetPutPosition(GameManager.I.FieldState(), GameManager.I.YourSprite(), GameManager.I.EnemySprite());
        string ePutStr = calcSprite.GetPutPosition(GameManager.I.FieldState(), GameManager.I.EnemySprite(), GameManager.I.YourSprite());

        // bool flg1 = yourScore + enemyScore == Math.Pow(GameManager.BOARD_SIZE, 2);
        // bool flg2 = yourScore == 0 || enemyScore == 0;
        bool flg3 = yPutStr == string.Empty && ePutStr == string.Empty;

        if (flg3)
        {
            GameManager.I.MainPhase((int)SystemPhase.GameEnd);
        }

        // return flg1 || flg2 || flg3;
        return flg3;
    }

    private void phaseGameEnd()
    {
        if (resultPanel.gameObject.activeSelf) return;

        GameManager.I.CountScore();

        int yourScore = GameManager.I.YourScore();
        int enemyScore = GameManager.I.EnemyScore();

        resultPanel.gameObject.SetActive(true);
        resultPanel.alpha = 0;

        if (yourScore > enemyScore) resultImg.sprite = winSprite;
        else if (yourScore < enemyScore) resultImg.sprite = loseSprite;
        else resultImg.sprite = drawSprite;

        Sequence sequence = DOTween.Sequence();

        sequence
            .Append(playPanel.DOFade(0, 0.3f))
            .AppendInterval(1.5f)
            .Append(resultPanel.DOFade(1, 0.3f))
            .Append(resultObject.DOFade(1, 1.0f))
            .Append(textYourScore.DOCounter(0, yourScore, 1.0f))
            .Join(textEnemyScore.DOCounter(0, enemyScore, 1.0f))
            .AppendInterval(1.5f)
            .Append(resultObject.transform.DOLocalMoveY(200f, 1.0f))
            .Append(resultImage.DOFade(1, 0.1f))
            .Append(resultImage.transform.DOShakePosition(1.0f, new Vector3(0, 20, 0), 10, 90))
            .AppendInterval(1.5f)
            .Append(resultButtons.DOFade(1, 0.3f));

        sequence.SetAutoKill(true).Play();
    }
}
