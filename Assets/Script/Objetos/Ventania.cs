using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ventania : MonoBehaviour
{
    public float forcaDoVento;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(rb.velocity.x, forcaDoVento);
    }
}
