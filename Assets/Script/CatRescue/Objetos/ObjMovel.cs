using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjMovel : MonoBehaviour
{
    [SerializeField]private SpriteRenderer spr;
    private Rigidbody2D rb;
    private float rotacao;
    private Vector2 movimento;
    private float velocidade;

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
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            rb.MovePosition(rb.position + movimento * velocidade * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation + rotacao * Time.fixedDeltaTime);
        }
        else
        {
            transform.position += new Vector3(movimento.x, movimento.y, 0) * velocidade * Time.fixedDeltaTime;
        }
    }

    private IEnumerator DestroiForaDaCamera()
    {
        yield return new WaitUntil(() => spr.isVisible);
        yield return new WaitUntil(() => !spr.isVisible);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}