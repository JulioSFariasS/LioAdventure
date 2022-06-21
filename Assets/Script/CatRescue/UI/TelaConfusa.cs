using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelaConfusa : TelaEmbolhada
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        img = GetComponent<Image>();
        corAlpha = new Color(0, 0, 0, 0.1f);
    }

    private void Update()
    {
        if (img.color.a <= 0)
        {
            anim.enabled = false;
        }
        else
            anim.enabled = true;
    }
}
