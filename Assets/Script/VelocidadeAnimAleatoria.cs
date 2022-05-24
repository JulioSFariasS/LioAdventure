using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocidadeAnimAleatoria : MonoBehaviour
{
    public Animator anim;
    public float vel;

    void Start()
    {
        anim = GetComponent<Animator>();
        vel = Random.Range(0.5f, 1.5f);
        anim.SetFloat("speed", vel);
    }
}
