using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuracoNegro : MonoBehaviour
{
    private Rigidbody2D rb;
    public float velocidade;
    public float duracao;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.MoveRotation(rb.rotation + velocidade);
    }

    public void StartaBuraco()
    {
        StartCoroutine(Duracao());
    }

    private IEnumerator Duracao()
    {
        while (transform.localScale.x < 1.2)
        {
            transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(duracao);

        while (transform.localScale.x > 0)
        {
            transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(gameObject);
    }
}
