using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] mrs = new MeshRenderer[5];
    [SerializeField] private float[] velocidades = new float[5]; 

    private void Update()
    {
        for(int i = 0; i<5; i++)
        {
            mrs[i].material.mainTextureOffset += new Vector2(velocidades[i] * Time.deltaTime, 0);
        }
    }
}
