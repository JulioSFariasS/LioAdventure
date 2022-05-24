using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hud : MonoBehaviour
{
    public TextMeshProUGUI vidas;
    public TextMeshProUGUI alienQtd;
    private GameSystem gameSystem;

    public GameObject painelVitoria;

    void Start()
    {
        gameSystem = GameSystem.getInstance();
        painelVitoria = transform.GetChild(2).gameObject;
        painelVitoria.SetActive(false);
    }

    void Update()
    {
        if(gameSystem!=null)
        {
            vidas.text = "Life: " + gameSystem.vidas;
            alienQtd.text = "Aliens: " + gameSystem.alienQuantidade;
        }
    }

    public void Vitoria()
    {
        painelVitoria.SetActive(true);
    }

    public void ReiniciaBut()
    {
        gameSystem.StartCoroutine(gameSystem.MudaCena(SceneManager.GetActiveScene().name));
    }
}
