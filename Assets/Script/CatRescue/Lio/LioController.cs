using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LioController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CapsuleCollider2D colisor;

    [SerializeField] private float velocidade;
    [SerializeField] private float velocidadeBoost;
    [SerializeField] private float tempoDeBoost;
    [SerializeField] private float tempoDeConfusao;
    private Vector2 movimento;
    private bool vulneravel=true;
    private bool super;
    private bool preso;
    private bool confuso;
    private float confusoCont;
    private bool boost;
    private Vector2 boostDir;
    private float boostCont;
    private bool venceu;
    //private bool morreu = false;

    [SerializeField] private int superQuantidade;
    private bool superContando;
    [SerializeField] private float velocidadeDeQueda;

    [Header("Arma")]
    [SerializeField] private GameObject tiroObj;
    [SerializeField] private float velocidadeDoTiro;
    [SerializeField] private ParticleSystem tiroDisponivelParticulas;
    [SerializeField] private ParticleSystem tiroDisparadoParticulas;
    [SerializeField] private int tiroQuantidade;
    [SerializeField] private float tiroDelay;
    private float tiroContador;

    [SerializeField] private SpriteRenderer[] partesCorpoSpr;
    [SerializeField] private Material materialPiscaBranco;
    [SerializeField] private Material materialPixelSnap;
    private Material[] materialBase;

    private Color[] coresOriginais;
    private Color[] coresOriginaisTranslucidas;
    private Color[] coresPretas;
    private Color[] coresPretasTranslucidas;
    [Header("Variaveis de cor")]
    
    [SerializeField] private Color corPreta;
    [SerializeField] private ParticleSystem estrelasPretasParticulas;
    [SerializeField] private ParticleSystem danoParticulas;
    [SerializeField] private ParticleSystem boostParticulas;
    [SerializeField] private ParticleSystem fumacaParticulas;

    PlayerControl controle;
    [SerializeField] Joystick joystickTouch;

    [Header("Sons")]
    [SerializeField] private AudioClip danoSom;
    [SerializeField] private AudioClip[] tiroSom = new AudioClip[2];

    private void Awake()
    {
        controle = new PlayerControl();
    }

    private void Start()
    {
        tiroContador = tiroDelay;
        StartComponentes();
        StartCores();
        StartCoroutine(ControlaParticulasEstrelaPreta());
        StartCoroutine(ControlaParticulasTiroDisponivel());
        StartCoroutine(ControlaParticulasBoost());
        StartCoroutine(ControlaParticulasFumaca());
    }

    private void Update()
    {
        if (GameController.getInstance().comecar)
        {
            if (venceu)
            {
                StopAllCoroutines();
            }

            if (GameController.getInstance().mobile)
            {
                PegaInputTouch();
            }
            else
            {
                PegaInput();
            }

            if (!boost)
            {
                boostDir = movimento;
            }
            else
            {
                boostCont += Time.deltaTime;
                if (boostCont > tempoDeBoost)
                {
                    boost = false;
                    boostCont = 0;
                }
            }

            if (confuso)
            {
                confusoCont += Time.deltaTime;
                if (confusoCont > tempoDeConfusao)
                {
                    confuso = false;
                    confusoCont = 0;
                    GameController.getInstance().ConfusaTela(false);
                }
            }

            if (!preso)
            {
                Flip();
            }

            if (preso && !super)
            {
                preso = false;
                AjustarRotacao();
            }

        }

        SuperForma();
        AtualizaHud();
    }

    private void FixedUpdate()
    {
        if (super)
        {
            switch (preso)
            {
                case false:
                    if (!confuso)
                    {
                        switch (boost)
                        {
                            case false: rb.velocity = movimento.normalized * velocidade * Time.fixedDeltaTime; break;
                            case true: rb.velocity = boostDir.normalized * velocidadeBoost * Time.fixedDeltaTime; break;
                        }
                    }
                    else
                    {
                        rb.velocity = -movimento.normalized * velocidade * Time.fixedDeltaTime;
                        switch (boost)
                        {
                            case false: rb.velocity = -movimento.normalized * velocidade * Time.fixedDeltaTime; break;
                            case true: rb.velocity = -boostDir.normalized * velocidadeBoost * Time.fixedDeltaTime; break;
                        }
                    }
                    break;
                case true: 
                    rb.velocity = movimento.normalized * 0f * Time.fixedDeltaTime;
                    rb.MoveRotation(rb.rotation + 360 * Time.fixedDeltaTime);
                    break;
            }
        }
        else
        {
            rb.velocity = new Vector2(movimento.normalized.x * velocidade * Time.fixedDeltaTime, -velocidadeDeQueda);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!venceu)
        {
            if (collision.name == "EstrelaRosa")
            {
                tiroQuantidade++;
                TocarSom(collision.GetComponent<AudioSource>());
                Destroy(collision.gameObject);
            }
            else
            if (collision.name == "EstrelaPreta")
            {
                superQuantidade += 1;
                TocarSom(collision.GetComponent<AudioSource>());
                Destroy(collision.gameObject);
            }
            else
            if (collision.name == "EstrelaAzul")
            {
                superQuantidade += 2;
                TocarSom(collision.GetComponent<AudioSource>());
                Destroy(collision.gameObject);
            }
            else
            if (collision.tag == "Respawn")
            {
                GameController.getInstance().StartCoroutine(GameController.getInstance().GameOver(transform, SceneManager.GetActiveScene().name));
            }
            else
            if (collision.name == "BolhaAlien" && !preso && super && vulneravel)
            {
                StartCoroutine(Prender());
                GameController.getInstance().EmbolharTela(true);
            }
            else
            if (collision.CompareTag("Confusao") && !confuso && super && vulneravel)
            {
                confuso = true;
                GameController.getInstance().ConfusaTela(true);

                if (collision.name.Substring(0, 12) == "BichoFuracao")
                {
                    collision.GetComponent<Animator>().Play("FuracaoDestroi");
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if((collision.CompareTag("Inimigo") || collision.CompareTag("ArmaInimigo")) && vulneravel && super && !venceu)
        {
            TomaDano();
        }
    }

    private void TomaDano()
    {
        vulneravel = false;
        StartCoroutine(PiscaBranco());
        GameController.getInstance().DanoTela();
        superQuantidade -= 5;
        danoParticulas.Play();

        if (preso)
        {
            AjustarRotacao();
        }

        if (superQuantidade > 0)
        {
            anim.SetTrigger("Dano");
        }
    }

    private void SuperForma()
    {
        if (superQuantidade < 0)
        {
            superQuantidade = 0;
        }

        if (superQuantidade > 10)
        {
            superQuantidade = 10;
        }

        
        if (superQuantidade > 0 && !super)
        {
            super = true;
            SpritePreta();
            anim.SetBool("Super",true);
            rb.isKinematic = true;
        }
        
        if(superQuantidade==0 && super)
        {
            super = false;
            SpriteNormal();
            anim.SetBool("Super", false);
            anim.SetTrigger("Morre");
            rb.isKinematic = false;
            rb.gravityScale = 0f;
        }

        if (!superContando && superQuantidade > 0 && !venceu && GameController.getInstance().comecar)
        {
            superContando = true;
            StartCoroutine(SuperContador());
        }
    }

    private void Atirar()
    {
        tiroQuantidade--;
        //AudioManager.instance.CriaTocaEDestroi(tiroSom[Random.Range(0, 2)], 0.1f, 1, false);
        tiroDisparadoParticulas.Play();
        var tiro = Instantiate(tiroObj, new Vector3(transform.position.x, transform.position.y + Random.Range(-0.05f,0.09f)), Quaternion.identity);
        tiro.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        int direcao = (transform.eulerAngles.y == 0f) ? 1 : -1;
        anim.SetTrigger("Atira");
        tiro.GetComponent<Tiro>().SetInfo(velocidadeDoTiro, new Vector2(direcao, 0));
    }

    private void Flip()
    {
        if(movimento.x>0f)
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
        else
        if (movimento.x < 0f)
            transform.eulerAngles = new Vector3(0, 180, transform.eulerAngles.z);

    }

    private void PegaInput()
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

        if (Controle.up)
        {
            movimento.y = 1;
        }
        else
        if (Controle.down)
        {
            movimento.y = -1;
        }
        else
        {
            movimento.y = 0;
        }

        if(Controle.attack && super && !preso &&!boost)
        {
            tiroContador += Time.deltaTime;
            if (tiroContador >= tiroDelay)
            {
                Atirar();
                tiroContador = 0;
            }
        }

        if (Controle.attackUp)
        {
            tiroContador = tiroDelay;
        }

        if(Controle.jumpDown && !boost && movimento!=Vector2.zero && super)
        {
            boost = true;
            boostCont = 0;
        }
    }

    private void PegaInputTouch()
    {
        movimento.x = joystickTouch.Horizontal;
        movimento.y = joystickTouch.Vertical;
    }

    public void AtiraTouch()
    {
        if (super && !preso)
        {
            tiroContador += Time.deltaTime;
            if (tiroContador >= tiroDelay)
            {
                Atirar();
                tiroContador = 0;
            }
        }
    }

    public void ParaAtiraTouch()
    {
            tiroContador = tiroDelay;
    }

    public void BoostTouch()
    {
        if (!boost && movimento != Vector2.zero && super)
        {
            boost = true;
            boostCont = 0;
        }
    }

    private void AtualizaHud()
    {
        GameController.getInstance().AtualizaSuperContadorTxt(superQuantidade);
        GameController.getInstance().AtualizaTiroContadorTxt(tiroQuantidade);
    }

    private void StartComponentes()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        colisor = GetComponent<CapsuleCollider2D>();
        GameController.getInstance().lioController = this;
    }

    private void StartCores()
    {
        ResetDeCores();
    }

    private void ResetDeCores()
    {
        //partesCorpoSpr = GetComponentsInChildren<SpriteRenderer>();
        materialBase = new Material[partesCorpoSpr.Length];
        coresOriginais = new Color[partesCorpoSpr.Length];
        coresOriginaisTranslucidas = new Color[partesCorpoSpr.Length];
        coresPretas = new Color[partesCorpoSpr.Length];
        coresPretasTranslucidas = new Color[partesCorpoSpr.Length];
        CoresOriginais();
        CoresPretas();
    }

    private void CoresOriginais()
    {
        int i = 0;

        foreach (SpriteRenderer spr in partesCorpoSpr)
        {
            coresOriginais[i] = spr.color;
            materialBase[i] = spr.material;

            i++;
        }

        CoresOriginaisTranslucidas();
    }

    private void CoresOriginaisTranslucidas()
    {
        int i = 0;

        foreach (SpriteRenderer spr in partesCorpoSpr)
        {
            Color corTranslucida = new Color(spr.color.r, spr.color.g, spr.color.b, 0.2f);
            coresOriginaisTranslucidas[i] = corTranslucida;
            i++;
        }
    }

    private void CoresPretas()
    {
        int i = 0;
        
        foreach (SpriteRenderer spr in partesCorpoSpr)
        {
            coresPretas[i] = corPreta;

            if (spr.gameObject.name == "Olho")
            {
                coresPretas[i] = Color.white;
            }

            if (spr.gameObject.name == "GuardaChuva_0")
            {
                coresPretas[i] = Color.white;
            }
            i++;
        }
        CoresPretasTranslucidas();
    }

    private void CoresPretasTranslucidas()
    {
        int i = 0;

        foreach (SpriteRenderer spr in partesCorpoSpr)
        {
            Color corTranslucida = new Color(coresPretas[i].r, coresPretas[i].g, coresPretas[i].b, 0.2f);
            coresPretasTranslucidas[i] = corTranslucida;
            i++;
        }
    }


    private void SpritePreta()
    {
        int i = 0;
        
        foreach (SpriteRenderer spr in partesCorpoSpr)
        {
            spr.material = materialBase[i];
            spr.color = coresPretas[i];
            i++;
        }
    }

    private void SpritePretaTranslucida()
    {
        int i = 0;
        foreach (SpriteRenderer spr in partesCorpoSpr)
        {
            spr.material = materialBase[i];
            spr.color = coresPretasTranslucidas[i];
            i++;
        }
    }

    private void SpriteBranca()
    {
        foreach (SpriteRenderer spr in partesCorpoSpr)
        {
            spr.material = materialPiscaBranco;
            spr.color = Color.white;
        }
    }

    private void SpriteNormal()
    {
        int i = 0;
        
        foreach (SpriteRenderer spr in partesCorpoSpr)
        {
            spr.material = materialPixelSnap;
            spr.color = coresOriginais[i];
            i++;
        }
    }

    private void SpriteTranslucida()
    {
        int i = 0;
        foreach (SpriteRenderer spr in partesCorpoSpr)
        {
            spr.material = materialBase[i];
            spr.color = coresOriginaisTranslucidas[i];
            i++;
        }
    }

    private IEnumerator PiscaBranco()
    {
        //SpriteBranca();
        //ResetDeCores();
        AudioManager.instance.CriaTocaEDestroi(danoSom, 1, 1, false);

        for (int i = 0; i < 5; i++)
        {
            if (!super)
                SpriteTranslucida();
            else
                SpritePretaTranslucida();

            yield return new WaitForSeconds(0.1f);

            if (!super)
                SpriteNormal();
            else
                SpritePreta();

            yield return new WaitForSeconds(0.1f);
        }
        vulneravel = true;
    }

    private IEnumerator ControlaParticulasEstrelaPreta()
    {
        yield return new WaitUntil(() => !super);
        estrelasPretasParticulas.Stop();
        yield return new WaitUntil(() => super);
        estrelasPretasParticulas.Play();
        StartCoroutine(ControlaParticulasEstrelaPreta());
    }

    private IEnumerator ControlaParticulasTiroDisponivel()
    {
        yield return new WaitUntil(() => super);
        tiroDisponivelParticulas.Play();
        yield return new WaitUntil(() => !super);
        tiroDisponivelParticulas.Stop();
        StartCoroutine(ControlaParticulasTiroDisponivel());
    }

    private IEnumerator ControlaParticulasBoost()
    {
        yield return new WaitUntil(() => boost);
        boostParticulas.Play();
        yield return new WaitUntil(() => !boost);
        boostParticulas.Stop();
        StartCoroutine(ControlaParticulasBoost());
    }

    private IEnumerator ControlaParticulasFumaca()
    {
        yield return new WaitUntil(() => !super);
        fumacaParticulas.Play();
        GameController.getInstance().AtivaOuDesativaGameOver(true);
        yield return new WaitUntil(() => super);
        GameController.getInstance().AtivaOuDesativaGameOver(false);
        fumacaParticulas.Play();
        StartCoroutine(ControlaParticulasFumaca());
    }

    private IEnumerator SuperContador()
    {
        yield return new WaitForSeconds(1);
        while (superQuantidade > 0)
        {
            superQuantidade -= 1;
            yield return new WaitForSeconds(1);
        }
        superContando = false;
    }

    private IEnumerator Prender()
    {
        rb.freezeRotation = false;
        preso = true;
        yield return new WaitForSeconds(2);
        AjustarRotacao();
    }

    private void AjustarRotacao()
    {
        preso = false;
        GameController.getInstance().EmbolharTela(false);
        rb.freezeRotation = true;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
    }

    public bool GetSuper()
    {
        return super;
    }

    public bool GetVulneravel()
    {
        return vulneravel;
    }

    public bool GetPreso()
    {
        return preso;
    }

    public bool GetVenceu()
    {
        return venceu;
    }

    public void Venceu()
    {
        venceu = true;
    }

    private void TocarSom(AudioSource source)
    {
        //AudioManager.instance.TocaEDestroi(source);
    }
}
