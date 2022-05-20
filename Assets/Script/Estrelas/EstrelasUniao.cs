using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstrelasUniao : MonoBehaviour
{
    public Rigidbody2D rbGeral, rbPreto, rbRosa, rbAzul;
    public FlutuarObj estrelaPreta, estrelaRosa, estrelaAzul;
    public EstrelaControl estrelaPretaCtrl, estrelaRosaCtrl, estrelaAzulCtrl;
    public RodarObj velocidadeDeGiro;
    public BuracoNegro buracoNegro;

    private void Start()
    {
        buracoNegro.gameObject.SetActive(false);
        rbGeral.gameObject.SetActive(false);
    }

    public void Iniciar()
    {
        rbGeral.gameObject.SetActive(true);
        StartCoroutine(Finaliza());
    }

    private IEnumerator Finaliza()
    {
        yield return new WaitForSeconds(0.1f);
        estrelaPreta.SetRbSeguir(rbPreto);
        estrelaRosa.SetRbSeguir(rbRosa);
        estrelaAzul.SetRbSeguir(rbAzul);
        estrelaPreta.seguirFinal = true;
        estrelaRosa.seguirFinal = true;
        estrelaAzul.seguirFinal = true;
        yield return new WaitForSeconds(1);
        StartCoroutine(estrelaPretaCtrl.AumentaEstrela());
        StartCoroutine(estrelaRosaCtrl.AumentaEstrela());
        StartCoroutine(estrelaAzulCtrl.AumentaEstrela());
        while(velocidadeDeGiro.velocidade < 30)
        {
            velocidadeDeGiro.velocidade += 1;
            yield return new WaitForSeconds(0.1f);
        }
        buracoNegro.gameObject.SetActive(true);
        StartCoroutine(buracoNegro.FinalDeFase());
        StartCoroutine(estrelaPretaCtrl.DiminuiMuitoEstrela());
        StartCoroutine(estrelaRosaCtrl.DiminuiMuitoEstrela());
        StartCoroutine(estrelaAzulCtrl.DiminuiMuitoEstrela());
        yield return new WaitForSeconds(0.25f);
    }
}
