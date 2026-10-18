using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerController))]
public class GameProgress : MonoBehaviour
{
    public static bool ContinueSaved;

    // Lives
    [SerializeField] public int lives = 3;
    [SerializeField] float fallLimitY = -30f;
    [SerializeField] float invulnerableTime = 1.5f;

    // Puntos
    [SerializeField] int pointsPerMeter = 1;

    [SerializeField] Rigidbody2D rb;
    PlayerController controller;
    SpriteRenderer sprite;
    Vector2 respawnPoint;
    float startX, maxDistance, invulnerableUntil;
    public int highScore;
    public bool IsGameOver;

    // Score points are the player's max distance
    public int Score => Mathf.FloorToInt(maxDistance) * pointsPerMeter;

    void Awake()
    {
        controller = GetComponent<PlayerController>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        startX = transform.position.x;
        respawnPoint = transform.position;
        highScore = PlayerPrefs.GetInt("highScore", 0);

        if (ContinueSaved && PlayerPrefs.HasKey("cpX"))
        {
            respawnPoint = new Vector2(PlayerPrefs.GetFloat("cpX"), PlayerPrefs.GetFloat("cpY"));
            lives = PlayerPrefs.GetInt("lives", lives);
            maxDistance = PlayerPrefs.GetFloat("maxDist", 0);
            transform.position = respawnPoint;
        }
        else DeleteSave();
        ContinueSaved = false;
    }

    void Update()
    {
        if (IsGameOver) return;

        // Only counts max distance even if you respawn further back
        maxDistance = Mathf.Max(maxDistance, transform.position.x - startX);

        // Invincibility frames
        if (sprite) sprite.enabled = Time.time > invulnerableUntil || (int)(Time.time * 10) % 2 == 0;

        if (transform.position.y < fallLimitY) LoseLife();
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Hazard")) LoseLife();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsGameOver) return;

        if (other.CompareTag("Checkpoint"))
        {
            if (other.transform.position.x > respawnPoint.x)
            {
                respawnPoint = other.transform.position;
                SaveGame();
            }
        }
        else if (other.CompareTag("Hazard")) LoseLife();
    }

    void LoseLife()
    {
        if (IsGameOver || Time.time < invulnerableUntil) return;

        lives--;
        if (lives <= 0) { EndGame(); return; }

        // Respawn in last checkpoint
        controller.ResetState();
        rb.position = respawnPoint;
        transform.position = respawnPoint;
        invulnerableUntil = Time.time + invulnerableTime;
    }

    void EndGame()
    {
        IsGameOver = true;
        controller.enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        if (sprite) sprite.enabled = true;

        if (Score > highScore)
        {
            highScore = Score;
            PlayerPrefs.SetInt("highScore", highScore);
        }
        DeleteSave();
        PlayerPrefs.Save();
    }

    // PlayerPrefs
    void SaveGame()
    {
        PlayerPrefs.SetFloat("cpX", respawnPoint.x);
        PlayerPrefs.SetFloat("cpY", respawnPoint.y);
        PlayerPrefs.SetInt("lives", lives);
        PlayerPrefs.SetFloat("maxDist", maxDistance);
        PlayerPrefs.Save();
    }

    static void DeleteSave()
    {
        foreach (var key in new[] { "cpX", "cpY", "lives", "maxDist" })
            PlayerPrefs.DeleteKey(key);
    }

    public static bool HasSave() => PlayerPrefs.HasKey("cpX");
}