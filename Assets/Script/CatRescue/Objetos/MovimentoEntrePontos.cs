using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoEntrePontos : MonoBehaviour
{
    [Header("Pontos de movimento")]
    [SerializeField] private Vector3[] pontos;
    [SerializeField] private float velocidade;
    private Rigidbody2D rb;
    private Vector3 pontoAtual;
    private bool movendo;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!movendo)
        {
            movendo = true;
            pontoAtual = pontos[Random.Range(0, pontos.Length)];
        }

        if (movendo)
        {
            if (Vector3.Distance(rb.position, pontoAtual) < 0.01f)
            {
                movendo = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (movendo)
        {
            rb.MovePosition(Vector3.MoveTowards(rb.position, pontoAtual, velocidade * Time.deltaTime));
        }
    }
}