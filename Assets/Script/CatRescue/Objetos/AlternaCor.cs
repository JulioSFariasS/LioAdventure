using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternaCor : MonoBehaviour
{
    private SpriteRenderer spr;
    [Header("Vari�veis para RGB")]
    [SerializeField] private bool RGB;
    [SerializeField] private float frequencia;

    [Header("Vari�veis para cor fixa")]
    [SerializeField] private Gradient gradiente;
    private Color corAleatoria;

    [Header("Vari�veis de glow")]
    [SerializeField] private bool alternaGlow;
    [SerializeField] private bool alternaSoGlow;
    [SerializeField] private SpriteRenderer sprGlow;

    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        if (RGB)
        {
            StartCoroutine(AlternadorDeCor());
        }
        else
        {
            corAleatoria = gradiente.Evaluate(Random.Range(0f, 1f));
            spr.color = corAleatoria;

            if (alternaGlow || alternaSoGlow)
                sprGlow.material.SetColor("_ColorRGB", corAleatoria);
        }
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
