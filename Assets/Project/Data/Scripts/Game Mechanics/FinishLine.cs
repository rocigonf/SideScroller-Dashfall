using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] float reloadDelay = 1f;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Debug.Log("Finish line");
            Invoke(nameof(NextLevel), reloadDelay);
        }
    }

    void NextLevel()
    {
        int current = int.Parse(SceneManager.GetActiveScene().name.Replace("Level", ""));   // "Level2" -> 2
        PlayerPrefs.Save();

        SceneManager.LoadScene(current >= 3 ? "MainMenu" : $"Level{current + 1}");
    }
}
