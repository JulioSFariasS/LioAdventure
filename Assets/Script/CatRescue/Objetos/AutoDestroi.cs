using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroi : MonoBehaviour
{
    [SerializeField] private bool temTempo;
    [SerializeField] private bool eParticula;
    private ParticleSystem particulas;
    private float tempo;

    private void Start()
    {
        if (temTempo)
        {
            StartCoroutine(DestroiComTempo());
        }
        else
        if (eParticula)
        {
            particulas = GetComponent<ParticleSystem>();
            StartCoroutine(DestroiParticula());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroiComTempo()
    {
        yield return new WaitForSeconds(tempo);
        Destroy(gameObject);
    }

    private IEnumerator DestroiParticula()
    {
        float duracao = particulas.main.duration;
        yield return new WaitForSeconds(duracao+0.5f);
        Destroy(gameObject);
    }
}
