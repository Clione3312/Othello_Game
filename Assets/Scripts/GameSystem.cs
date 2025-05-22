using Unity.VisualScripting;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    // [SerializeField, Header("Othello Board")]
    // private GameObject[] OthelloBoard;

    [SerializeField, Header("Sprite")]
    private Sprite _Sprite;
    [SerializeField] private Transform _Sprites;

    [SerializeField, Header("Selecter")]
    private Selecter _Selecter;
    [SerializeField] private Transform _Selecters;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TurnSet();
        InitializeFieldState();
        SetState();
        SetGuide();
    }

    private void Update()
    {
        SetGuide();
    }


    /// <summary>
    /// ターンのセット
    /// </summary>
    private void TurnSet()
    {
        // 駒を取得する
        GameManager.I.yourSprite = GameManager.I.selectSprite == GameManager.SPRITE_BLACK ? GameManager.SPRITE_BLACK : GameManager.SPRITE_WHITE;
        GameManager.I.enemySprite = GameManager.I.selectSprite == GameManager.SPRITE_BLACK ? GameManager.SPRITE_WHITE : GameManager.SPRITE_BLACK;

        // ターンをセットする
        GameManager.I.currTurn = GameManager.I.selectSprite == GameManager.SPRITE_BLACK ? GameManager.TURN_YOUR : GameManager.TURN_ENEMY;
    }

    private void InitializeFieldState()
    {
        for (int posY = 0; posY < GameManager.BOARD_SIZE; posY++)
        {
            for (int posX = 0; posX < GameManager.BOARD_SIZE; posX++)
            {
                float offsetY = (posY >= 3 && posY <= 4 && posX >= 3 && posX <= 4) ? 0f : 1f;

                var sprite = Instantiate(_Sprite, new Vector3(posY - GameManager.INST_OFFSET, GameManager.INST_OFFSET_Y + offsetY, posX - GameManager.INST_OFFSET), Quaternion.identity, _Sprites);
                var selecter = Instantiate(_Selecter, new Vector3(posY - GameManager.INST_OFFSET, GameManager.INST_OFFSET_Y, posX - GameManager.INST_OFFSET), Quaternion.identity, _Selecters);

                GameManager.I.fieldState[posY][posX] = GameManager.SPRITE_NONE;

                // 駒の設定
                GameManager.I.fieldSprite[posY][posX] = sprite;

                // セレクターの設定
                GameManager.I.fieldSelecter[posY][posX] = selecter;
                GameManager.I.fieldSelecter[posY][posX].posY = posY;
                GameManager.I.fieldSelecter[posY][posX].posX = posX;
            }
        }

        GameManager.I.fieldState[3][3] = GameManager.SPRITE_WHITE;
        GameManager.I.fieldState[3][4] = GameManager.SPRITE_BLACK;
        GameManager.I.fieldState[4][3] = GameManager.SPRITE_BLACK;
        GameManager.I.fieldState[4][4] = GameManager.SPRITE_WHITE;
    }

    private void SetState()
    {
        for (int posY = 0; posY < GameManager.BOARD_SIZE; posY++)
        {
            for (int posX = 0; posX < GameManager.BOARD_SIZE; posX++)
            {
                GameManager.I.fieldSprite[posY][posX].SetState(GameManager.I.fieldState[posY][posX]);
                GameManager.I.fieldSelecter[posY][posX].SetState(GameManager.I.fieldState[posY][posX]);
            }
        }
    }

    private void SetGuide()
    {
        for (int posY = 0; posY < GameManager.BOARD_SIZE; posY++)
        {
            for (int posX = 0; posX < GameManager.BOARD_SIZE; posX++)
            {
                GameManager.I.fieldSelecter[posY][posX].SetGuide(GameManager.I.fieldState[posY][posX]);
            }
        }
    }
}
