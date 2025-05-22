using UnityEngine;

public class Selecter : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material _None;
    [SerializeField] private Material _Select;

    [Header("Calc Sprite")]
    private CalcSprite calcSprite = new CalcSprite();

    [Header("Guide")]
    [SerializeField] private Transform _guide;

    public int posY = 0;
    public int posX = 0;

    public void OnMouseDown()
    {
        string putPosition = calcSprite.GetSpritesPosition(posY, posX, GameManager.I.fieldState, GameManager.I.yourSprite, GameManager.I.enemySprite);
        if (putPosition != string.Empty) GameManager.I.selectAddress = string.Format("{0},{1}", posY, posX);
    }

    public void OnMouseEnter()
    {
        string putPosition = calcSprite.GetSpritesPosition(posY, posX, GameManager.I.fieldState, GameManager.I.yourSprite, GameManager.I.enemySprite);
        if (putPosition != string.Empty) GetComponent<MeshRenderer>().material = _Select;
    }

    public void OnMouseExit()
    {
        GetComponent<MeshRenderer>().material = _None;
    }

    public void SetState(int state)
    {
        string putPosition = calcSprite.GetSpritesPosition(posY, posX, GameManager.I.fieldState, GameManager.I.yourSprite, GameManager.I.enemySprite);

        gameObject.SetActive(state == GameManager.SPRITE_NONE && GameManager.I.currTurn == GameManager.TURN_YOUR);
    }

    public void SetGuide(int state)
    {
        string putPosition = calcSprite.GetSpritesPosition(posY, posX, GameManager.I.fieldState, GameManager.I.yourSprite, GameManager.I.enemySprite);

        _guide.gameObject.SetActive(state == GameManager.SPRITE_NONE && GameManager.I.currTurn == GameManager.TURN_YOUR && GameManager.I.isGuide && putPosition != string.Empty);
    }
}
