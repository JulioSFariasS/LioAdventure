using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstrelaControl : MonoBehaviour
{
    public FlutuarObj flutuarObj;
    public ParticleSystem particulas;
    public float tamanhoOriginal, tamanhoReduzido, tamanhoAumentado;
    public Animator anim;
    public Collider2D trigger;
    private GameSystem gameSystem;

    void Start()
    {
        gameSystem = GameSystem.getInstance();
        trigger = GetComponent<CircleCollider2D>();
        switch(name)
        {
            case "EstrelaNegra": anim.SetBool("Preta", true); break;
            case "EstrelaAzul": anim.SetBool("Azul", true); break;
            case "EstrelaRosa": anim.SetBool("Rosa", true); break;
        }

        flutuarObj = GetComponent<FlutuarObj>();
        tamanhoOriginal = transform.localScale.x;
        tamanhoReduzido = transform.localScale.x-0.5f;
        tamanhoAumentado = transform.localScale.x+0.5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameSystem.SomaEstrelasPegas();
            trigger.enabled = false;
            flutuarObj.seguir = true;
            flutuarObj.SetRbSeguir(collision.GetComponent<Rigidbody2D>());
            if (gameSystem.estrelasPegas < 3)
            {
                StartCoroutine(DiminuiEstrela());
            }
        }
    }

    public IEnumerator DiminuiEstrela()
    {
        var main = particulas.main;
        var emission = particulas.emission;
        var shape = particulas.shape;
        main.startLifetime = 0.3f;
        main.startSpeed = 0.1f;
        main.startSize = 0.02f;
        emission.rateOverTime = 0f;
        emission.rateOverDistance = 20f;
        shape.shapeType = ParticleSystemShapeType.Circle;

        while (transform.localScale.x > tamanhoReduzido)
        {
            transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator DiminuiMuitoEstrela()
    {
        var main = particulas.main;
        var emission = particulas.emission;
        var shape = particulas.shape;
        main.startLifetime = 0.3f;
        main.startSpeed = 0.1f;
        main.startSize = 0.02f;
        emission.rateOverTime = 0f;
        emission.rateOverDistance = 20f;
        shape.shapeType = ParticleSystemShapeType.Circle;

        while (transform.localScale.x > 0)
        {
            transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator AumentaEstrela()
    {
        var main = particulas.main;
        var emission = particulas.emission;
        var shape = particulas.shape;
        main.startLifetime = 0.2f;
        main.startSpeed = 0f;
        main.startSize = 0.05f;
        emission.rateOverTime = 0f;
        emission.rateOverDistance = 10f;
        shape.shapeType = ParticleSystemShapeType.Circle;

        while (transform.localScale.x < tamanhoAumentado)
        {
            transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator NormalEstrela()
    {
        while (transform.localScale.x > tamanhoOriginal)
        {
            transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
        while (transform.localScale.x < tamanhoOriginal)
        {
            transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
