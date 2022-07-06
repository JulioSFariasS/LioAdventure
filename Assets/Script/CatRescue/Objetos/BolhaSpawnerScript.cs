using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolhaSpawnerScript : MonoBehaviour
{
    [SerializeField] private Vector3 pontoDeInicio;
    private Rigidbody2D rb;
    public bool liberar;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Liberar());
    }

    private void FixedUpdate()
    {
        if (liberar)
        {
            rb.MovePosition(Vector3.MoveTowards(rb.position, pontoDeInicio, 1 * Time.deltaTime));

            if(Vector3.Distance(rb.position, pontoDeInicio) < 0.01f)
            {
                liberar = false;
                GetComponent<Flutuantes>().enabled = true;
            }
        }
    }

    private IEnumerator Liberar()
    {
        yield return new WaitUntil(() => liberar);
        gameObject.transform.SetParent(null);
    }
}
