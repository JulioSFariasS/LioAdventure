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
    private int vidaTotal;
    private int vidaEstagios=1;
    private bool enfureceu;
    private bool enfurecido;

    [Header("Movimento")]
    [SerializeField] private float velNormal;
    private Flutuantes flutuantes;

    [Header("Ataques")]
    [SerializeField] private bool jogandoBolhas;
    [SerializeField] private bool jogandoFogo;

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
        GameObject obj = null;
        switch (i)
        {
            case 1:
                obj = Instantiate(folha, maoDir.position, Quaternion.identity);
                obj.GetComponent<ObjMovel>().SetInfo(3, 360);
                break;
            case 2:
                obj = Instantiate(folha, maoEsq.position, Quaternion.identity);
                obj.GetComponent<ObjMovel>().SetInfo(3, 360);
                break;
        }
    }

    private IEnumerator AtiraMeteoro()
    {
        bichoPedra.GetComponent<Animator>().SetTrigger("AbreBoca");
        yield return new WaitForSeconds(0.5f);
        var obj = Instantiate(meteoro, new Vector3(bichoPedra.position.x+0.3f,bichoPedra.position.y+ 0.163f), Quaternion.identity);
        obj.GetComponent<ObjMovel>().SetInfo(0.9f, new Vector2(-1, 0), 90);
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

    private IEnumerator InvocaFogo()
    {
        jogandoFogo = true;
        anim.SetTrigger("Invoca");
        yield return new WaitForSeconds(1);
        Vector3 posAtual = transform.position;
        float somaOuSubtraiY = posAtual.y < 0 ? 1f : -1f;
        float somaOuSubtraiX = Random.Range(1f, 2f);
        int quant = !enfurecido ? 10 : 20;
        int i = 0;
        float forca = !enfurecido ? 0.3f : 0.4f;
        float velocidadeSeno = !enfurecido ? 3 : 4f;
        float velocidadeBase = !enfurecido ? 1 : 1.5f;

        while (i < quant)
        {
            var obj = Instantiate(fogo, new Vector3(posAtual.x + somaOuSubtraiX, posAtual.y), Quaternion.identity);
            int direcao = (transform.eulerAngles.y == 0) ? -1 : 1;

            //obj.GetComponent<Fogo>().SetInfo(new Vector2(direcao, 0), velocidadeBase, velocidadeSeno, forca);

            if (enfurecido)
            {
                var mini2 = Instantiate(fogo, new Vector3(posAtual.x + somaOuSubtraiX, posAtual.y + somaOuSubtraiY), Quaternion.identity);
                //mini2.GetComponent<Fogo>().SetInfo(new Vector2(direcao, 0), velocidadeBase, -velocidadeSeno, forca);
            }

            i++;
            yield return new WaitForSeconds(0.2f);
        }
        jogandoFogo = false;
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
        vidaEstagios = 3;
        anim.SetTrigger("Enfurece");
        acao = 0;
        yield return new WaitForSeconds(1);
        bichoPedra.gameObject.SetActive(true);
        acao = 1;
        yield return new WaitUntil(() => vida <= (int)(vidaTotal * 0.25f));
        vidaEstagios = 4;
        anim.SetTrigger("Enfurece");
        acao = 0;
        yield return new WaitForSeconds(1);
        SpawnaFogo();
        SpawnaFogo();
        SpawnaFogo();
        SpawnaFogo();
        acao = 1;
        yield return new WaitUntil(() => vida <= (int)(vidaTotal * 0));
        vidaEstagios = 5;
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
