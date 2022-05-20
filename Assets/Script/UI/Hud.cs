using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public TextMeshProUGUI vidas;
    public TextMeshProUGUI alienQtd;
    private GameSystem gameSystem;

    void Start()
    {
        gameSystem = GameSystem.getInstance();
    }

    void Update()
    {
        if(gameSystem!=null)
        {
            vidas.text = "Life: " + gameSystem.vidas;
            alienQtd.text = "Aliens: " + gameSystem.alienQuantidade;
        }
    }
}
