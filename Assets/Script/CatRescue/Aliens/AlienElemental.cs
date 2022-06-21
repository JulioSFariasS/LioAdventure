using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienElemental : MonoBehaviour
{   
    //Componentes
    private Rigidbody2D rb;
    private Animator anim;
    

    [Header("Status")]
    [SerializeField] private int vida;
    private bool morreu;
    private int vidaTotal;
    private int vidaEstagios=1;
    private bool enfureceu;
    private bool enfurecido;

    [Header("Movimento")]
    [SerializeField] private float velNormal;
    private Flutuantes flutuantes;

    //Ações
    /* 0 - parado
     * 1 - movimento sem ataque
     */
    [SerializeField] private int acao;
    private int ataqueAnterior;

    private PiscarBranco piscarBranco;

    [Header("Objetos para instanciar")]
    [SerializeField] private GameObject bolha;
    [SerializeField] private GameObject fogo;
    [SerializeField] private GameObject meteoro;
    [SerializeField] private GameObject folha;

    [Header("Objetos base para instancias")]
    [SerializeField] private Transform bolhaSpawn;
    [SerializeField] private Transform bolhaSpawnBase;
    [SerializeField] private Transform bichoPedra;
    [SerializeField] private Transform maoDir;
    [SerializeField] private Transform maoEsq;

    [Header("Sons")]
    [SerializeField] private AudioClip laserSom;
    [SerializeField] private AudioClip alertaSom;
    [SerializeField] private AudioClip invocaMiniSom;
    [SerializeField] private AudioClip soltaBolhaSom;

    [Header("Particulas")]
    [SerializeField] private ParticleSystem danificadoUmParticulas;
    [SerializeField] private ParticleSystem danificadoDoisParticulas;
    [SerializeField] private ParticleSystem danificadoTresParticulas;


    private void Start()
    {
        vidaTotal = vida;
        StartComponentes();
        piscarBranco = GetComponent<PiscarBranco>();
        StartCoroutine(MudaEstagioVida());
        StartCoroutine(Enfurecer());
        StartCoroutine(EscolhendoAcao());
    }

    private void Update()
    {
        if (morreu)
        {
            morreu = false;
            StopAllCoroutines();
            acao = 0;
            GameController.getInstance().StartCoroutine(GameController.getInstance().DerrotaChefe("Elemental", gameObject.transform, "AlienVerde"));
            anim.SetTrigger("Morre");
        }

        switch (acao)
        {
            case 0: flutuantes.velocidade = 0;
                flutuantes.stopContaTempo = true;
                break;

            case 1:
                flutuantes.velocidade = velNormal;
                flutuantes.stopContaTempo = false;
                break;

            case 2:
                break;
        }
        Flipar();
        AtualizaHud();
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
        //anim.Play("Parado");
        acao = 1;

        if (!enfurecido)
            yield return new WaitForSeconds(Random.Range(1, 7));
        else
            yield return new WaitForSeconds(Random.Range(1, 3));

        int ataque = 5;

        Escolha:
        do
        {
            ataque = Random.Range(2, 4);
        }
        while (ataque == ataqueAnterior);
        
        ataqueAnterior = ataque;

        switch (ataque)
        {
            case 2:
                yield return StartCoroutine(AtiraFolhas());
                break;
            case 3:
                if(vidaEstagios>2)
                    yield return StartCoroutine(AtiraMeteoro());
                else
                    goto Escolha;
                break;
        }

        StartCoroutine(EscolhendoAcao());
    }

    private IEnumerator AtiraFolhas()
    {
        anim.SetTrigger("Procura");
        yield return new WaitForSeconds(2.5f);
    }

    public void AtiraFolhasPeloAnim(int i)
    {
        switch (i)
        {
            case 1:
                Instantiate(folha, maoDir.position, Quaternion.identity);
                break;
            case 2:
                Instantiate(folha, maoEsq.position, Quaternion.identity);
                break;
        }
    }

    private IEnumerator AtiraMeteoro()
    {
        //bichoPedra.GetComponent<Animator>().SetTrigger("AbreBoca");
        yield return new WaitForSeconds(0.5f);
        var obj = Instantiate(meteoro, new Vector3(bichoPedra.position.x,bichoPedra.position.y- 0.163f), Quaternion.identity);
        obj.GetComponent<ObjMovel>().SetInfo(0.9f, new Vector2(-1, 0), 0);
    }

    private IEnumerator AtiraBolhas()
    {
        bolhaSpawn.GetChild(0).GetChild(0).GetComponent<Animator>().SetBool("Soprando", true);
        yield return new WaitForSeconds(0.6f);
        for (int i = 0; i < 5; i++)
        {
            var bolhaObj = Instantiate(bolha, bolhaSpawn.position, Quaternion.identity);
            bolhaObj.GetComponent<Bolha>().SetInfo(false);
            AudioManager.instance.CriaTocaEDestroi(soltaBolhaSom, 0.2f, 1, false);
            yield return new WaitForSeconds(0.2f);
        }
        bolhaSpawn.GetChild(0).GetChild(0).GetComponent<Animator>().SetBool("Soprando", false);
        yield return new WaitForSeconds(Random.Range(1f, 6f));
        StartCoroutine(AtiraBolhas());
    }

    private void SpawnaFogo()
    {
        Instantiate(fogo, transform.position, Quaternion.identity);
    }

    private IEnumerator Enfurecer()
    {
        yield return new WaitUntil(() => vidaEstagios==3);
        enfurecido = true;
    }

    private IEnumerator MudaEstagioVida()
    {
        yield return new WaitUntil(() => vida <= (int)(vidaTotal * 0.75f));
        danificadoUmParticulas.Play();
        vidaEstagios = 2;
        anim.SetTrigger("Enfurece");
        acao = 0;
        yield return new WaitForSeconds(1.3f);
        anim.SetTrigger("SoltaBichoBolha");
        yield return new WaitForSeconds(1.5f);
        bolhaSpawn = Instantiate(bolhaSpawn, bolhaSpawnBase.position, Quaternion.identity);
        bolhaSpawn.GetComponent<BolhaSpawnerScript>().liberar = true;
        acao = 1;
        yield return new WaitForSeconds(2f);
        StartCoroutine(AtiraBolhas());


        yield return new WaitUntil(() => vida <= (int)(vidaTotal * 0.5f));
        danificadoDoisParticulas.Play();
        vidaEstagios = 3;
        anim.SetTrigger("Enfurece");
        acao = 0;
        yield return new WaitForSeconds(1);
        bichoPedra.gameObject.SetActive(true);
        acao = 1;


        yield return new WaitUntil(() => vida <= (int)(vidaTotal * 0.25f));
        danificadoTresParticulas.Play();
        vidaEstagios = 4;
        anim.SetTrigger("Enfurece");
        acao = 0;
        yield return new WaitForSeconds(1);
        SpawnaFogo();
        SpawnaFogo();
        SpawnaFogo();
        SpawnaFogo();
        acao = 1;


        yield return new WaitUntil(() => vida <= 0);
        morreu = true;
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

    private void StartComponentes()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        flutuantes = GetComponent<Flutuantes>();
    }

    private void AtualizaHud()
    {
        GameController.getInstance().alienVidaSlider.value = vida;
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
