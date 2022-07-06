using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] mrs = new MeshRenderer[6];
    [SerializeField] private float[] velocidades = new float[6]; 

    private void Update()
    {
        for(int i = 0; i<6; i++)
        {
            mrs[i].material.mainTextureOffset += new Vector2(velocidades[i] * Time.deltaTime, 0);
        }
    }
}
