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
    [SerializeField] private GameObject vitoriaPanel;
    [SerializeField] private ParticleSystem vitoriaParticulaEsq;
    [SerializeField] private ParticleSystem vitoriaParticulaDir;

    private void Start()
    {
        GameController.getInstance().superContadorTxt = superContadorTxt;
        GameController.getInstance().alienVidaSlider = alienVidaSlider;
        GameController.getInstance().tiroContadorTxt = tiroContadorTxt;
        GameController.getInstance().vitoriaPanel = vitoriaPanel;
        GameController.getInstance().vitoriaParticulaEsq = vitoriaParticulaEsq;
        GameController.getInstance().vitoriaParticulaDir = vitoriaParticulaDir;
    }
}
