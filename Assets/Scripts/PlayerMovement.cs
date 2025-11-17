using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
//Initializing variables
    public float MoveSpeed = 8f;
    public float jumpForce = 16f;

    private Rigidbody2D rb;
    private float horizontal;
    private bool isJumping;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;



//Finds Rigidbody2D on objected connected to script
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

//Coyote time
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

//Jump buffer
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

//Jump
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && !isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            jumpBufferCounter = 0f;

            StartCoroutine(JumpCooldown());
        }

//Variable jump height
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);

            coyoteTimeCounter = 0f;
        }
    }

//Jump cooldown
    private IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(0.4f);
        isJumping = false;
    }



//Horizontal movement
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * MoveSpeed, rb.linearVelocity.y);
    }

//Checks if GroundCheck is touching ground
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}