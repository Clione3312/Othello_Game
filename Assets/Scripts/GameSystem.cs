using UnityEngine;

public class GameSystem : MonoBehaviour
{
    [SerializeField, Header("Othello Board")]
    private GameObject _Board;

    [SerializeField, Header("Sprite")]
    private Sprite _Sprite;
    [SerializeField]private Transform _Sprites;

    [SerializeField, Header("Selecter")]
    private Selecter _Selecter;
    [SerializeField]private Transform _Selecters;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameSetting();
        InitializeGame();

        if(!Locator<SpriteCount>.IsValid()) Locator<SpriteCount>.Bind(new SpriteCount());
        Locator<SpriteCount>.I.CalcSpriteCount();
    }

    private void GameSetting(){
        if (true) {
            GameManager.I.yourSprite = GameManager.SPRITE_BLACK;
            GameManager.I.enemySprite = GameManager.SPRITE_WHITE;
        } else {

        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void InitializeGame(){
        for(int y = 0; y < GameManager.BOARD_SIZE; y++){
            for(int x = 0; x < GameManager.BOARD_SIZE; x++){
                var sprite = Instantiate(_Sprite, new Vector3(y - GameManager.INST_OFFSET, GameManager.INST_OFFSET_Y, x - GameManager.INST_OFFSET), Quaternion.identity, _Sprites);
                var selecter = Instantiate(_Selecter, new Vector3(y - GameManager.INST_OFFSET, GameManager.INST_OFFSET_Y, x - GameManager.INST_OFFSET), Quaternion.identity, _Selecters);

                GameManager.I.fieldSprite[y][x] = sprite;

                GameManager.I.fieldSelecter[y][x] = selecter;
                GameManager.I.fieldSelecter[y][x].posY = y;
                GameManager.I.fieldSelecter[y][x].posX = x;
            }
        }

        GameManager.I.fieldState[3][3] = GameManager.SPRITE_WHITE;
        GameManager.I.fieldState[3][4] = GameManager.SPRITE_BLACK;
        GameManager.I.fieldState[4][3] = GameManager.SPRITE_BLACK;
        GameManager.I.fieldState[4][4] = GameManager.SPRITE_WHITE;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFieldState();
    }

    /// <summary>
    /// フィールドの状態を更新
    /// </summary>
    private void UpdateFieldState(){
        for(int y = 0; y < GameManager.BOARD_SIZE; y++){
            for(int x = 0; x < GameManager.BOARD_SIZE; x++){
                GameManager.I.fieldSprite[y][x].SetState(GameManager.I.fieldState[y][x]);
                GameManager.I.fieldSelecter[y][x].SetState(GameManager.I.fieldState[y][x]);
            }
        }
    }
}
