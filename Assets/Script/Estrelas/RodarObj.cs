using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodarObj : MonoBehaviour
{
    protected Rigidbody2D rb;
    public float velocidade;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        rb.MoveRotation(rb.rotation + velocidade);
    }
}
