using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Estrela : MonoBehaviour
{
    [SerializeField] private string cor;
    private Animator anim;

    private void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();

        switch (cor)
        {
            case "Preta": anim.SetBool("Preta", true); name = "EstrelaPreta"; break;
            case "Rosa": anim.SetBool("Rosa", true); name = "EstrelaRosa"; break;
            case "Azul": anim.SetBool("Azul", true); name = "EstrelaAzul"; break;
        }
    }
}
