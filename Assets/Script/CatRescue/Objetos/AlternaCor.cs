using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternaCor : MonoBehaviour
{
    private SpriteRenderer spr;
    [SerializeField] private float frequencia;
    [SerializeField] private bool alternaGlow;
    [SerializeField] private bool alternaSoGlow;
    [SerializeField] private SpriteRenderer sprGlow;

    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        StartCoroutine(AlternadorDeCor());
    }

    public IEnumerator AlternadorDeCor()
    {
        float r = 0, g = 1, b = 0;

        while (r < 1)
        {
            r += 0.01f;
            if(!alternaSoGlow)
                spr.color = new Color(r, g, b, 1);

            if(alternaGlow || alternaSoGlow)
                sprGlow.material.SetColor("_ColorRGB", new Color(r, g, b, 1));
            yield return new WaitForEndOfFrame();
        }
        while (g > 0)
        {
            g -= 0.01f;
            if (!alternaSoGlow)
                spr.color = new Color(r, g, b, 1);

            if (alternaGlow || alternaSoGlow)
                sprGlow.material.SetColor("_ColorRGB", new Color(r, g, b, 1));
            yield return new WaitForEndOfFrame();
        }
        while (b < 1)
        {
            b += 0.01f;
            if (!alternaSoGlow)
                spr.color = new Color(r, g, b, 1);

            if (alternaGlow || alternaSoGlow)
                sprGlow.material.SetColor("_ColorRGB", new Color(r, g, b, 1));
            yield return new WaitForEndOfFrame();
        }
        while (r > 0)
        {
            r -= 0.01f;
            if (!alternaSoGlow)
                spr.color = new Color(r, g, b, 1);

            if (alternaGlow || alternaSoGlow)
                sprGlow.material.SetColor("_ColorRGB", new Color(r, g, b, 1));
            yield return new WaitForEndOfFrame();
        }
        while (g < 1)
        {
            g += 0.01f;
            if (!alternaSoGlow)
                spr.color = new Color(r, g, b, 1);

            if (alternaGlow || alternaSoGlow)
                sprGlow.material.SetColor("_ColorRGB", new Color(r, g, b, 1));
            yield return new WaitForEndOfFrame();
        }
        while (b > 0)
        {
            b -= 0.01f;
            if (!alternaSoGlow)
                spr.color = new Color(r, g, b, 1);

            if (alternaGlow || alternaSoGlow)
                sprGlow.material.SetColor("_ColorRGB", new Color(r, g, b, 1));
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(AlternadorDeCor());
    }
}
