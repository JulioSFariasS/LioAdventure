using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRaivoso : Enemy
{
    public bool raivoso;
    public bool frustrado;
    public bool duvida;
    public float forcaDePulo;

    new void Update()
    {
        isGrounded = Physics2D.OverlapCircle(posPe.position, checkGroundRadius, whatIsGround);
        naBorda = !Physics2D.OverlapCircle(verificadorDeBorda.position, checkBordaRadius, whatIsGround) && !Physics2D.OverlapCircle(verificadorDeBorda2.position, checkBordaRadius, whatIsGround);
        naParede = Physics2D.OverlapCircle(verificadorDeParede.position, checkBordaRadius, whatIsGround) && Physics2D.OverlapCircle(verificadorDeParede2.position, checkBordaRadius, whatIsGround);
        noInimigo = Physics2D.OverlapCircle(verificadorDeOutroInimigo.position, 0.05f, inimigoLayer);

        if (vida <= 0 && !morreu)
        {
            morreu = true;
            StartCoroutine(Morte());
        }

        if (!morreu)
        {
            if (player.transform.position.x < rb.position.x - 0.1f)
            {
                movimentoHorizontal = -1f;
            }
            else
            if (player.transform.position.x > rb.position.x + 0.1f)
            {
                movimentoHorizontal = 1f;
            }

            anim.SetBool("Raivoso", true);
            velocidade = 60;


            Flipar();
        }        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Respawn")
        {
            StartCoroutine(Morte());
        }

        if (collision.gameObject.layer == 7 && vulneravel)
        {
            vulneravel = false;
            anim.SetBool("Raivoso", true);
            velocidade = 40;
            StartCoroutine(PiscaBranco());
            vida--;
            /*
            if (vida > 0)
            {
                if(player.transform.position.x<rb.position.x)
                {
                    rb.velocity = new Vector2(forcaDeRecuo, forcaDeRecuo);
                }
                else
                {
                    rb.velocity = new Vector2(-forcaDeRecuo, forcaDeRecuo);
                }
            }
            */
        }
    }
}
