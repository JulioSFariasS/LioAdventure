using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniAlien : MonoBehaviour
{
    [SerializeField] private int vida;
    [SerializeField] private float velocidade;
    [SerializeField] private Vector3 direcao;
    private Rigidbody2D rb;
    [SerializeField] private Transform posBase;
    
    [SerializeField] private SpriteRenderer sprCorpo;
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();

    //movimento flutuante
    [SerializeField] protected float velocidadeSeno;
    [SerializeField] protected float floatStrength;
    protected Vector2 position;
    private float contaTempo;

    private void Start()
    {
        posBase.SetParent(null);
        rb=GetComponent<Rigidbody2D>();
        StartCoroutine(Destroi());
    }

    public void SetInfo(Vector2 dir, float vel,float velSeno, float forcaSeno)
    {
        velocidade = vel;
        direcao = dir;
        velocidadeSeno = velSeno;
        floatStrength = forcaSeno;

        sprCorpo.sprite = sprites[Random.Range(0, 3)];
    }

    private void Update()
    {
        contaTempo += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        float newY = Mathf.Sin(contaTempo * velocidadeSeno) * floatStrength;
        position = new Vector2(0, newY) + new Vector2(posBase.position.x, posBase.position.y);

        posBase.position += direcao * velocidade * Time.fixedDeltaTime;
        rb.MovePosition(position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ArmaLio")
        {
            StartCoroutine(GetComponent<PiscarBranco>().PiscaBranco());
            vida--;
            if (vida <= 0)
            {
                Destroy(posBase.gameObject);
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator Destroi()
    {
        yield return new WaitUntil(() => sprCorpo.isVisible);
        yield return new WaitUntil(() => !sprCorpo.isVisible);
        yield return new WaitForSeconds(2);
        
        if(!sprCorpo.isVisible)
        {
            Destroy(posBase.gameObject);
            Destroy(gameObject);
            yield break;
        }

        StartCoroutine(Destroi());
    }
}
