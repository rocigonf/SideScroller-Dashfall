using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerController))]
public class GameProgress : MonoBehaviour
{
    // Lives
    [SerializeField] public int lives = 3;
    [SerializeField] float fallLimitY = -30f;
    [SerializeField] float invulnerableTime = 1.5f;

    // Score
    [SerializeField] int pointsPerMeter = 1;


    [SerializeField] Rigidbody2D rb;
    PlayerController player;
    SpriteRenderer sprite;
    Vector2 respawnPoint;
    float startX, maxDistance, invulnerableUntil;
    public bool IsGameOver;

    // Score points are the player's max distance multiplied by 100
    public int Score => Mathf.FloorToInt(maxDistance) * pointsPerMeter * 100;

    void Awake()
    {
        player = GetComponent<PlayerController>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        startX = transform.position.x;
        respawnPoint = transform.position;
        Time.timeScale = 1f;
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
                respawnPoint = other.transform.position;
        }
        else if (other.CompareTag("Hazard")) LoseLife();
    }

    void LoseLife()
    {
        if (IsGameOver || Time.time < invulnerableUntil) return;

        lives--;
        ResetPowerUps();

        if (lives <= 0)
        {
            EndGame();
            return;
        }

        player.ResetState();
        rb.position = respawnPoint;
        transform.position = respawnPoint;
        invulnerableUntil = Time.time + invulnerableTime;
    }

    void ResetPowerUps()
    {
        foreach (var powerUp in FindObjectsByType<PowerUpManager>())
            powerUp.ResetPowerUp();
    }

    void EndGame()
    {
        IsGameOver = true;
        player.enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        if (sprite) sprite.enabled = true;
    }
}