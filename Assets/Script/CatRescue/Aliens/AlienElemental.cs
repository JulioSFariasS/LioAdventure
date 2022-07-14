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

    [Header("Movimento")]
    [SerializeField] private float velNormal;
    private Flutuantes flutuantes;

    //Ações
    /* 0 - parado
     * 1 - movimento sem ataque
     */
    [SerializeField] private int acao;

    private PiscarBranco piscarBranco;

    [Header("Atributos de ataques")]
    [SerializeField] private float velocidadeDasNuvens;
    [SerializeField] private float intervaloDeSpawnNuvens;
    [SerializeField] private int quantidadeDeNuvens;
    [SerializeField] private Transform baseDeDirecoesDeFogo;
    [SerializeField] private float velocidadeDoFogo;
    [SerializeField] private int quantidadeDeRajadasDeFogo;
    [SerializeField] private Vector3 posicaoAtaqueFinal;

    [Header("Objetos para controlar")]
    [SerializeField] private GameObject nuvenzona;

    [Header("Objetos para instanciar")]
    [SerializeField] private GameObject bolha;
    [SerializeField] private GameObject fogo;
    [SerializeField] private GameObject folha;
    [SerializeField] private GameObject nuvenzinha;
    [SerializeField] private GameObject gota;

    [Header("Objetos base para instancias")]
    [SerializeField] private Transform maoEsq;
    [SerializeField] private Transform cabecaPos;
    [SerializeField] private Transform[] direcoesFogo; 

    [Header("Sons")]
    [SerializeField] private AudioClip atiraFogoSom;
    [SerializeField] private AudioClip invocaMiniSom;
    [SerializeField] private AudioClip soltaBolhaSom;

    private void Start()
    {
        vidaTotal = vida;
        StartComponentes();
        piscarBranco = GetComponent<PiscarBranco>();
        StartCoroutine(MudaEstagioVida());
    }

    private void Update()
    {
        if (GameController.getInstance().comecar)
        {
            if (morreu)
            {
                morreu = false;
                StopAllCoroutines();
                acao = 0;
                GameController.getInstance().StartCoroutine(GameController.getInstance().DerrotaChefe("Elemental", cabecaPos, "AlienEletrico"));
                anim.SetTrigger("Morre");
            }

            switch (acao)
            {
                case 0:
                    flutuantes.velocidade = 0;
                    flutuantes.stopContaTempo = true;
                    break;

                case 1:
                    flutuantes.enabled = true;
                    flutuantes.velocidade = velNormal;
                    flutuantes.stopContaTempo = false;
                    break;

                case 2:
                    flutuantes.enabled = false;
                    break;
            }
            Flipar();
        }
        
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
            if(Vector3.Distance(rb.position, posicaoAtaqueFinal) > 0.01f)
            {
                rb.MovePosition(Vector3.MoveTowards(rb.position, posicaoAtaqueFinal, velNormal * Time.deltaTime));
            }
            //rb.MovePosition(Vector3.SmoothDamp(rb.position, direcao.transform.position, ref m_Velocity, movementSmoothing, velocidade));
        }
    }

    private IEnumerator EscolhendoAcaoFolhas()
    {
        yield return new WaitForSeconds(3f);
        anim.SetInteger("Ataque", 0);
        anim.SetTrigger("Troca");
        yield return new WaitForSeconds(1.5f);
        acao = 1;
        do
        {

            anim.SetTrigger("Atacou");
            yield return new WaitForSeconds(1.7f);
            yield return new WaitForSeconds(Random.Range(1f, 3f));

        } while (vida >= (int)(vidaTotal * 0.66f));
        acao = 0;
        anim.SetTrigger("Parado");
        yield return new WaitForSeconds(0.7f);
    }

    private IEnumerator EscolhendoAcaoAguaVento()
    {
        acao = 0;
        yield return new WaitForSeconds(1f);
        anim.SetInteger("Ataque", 1);
        anim.SetTrigger("Troca");
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(DesceNuvenzona());
        StartCoroutine(Nuvenzinhas());
        acao = 1;
        do
        {
            anim.SetTrigger("Atacou");
            yield return new WaitForSeconds(1.2f);
            yield return new WaitForSeconds(Random.Range(3f, 5f));
        } while (vida >= (int)(vidaTotal * 0.33f));
        acao = 0;
        anim.SetTrigger("Parado");
        yield return new WaitForSeconds(0.7f);
    }

    private IEnumerator DesceNuvenzona()
    {
        yield return new WaitForSeconds(2);
        nuvenzona.SetActive(true);
        do
        {
            float xAleatorio = Random.Range(nuvenzona.GetComponent<SpriteRenderer>().bounds.min.x, nuvenzona.GetComponent<SpriteRenderer>().bounds.max.x);
            Vector2 pos = new Vector2(xAleatorio, nuvenzona.GetComponent<SpriteRenderer>().bounds.max.y);
            var obj = Instantiate(gota, pos, Quaternion.identity);
            obj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            obj.GetComponent<ObjMovel>().SetInfo(1, new Vector2(0, -1), 0);
            yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));

        } while (vida >= (int)(vidaTotal * 0.33f));
        nuvenzona.GetComponent<Animator>().SetTrigger("Sobe");
    }

    private IEnumerator Nuvenzinhas()
    {
        yield return new WaitForSeconds(Random.Range(2f, 4f));
        nuvenzona.SetActive(true);
        do
        {
            var obj = Instantiate(nuvenzinha, cabecaPos.position, Quaternion.identity);
            obj.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            obj.GetComponent<ObjMovel>().SetInfo(1, new Vector2(-1, 0), 0);
            yield return new WaitForSeconds(Random.Range(2f, 4f));

        } while (vida >= (int)(vidaTotal * 0.33f));
    }

    public void AtiraFolhasPeloAnim()
    {
        Instantiate(folha, maoEsq.position, Quaternion.identity);
    }

    public void CriaNuvensPeloAnim()
    {
        var obj = Instantiate(nuvenzinha, maoEsq.position, Quaternion.identity);
        obj.GetComponent<ObjMovel>().SetInfo(0.5f, new Vector2(0, 1), 0);
    }

    public void CriaBolhaPeloAnim()
    {
        Instantiate(bolha, maoEsq.position, Quaternion.identity);
        AudioManager.instance.CriaTocaEDestroi(soltaBolhaSom, 0.2f, 1, false);
    }

    private IEnumerator EscolhendoAcaoFogo()
    {
        StartCoroutine(ChecaVidaEstagioNoFinal());
        acao = 0;
        yield return new WaitForSeconds(1f);
        anim.SetInteger("Ataque", 2);
        anim.SetTrigger("Troca");
        yield return new WaitForSeconds(1.5f);
        acao = 1;
        do
        {
            anim.SetTrigger("Atacou");
            yield return new WaitForSeconds(1.5f);
            yield return StartCoroutine(SpawnaFogo());
            anim.SetTrigger("Parado");
            yield return new WaitForSeconds(1f);

        } while (vida >= 0);
        acao = 0;
        anim.SetTrigger("Parado");
        yield return new WaitForSeconds(0.7f);
    }

    private IEnumerator SpawnaFogo()
    {
        for (int i = 0; i < quantidadeDeRajadasDeFogo; i++)
        {
            foreach (Transform dir in direcoesFogo)
            {
                var obj = Instantiate(fogo, baseDeDirecoesDeFogo.position, Quaternion.identity);
                obj.GetComponent<ObjMovel>().SetInfo(velocidadeDoFogo, (dir.position - obj.transform.position).normalized , 0);
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator MudaEstagioVida()
    {
        yield return new WaitUntil(() => GameController.getInstance().comecar);
        yield return StartCoroutine(EscolhendoAcaoFolhas());
        yield return StartCoroutine(EscolhendoAcaoAguaVento());
        yield return StartCoroutine(EscolhendoAcaoFogo());
    }

    private IEnumerator ChecaVidaEstagioNoFinal()
    {
        yield return new WaitUntil(() => vida <= 0);
        anim.SetTrigger("Morre");
        morreu = true;
    }

    protected void Flipar()
    {
        if (acao != 2)
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ArmaLio")
        {
            vida--;
            piscarBranco.StartCoroutine(piscarBranco.PiscaBranco());
        }
    }

    private void StartComponentes()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        flutuantes = GetComponent<Flutuantes>();
    }

    public void PlaySom(string nome)
    {
        switch (nome)
        {
            case "fogo": AudioManager.instance.CriaTocaEDestroi(atiraFogoSom, 0.8f, 1, false); break;
            case "invocaMini": AudioManager.instance.CriaTocaEDestroi(invocaMiniSom, 0.8f, 1, false); break;
        }
    }
}
