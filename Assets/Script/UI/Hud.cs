using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hud : MonoBehaviour
{
    public TextMeshProUGUI saude;
    public Image saudeImg;
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
            saude.text = gameSystem.vidas.ToString();
            alienQtd.text = "Aliens: " + gameSystem.alienQuantidade;

            switch (gameSystem.vidas)
            {
                case 3: saudeImg.color = new Color(0.3389141f, 1f, 0.3349057f,1f); break;
                case 2: saudeImg.color = new Color(0.9729285f, 1f, 0.3333334f, 1f); break;
                case 1: saudeImg.color = new Color(1f, 0.415778f, 0.3333334f, 1f); break;
                case 0: saudeImg.color = new Color(0f, 0f, 0f, 1f); break;
            }
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
