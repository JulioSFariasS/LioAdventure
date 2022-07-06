using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasJoy : MonoBehaviour
{
    [SerializeField] LioController lioController;
    [SerializeField] But[] botoes = new But[2];

    private void Start()
    {
        botoes = GetComponentsInChildren<But>();

        foreach(But bt in botoes)
        {
            bt.SetLio(lioController);
        }
    }
}
