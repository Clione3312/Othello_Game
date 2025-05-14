using TMPro;
using UnityEngine;
using DG.Tweening;

public class DisplaySpriteCount : MonoBehaviour
{
    public static DisplaySpriteCount I { get; private set; } = null;

    [SerializeField, Header("Textarea")]
    private TextMeshProUGUI yText;
    [SerializeField] private TextMeshProUGUI eText;

    [SerializeField, Header("Animation config")]
    private float tFontSizeRate = 1.15f;
    [SerializeField] private float fsAnim = 0.2f;
    [SerializeField] private float ctAnim = 1.0f;

    private float yFontSize, eFontSize;
    private bool isAnimating = false;

    Sequence seq;

    private void Awake() {
        if (I == null) {
            I = this;
            return;
        }
        Destroy(gameObject);
    }

    private void Start() {
        yFontSize = yText.fontSize;
        eFontSize = eText.fontSize;
    }

    public void ShowScore() {
        if (isAnimating) return;
        if(GameManager.I.prevYourScore == GameManager.I.yourScore && GameManager.I.prevEnemyScore == GameManager.I.enemyScore) return;

        seq = DOTween.Sequence();
        seq
            .OnStart(() => {isAnimating = true;})
            .Append(yText.DOFontSize(yFontSize * tFontSizeRate, fsAnim))
            .Join(eText.DOFontSize(eFontSize * tFontSizeRate, fsAnim))
            .AppendInterval(0.2f)
            .Append(yText.DOCounter(GameManager.I.prevYourScore, GameManager.I.yourScore, ctAnim))
            .Join(eText.DOCounter(GameManager.I.prevEnemyScore, GameManager.I.enemyScore, ctAnim))
            .AppendInterval(0.2f)
            .Append(yText.DOFontSize(yFontSize, fsAnim))
            .Join(eText.DOFontSize(eFontSize, fsAnim))
            .OnComplete(() => { isAnimating = false; })
            .Play();


    }
}
