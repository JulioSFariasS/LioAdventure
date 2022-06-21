using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelaConfusaController : MonoBehaviour
{
    public TelaConfusa[] espirais = new TelaConfusa[20];

    private void Start()
    {
        GameController.getInstance().telaConfusa = this;
    }
}
