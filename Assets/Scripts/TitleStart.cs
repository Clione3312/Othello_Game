using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class TitleStart : MonoBehaviour
{
    #region Title
    [Header("Start")]
    [SerializeField] public CanvasGroup titlePanel;
    [SerializeField] public CanvasGroup titleLogo;
    [SerializeField] public CanvasGroup titleText;
    #endregion

    public bool isStart { get; private set; }
    public void SetIsStart(bool value) { isStart = value; }

    public async UniTask ShowTitle()
    {
        titlePanel.gameObject.SetActive(true);
        titleText.gameObject.SetActive(true);

        await titleLogo.DOFade(1, 5f).Play().AsyncWaitForCompletion();
        await UniTask.WaitForSeconds(1.0f);
        await titleText.DOFade(1, 2.0f).SetLoops(-1, LoopType.Yoyo).Play().AsyncWaitForCompletion();

        SetIsStart(true);
    }

    public async UniTask HideTitle()
    {
        titleText.gameObject.SetActive(false);
        await UniTask.WaitForSeconds(0.5f);
        await titleLogo.DOFade(0, 2f).Play().AsyncWaitForCompletion();
        titlePanel.gameObject.SetActive(false);

        SetIsStart(false);
    }

    public void ShowTitleMenu()
    {
        GameManager.I.titleMode = GameManager.Title.Menu;
    }
}
