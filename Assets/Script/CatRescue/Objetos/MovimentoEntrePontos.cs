using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoEntrePontos : MonoBehaviour
{
    [SerializeField] private float velocidade;
    [SerializeField] private float delayDeMovimento;

    [Header("Tipo de movimento")]
    [Tooltip("0-Livre | 1-Subir | 2-Descer")]
    [SerializeField] private int tipo;
    private float contDelayDeMovimento;
    private Rigidbody2D rb;
    private Transform pontoAtual, pontoAntigo;
    private bool escolhendo;
    [HideInInspector] public PontoDeMovimento pontoDeMovimento;
    [HideInInspector] public Transform pontoFuturo;
    [HideInInspector] public bool movendo;

    private void Start()
    {
        escolhendo = true;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (GameController.getInstance().comecar)
        {
            if (movendo)
            {
                escolhendo = false;
            }

            if (escolhendo)
            {
                if (contDelayDeMovimento >= delayDeMovimento)
                {
                    EscolhePontoNovo();
                    escolhendo = false;
                }
                contDelayDeMovimento += 0.2f;
            }

            if (movendo)
            {
                contDelayDeMovimento = 0;
                if (Vector3.Distance(rb.position, pontoFuturo.position) < 0.01f)
                {
                    pontoAntigo = pontoAtual;
                    pontoAtual = pontoFuturo;

                    pontoDeMovimento = pontoAtual.GetComponent<PontoDeMovimento>();
                    movendo = false;
                    escolhendo = true;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (GameController.getInstance().comecar)
        {
            if (movendo)
            {
                rb.MovePosition(Vector3.MoveTowards(rb.position, pontoFuturo.position, velocidade * Time.deltaTime));
            }
        }
    }

    private void EscolhePontoNovo()
    {
        if (tipo == 0)
        {
            bool continuar = false;
            do
            {
                int novoPonto = Random.Range(1, 5);

                switch (novoPonto)
                {
                    case 1: pontoFuturo = pontoDeMovimento.pontoCima != null && pontoDeMovimento.pontoCima.GetComponent<PontoDeMovimento>().acessivel ? pontoDeMovimento.pontoCima : null; break;
                    case 2: pontoFuturo = pontoDeMovimento.pontoDireita != null && pontoDeMovimento.pontoDireita.GetComponent<PontoDeMovimento>().acessivel ? pontoDeMovimento.pontoDireita : null; break;
                    case 3: pontoFuturo = pontoDeMovimento.pontoBaixo != null && pontoDeMovimento.pontoBaixo.GetComponent<PontoDeMovimento>().acessivel ? pontoDeMovimento.pontoBaixo : null; break;
                    case 4: pontoFuturo = pontoDeMovimento.pontoEsquerda != null && pontoDeMovimento.pontoEsquerda.GetComponent<PontoDeMovimento>().acessivel ? pontoDeMovimento.pontoEsquerda : null; break;
                }

                if (pontoFuturo != null && pontoFuturo != pontoAntigo)
                {
                    continuar = true;
                }
            }
            while (!continuar);
        }
        else
        if (tipo == 1)
        {
            if (pontoDeMovimento.pontoCima != null)
            {
                bool continuar = false;
                do
                {
                    int novoPonto = Random.Range(1, 4);

                    switch (novoPonto)
                    {
                        case 1: pontoFuturo = pontoDeMovimento.pontoCima; break;
                        case 2: pontoFuturo = pontoDeMovimento.pontoDireita; break;
                        case 3: pontoFuturo = pontoDeMovimento.pontoEsquerda; break;
                    }

                    if (pontoFuturo != null && pontoFuturo != pontoAntigo)
                    {
                        continuar = true;
                    }
                }
                while (!continuar);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        if (tipo == 2)
        {
            if (pontoDeMovimento.pontoBaixo != null)
            {
                bool continuar = false;
                do
                {
                    int novoPonto = Random.Range(1, 4);

                    switch (novoPonto)
                    {
                        case 1: pontoFuturo = pontoDeMovimento.pontoBaixo; break;
                        case 2: pontoFuturo = pontoDeMovimento.pontoDireita; break;
                        case 3: pontoFuturo = pontoDeMovimento.pontoEsquerda; break;
                    }

                    if (pontoFuturo != null && pontoFuturo != pontoAntigo)
                    {
                        continuar = true;
                    }
                }
                while (!continuar);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        movendo = true;
    }

    public void SetVelocidade(float velocidade)
    {
        this.velocidade = velocidade;
    }

    public void SetDelay(float delay)
    {
        delayDeMovimento = delay;
    }
}
