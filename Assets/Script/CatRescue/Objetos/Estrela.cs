using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Estrela : MonoBehaviour
{
    [SerializeField] private string cor;
    private Animator anim;
    private SpriteRenderer spr;
    private Rigidbody2D rb;
    private bool seguidora;
    private float tempoParaSeguir;
    private float rotacao;
    public Vector2 movimento;

    private void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spr = anim.gameObject.GetComponent<SpriteRenderer>();

        switch (cor)
        {
            case "Preta": anim.SetBool("Preta", true); name = "EstrelaPreta"; break;
            case "Rosa": anim.SetBool("Rosa", true); name = "EstrelaRosa"; break;
            case "Azul": anim.SetBool("Azul", true); name = "EstrelaAzul"; break;
        }

        //StartCoroutine(DestroiForaDaCamera());
    }
    /*
    private void Update()
    {
        tempoParaSeguir += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if(cor=="Preta" && seguidora)
        {
            if(tempoParaSeguir >= 0.5f)
            {
                rb.isKinematic = true;
                rb.MovePosition(Vector3.MoveTowards(rb.position, GameController.getInstance().lioController.transform.position, 5f * Time.fixedDeltaTime));
                rotacao = (rb.velocity.x > 0) ? -360 : 360;
                rb.MoveRotation(rb.rotation + rotacao * Time.fixedDeltaTime);
            }
        }
        else
        {
            rb.MovePosition(rb.position + movimento * 1.5f * Time.fixedDeltaTime);
            rotacao = (rb.velocity.x > 0) ? -90 : 90;
            rb.MoveRotation(rb.rotation + rotacao * Time.fixedDeltaTime);
        }
    }

    public void ESeguidora()
    {
        seguidora = true;
    }

    private IEnumerator DestroiForaDaCamera()
    {
        yield return new WaitUntil(() => spr.isVisible);
        yield return new WaitUntil(() => !spr.isVisible);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }*/
}
