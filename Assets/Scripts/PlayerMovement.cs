using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
//Initializing variables
    public Animator animator;

    public float MoveSpeed = 8f;
    public float jumpForce = 16f;

    private Rigidbody2D rb;
    public SpriteRenderer sr;
    private float horizontal;
    private bool isJumping;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
    public bool flipped = false;

    private float fallSpeedYDampingChangeThreshold;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;


//Finds Rigidbody2D on objected connected to script
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        fallSpeedYDampingChangeThreshold = CameraManager.instance.fallSpeedYDampingChangeThreshold;
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        // Play walk animation
        animator.SetFloat("Speed", Mathf.Abs(horizontal));

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

        if (rb.linearVelocityY < fallSpeedYDampingChangeThreshold && !CameraManager.instance.isLerpingYDamping && !CameraManager.instance.lerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(true);
        }

        if(rb.linearVelocityY >= 0f && !CameraManager.instance.isLerpingYDamping && CameraManager.instance.lerpedFromPlayerFalling)
        {
            CameraManager.instance.lerpedFromPlayerFalling = false;
            CameraManager.instance.LerpYDamping(false);
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

        // Call to flip player depending on movement direction
        if (horizontal > 0 && flipped)
        {
            Flip();
        }
        if (horizontal < 0 && !flipped)
        {
            Flip();
        }
    }

//Checks if GroundCheck is touching ground
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    // Flip player
    private void Flip()
    {
        sr.flipX = !flipped;

        flipped = !flipped;
    }

    //Spike collision check
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Spike"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}