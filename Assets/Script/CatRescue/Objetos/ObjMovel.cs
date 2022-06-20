using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjMovel : MonoBehaviour
{
    [SerializeField]private SpriteRenderer spr;
    [SerializeField]private bool dirPlayer;
    private Rigidbody2D rb;
    private float rotacao;
    private Vector2 movimento;
    private float velocidade;
    private bool iniciado;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(DestroiForaDaCamera());
    }

    public void SetInfo(float velocidade, Vector2 direcao, float rotacao)
    {
        this.velocidade = velocidade;
        movimento = direcao;
        this.rotacao = rotacao;
        iniciado = true;
    }

    public void SetInfo(float velocidade, float rotacao)
    {
        this.velocidade = velocidade;
        this.rotacao = rotacao;
        movimento = GameController.getInstance().lioController.transform.position - transform.position;
        iniciado = true;
    }

    private void FixedUpdate()
    {
        if (iniciado)
        {
            if (rb != null)
            {
                rb.MovePosition(rb.position + movimento.normalized * velocidade * Time.fixedDeltaTime);
                rb.MoveRotation(rb.rotation + rotacao * Time.fixedDeltaTime);
            }
            else
            {
                transform.position += new Vector3(movimento.x, movimento.y, 0).normalized * velocidade * Time.fixedDeltaTime;
            }
        }
    }

    private IEnumerator DestroiForaDaCamera()
    {
        yield return new WaitUntil(() => spr.isVisible);
        yield return new WaitUntil(() => spr==null || (spr!=null && !spr.isVisible));
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}