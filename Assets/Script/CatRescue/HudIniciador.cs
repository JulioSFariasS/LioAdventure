using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class HudIniciador : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI superContadorTxt;
    [SerializeField] private GameObject vitoriaPanel;
    [SerializeField] private ParticleSystem vitoriaParticulaEsq;
    [SerializeField] private ParticleSystem vitoriaParticulaDir;
    [SerializeField] private CinemachineVirtualCamera cameraNormal;
    [SerializeField] private GameObject canvasJoy;

    private void Start()
    {
        GameController.getInstance().superContadorTxt = superContadorTxt;
        GameController.getInstance().vitoriaPanel = vitoriaPanel;
        GameController.getInstance().vitoriaParticulaEsq = vitoriaParticulaEsq;
        GameController.getInstance().vitoriaParticulaDir = vitoriaParticulaDir;
        GameController.getInstance().cameraNormal = cameraNormal;
        GameController.getInstance().canvasJoy = canvasJoy;
    }
}
