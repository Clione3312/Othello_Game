using Cysharp.Threading.Tasks;
using UnityEngine;

public class Selecter : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material _None;
    [SerializeField] private Material _Select;

    [Header("Calc Sprite")]
    private CalcSprite calcSprite = new CalcSprite();

    [Header("Guide")]
    [SerializeField] private GameObject _guide;

    private int posY = 0;
    private int posX = 0;
    public void SetPosition(int posY, int posX)
    {
        this.posY = posY;
        this.posX = posX;
    }

    private const int SPRITE_NONE = GameManager.SPRITE_NONE;
    private const int SPRITE_BLACK = GameManager.SPRITE_BLACK;
    private const int SPRITE_WHITE = GameManager.SPRITE_WHITE;

    private const int TURN_IDLE = GameManager.TURN_IDLE;
    private const int TURN_YOUR = GameManager.TURN_YOUR;
    private const int TURN_ENEMY = GameManager.TURN_ENEMY;

    public void OnMouseDown()
    {
        string putPosition = calcSprite.GetSpritesPosition(posY, posX, GameManager.I.FieldState(), GameManager.I.YourSprite(), GameManager.I.EnemySprite());
        if (putPosition != string.Empty) GameManager.I.SelectAddress(string.Format("{0},{1}", posY, posX));
    }

    public void OnMouseEnter()
    {
        string putPosition = calcSprite.GetSpritesPosition(posY, posX, GameManager.I.FieldState(), GameManager.I.YourSprite(), GameManager.I.EnemySprite());
        if (putPosition != string.Empty) GetComponent<MeshRenderer>().material = _Select;
    }

    public void OnMouseExit()
    {
        GetComponent<MeshRenderer>().material = _None;
    }

    public void ChangeState(int state)
    {
        gameObject.SetActive(state == SPRITE_NONE && GameManager.I.Turn() == TURN_YOUR);
    }

    public void SetGuide(int state)
    {
        string putPosition = calcSprite.GetSpritesPosition(posY, posX, GameManager.I.FieldState(), GameManager.I.YourSprite(), GameManager.I.EnemySprite());

        _guide.SetActive(state == SPRITE_NONE && GameManager.I.Turn() == TURN_YOUR && GameManager.I.IsGuide() && putPosition != string.Empty);
    }
}
