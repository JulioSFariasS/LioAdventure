using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaMovel : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private bool vertical;
    [SerializeField] private SpriteRenderer corpoSpr;
    [SerializeField] private GameObject pontoUm;
    [SerializeField] private GameObject pontoDois;
    private Rigidbody2D rb;
    private GameObject direcao;
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
    private Vector3 m_Velocity = Vector3.zero;


    private void Start()
    {
        name = "PlataformaMovel";
        direcao = pontoUm;
        rb = GetComponent<Rigidbody2D>();
        SoltaPontos();
    }

    private void SoltaPontos()
    {
        pontoUm.transform.SetParent(null);
        pontoUm.GetComponent<SpriteRenderer>().enabled = false;
        pontoUm.layer = 0;
        pontoDois.transform.SetParent(null);
        pontoDois.GetComponent<SpriteRenderer>().enabled = false;
        pontoDois.layer = 0;
    }

    private void FixedUpdate()
    {
        if (vertical)
        {
            //Vector3 targetVelocity = new Vector2(direcao.transform.position.x, direcao.transform.position.y);
            rb.MovePosition(Vector3.SmoothDamp(rb.position, direcao.transform.position, ref m_Velocity, movementSmoothing, speed));
            //rb.velocity = new Vector2(0f, direcao * speed * Time.fixedDeltaTime);
        }
        else
        {
            //rb.velocity = new Vector2(direcao * speed * Time.fixedDeltaTime, 0f);
        }
    }

    private void Flip()
    {
        if (direcao == pontoUm)
        {
            direcao = pontoDois;
        }
        else
        {
            direcao = pontoUm;
        }
        //direcao *= -1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == pontoUm || collision.gameObject == pontoDois)
        {
            Flip();
        }
    }

    public SpriteRenderer GetCorpoSpr()
    {
        return corpoSpr;
    }
}
