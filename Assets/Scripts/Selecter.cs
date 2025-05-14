using UnityEngine;

public class Selecter : MonoBehaviour
{
    [SerializeField,Header("Materials")]
    private Material PutOn;
    [SerializeField] private Material PutError;
    [SerializeField] private Material Normal;

    [SerializeField,Header("Guide")]
    private GameObject goGuide;

    /// <summary>
    /// y座標とx座標
    /// </summary>
    public int posY;
    public int posX;

    /// <summary>
    /// マウスが押された時
    /// </summary>
    public void OnMouseDown()
    {
        if (GameManager.I.currTurn == GameManager.TURN_ENEMY) return;
        PlayerSystem.I.PutSprite(posY, posX);
    }

    /// <summary>
    /// マウスが入った時
    /// </summary>
    public void OnMouseEnter()
    {
        string spritePos = GetSpriteCheck.I.GetSpritePosition(posY, posX, GameManager.I.fieldState, GameManager.I.yourSprite, GameManager.I.enemySprite);
        gameObject.GetComponent<MeshRenderer>().material = spritePos != string.Empty ? PutOn : PutError;
    }

    /// <summary>
    /// マウスが離れた時
    /// </summary>
    public void OnMouseExit()
    {
        gameObject.GetComponent<MeshRenderer>().material = Normal;
    }

    /// <summary>
    /// 状態を設定
    /// </summary>
    /// <param name="State">駒の状態</param>
    public void SetState(int State){
        string spritePos = GetSpriteCheck.I.GetSpritePosition(posY, posX, GameManager.I.fieldState, GameManager.I.yourSprite, GameManager.I.enemySprite);

        gameObject.SetActive(State == GameManager.SPRITE_NONE && GameManager.I.currTurn == GameManager.TURN_YOUR );
        goGuide.SetActive(spritePos != string.Empty && GameManager.I.isGuide);
    }
}
