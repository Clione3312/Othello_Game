using System;
using TMPro;
using UnityEngine;

public class GamePlayUI : MonoBehaviour
{
    [SerializeField, Header("GuideText")]
    private TextMeshProUGUI guideText;

    private const string GUIDE_TEXT_HEADER = "Ｇ：アシスト ";

    public void ToggleShowGuide(){
        GameManager.I.IsGuide(!GameManager.I.IsGuide());
        guideText.text = GUIDE_TEXT_HEADER + (GameManager.I.IsGuide() ? "ＯＦＦ" : "ＯＮ");
    }
}
