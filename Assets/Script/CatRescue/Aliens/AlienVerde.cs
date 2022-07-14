using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienVerde : MonoBehaviour
{
    //Componentes
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Status")]
    [SerializeField] private int vida;
    private int vidaMetade;
    private bool enfureceu;
    private bool enfurecido;

    [Header("Movimentação")]
    [SerializeField] private Transform pontoAlto;
    [SerializeField] private Transform pontoBaixo;
    [SerializeField] private float velPontos;
    [SerializeField] private float velNormal;
    [SerializeField] private float velAvanco;
    private float multiplicadorVelocidade=1;
    //[Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
    //private Vector3 m_Velocity = Vector3.zero;
    private float velocidade;
    private Transform direcao;
    private float contaTempo;

    //movimento flutuante
    [SerializeField]private float floatStrength;
    [SerializeField]private Transform posBase;
    private Vector2 position;

    //Ações
    /* 0 - parado
     * 1 - movimento sem ataque
     */
    [SerializeField] private int acao;
    private int ataqueAnterior;

    private PiscarBranco piscarBranco;

    [Header("Objetos para instanciar")]
    [SerializeField] private GameObject estrelaPreta;
    [SerializeField] private GameObject bolha;
    [SerializeField] private GameObject miniAlien;

    [Header("Objetos base para instancias")]
    [SerializeField] private Transform bolhaSpawn;

    [Header("Objetos para controlar")]
    [SerializeField] private GameObject luzes;
    [SerializeField] private GameObject bracoLaser;

    [Header("Sons")]
    [SerializeField] private AudioClip laserSom;
    [SerializeField] private AudioClip alertaSom;
    [SerializeField] private AudioClip invocaMiniSom;
    [SerializeField] private AudioClip soltaBolhaSom;


    private void Start()
    {
        vidaMetade = vida / 2;
        StartComponentes();
        LiberaPontos();
        piscarBranco = GetComponent<PiscarBranco>();
        direcao = pontoAlto;
        StartCoroutine(EscolhendoAcao());
        StartCoroutine(Enfurecer());
        ControlaLuzes("amarelo", 1);
    }

    private void Update()
    {
        switch (acao)
        {
            case 0: velocidade = 0; break;

            case 1:
                velocidade = velNormal;
                contaTempo += Time.deltaTime;
                MovimentoEmSeno();
                break;

            case 2:
                RotacionaBracoLaser();
                break;
        }
        Flipar();
    }

    private void FixedUpdate()
    {
        if (acao == 0)
        {
            //rb.velocity = Vector2.zero;
        }
        else
        if (acao == 1)
        {
            
            rb.MovePosition(position);

            
            //rb.MovePosition(Vector3.SmoothDamp(rb.position, direcao.transform.position, ref m_Velocity, movementSmoothing, velocidade));
        }
        else
        if (acao == 2)
        {
            //RotacionaBracoLaser();
            //rb.MovePosition(Vector3.SmoothDamp(rb.position, direcao.transform.position, ref m_Velocity, movementSmoothing, velocidade));
        }
    }

    private IEnumerator EscolhendoAcao()
    {
        if(!enfureceu && enfurecido)
        {
            enfureceu = true;
            anim.Play("Enfurece");
            ControlaLuzes("vermelho", 5);
            acao = 0;
            yield return new WaitForSeconds(2.6f);
        }

        anim.Play("Parado");
        acao = 1;        

        if (!enfurecido)
            yield return new WaitForSeconds(Random.Range(1,7));
        else
            yield return new WaitForSeconds(Random.Range(1, 3));

        int ataque = 0;

        do
        {
            ataque = Random.Range(2, 5);
        }
        while (ataque == ataqueAnterior);
        
        ataqueAnterior = ataque;

        switch (ataque)
        {
            case 2:
                yield return StartCoroutine(AtiraLaser());
                break;
            case 3:
                yield return StartCoroutine(AtiraBolhas());
                break;
            case 4:
                yield return StartCoroutine(InvocaMiniAlien());
                break;
        }

        StartCoroutine(EscolhendoAcao());
    }

    private IEnumerator AtiraLaser()
    {
        anim.Play("AchaArmaLaser");
        yield return new WaitForSeconds(0.5f);
        acao = 2;
        anim.Play("PreparaTiroLaser");
        if (!enfurecido)
        {
            yield return new WaitForSeconds(1.16f);
            acao = 0;
            yield return new WaitForSeconds(0.66f);
        }
        else
        {
            yield return new WaitForSeconds(1.16f);
            acao = 0;
            yield return new WaitForSeconds(0.7f);
            acao = 2;
            yield return new WaitForSeconds(0.13f);
            acao = 0;
            yield return new WaitForSeconds(0.7f);
            acao = 2;
            yield return new WaitForSeconds(0.13f);
            acao = 0;
            
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator AtiraBolhas()
    {
        anim.Play("AchaArmaBolha");
        yield return new WaitForSeconds(0.5f);
        acao = 1;
        anim.Play("PreparaBolhas");
        yield return new WaitForSeconds(0.5f);

        for(int i = 0; i<10; i++)
        {
            var bolhaObj = Instantiate(bolha, bolhaSpawn.position, Quaternion.identity);
            //bolhaObj.GetComponent<Bolha>().SetInfo(enfurecido);
            AudioManager.instance.CriaTocaEDestroi(soltaBolhaSom, 0.2f, 1, false);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator InvocaMiniAlien()
    {
        anim.Play("InvocaMini");
        yield return new WaitForSeconds(1);
        anim.Play("Parado");
        Vector3 posAtual = transform.position;
        float somaOuSubtraiY = posAtual.y < 0 ? 1f : -1f;
        float somaOuSubtraiX = Random.Range(1f, 2f);
        int quant = !enfurecido? 10:20;
        int i = 0;
        float forca = !enfurecido? 0.3f : 0.4f;
        float velocidadeSeno = !enfurecido? 3 : 4f;
        float velocidadeBase = !enfurecido? 1 : 1.5f;

        while (i < quant)
        {
            var mini = Instantiate(miniAlien, new Vector3(posAtual.x + somaOuSubtraiX, posAtual.y), Quaternion.identity);
            int direcao = (transform.eulerAngles.y == 0) ? -1 : 1;

            mini.GetComponent<MiniAlien>().SetInfo(new Vector2(direcao, 0), velocidadeBase, velocidadeSeno, forca);

            if (enfurecido)
            {
                var mini2 = Instantiate(miniAlien, new Vector3(posAtual.x + somaOuSubtraiX, posAtual.y+ somaOuSubtraiY), Quaternion.identity);
                mini2.GetComponent<MiniAlien>().SetInfo(new Vector2(direcao, 0), velocidadeBase, -velocidadeSeno, forca);
            }

            i++;
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator Enfurecer()
    {
        yield return new WaitUntil(() => vida <= vidaMetade);
        enfurecido = true;
    }

    private void RotacionaBracoLaser()
    {
        Vector3 dir = GameController.getInstance().lioController.transform.position - bracoLaser.transform.position;
        Quaternion lookRot = Quaternion.LookRotation(dir);
        lookRot.x = 0; lookRot.y = 0;
        bracoLaser.transform.rotation = Quaternion.Slerp(bracoLaser.transform.rotation, lookRot, Mathf.Clamp01(3.0f * Time.maximumDeltaTime));
    }

    private void MovimentoEmSeno()
    {
        float newY = Mathf.Sin(contaTempo * velocidade * multiplicadorVelocidade) * floatStrength;
        position = new Vector2(0, newY) + new Vector2(posBase.position.x, posBase.position.y);
    }

    protected void Flipar()
    {
        if (GameController.getInstance().lioController.transform.position.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        if (GameController.getInstance().lioController.transform.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name=="PontoAlto" || collision.gameObject.name == "PontoBaixo")
        {
            FlipDirecao();
        }

        if (collision.tag == "ArmaLio")
        {
            vida--;
            //var estrela = Instantiate(estrelaPreta, new Vector3(transform.position.x, transform.position.y-0.08f), Quaternion.identity);
            //estrela.transform.localScale = new Vector3(0.5f, 0.5f);

            //estrela.GetComponent<Estrela>().movimento.x = -1;
            //estrela.GetComponent<Estrela>().ESeguidora();
            piscarBranco.StartCoroutine(piscarBranco.PiscaBranco());
        }
    }

    private void FlipDirecao()
    {
        if (direcao == pontoAlto)
        {
            direcao = pontoBaixo;
        }
        else
        {
            direcao = pontoAlto;
        }
    }

    private void StartComponentes()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void LiberaPontos()
    {
        posBase.parent = null;
        pontoAlto.GetComponent<SpriteRenderer>().enabled = false;
        pontoBaixo.GetComponent<SpriteRenderer>().enabled = false;
        pontoAlto.parent = null;
        pontoBaixo.parent = null;
    }



    public void ControlaLuzes(string cor, float velocidade)
    {
        switch (cor)
        {
            case "amarelo": luzes.GetComponent<SpriteRenderer>().material.SetColor("_Cor", new Color(253.0143f, 241.0921f, 0, 0)); break;
            case "vermelho": luzes.GetComponent<SpriteRenderer>().material.SetColor("_Cor", new Color(337.7939f, 0, 17.05116f, 0)); break;
        }

        luzes.GetComponent<Animator>().SetFloat("velocidade",velocidade);
    }

    public void PlaySom(string nome)
    {
        switch (nome)
        {
            case "alerta": AudioManager.instance.CriaTocaEDestroi(alertaSom, 0.6f, 1, false); break;
            case "laser": AudioManager.instance.CriaTocaEDestroi(laserSom, 0.8f, 1, false); break;
            case "invocaMini": AudioManager.instance.CriaTocaEDestroi(invocaMiniSom, 0.8f, 1, false); break;
        }
    }

    public bool GetEnfurecido()
    {
        return enfurecido;
    }
}
