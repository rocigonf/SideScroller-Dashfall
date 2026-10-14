using UnityEngine;

public class PlayerSlide : MonoBehaviour
{
    [SerializeField] BoxCollider2D bc;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float slideHeight = 0.7f;

    public bool IsSliding { get; private set; }

    PlayerController player;
    Vector2 originalSize, originalOffset;
    Vector2 slideSize, slideOffset;
    Vector2 checkSize;

    void Awake()
    {
        player = GetComponent<PlayerController>();

        originalSize = bc.size;
        originalOffset = bc.offset;

        // Gets the height difference between the player's collision while sliding and the obstacle
        float heightDiff = originalSize.y - slideHeight;
        slideSize = new Vector2(originalSize.x, slideHeight);
        slideOffset = new Vector2(originalOffset.x, originalOffset.y - heightDiff / 2);
        checkSize = new Vector2(originalSize.x * 0.9f, heightDiff);
    }

    public void UpdateSlide()
    {
        bool canSlide = player.IsGrounded && !player.IsDashing && Input.GetKey(KeyCode.LeftShift);

        if (canSlide && !IsSliding)
            StartSlide();
        else if (!canSlide && IsSliding && !HasCeiling())
            EndSlide();
    }

    void StartSlide()
    {
        IsSliding = true;
        bc.size = slideSize;
        bc.offset = slideOffset;
    }

    void EndSlide()
    {
        IsSliding = false;
        bc.size = originalSize;
        bc.offset = originalOffset;
    }

    // Calculates the box above the player's head used to detect an ceiling
    void GetCeilingCheckBox(out Vector2 center, out Vector2 size)
    {
        Bounds b = bc.bounds;
        size = checkSize;
        center = new Vector2(b.center.x, b.max.y + checkSize.y / 2);
    }

    // Checks if the player has a ceiling on top while sliding
    bool HasCeiling()
    {
        GetCeilingCheckBox(out Vector2 center, out Vector2 size);
        return Physics2D.OverlapBox(center, size, 0f, obstacleLayer) != null;
    }

    void OnDrawGizmosSelected()
    {
        if (!IsSliding) return;

        GetCeilingCheckBox(out Vector2 center, out Vector2 size);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(center, size);
    }
}