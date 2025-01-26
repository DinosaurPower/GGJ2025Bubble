using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Components")]
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;  // Optional, if you want to flip the sprite
    private Animator animator;              // Optional, if you have animations

    [Header("Ground Check")]
    public Transform groundCheck;       // Assign in Inspector
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;       // Assign a layer for the ground

    private bool isGrounded;
    private float horizontalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get horizontal input (A/D or Left/Right arrows)
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // Handle sprite flipping (optional)
        if (horizontalInput > 0)
            spriteRenderer.flipX = true;
        else if (horizontalInput < 0)
            spriteRenderer.flipX = false;

        // Update Animator if you want to show running animation
        // e.g. animator.SetBool("isRunning", horizontalInput != 0);
    }

    void FixedUpdate()
    {
        // Horizontal movement
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // Check if player is grounded
        CheckGrounded();
    }

    void CheckGrounded()
    {
        // Circle/Raycast overlap check at groundCheck position
        // Make sure your groundCheck is placed near the bottom of the player
        Collider2D hit = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isGrounded = (hit != null);
    }

    void Jump()
    {
        // Clear existing Y velocity before jump
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    // Draw the groundCheck in the Scene View for debugging
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
