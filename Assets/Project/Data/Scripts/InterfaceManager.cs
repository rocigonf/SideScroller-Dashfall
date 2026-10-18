using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceManager : MonoBehaviour
{
    [SerializeField] MenuManager menuManager;
    [SerializeField] GameProgress progress;

    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text livesText;

    // Game over
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TMP_Text finalScoreText;

    int lastScore = -1, lastLives = -1;

    void Awake()
    {
        if (progress == null) progress = FindAnyObjectByType<GameProgress>();
    }

    void Start()
    {
        if (gameOverPanel) gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (progress.Score != lastScore)
        {
            lastScore = progress.Score;
            if (scoreText) scoreText.text = $"Score: {lastScore}";
        }

        if (progress.lives != lastLives)
        {
            lastLives = progress.lives;
            if (livesText) livesText.text = $"Lives: {lastLives}";
        }

        if (gameOverPanel && gameOverPanel.activeSelf != progress.IsGameOver)
        {
            gameOverPanel.SetActive(progress.IsGameOver);
            if (progress.IsGameOver && finalScoreText)
                finalScoreText.text = $"Final score: {progress.Score}";
        }
    }

    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}