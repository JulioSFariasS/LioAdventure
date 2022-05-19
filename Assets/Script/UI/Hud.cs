using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public TextMeshProUGUI vidas;
    private GameSystem gameSystem;

    void Start()
    {
        gameSystem = GameSystem.getInstance();
    }

    void Update()
    {
        vidas.text = "Life: " + gameSystem.vidas;
    }
}
