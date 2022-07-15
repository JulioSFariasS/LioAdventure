using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;


public class GameController : Singleton<GameController>
{
    public bool mobile;
    public GameObject canvasJoy;
    public CinemachineVirtualCamera cameraNormal;
    public LioController lioController;
    public TextMeshProUGUI superContadorTxt;
    public TelaEmbolhada telaEmbolhada;
    public TelaDano telaDano;
    public GameObject gameOver;
    public FundoPreto fundoPreto;
    public GameObject vitoriaPanel;
    public ParticleSystem vitoriaParticulaEsq, vitoriaParticulaDir;
    public bool comecar;
    public int dificuldade;

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

                if (mobile)
                    canvasJoy.SetActive(true);

                StartCoroutine(ComecarAcoes());
                Debug.Log("alien verde");
                fundoPreto.StartCoroutine(fundoPreto.IniciaCena(lioController.transform));
                gameOver = Camera.main.transform.GetChild(0).gameObject;
                break;
            case "AlienEletrico":

                if (mobile)
                    canvasJoy.SetActive(true);

                StartCoroutine(ComecarAcoes());
                Debug.Log("alien eletrico");
                fundoPreto.StartCoroutine(fundoPreto.IniciaCena(lioController.transform));
                gameOver = Camera.main.transform.GetChild(0).gameObject;
                break;
            case "AlienRosa":

                if (mobile)
                    canvasJoy.SetActive(true);

                StartCoroutine(ComecarAcoes());
                Debug.Log("alien rosa");
                fundoPreto.StartCoroutine(fundoPreto.IniciaCena(lioController.transform));
                gameOver = Camera.main.transform.GetChild(0).gameObject;
                break;
        }
    }

    public IEnumerator GameOver(Transform alvo, string nomeCena)
    {
        yield return fundoPreto.StartCoroutine(fundoPreto.AcabaCena(alvo, nomeCena));
    }

    public IEnumerator ComecarAcoes()
    {
        yield return new WaitForSeconds(4);
        comecar = true;
    }

    public void AtualizaSuperContadorTxt(int quant, int quantMax)
    {
        superContadorTxt.text = quant.ToString() +"/"+quantMax.ToString();
    }

    public void AtivaOuDesativaGameOver(bool ativar)
    {
        gameOver.GetComponent<BoxCollider2D>().enabled=ativar;
        gameOver.GetComponent<Animator>().SetBool("Pisca", ativar);
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
            case "Eletrico":
                yield return new WaitForSeconds(2);
                fundoPreto.StartCoroutine(fundoPreto.AcabaCena(objAlvo, cenaNome));
                break;
            case "Rosa":
                yield return new WaitForSeconds(2);
                fundoPreto.StartCoroutine(fundoPreto.AcabaCena(objAlvo, cenaNome));
                break;
        }
    }

    public void QualCamera(string nome)
    {
        switch (nome)
        {
            case "Normal":
                cameraNormal.Priority = 10;
                break;
        }
    }
}
