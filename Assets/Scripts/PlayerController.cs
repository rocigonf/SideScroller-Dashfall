using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] BoxCollider2D bc;
    [SerializeField] LayerMask groundLayer;

    // Jump variables
    [SerializeField] float jumpForce = 10f;
    [SerializeField] Transform feetPos;
    [SerializeField] float groundDistance = 0.25f;
    [SerializeField] float jumpTime = 0.3f; // Seconds you can hold down the jump button to perform a high jump
    public bool IsGrounded { get; private set; } = false;
    public bool IsJumping { get; private set; } = false;
    float jumpTimer;

    // Run variables
    [SerializeField] float runSpeed = 8f;
    [SerializeField] float acceleration = 150f, deceleration = 50f;
    float currentSpeed;

    // Grappling Hook
    [SerializeField] GrapplingHook grapple;

    // Power Ups
    [SerializeField] float currentDashBonus = 0f;
    public bool IsDashing { get; private set; } = false;
    float originalGravity;

    // Player Slide
    [SerializeField] PlayerSlide playerSlide;

    // Animator
    public Animator animator;

    void Awake()
    {
        originalGravity = rb.gravityScale;
    }

    void Update()
    {
        CheckGround();
        Jumping();
        UpdateAnimator();
    }

    void FixedUpdate()
    {
        Running();
        playerSlide.UpdateSlide();
    }

    // Running logic
    void Running()
    {
        if (!grapple.isGrappling)
        {
            float objectiveSpeed = runSpeed + currentDashBonus;
            float rate;
            if (objectiveSpeed > currentSpeed)
            {
                rate = acceleration;
            }
            else
            {
                rate = deceleration;
            }
            currentSpeed = Mathf.MoveTowards(currentSpeed, objectiveSpeed, rate * Time.fixedDeltaTime);
            rb.linearVelocityX = currentSpeed;
        }
        else
        {
            rb.linearVelocityX = currentSpeed;
        }
    }

    // Draws an invisible circle in the player's feet position to check if they're touching the ground
    void CheckGround()
    {
        IsGrounded = Physics2D.OverlapCircle(feetPos.position, groundDistance, groundLayer);
    }

    void Jumping()
    {
        // Jump logic
        // Checks if player is on the ground AND presses jump button
        if (IsGrounded && !playerSlide.IsSliding && !IsDashing && Input.GetKeyDown(KeyCode.Space))
        {
            // If true, rigid body's vertical speed is equivalent to jumpForce
            IsJumping = true;
            rb.linearVelocityY = jumpForce;
        }

        // High jump logic
        // Checks if player is currently jumping AND holding down jump button
        if (IsJumping && !IsDashing && Input.GetKey(KeyCode.Space))
        {
            // If the player has been jumping less time than the max jumpTime, the player keeps going up
            if (jumpTimer < jumpTime)
            {
                rb.linearVelocityY = jumpForce;
                jumpTimer += Time.deltaTime;
            }
            else
            {
                IsJumping = false;
            }
        }

        // Checks if player stops holding jump button
        if (Input.GetKeyUp(KeyCode.Space))
        {
            IsJumping = false;
            jumpTimer = 0;
        }
    }

    // Updates movement animations
    void UpdateAnimator()
    {
        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocityX));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetBool("isGrounded", IsGrounded);
        animator.SetBool("isSliding", playerSlide.IsSliding);
        animator.SetBool("isDashing", IsDashing);
    }

    public void ApplyPowerUp(PowerUpScriptableObject powerUpData)
    {
        switch (powerUpData.type)
        {
            case PowerUpType.Dash:
                currentDashBonus = powerUpData.PowerUpValue;
                rb.gravityScale = 0f;
                rb.linearVelocityY = 0f;
                IsJumping = false;
                IsDashing = true;
                break;
        }
    }

    public void DeactivatePowerUp(PowerUpScriptableObject powerUpData)
    {
        switch (powerUpData.type)
        {
            case PowerUpType.Dash:
                currentDashBonus = 0f;
                rb.gravityScale = originalGravity;
                IsDashing = false;
                break;
        }
    }

    // Resets player state when respawning
    public void ResetState()
    {
        currentSpeed = runSpeed;
        currentDashBonus = 0f;
        IsDashing = false;
        IsJumping = false;
        jumpTimer = 0f;
        rb.gravityScale = originalGravity;
        grapple.ReleaseGrapple();
        playerSlide.ResetSlide();
        rb.linearVelocity = Vector2.zero;
    }
}
