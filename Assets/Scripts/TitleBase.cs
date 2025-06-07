using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class TitleBase : MonoBehaviour
{
    #region Base
    [Header("Base Panel")]
    [SerializeField] private CanvasGroup bgPanel;
    #endregion

    public async UniTask UpdateBgPanel()
    {
        switch (GameManager.I.titleMode)
        {
            case GameManager.Title.Title:
                await bgPanel.transform.DOScale(0f, 1.0f).Play().AsyncWaitForCompletion();
                break;
            case GameManager.Title.Menu:
                await bgPanel.transform.DOScale(1f, 1.0f).Play().AsyncWaitForCompletion();
                break;
        }
    }
}
