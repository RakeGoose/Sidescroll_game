using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed = 5f;
    public float jumpForce = 12f;
    private int collisionDamage = 5;
    private float bounceForce = 5f;

    public Rigidbody2D playerRb;
    private Animator animator;
    private float movementX;
    private bool isGrounded;
    private bool movingRight = true;
    public float airControlFactor = 0.8f;

    public Transform groundCheck;
    public LayerMask groundLayers;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        movementX = Input.GetAxisRaw("Horizontal");

        bool isMoving = movementX != 0;
        animator.SetBool("IsMoving", isMoving);

        animator.SetBool("IsJumping", !isGrounded && playerRb.velocity.y > 0);
        animator.SetBool("IsFalling", !isGrounded && playerRb.velocity.y < 0);

        if (movementX > 0 && !movingRight)
        {
            Flip();
        }
        else if (movementX < 0 && movingRight)
        {
            Flip();
        }

        if (Input.GetButtonDown("Vertical") || Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump();
            }
        }
    }

    private void FixedUpdate()
    {
        float currentSpeed = playerSpeed;

        if (!isGrounded)
        {
            currentSpeed *= airControlFactor;
        }

        playerRb.velocity = new Vector2(movementX * currentSpeed, playerRb.velocity.y);
    }

    private void Jump()
    {
        playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamageOnCollision(collision);
        }
    }

    void Flip()
    {
        movingRight = !movingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void TakeDamageOnCollision(Collision2D collision)
    {
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(collisionDamage);
        }

        Vector2 bounceDirection = (transform.position - collision.transform.position).normalized;
        playerRb.velocity = new Vector2(playerRb.velocity.x, 0);
        playerRb.AddForce(new Vector2(bounceDirection.x * bounceForce, bounceForce), ForceMode2D.Impulse);
    }
}
