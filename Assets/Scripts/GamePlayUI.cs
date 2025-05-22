using System;
using TMPro;
using UnityEngine;

public class GamePlayUI : MonoBehaviour
{
    [SerializeField, Header("GuideText")]
    private TextMeshProUGUI guideText;

    private const string GUIDE_TEXT_HEADER = "Ｇ：アシスト ";

    public void ToggleShowGuide(){
        GameManager.I.isGuide = !GameManager.I.isGuide;
        guideText.text = GUIDE_TEXT_HEADER + (GameManager.I.isGuide ? "ＯＦＦ" : "ＯＮ");
    }
}
