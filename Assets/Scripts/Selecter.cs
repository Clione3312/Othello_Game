using UnityEngine;

public class Selecter : MonoBehaviour
{
    [SerializeField,Header("Materials")]
    private Material PutOn;
    [SerializeField] private Material PutError;
    [SerializeField] private Material Normal;

    /// <summary>
    /// y座標とx座標
    /// </summary>
    public int posY;
    public int posX;

    /// <summary>
    /// マウスが入った時
    /// </summary>
    public void OnMouseEnter()
    {
        if (!Locator<GetSpriteCheck>.IsValid()) {
            Locator<GetSpriteCheck>.Bind(new GetSpriteCheck());
        }

        string spritePos = Locator<GetSpriteCheck>.I.GetSpritePosition(posY, posX, GameManager.I.fieldState, GameManager.I.yourSprite, GameManager.I.enemySprite);
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
        if (!Locator<GetSpriteCheck>.IsValid()) {
            Locator<GetSpriteCheck>.Bind(new GetSpriteCheck());
        }

        string spritePos = Locator<GetSpriteCheck>.I.GetSpritePosition(posY, posX, GameManager.I.fieldState, GameManager.I.yourSprite, GameManager.I.enemySprite);

        gameObject.SetActive(State == GameManager.SPRITE_NONE);
        gameObject.transform.gameObject.SetActive(spritePos != string.Empty && State == GameManager.SPRITE_NONE && GameManager.I.isGuide);
    }
}
