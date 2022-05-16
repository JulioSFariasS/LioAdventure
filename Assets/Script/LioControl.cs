using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LioControl : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator visualAnim;
    private float horizontalMove = 0f;
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
    private Vector3 m_Velocity = Vector3.zero;
    public float runSpeed = 2f;
    public float glideFallSpeed;
    public float glideMoveSpeed;
    private float speed;
    public float jumpForce = 2f;
    public bool isJumping, isFalling, isGrounded, isAttacking; 
    public float isGliding;

    private bool facingRight = true;

    public Transform posPe;
    public float checkRadius;
    public LayerMask whatIsGround;
    public float jumpTime;
    public float jumpTimeCounter;

    private float _initialGravityScale;
    public bool caindo;

    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _initialGravityScale = rb.gravityScale;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(posPe.position, checkRadius, whatIsGround);
        horizontalMove = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Ataque") && !isAttacking)
        {
            StartCoroutine(GetAtaque());
        }

        m_Velocity = rb.velocity;

        if (rb.velocity.y >=0)
        {
            caindo = false;
        }
        else
        if (rb.velocity.y <=0)
        {
            caindo = true;
        }


        if (horizontalMove > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        if (horizontalMove < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        Pular();

        AnimaVisual();
    }


    private void FixedUpdate()
    {
        Movimento();
    }

    public void Movimento()
    {
        Vector3 targetVelocity = new Vector2(horizontalMove * runSpeed * Time.fixedDeltaTime, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, movementSmoothing);
        //rb.velocity = new Vector2(horizontalMove * speed * Time.fixedDeltaTime, rb.velocity.y);
    }

    public void Pular()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetButton("Jump") && isJumping)
        {

            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }

        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }

        if (caindo && !isJumping && Input.GetButton("Jump") && !isGrounded)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, -glideFallSpeed);
            speed = glideMoveSpeed;
            movementSmoothing = 0.15f;
            isGliding = 1f;
        }
        else
        {
            rb.gravityScale = _initialGravityScale;
            speed = runSpeed;
            movementSmoothing = 0f;
            isGliding = 0f;
        }
    }

    private IEnumerator GetAtaque()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.25f);
        isAttacking = false;
    }

    public void OnLanding()
    {
        isJumping = false;
    }

    private void AnimaVisual()
    {
        visualAnim.SetFloat("Speed", Mathf.Abs(horizontalMove));
        visualAnim.SetBool("IsFalling", caindo);
        visualAnim.SetBool("IsGrounded", isGrounded);
        visualAnim.SetFloat("IsGliding", isGliding);
        visualAnim.SetBool("IsAttacking", isAttacking);
    }
}
