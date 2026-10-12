using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;

    // Jump variables
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float groundDistance = 0.25f;
    [SerializeField] private float jumpTime = 0.3f; // Seconds you can hold down the jump button to perform a high jump
    private bool isGrounded = false;
    private bool isJumping = false;
    private float jumpTimer;

    // Run variables
    [SerializeField] private float runSpeed = 5f;

    public Animator animator;

    void Update()
    {
        CheckGround();
        Jumping();
        UpdateAnimator();
    }

    void FixedUpdate()
    {
        Running();
    }

    // Running logic
    void Running()
    {
        rb.linearVelocityX = runSpeed;
    }

    // Draws an invisible circle in the player's feet position to check if they're touching the ground
    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, groundDistance, groundLayer);
    }

    void Jumping()
    {
        // Jump logic
        // Checks if player is on the ground AND presses jump button
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            // If true, rigid body's vertical speed is equivalent to jumpForce
            isJumping = true;
            rb.linearVelocityY = jumpForce;
        }

        // High jump logic
        // Checks if player is currently jumping AND holding down jump button
        if (isJumping && Input.GetButton("Jump"))
        {
            // If the player has been jumping less time than the max jumpTime, the player keeps going up
            if (jumpTimer < jumpTime)
            {
                rb.linearVelocityY = jumpForce;
                jumpTimer += Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        // Checks if player stops holding jump button
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            jumpTimer = 0;
        }
    }

    // Updates movement animations
    void UpdateAnimator()
    {
        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocityX));
        animator.SetFloat("yVelocity", Mathf.Abs(rb.linearVelocity.y));
        animator.SetBool("IsGrounded", isGrounded);
    }
}
