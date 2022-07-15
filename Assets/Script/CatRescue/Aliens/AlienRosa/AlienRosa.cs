using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienRosa : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    [Header("Atributos")]
    [SerializeField]private int vida;
    private int vidaTotal;
    private bool morreu;

    [Header("Ataques")]
    [SerializeField] private float velocidadeAnel;
    [SerializeField] private float velocidadeEspalhador;
    [SerializeField] private bool ataqueLaser;

    [Header("Movimentação")]
    [SerializeField] private bool pararMovimento;

    private Flutuantes movHorizontal;
    private Flutuantes movVertical;

    [Header("Objetos para instanciar")]
    [SerializeField] private GameObject anel;
    [SerializeField] private GameObject tiroEspalhador;

    [Header("Objetos para controlar")]
    [SerializeField] private GameObject laser;

    [Header("Config de ataques")]
    [SerializeField] private float laserVelocidadeRotacao;
    [SerializeField] private float laserRotationModifier;

    [SerializeField] private GameObject player;

    private PiscarBranco piscaBranco;

    private int momento;
    private int opAnterior;

    private void Start()
    {
        vidaTotal = vida;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        piscaBranco = GetComponent<PiscarBranco>();
        movVertical = GetComponent<Flutuantes>();
        movHorizontal = movVertical.GetBase().GetComponent<Flutuantes>();
        StartCoroutine(Iniciar());
        pararMovimento = true;
        //player = GameController.getInstance().lioController.gameObject;
    }

    private void Update()
    {
        movVertical.stopContaTempo = pararMovimento;
        movHorizontal.stopContaTempo = pararMovimento;

        if (morreu)
        {
            pararMovimento = true;
            morreu = false;
            StopAllCoroutines();
            GameController.getInstance().StartCoroutine(GameController.getInstance().DerrotaChefe("Rosa", transform, "AlienRosa"));
            //anim.SetTrigger("Morre");
        }

        Flipar();
    }

    private void FixedUpdate()
    {
        if (ataqueLaser)
        {
            RotacionaBracoLaser();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ArmaLio")
        {
            vida--;
            piscaBranco.StartCoroutine(piscaBranco.PiscaBranco());
        }
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

    private void RotacionaBracoLaser()
    {
        if (player != null)
        {
            laser.transform.position = transform.position;
            Vector3 vectorToTarget = player.transform.position - laser.transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - laserRotationModifier;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            laser.transform.rotation = Quaternion.Slerp(laser.transform.rotation, q, Time.deltaTime * laserVelocidadeRotacao);
        }
    }

    private IEnumerator Iniciar()
    {
        yield return new WaitUntil(() => GameController.getInstance().comecar);
        pararMovimento = false;
        StartCoroutine(MudaEstagio());
        StartCoroutine(EscolheTiro());
    }

    private IEnumerator MudaEstagio()
    {
        momento = 0;
        yield return new WaitUntil(()=>GameController.getInstance().comecar);

        yield return new WaitUntil(() => vida <= (int)vidaTotal * 0.75f);
        momento = 1;
        GetComponent<Flutuantes>().velocidade = 1f;

        yield return new WaitUntil(() => vida <= (int)vidaTotal * 0.5f);
        momento = 2;
        GetComponent<Flutuantes>().velocidade = 1.5f;

        yield return new WaitUntil(() => vida <= (int)vidaTotal * 0.25f);
        momento = 3;
        GetComponent<Flutuantes>().velocidade = 2f;

        yield return new WaitUntil(() => vida <= 0);
        morreu = true;
    }

    private IEnumerator EscolheTiro()
    {
        while (!morreu)
        {

            int op = 0;

            Escolha:

            switch (momento)
            {
                case 0: op = 0; break;
                case 1: op = Random.Range(0, 2); break;
                case 2: op = Random.Range(0, 3); break;
                case 3: op = Random.Range(0, 3); break;
            }

            if (momento != 0)
            {
                if (op == opAnterior)
                {
                    goto Escolha;
                }
            }

            switch (op)
            {
                case 0:
                    yield return StartCoroutine(AtiraEspalhador());
                    break;

                case 1:
                    yield return StartCoroutine(AtiraAnel());
                    break;

                case 2:
                    yield return StartCoroutine(AtaqueLaser());
                    break;
            }

            opAnterior = op;

            if(momento!=3)
                yield return new WaitForSeconds(Random.Range(2f, 4f));
            else
                yield return new WaitForSeconds(Random.Range(1f, 2f));
        }
    }

    private IEnumerator AtiraEspalhador()
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(tiroEspalhador, transform.position, Quaternion.identity);

        if (momento == 3)
        {
            yield return new WaitForSeconds(0.5f);
            Instantiate(tiroEspalhador, transform.position, Quaternion.identity);
        }
    }

    private IEnumerator AtiraAnel()
    {
        yield return new WaitForSeconds(0.5f);
        var anel = Instantiate(this.anel, transform.position, Quaternion.identity);
        anel.GetComponent<ObjMovel>().SetInfo(Random.Range(0.5f,velocidadeAnel), new Vector2(-1, 1), 0);
        anel = Instantiate(this.anel, transform.position, Quaternion.identity);
        anel.GetComponent<ObjMovel>().SetInfo(Random.Range(0.5f, velocidadeAnel), new Vector2(1, 1), 0);

        if (momento == 3)
        {
            yield return new WaitForSeconds(1);
            anel = Instantiate(this.anel, transform.position, Quaternion.identity);
            anel.GetComponent<ObjMovel>().SetInfo(Random.Range(0.5f, velocidadeAnel), new Vector2(-1, 1), 0);
            anel = Instantiate(this.anel, transform.position, Quaternion.identity);
            anel.GetComponent<ObjMovel>().SetInfo(Random.Range(0.5f, velocidadeAnel), new Vector2(1, 1), 0);
        }
    }

    private IEnumerator AtaqueLaser()
    {
        ataqueLaser = true;
        pararMovimento = true;
        laser.GetComponent<Animator>().SetTrigger("Ativar");
        yield return new WaitForSeconds(0.7f);
        ataqueLaser = false;
        yield return new WaitForSeconds(1.3f);
        ataqueLaser = true;
        pararMovimento = false;
    }
}
