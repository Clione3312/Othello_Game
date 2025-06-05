using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ESCMenu : MonoBehaviour
{
    [SerializeField]private FadeSystem fade;
    private enum Title : int
    {
        Title,
        Menu,
        Setting,
        Option,
        Exit
    }

    private string sceneName = string.Empty;
    private const string TITLE_SCENE = "Title";

    [SerializeField, Header("ESC Menu")]
    private CanvasGroup escMenu;

    public void ShowESCMenu()
    {
        escMenu.gameObject.SetActive(!escMenu.gameObject.activeSelf);
    }

    public void CloseESCMenu()
    {
        escMenu.gameObject.SetActive(false);
    }

    public void RestartMenu()
    {
        sceneName = SceneManager.GetActiveScene().name;
        ChangeScene();
    }

    public void ResettingMenu()
    {
        GameManager.I.TitleMode((int)Title.Setting);
        sceneName = TITLE_SCENE;
        ChangeScene();
    }

    public void TitleMenu()
    {
        GameManager.I.TitleMode((int)Title.Menu);
        sceneName = TITLE_SCENE;
        ChangeScene();
    }

    private async void ChangeScene() {
        await fade.FadeOut();

        SceneManager.LoadScene(sceneName);
    }
}
