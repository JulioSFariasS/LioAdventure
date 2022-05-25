using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float vida;
    public int dano;
    public float velocidade=30;
    public bool morreu;
    protected bool iniciado;

    public bool isGrounded;
    public LayerMask whatIsGround;
    public float checkGroundRadius;
    public Transform posPe;
    public Transform verificadorDeBorda, verificadorDeBorda2;
    public float checkBordaRadius;
    public bool naBorda;
    public Transform verificadorDeParede, verificadorDeParede2;
    public Transform verificadorDeOutroInimigo;
    public LayerMask inimigoLayer;
    public bool naParede;
    public bool noInimigo;

    public Transform player;
    protected LioControl playerCtrl;
    public Rigidbody2D rb;
    public Animator anim;
    public SpriteRenderer corpo;
    protected SpriteRenderer[] partesCorpoSpr;
    protected Shader shaderGUItext;
    protected Shader shaderSpritesDefault;
    protected Color[] coresOriginais;
    public Collider2D colisor;

    public float movimentoHorizontal;
    [Range(0, .3f)] [SerializeField] protected float movementSmoothing = .05f;
    protected Vector3 m_Velocity = Vector3.zero;

    public float forcaDeRecuo;
    protected bool vulneravel = true;
    public BuracoNegro buracoNegro;

    protected GameSystem gameSystem;

    protected void Start()
    {
        gameSystem = GameSystem.getInstance();
        StartCoroutine(Iniciador());
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCtrl = player.GetComponent<LioControl>();
        partesCorpoSpr = GetComponentsInChildren<SpriteRenderer>();
        coresOriginais = new Color[partesCorpoSpr.Length];
        CoresOriginais();
        buracoNegro = transform.GetChild(0).GetComponent<BuracoNegro>();
        buracoNegro.gameObject.SetActive(false);
        //Pisca branco
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");
    }

    protected void Update()
    {
        isGrounded = Physics2D.OverlapCircle(posPe.position, checkGroundRadius, whatIsGround);
        naBorda = !Physics2D.OverlapCircle(verificadorDeBorda.position, checkBordaRadius, whatIsGround) && !Physics2D.OverlapCircle(verificadorDeBorda2.position, checkBordaRadius, whatIsGround);
        naParede = Physics2D.OverlapCircle(verificadorDeParede.position, checkBordaRadius, whatIsGround) && Physics2D.OverlapCircle(verificadorDeParede2.position, checkBordaRadius, whatIsGround);
        noInimigo = Physics2D.OverlapCircle(verificadorDeOutroInimigo.position, 0.05f, inimigoLayer);

        if (vida <= 0 && !morreu)
        {
            morreu = true;
            StartCoroutine(Morte());
        }

        if (!morreu)
        {
            if ((naBorda && isGrounded) || naParede || noInimigo)
            {
                movimentoHorizontal *= -1;
            }

            Flipar();
        }
    }

    protected void FixedUpdate()
    {
        if (iniciado)
        {
            MovimentoGeral();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Respawn")
        {
            StartCoroutine(Morte());
        }

        if (collision.tag == "ArmaLio" && vulneravel)
        {
            vulneravel = false;
            StartCoroutine(PiscaBranco());
            vida--;

            /*
            if(vida>0)
            {
                if (player.transform.position.x < rb.position.x)
                {
                    rb.velocity = new Vector2(forcaDeRecuo, forcaDeRecuo);
                }
                else
                {
                    rb.velocity = new Vector2(-forcaDeRecuo, forcaDeRecuo);
                }
            }
            */
        }
    }


    protected IEnumerator Morte()
    {
        rb.isKinematic = true;
        velocidade = 0;
        rb.velocity = new Vector2(0, 0);
        colisor.enabled = false;
        anim.SetBool("Morreu", morreu);
        buracoNegro.gameObject.SetActive(true);
        buracoNegro.StartaBuraco();
        buracoNegro.gameObject.transform.SetParent(null);
        yield return new WaitForSeconds(0.75f);
        gameSystem.QuantidadeDeAlienDiminui();
        Destroy(gameObject);
    }

    protected void Flipar()
    {
        if (movimentoHorizontal < 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        if (movimentoHorizontal > 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    protected void MovimentoGeral()
    {
        Vector3 targetVelocity = new Vector2(movimentoHorizontal * velocidade * Time.fixedDeltaTime, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, movementSmoothing);
    }

    protected IEnumerator Iniciador()
    {
        yield return new WaitUntil(() => corpo.isVisible);
        iniciado = true;
    }

    protected void CoresOriginais()
    {
        int i = 0;

        foreach(SpriteRenderer spr in partesCorpoSpr)
        {
            coresOriginais[i] = spr.color;
            i++;
        }
    }

    protected void SpriteBranca()
    {
        foreach(SpriteRenderer spr in partesCorpoSpr)
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

    protected IEnumerator PiscaBranco()
    {
        SpriteBranca();
        yield return new WaitForSeconds(0.1f);
        vulneravel = true;
        SpriteNormal();
    }
}
