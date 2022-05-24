using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDiscoVoadorSimples : Enemy
{
    public float floatStrength;
    private Vector2 posIni;
    private Vector2 position;
    public GameObject luzinha;
    public float tempoDeSpawnLuzinha;
    public List<Color> corDaLuzinha = new List<Color>();
    private Color cor;
    public float frequenciaDeMudancaDeCorLuzinha;
    public GameObject nave;
       

    new void Start()
    {
        posIni = rb.position;
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

    new void Update()
    {
        if (vida <= 0 && !morreu)
        {
            morreu = true;
            StartCoroutine(Morte());
        }

        if (!morreu)
        {
            Flipar();
        }
    }

    new void FixedUpdate()
    {
        if (iniciado)
        {
            if (!morreu)
            {
                float newY = Mathf.Sin(Time.time * velocidade) * floatStrength;

                position = new Vector2(0, newY) + posIni;
                rb.MovePosition(position);
            }
        }
    }

    new protected void Flipar()
    {
        if (player.transform.position.x < rb.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    new protected IEnumerator Morte()
    {
        rb.isKinematic = true;
        velocidade = 0;
        rb.velocity = new Vector2(0, 0);
        colisor.enabled = false;
        anim.SetTrigger("Morreu");
        buracoNegro.gameObject.SetActive(true);
        buracoNegro.StartaBuraco();
        buracoNegro.gameObject.transform.SetParent(null);
        yield return new WaitForSeconds(0.75f);
        gameSystem.QuantidadeDeAlienDiminui();
        Destroy(gameObject);
    }

    new private IEnumerator Iniciador()
    {
        yield return new WaitUntil(() => nave.GetComponent<SpriteRenderer>().isVisible);
        iniciado = true;
        StartCoroutine(SpawnLuzinha());
        StartCoroutine(MudaCorLuzinha());
    }

    private IEnumerator SpawnLuzinha()
    {
        while (!morreu)
        {
            var obj = Instantiate(luzinha, nave.transform, false);
            obj.transform.SetParent(nave.transform);
            obj.GetComponent<SpriteRenderer>().color = cor;
            yield return new WaitForSeconds(tempoDeSpawnLuzinha);
        }
    }

    public IEnumerator MudaCorLuzinha()
    {
        while (!morreu)
        {
            //float[] cores = AleatorizaCor();
            //corDaLuzinha = new Color(cores[0], cores[1], cores[2], 1);
            cor = AleatorizaCor();
            yield return new WaitForSeconds(frequenciaDeMudancaDeCorLuzinha);
        }
    }

    private Color AleatorizaCor()
    {
        return corDaLuzinha[Random.Range(0, 5)];
        /*
        float[] cores = new float[3];

        float a = 0, b = 0, c = 0;

        int num = Random.Range(0, 3);
        int pNum = num, sNum, tNum;
        switch (num)
        {
            case 0: a = Random.Range(0f, 1f); break;
            case 1: a = 1; break;
            case 2: a = 0; break;
        }

        do
        {
            num = Random.Range(0, 3);
            sNum = num;
            switch (num)
            {
                case 0: b = Random.Range(0f, 1f); break;
                case 1: b = 1; break;
                case 2: b = 0; break;
            }
        } while (sNum==pNum);

        do
        {
            num = Random.Range(0, 3);
            tNum = num;
            switch (num)
            {
                case 0: c = Random.Range(0f, 1f); break;
                case 1: c = 1; break;
                case 2: c = 0; break;
            }
        } while (tNum==pNum || tNum == sNum);

        cores[0] = a;
        cores[1] = b;
        cores[2] = c;

        return cores;
        */
    }
}
