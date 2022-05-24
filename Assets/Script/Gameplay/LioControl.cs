using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LioControl : MonoBehaviour
{
    public Rigidbody2D rb, rbSeguir;
    public Animator visualAnim;
    public Collider2D colisor, trigger, colisorArma;
    private float horizontalMove = 0f;
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
    private Vector3 m_Velocity = Vector3.zero;
    public float runSpeed = 2f;
    public float glideFallSpeed;
    public float glideMoveSpeed;
    private float speed;
    public float jumpForce = 2f;
    public bool canJump=true, isJumping, isFalling, isGrounded, isAttacking; 
    public float isGliding;
    public float attackCooldown;

    public Transform posPe, posPeEsq, posPeDir;
    public float checkRadius;
    public LayerMask whatIsGround;
    public float jumpTime;
    public float jumpTimeCounter;

    private float _initialGravityScale;
    public bool caindo;

    private GameSystem gameSystem;

    public ParticleSystem attackParticles, glideParticles, normalParticles, poeiraGroundedParticles, damageParticles;

    public bool vulneravel;
    protected SpriteRenderer[] partesCorpoSpr;
    protected Shader shaderGUItext;
    protected Shader shaderSpritesDefault;
    protected Color[] coresOriginais, coresOriginaisTranslucidas;

    public EstrelasUniao estrelasUniao;
    public bool morte, vitoria;

    PlayerControl controls;
    Vector2 movimento;

    private void Awake()
    {
        controls = new PlayerControl();
    }

    void Start()
    {
        gameSystem = GameSystem.getInstance();
        gameSystem.lioCtrl = this;
        rb = GetComponent<Rigidbody2D>();
        _initialGravityScale = rb.gravityScale;
        StartCoroutine(GetGlide());
        StartCoroutine(GetNormal());
        StartCoroutine(GetPoeiraGrounded());
        Physics.IgnoreLayerCollision(3, 8);

        //Pisca branco
        vulneravel = true;
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");
        partesCorpoSpr = GetComponentsInChildren<SpriteRenderer>();
        coresOriginais = new Color[partesCorpoSpr.Length];
        coresOriginaisTranslucidas = new Color[partesCorpoSpr.Length];
        CoresOriginais();
    }

    void Update()
    {
        if (!morte && !vitoria)
        {
            
            isGrounded = (Physics2D.OverlapCircle(posPe.position, checkRadius, whatIsGround) || Physics2D.OverlapCircle(posPeDir.position, checkRadius, whatIsGround) || Physics2D.OverlapCircle(posPeEsq.position, checkRadius, whatIsGround))
               && !isJumping;

            PegaInput();

            horizontalMove = movimento.x;

            
            if (Controle.attackDown && !isAttacking && attackCooldown == 0)
            {
                StartCoroutine(GetAtaque());
            }

            AttackCooldown();

            m_Velocity = rb.velocity;

            if (rb.velocity.y >= 1)
            {
                isJumping = true;
                caindo = false;
            }
            else
            if (rb.velocity.y <= -0.1)
            {
                isJumping = false;
                caindo = true;
            }
            else
            {
                isJumping = false;
                caindo = false;
            }

            if (!isAttacking)
            {
                if (horizontalMove > 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
                else
            if (horizontalMove < 0)
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                }
            }
            Pular();
            Planar();
        }


        AnimaVisual();
    }


    private void FixedUpdate()
    {
        Movimento();
    }

    public void PegaInput()
    {
        if (Controle.left)
        {
            movimento.x = -1;
        }
        else
        if (Controle.right)
        {
            movimento.x = 1;
        }
        else
        {
            movimento.x = 0;
        }
    }

    public void Movimento()
    {
        if (!morte && !vitoria)
        {
            Vector3 targetVelocity = new Vector2(horizontalMove * runSpeed * Time.fixedDeltaTime, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, movementSmoothing);
        }
        else
        if (vitoria)
        {
            rb.MovePosition(rbSeguir.position);
            rb.freezeRotation = false;
            rb.MoveRotation(rbSeguir.rotation + 4);
        }
    }

    public void Pular()
    {
        if (isGrounded && Controle.jumpDown)
        {
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        
        if (Controle.jumpUp)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
            isJumping = false;
        }
    }

    public void Planar()
    {
        if (caindo && Controle.jump && !isGrounded)
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
            glideParticles.Stop();
        }
    }

    private void AttackCooldown()
    {
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
        if (attackCooldown < 0)
        {
            attackCooldown = 0;
        }
    }

    private IEnumerator GetGlide()
    {
        yield return new WaitUntil(() => isGliding == 1f && !isAttacking);
        glideParticles.Play();
        yield return new WaitUntil(() => isGliding == 0f || isAttacking);
        glideParticles.Stop();
        StartCoroutine(GetGlide());
    }

    private IEnumerator GetAtaque()
    {
        visualAnim.SetBool("WasGrounded", isGrounded);
        isAttacking = true;
        attackParticles.Play();

        if (visualAnim.GetBool("WasGrounded"))
        {
            attackCooldown = 0.3f;
            yield return new WaitForSeconds(0.2f);
        }
        else
        {
            attackCooldown = 0.2f;
            yield return new WaitForSeconds(0.25f);
        }

        isAttacking = false;
        attackParticles.Stop();
    }

    private IEnumerator GetNormal()
    {
        yield return new WaitUntil(()=> !isAttacking && isGliding==0f);
        //normalParticles.Play();
        yield return new WaitUntil(() => isAttacking || isGliding == 1f);
        //normalParticles.Stop();
        StartCoroutine(GetNormal());
    }

    private IEnumerator GetPoeiraGrounded()
    {
        yield return new WaitUntil(() => !isGrounded);
        yield return new WaitUntil(() => isGrounded);
        poeiraGroundedParticles.Play();
        yield return new WaitForSeconds(0.1f);
        poeiraGroundedParticles.Stop();
        StartCoroutine(GetPoeiraGrounded());
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Finish" && !vitoria)
        {
            SetRbSeguir(collision.GetComponent<Rigidbody2D>());
            vitoria = true;
            gameSystem.ChamaVitoria();
            StartCoroutine(DiminuiMuitoLio());
        }
        else if (collision.tag == "Respawn")
        {
            gameSystem.StartCoroutine(gameSystem.MudaCena(SceneManager.GetActiveScene().name));
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Inimigo" && vulneravel)
        {
            vulneravel = false;
            Enemy inimigo = collision.gameObject.GetComponent<Enemy>();
            gameSystem.LevarDano(this,inimigo.dano);
        }
    }

    protected void CoresOriginais()
    {
        int i = 0;

        foreach (SpriteRenderer spr in partesCorpoSpr)
        {
            coresOriginais[i] = spr.color;
            i++;
        }

        CoresOriginaisTranslucidas();
    }

    protected void CoresOriginaisTranslucidas()
    {
        int i = 0;

        foreach (SpriteRenderer spr in partesCorpoSpr)
        {
            Color corTranslucida = new Color(spr.color.r, spr.color.g, spr.color.b, 0.2f);
            coresOriginaisTranslucidas[i] = corTranslucida;
            i++;
        }
    }

    protected void SpriteBranca()
    {
        foreach (SpriteRenderer spr in partesCorpoSpr)
        {
            spr.material.shader = shaderGUItext;
            spr.color = Color.white;
        }
    }

    protected void SpriteNormal()
    {
        int i = 0;
        foreach (SpriteRenderer spr in partesCorpoSpr)
        {
            spr.material.shader = shaderSpritesDefault;
            spr.color = coresOriginais[i];
            i++;
        }
    }

    protected void SpriteTranslucida()
    {
        int i = 0;
        foreach (SpriteRenderer spr in partesCorpoSpr)
        {
            spr.material.shader = shaderSpritesDefault;
            spr.color = coresOriginaisTranslucidas[i];
            i++;
        }
    }

    public IEnumerator PiscaBranco()
    {
        //SpriteBranca();
        visualAnim.SetBool("Dano", true);
        damageParticles.Play();
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(0.05f);
        Physics2D.IgnoreLayerCollision(3, 8);
        damageParticles.Stop();
        Time.timeScale = 1;
        visualAnim.SetBool("Dano", false);
        for (int i = 0; i < 5; i++)
        {
            SpriteTranslucida();
            yield return new WaitForSeconds(0.1f);
            SpriteNormal();
            yield return new WaitForSeconds(0.1f);
        }
        Physics2D.IgnoreLayerCollision(3, 8, false);
        vulneravel = true;
    }

    public IEnumerator VitoriaDeFase()
    {
        estrelasUniao.transform.eulerAngles = new Vector3(0, 0, 0);
        estrelasUniao.gameObject.transform.SetParent(null);
        estrelasUniao.Iniciar();
        yield return new WaitForSeconds(1);
    }

    public void SetRbSeguir(Rigidbody2D rigidbody)
    {
        rbSeguir = rigidbody;
    }

    public IEnumerator DiminuiMuitoLio()
    {

        while (transform.localScale.x > 0)
        {
            transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localScale = new Vector3(0, 0, 0);
    }
}
