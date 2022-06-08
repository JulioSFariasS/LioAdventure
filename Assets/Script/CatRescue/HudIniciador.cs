using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HudIniciador : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI superContadorTxt;
    [SerializeField] private Slider alienVidaSlider;
    [SerializeField] private TextMeshProUGUI tiroContadorTxt;

    private void Start()
    {
        GameController.getInstance().superContadorTxt = superContadorTxt;
        GameController.getInstance().alienVidaSlider = alienVidaSlider;
        GameController.getInstance().tiroContadorTxt = tiroContadorTxt;
    }
}
