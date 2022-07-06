using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolha : MonoBehaviour
{
    private Vector2 movimento;
    [Header("Sprites")]
    [SerializeField] private Sprite normal;
    [SerializeField] private Sprite estoura;
    private float velocidade;
    [SerializeField] private Rigidbody2D rb;
    private bool agarrou;
    private Transform player;

    void Start()
    {
        name = "BolhaAlien";
        movimento = new Vector2(-1f, Random.Range(-1f, 1f));
        rb = GetComponent<Rigidbody2D>();
        velocidade = Random.Range(0.4f, 0.8f);
    }

    private void Update()
    {
        if(!transform.GetChild(0).GetComponent<SpriteRenderer>().isVisible)
            Destroy(gameObject);
    }

    void LateUpdate()
    {
        if(agarrou && (!player.GetComponent<LioController>().GetSuper() || !player.GetComponent<LioController>().GetVulneravel()))
        {
            agarrou = false;
            StopAllCoroutines();
            StartCoroutine(Estoura());
        }
    }

    private void FixedUpdate()
    {
        if (!agarrou)
        {
            rb.MovePosition(rb.position + movimento.normalized * velocidade * Time.fixedDeltaTime);
        }
        else
        {
            rb.position = new Vector2(player.position.x, player.position.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Player")
        {
            if (!collision.GetComponent<LioController>().GetPreso() && collision.GetComponent<LioController>().GetVulneravel() && collision.GetComponent<LioController>().GetSuper()
                && !collision.GetComponent<LioController>().GetVenceu())
            {
                GetComponent<Collider2D>().enabled = false;
                player = collision.transform;
                StartCoroutine(Agarra());
            }
            else
            {
                StartCoroutine(Estoura());
            }
        }
    }

    private IEnumerator Agarra()
    {
        AudioManager.instance.Play("BolhaAgarra");
        GetComponent<Collider2D>().enabled = false;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = "Player";
        transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 15;
        agarrou = true;
        StartCoroutine(Aumenta());
        yield return new WaitForSeconds(2);
        StartCoroutine(Estoura());
    }

    private IEnumerator Aumenta()
    {
        while (transform.localScale.x < 3.5f)
        {
            transform.localScale += new Vector3(0.4f, 0.4f, 0.4f);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator Estoura()
    {
        AudioManager.instance.Play("BolhaEstoura");
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = estoura;
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}