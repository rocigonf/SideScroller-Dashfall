using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("ChooseLevel");
    }

    public void GoToLevel(int level)
    {
        SceneManager.LoadScene($"Level{level}");
        Debug.Log($"Going to level {level}");
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game is closing");
    }
}
