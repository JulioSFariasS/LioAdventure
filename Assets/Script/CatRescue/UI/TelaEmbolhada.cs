using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelaEmbolhada : MonoBehaviour
{
    protected Image img;
    protected Color corAlpha;

    private void Start()
    {
        GameController.getInstance().telaEmbolhada = this;
        img = GetComponent<Image>();
        corAlpha = new Color(0, 0, 0, 0.1f);
    }

    public IEnumerator SobeAlpha()
    {
        while (img.color.a < 1)
        {
            img.color += corAlpha;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator DiminuiAlpha()
    {
        while (img.color.a > 0)
        {
            img.color -= corAlpha;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
