using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultMenu : MonoBehaviour
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

    public void RestartMenu()
    {
        sceneName = SceneManager.GetActiveScene().name;
        ChangeScene();
    }

    public void ResettingMenu()
    {
        GameManager.I.titleMode = GameManager.Title.Setting;
        sceneName = TITLE_SCENE;
        ChangeScene();
    }

    public void TitleMenu()
    {
        GameManager.I.titleMode = GameManager.Title.Menu;
        sceneName = TITLE_SCENE;
        ChangeScene();
    }

    private async void ChangeScene() {
        await fade.FadeOut();

        SceneManager.LoadScene(sceneName);
    }
}
