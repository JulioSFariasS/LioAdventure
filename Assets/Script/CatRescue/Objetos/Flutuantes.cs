using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flutuantes : MonoBehaviour
{
    protected float contaTempo;
    protected Rigidbody2D rb;

    //movimento flutuante
    [SerializeField] protected float velocidade;
    [SerializeField] protected float floatStrength;
    [SerializeField] protected Transform posBase;
    protected Vector2 position;

    protected void Start()
    {
        posBase.parent = null;
        rb = GetComponent<Rigidbody2D>();
    }

    protected void Update()
    {
        contaTempo += Time.deltaTime;
    }

    protected void FixedUpdate()
    {
        float newY = Mathf.Sin(contaTempo * velocidade) * floatStrength;
        position = new Vector2(0, newY) + new Vector2(posBase.position.x, posBase.position.y);
        rb.MovePosition(position);
    }

    public Transform GetBase()
    {
        return posBase;
    }
}
