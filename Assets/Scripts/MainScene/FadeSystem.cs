using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading.Tasks;

public class FadeSystem : MonoBehaviour
{
    [Header("Fade Panel")]
    [SerializeField] private CanvasGroup fadePanel;

    [Header("Fade Time")]
    [SerializeField] private float fadeTime = 3f;

    private Sequence _sequence;
    public IEnumerator WaitSequence()
    {
        yield return new WaitWhile(() => _sequence.IsPlaying());
    }


    public async UniTask FadeIn()
    {
        await fadePanel.DOFade(0, fadeTime).Play().AsyncWaitForCompletion();
        await UniTask.WaitUntil(() => fadePanel.alpha == 0);
        fadePanel.gameObject.SetActive(false);
    }

    public async UniTask FadeOut()
    {
        fadePanel.gameObject.SetActive(true);
        await fadePanel.DOFade(1, fadeTime).Play().AsyncWaitForCompletion();
        await UniTask.WaitUntil(() => fadePanel.alpha == 1);
    }
}
