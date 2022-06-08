using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class Tiro : MonoBehaviour
{
    [SerializeField] private GameObject explodeParticulas;
    private float velocidade;
    private Vector2 direcao;
    private float rotacao;
    private Rigidbody2D rb;
    private SpriteRenderer spr;
    private bool start;
    private float autoDestruicaoTempo = 1;
    private bool autoDestruir;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        Physics2D.IgnoreLayerCollision(7, 9); 
    }

    public void SetInfo(float velocidade, Vector2 direcao)
    {
        this.velocidade = velocidade;
        this.direcao = direcao;

        transform.localEulerAngles = direcao.x > 0 ? new Vector3(0, 0, 0) : new Vector3(0, 180, 0);

        rotacao = (direcao.x > 0) ? -360 : 360;
        start = true;
    }

    private void Update()
    {
        if (!spr.isVisible && !autoDestruir)
        {
            autoDestruir = true;
            StartCoroutine(AutoDestruicao());
        }
    }

    public void FixedUpdate()
    {
        //rb.MoveRotation(rb.rotation + rotacao * Time.fixedDeltaTime);

        if(start)
            rb.velocity = direcao * velocidade * Time.fixedDeltaTime;
    }

    private IEnumerator AutoDestruicao()
    {
        yield return new WaitForSeconds(autoDestruicaoTempo);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Inimigo")
        {
            Instantiate(explodeParticulas, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
