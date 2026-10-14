using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] PowerUpScriptableObject powerUpData;
    PlayerController playerController;
    SpriteRenderer powerUpSpriteRenderer;
    Collider2D powerUpCollider;

    float timeLeft;
    bool isActive;

    void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        powerUpSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        powerUpCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (!isActive) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            playerController.DeactivatePowerUp(powerUpData);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive) return;
        if (!other.CompareTag("Player")) return;

        isActive = true;
        timeLeft = powerUpData.TimeLimit;

        powerUpSpriteRenderer.enabled = false;
        powerUpCollider.enabled = false;

        playerController.ApplyPowerUp(powerUpData);
    }
}
