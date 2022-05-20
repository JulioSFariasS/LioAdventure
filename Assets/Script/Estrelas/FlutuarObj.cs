using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlutuarObj : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 posIni;
    private Vector2 position;
    private Rigidbody2D rbSeguir;
    public float velocidade = 5;
    public float velocidadeSeguir = 0.2f;
    public float floatStrength = .1f;
    public bool seguir;
    public bool seguirFinal;

    public float distHorizontal, distVertical;

    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
    private Vector3 m_Velocity = Vector3.zero;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        posIni = rb.position;

        switch (name)
        {
            case "EstrelaNegra": distHorizontal=0.3f ; distVertical = 0.25f; break;
            case "EstrelaAzul": distHorizontal = 0.3f; distVertical = 0.2f; break;
            case "EstrelaRosa": distHorizontal = 0.3f; distVertical = 0.15f; break;
        }
    }

    void FixedUpdate()
    {
        float newY = Mathf.Sin(Time.time * velocidade) * floatStrength;
        
        if (!seguir && !seguirFinal)
        {
            position = new Vector2(0, newY) + posIni;
            rb.MovePosition(position);
        }
        else
        if(seguir && !seguirFinal)
        {
            float movimentoHorizontal=0f;
            float movimentoVertical=0f;

            if (rbSeguir.position.x + 0.1f < rb.position.x - distHorizontal)
            {
                movimentoHorizontal = -1f;
            }
            else
            if (rbSeguir.position.x - 0.1f > rb.position.x + distHorizontal)
            {
                movimentoHorizontal = 1f;
            }

            if (rbSeguir.position.y + 0.2f < rb.position.y - distVertical)
            {
                movimentoVertical = -1f;
            }
            else
            if (rbSeguir.position.y + 0.2f > rb.position.y + distVertical)
            {
                movimentoVertical = 1f;
            }

            Vector3 targetVelocity = new Vector2(movimentoHorizontal * velocidadeSeguir * Time.fixedDeltaTime, movimentoVertical * velocidadeSeguir * Time.fixedDeltaTime);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, movementSmoothing);
        }
        else
        {
            rb.MovePosition(rbSeguir.position);
        }
    }

    public void SetRbSeguir(Rigidbody2D rigidbody)
    {
        rbSeguir = rigidbody;
    }
}
