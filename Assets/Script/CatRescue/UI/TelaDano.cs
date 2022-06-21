using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelaDano : TelaEmbolhada
{
    private void Start()
    {
        GameController.getInstance().telaDano = this;
        img = GetComponent<Image>();
        corAlpha = new Color(0, 0, 0, 0.1f);
    }

    public IEnumerator ApareceESome()
    {
        yield return StartCoroutine(SobeAlpha());
        yield return StartCoroutine(DiminuiAlpha());
    }
}
