using UnityEngine;
using Unity.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void playGame()
    {
        SceneManager.LoadScene("Cutscene");
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
