using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : Singleton<GameController>
{
    public LioController lioController;
    public TextMeshProUGUI superContadorTxt;
    public TextMeshProUGUI tiroContadorTxt;
    public Slider alienVidaSlider;
    public TelaEmbolhada telaEmbolhada;
    public TelaDano telaDano;
    public TelaConfusaController telaConfusa;
    public GameObject gameOver;
    public FundoPreto fundoPreto;
    public GameObject vitoriaPanel;
    public ParticleSystem vitoriaParticulaEsq, vitoriaParticulaDir;

    void Start()
    {
        if (SingletonStart())
        {
            AjeitaCena();
        }
    }

    public IEnumerator MudaCena(string nome)
    {
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene(nome);
        yield return new WaitForEndOfFrame();
        AjeitaCena();
    }

    public string GetNomeDaCena()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void AjeitaCena()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "AlienVerde":
                Debug.Log("alien verde");
                fundoPreto.StartCoroutine(fundoPreto.IniciaCena(lioController.transform));
                gameOver = Camera.main.transform.GetChild(0).gameObject;
                break;
        }
    }

    public IEnumerator GameOver(Transform alvo, string nomeCena)
    {
        yield return fundoPreto.StartCoroutine(fundoPreto.AcabaCena(alvo, nomeCena));
    }

    public void AtualizaSuperContadorTxt(int quant)
    {
        superContadorTxt.text = quant.ToString() +"/10";
    }

    public void AtualizaAlienVidaSlider(int quant)
    {
        alienVidaSlider.value = quant;
    }

    public void AtualizaTiroContadorTxt(int quant)
    {
        tiroContadorTxt.text = quant.ToString();
    }

    public void AtivaOuDesativaGameOver(bool ativar)
    {
        gameOver.SetActive(ativar);
    }

    public void EmbolharTela(bool embolhar)
    {
        telaEmbolhada.StopAllCoroutines();
        switch (embolhar)
        {
            case true: telaEmbolhada.StartCoroutine(telaEmbolhada.SobeAlpha()); break;
            case false: telaEmbolhada.StartCoroutine(telaEmbolhada.DiminuiAlpha()); break;
        }
    }

    public void DanoTela()
    {
        telaDano.StopAllCoroutines();
        telaDano.StartCoroutine(telaDano.ApareceESome());
    }

    public void ConfusaTela(bool confuso)
    {
        foreach(TelaConfusa espiral in telaConfusa.espirais)
        {
            espiral.StopAllCoroutines();

            switch (confuso)
            {
                case true: espiral.StartCoroutine(espiral.SobeAlpha()); break;
                case false: espiral.StartCoroutine(espiral.DiminuiAlpha()); break;
            }
        }
    }

    public IEnumerator DerrotaChefe(string chefe, Transform objAlvo, string cenaNome)
    {
        lioController.Venceu();
        vitoriaPanel.SetActive(true);
        vitoriaParticulaEsq.Play();
        vitoriaParticulaDir.Play();

        switch (chefe)
        {
            case "Elemental":
                yield return new WaitForSeconds(2);
                fundoPreto.StartCoroutine(fundoPreto.AcabaCena(objAlvo, cenaNome));
                break;
        }
    }
}
