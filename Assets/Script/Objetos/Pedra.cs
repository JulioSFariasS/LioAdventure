using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedra : MonoBehaviour
{
    private Rigidbody2D[] partes = new Rigidbody2D[4];

    private void Start()
    {
        partes[0] = transform.GetChild(0).GetComponent<Rigidbody2D>();
        partes[1] = transform.GetChild(1).GetComponent<Rigidbody2D>();
        partes[2] = transform.GetChild(2).GetComponent<Rigidbody2D>();
        partes[3] = transform.GetChild(3).GetComponent<Rigidbody2D>();
        //Physics2D.IgnoreLayerCollision(2, 2);
        Physics2D.IgnoreLayerCollision(3, 2);
        Physics2D.IgnoreLayerCollision(8, 2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ArmaLio")
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            int i = 0;
            float x=0, y=0;

            foreach(Rigidbody2D rb in partes)
            {
                rb.isKinematic = false;

                switch (i)
                {
                    case 0: x = -50f; y = 50f; break;
                    case 1: x = -40f; y = 20f; break;
                    case 2: x = 50f; y = 50f; break;
                    case 3: x = 50f; y = 40f; break;
                }
                rb.AddForce(new Vector2(x, y));

                i++;
            }

            StartCoroutine(SomeExclui());
        }
    }

    private IEnumerator SomeExclui()
    {
        foreach (Rigidbody2D rb in partes)
        {
            SpriteRenderer spr = rb.GetComponent<SpriteRenderer>();

            while (spr.color.a > 0f)
            {
                spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, spr.color.a-0.1f);
                yield return new WaitForSeconds(0.1f);
            }
            Destroy(rb.gameObject);
        }

        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}
