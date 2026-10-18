using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] PowerUpScriptableObject powerUpData;
    [SerializeField] float respawnDelay = 2f;
    PlayerController playerController;
    SpriteRenderer powerUpSpriteRenderer;
    Collider2D powerUpCollider;

    float timeLeft;
    bool isActive;

    float respawnTimer;
    bool isWaitingToRespawn;


    void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        powerUpSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        powerUpCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (isWaitingToRespawn)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0f)
            {
                isWaitingToRespawn = false;
                powerUpSpriteRenderer.enabled = true;
                powerUpCollider.enabled = true;
            }
            return;
        }

        if (!isActive) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            playerController.DeactivatePowerUp(powerUpData);
            isActive = false;
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

    public void CancelIfActive()
    {
        if (!isActive) return;

        isActive = false;
        playerController.DeactivatePowerUp(powerUpData);

        respawnTimer = respawnDelay;
        isWaitingToRespawn = true;
    }

    public void ResetPowerUp()
    {
        if (isActive) playerController.DeactivatePowerUp(powerUpData);

        isActive = false;
        isWaitingToRespawn = false;
        powerUpSpriteRenderer.enabled = true;
        powerUpCollider.enabled = true;
    }
}
