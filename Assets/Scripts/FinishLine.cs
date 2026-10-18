using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] float reloadDelay = 1f; // TODO: Change this to next level transition
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Finish line");
            Invoke(nameof(NextLevel), reloadDelay);
        }
    }

    void NextLevel()
    {
        int current = int.Parse(SceneManager.GetActiveScene().name.Replace("Level", ""));   // "Level2" -> 2
        int unlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);
        PlayerPrefs.SetInt("UnlockedLevel", Mathf.Max(unlocked, Mathf.Min(current + 1, 3)));
        PlayerPrefs.Save();

        SceneManager.LoadScene(current >= 3 ? "MainMenu" : $"Level{current + 1}");
    }
}
