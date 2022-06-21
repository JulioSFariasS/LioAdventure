using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BichoFolha : MonoBehaviour
{
    [Header("Ponto para seguir")]
    [SerializeField] private Transform objPontoPraIr;
    [SerializeField] private float velocidadePonto;
    [SerializeField] private float velocidadeAtaque;
    [SerializeField] private ObjMovel objMovel;

    private Vector3 pontoParaIr;

    private BichoFolha bichoFolha;
    
    private Rigidbody2D rb;

    private bool parar;

    private void Start()
    {
        objPontoPraIr.SetParent(null);
        pontoParaIr = objPontoPraIr.position;
        bichoFolha = this;
        rb = transform.parent.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!parar)
            rb.MovePosition(Vector3.MoveTowards(rb.position, pontoParaIr, velocidadePonto * Time.deltaTime));

        if (Vector3.Distance(rb.position, pontoParaIr) < 0.01f)
        {
            parar = true;
        }
    }

    public void SoltarObj()
    {
        objMovel.enabled = true;
        objMovel.SetInfo(velocidadeAtaque, 0);
        Destroy(bichoFolha);
    }
}
