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
    public GameObject gameOver;

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

    private void AjeitaCena()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "AlienVerde":
                Debug.Log("alien verde");
                gameOver = Camera.main.transform.GetChild(0).gameObject;
                break;
        }
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
}
