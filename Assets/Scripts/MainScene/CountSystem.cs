using TMPro;
using UnityEngine;
using DG.Tweening;
using UniRx;
using Cysharp.Threading.Tasks;

public class CountSystem : MonoBehaviour
{
    [Header("Text Score")]
    [SerializeField] private TextMeshProUGUI _YourScoreText;
    [SerializeField] private TextMeshProUGUI _EnemyScoreText;

    private void Start()
    {
        GameManager.I.yourScore.Subscribe(async _ => await  SetYourCount());
        GameManager.I.yourScore.Subscribe(async _ => await SetEnemyCount());
    }

    private async UniTask SetYourCount()
    {
        int prevYourScore = GameManager.I.PrevYourScore();
        int currYourScore = GameManager.I.YourScore();

        await _YourScoreText.DOCounter(prevYourScore, currYourScore, 1.0f).Play();
    }

    private async UniTask SetEnemyCount()
    {
        int prevEnemyScore = GameManager.I.PrevEnemyScore();
        int currEnemyScore = GameManager.I.EnemyScore();

        await _EnemyScoreText.DOCounter(prevEnemyScore, currEnemyScore, 1.0f).Play();
    }
}
