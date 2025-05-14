using UnityEngine;

public class ESCMenu : MonoBehaviour
{
    [SerializeField, Header("ESC Menu")]
    private CanvasGroup escMenu;

    public void ShowESCMenu() {
        escMenu.gameObject.SetActive(!escMenu.gameObject.activeSelf);
    }

    public void CloseESCMenu() {
        escMenu.gameObject.SetActive(false);
    }
}
