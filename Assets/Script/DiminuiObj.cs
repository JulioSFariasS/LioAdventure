using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiminuiObj : MonoBehaviour
{
    public float tempoParaSumir;
    public bool movimenta;
    public Vector3 direcao;
    public float velocidadeDeMovimento;

    private void Start()
    {
        StartCoroutine(DiminuiExclui());
    }

    void FixedUpdate()
    {
        if (movimenta)
        {
            transform.position += direcao * velocidadeDeMovimento * Time.deltaTime;
        }
    }

    public IEnumerator DiminuiExclui() 
    {
        while (transform.localScale.x > 0)
        {
            transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
            yield return new WaitForSeconds(tempoParaSumir);
        }
        Destroy(gameObject);
    }
}
