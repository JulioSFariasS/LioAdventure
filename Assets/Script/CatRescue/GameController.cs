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

    void Start()
    {
        if (SingletonStart())
        {

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
                Debug.Log("alien verde"); break;
        }
    }

    public void AtualizaSuperContadorTxt(int quant)
    {
        superContadorTxt.text = quant.ToString();
    }

    public void AtualizaAlienVidaSlider(int quant)
    {
        alienVidaSlider.value = quant;
    }

    public void AtualizaTiroContadorTxt(int quant)
    {
        tiroContadorTxt.text = quant.ToString();
    }
}
