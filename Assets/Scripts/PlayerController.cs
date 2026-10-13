using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D bc;
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

    // Sliding variables
    private bool isSliding = false;
    private Vector2 originalSize;
    private Vector2 originalOffset;
    [SerializeField] private float slideCollider = 0.7f;
    private float newSlideOffset;

    // Grappling Hook
    [SerializeField] private GrapplingHook grapple;

    public Animator animator;

    void Awake()
    {
        originalSize = bc.size;
        originalOffset = bc.offset;
    }

    void Update()
    {
        CheckGround();
        Jumping();
        Sliding();
        UpdateAnimator();
    }

    void FixedUpdate()
    {
        Running();
    }

    // Running logic
    void Running()
    {
        if (!grapple.isGrappling)
        {
            rb.linearVelocityX = runSpeed;
        }
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
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            // If true, rigid body's vertical speed is equivalent to jumpForce
            isJumping = true;
            rb.linearVelocityY = jumpForce;
        }

        // High jump logic
        // Checks if player is currently jumping AND holding down jump button
        if (isJumping && Input.GetKey(KeyCode.Space))
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
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
            jumpTimer = 0;
        }
    }

    void Sliding()
    {
        // If player is on the ground AND holding left shift
        if (isGrounded && Input.GetKey(KeyCode.LeftShift))
        {
            isSliding = true;
            bc.size = new Vector2(bc.size.x, slideCollider); // Reduces player collider's height

            // Calculates the new offset of the player while sliding so the sprite doesn't move
            newSlideOffset = originalOffset.y - (originalSize.y - slideCollider) / 2;
            bc.offset = new Vector2(bc.offset.x, newSlideOffset);
        }
        else
        {
            isSliding = false;
            bc.size = originalSize;
            bc.offset = originalOffset;
            // TODO: Check if there's a box on top of player when finishing slide with Physics2D.OverlapBox or Raycast with a LayerMask
        }
    }

    // Updates movement animations
    void UpdateAnimator()
    {
        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocityX));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isSliding", isSliding);
    }
}
