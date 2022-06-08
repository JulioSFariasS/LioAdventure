using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyForte : Enemy
{
    public float frequenciaDeAtaque;
    public bool frente, cima;
    private bool atacando;

    new private void Start()
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
        Physics2D.IgnoreLayerCollision(6, 9);
        Physics2D.IgnoreLayerCollision(8, 9);
    }

    new private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(posPe.position, checkGroundRadius, whatIsGround);

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

    new private void FixedUpdate()
    {
    }

    new protected void Flipar()
    {
        if (!atacando)
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
    }

    private IEnumerator Ataca()
    {
        while (!morreu)
        {
            if (cima)
            {
                anim.SetTrigger("AtaqueCima");
            }
            else
            if(frente)
            {
                anim.SetTrigger("AtaqueFrente");
            }
            StartCoroutine(Atacando());
            yield return new WaitForSeconds(frequenciaDeAtaque);
        }
    }

    private IEnumerator Atacando()
    {
        atacando = true;
        yield return new WaitForSeconds(1.1f);
        atacando = false;
    }

    new private IEnumerator Iniciador()
    {
        yield return new WaitUntil(() => corpo.isVisible);
        iniciado = true;
        yield return new WaitForSeconds(frequenciaDeAtaque);
        StartCoroutine(Ataca());
    }

}
