using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienEletrico : MonoBehaviour
{
    private Animator anim;
    [Header("Status")]
    [SerializeField] private int vida;
    private int vidaTotal;
    private bool morreu;

    private MovimentoEntrePontos movimento;

    [Header("Objetos para controlar")]
    [SerializeField] private Animator[] caixaEletrica;
    [SerializeField] private Animator[] caboEletrico;

    private PiscarBranco piscarBranco;
    
    void Start()
    {
        vidaTotal = vida;
        movimento = GetComponent<MovimentoEntrePontos>();
        anim = GetComponent<Animator>();
        piscarBranco = GetComponent<PiscarBranco>();
        StartCoroutine(MudaEstagioVida());
    }

    private void Update()
    {
        if (morreu)
        {
            morreu = false;
            StopAllCoroutines();
            movimento.enabled = false;
            GameController.getInstance().StartCoroutine(GameController.getInstance().DerrotaChefe("Eletrico", transform, "AlienEletrico"));
            //anim.SetTrigger("Morre");
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

    private IEnumerator AtaqueRaio()
    {
        yield return new WaitUntil(() => GameController.getInstance().comecar);

        anim.SetInteger("Ataque", 0);
        anim.SetTrigger("Ataca");

        yield return new WaitForSeconds(Random.Range(5f, 7f));
        StartCoroutine(AtaqueRaio());
    }

    private IEnumerator AtaqueCaixa()
    {
        yield return new WaitUntil(() => GameController.getInstance().comecar);

        int padrao = Random.Range(0, 4);

        switch (padrao)
        {
            case 0:
                caixaEletrica[Random.Range(0, 6)].SetTrigger("Ativar");
                break;

            case 1:
                foreach(Animator cx in caixaEletrica)
                {
                    cx.SetTrigger("Ativar");
                    yield return new WaitForSeconds(1.5f);
                }
                break;

            case 2:
                for(int i = 5; i>=0; i--)
                {
                    caixaEletrica[i].SetTrigger("Ativar");
                    yield return new WaitForSeconds(1.5f);
                }
                break;

            case 3:
                for (int i = 0; i <= 5; i++)
                {
                    if (i % 2 == 0)
                    {
                        caixaEletrica[i].SetTrigger("Ativar");
                    }
                }
                yield return new WaitForSeconds(1.5f);
                for (int i = 0; i <= 5; i++)
                {
                    if (i % 2 != 0)
                    {
                        caixaEletrica[i].SetTrigger("Ativar");
                    }
                }
                break;
        }
        yield return new WaitForSeconds(5);
        StartCoroutine(AtaqueCaixa());
    }

    private IEnumerator AtaqueCabo()
    {
        yield return new WaitUntil(() => GameController.getInstance().comecar);

        foreach(Animator cb in caboEletrico)
        {
            int a = Random.Range(0, 2);
            bool ataca = a == 0 ? false : true;

            if (ataca)
                cb.SetTrigger("Ativar");
        }
        yield return new WaitForSeconds(Random.Range(3f, 5f));
        StartCoroutine(AtaqueCabo());
    }

    private IEnumerator MudaEstagioVida()
    {
        yield return new WaitUntil(() => vida <= (int)vidaTotal * 0.75f);
        StartCoroutine(AtaqueRaio());
        movimento.SetVelocidade(0.7f);
        yield return new WaitUntil(() => vida <= (int)vidaTotal * 0.5f);
        StartCoroutine(AtaqueCaixa());
        movimento.SetVelocidade(0.9f);
        yield return new WaitUntil(() => vida <= (int)vidaTotal * 0.25f);
        StartCoroutine(AtaqueCabo());
        movimento.SetVelocidade(1.1f);
        yield return new WaitUntil(() => vida <= 0);
        morreu = true;
    }
}
