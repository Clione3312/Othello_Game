using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEditor.PackageManager;

public class TitleMenu : MonoBehaviour
{
    #region Menu
    [Header("Menu Panel")]
    [SerializeField] public CanvasGroup menuPanel;
    [SerializeField] public CanvasGroup menuObject;
    [SerializeField] public GameObject firstObject;
    #endregion

    public async UniTask ShowMenu()
    {
        menuPanel.gameObject.SetActive(true);

        EventSystem.current.firstSelectedGameObject = firstObject;
        EventSystem.current.SetSelectedGameObject(firstObject);

        await UniTask.WaitForSeconds(0.5f);
        await menuObject.DOFade(1, 3f).Play().AsyncWaitForCompletion();
        await UniTask.WaitForSeconds(0.5f);

    }

    public async UniTask HideMenu()
    {
        await UniTask.WaitForSeconds(0.5f);
        await menuObject.DOFade(0, 1f).Play().AsyncWaitForCompletion();
        await UniTask.WaitForSeconds(0.5f);

        menuPanel.gameObject.SetActive(false);
    }

    public void CancelMenu()
    {
        GameManager.I.titleMode = GameManager.Title.Title;
    }
}
