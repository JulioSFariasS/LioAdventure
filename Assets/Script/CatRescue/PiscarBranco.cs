using System.Collections;
using UnityEngine;

public class PiscarBranco : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer[] partesCorpoSpr;
    [SerializeField] protected Material materialPiscaBranco;
    protected Material[] materialBase;
    protected Color[] coresOriginais;

    private void Start()
    {
        coresOriginais = new Color[partesCorpoSpr.Length];
        materialBase = new Material[partesCorpoSpr.Length];
        CoresOriginais();
    }

    protected void CoresOriginais()
    {
        int i = 0;

        foreach (SpriteRenderer spr in partesCorpoSpr)
        {
            coresOriginais[i] = spr.color;
            materialBase[i] = spr.material;
            i++;
        }
    }

    protected void SpriteBranca()
    {
        foreach (SpriteRenderer spr in partesCorpoSpr)
        {
            spr.material = materialPiscaBranco;
        }
    }

    protected void SpriteNormal()
    {
        int i = 0;
        foreach (SpriteRenderer spr in partesCorpoSpr)
        {
            spr.material = materialBase[i];
            //spr.color = coresOriginais[i];
            i++;
        }
    }

    public IEnumerator PiscaBranco()
    {
        SpriteBranca();
        yield return new WaitForSeconds(0.1f);
        SpriteNormal();
    }
}
