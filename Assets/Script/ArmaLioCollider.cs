using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaLioCollider : MonoBehaviour
{
    private CircleCollider2D trigger;
    private Rigidbody2D rb;
    public LioControl lioCtrl;

    void Start()
    {
        trigger = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Inimigo")
        {
            
        }
    }
}
