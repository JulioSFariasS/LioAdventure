using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteoro : MonoBehaviour
{
    [SerializeField] private int vida;
    [SerializeField] private GameObject objDentro;
    [SerializeField] private SpriteRenderer rachaduraRenderer;
    [SerializeField] private Sprite[] rachaduras = new Sprite[3];
    [SerializeField] private ObjMovel[] pedacos = new ObjMovel[4];
    [SerializeField] private AudioClip destroiSom;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(transform.GetChild(0).GetComponent<SpriteRenderer>().isVisible)
        {
            if (collision.tag == "ArmaLio")
            {
                vida--;
                rachaduraRenderer.sprite = rachaduras[vida];
                if (vida <= 0)
                {
                    Destruir();
                }
            }
            else
        if (collision.name == "Estilhaco")
            {
                Destruir();
            }
        }
    }

    private void Destruir() 
    {
        AudioManager.instance.CriaTocaEDestroi(destroiSom, 0.2f, 1, false);
        float velocidadeEstilha = 1.5f;
        foreach(ObjMovel pedaco in pedacos)
        {
            pedaco.gameObject.SetActive(true);
            pedaco.transform.SetParent(null);
        }
        pedacos[0].SetInfo(velocidadeEstilha, new Vector2(-1, 1), 90);
        pedacos[1].SetInfo(velocidadeEstilha, new Vector2(1, 1), -90);
        pedacos[2].SetInfo(velocidadeEstilha, new Vector2(-1, -1), 90);
        pedacos[3].SetInfo(velocidadeEstilha, new Vector2(1, -1), -90);

        Destroy(gameObject);
    }
}
