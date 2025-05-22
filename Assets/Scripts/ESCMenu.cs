using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void RestartMenu() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
