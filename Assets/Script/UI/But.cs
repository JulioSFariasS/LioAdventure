using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class But : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    LioController lioController;
    string nome;
    bool apertando;

    private void Start()
    {
        nome = name;
    }

    private void Update()
    {
        if (apertando)
        {
            switch (name)
            {
                case "AtirarBut": lioController.AtiraTouch(); break;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        apertando = true;
        switch (name)
        {
            case "BoostBut": lioController.BoostTouch(); break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        apertando=false;
        switch (name)
        {
            case "AtirarBut": lioController.ParaAtiraTouch(); break;
        }
    }

    public void SetLio(LioController lio)
    {
        lioController = lio;
    }
}
