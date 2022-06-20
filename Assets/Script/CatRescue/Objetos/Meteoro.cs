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
    private Vector2 camPos;

    private void Start()
    {
        camPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        //StartCoroutine(EsperaBorda());
    }

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
        float velocidadeEstilha = 0.5f;
        foreach(ObjMovel pedaco in pedacos)
        {
            pedaco.gameObject.SetActive(true);
            pedaco.transform.SetParent(null);
        }
        pedacos[0].SetInfo(velocidadeEstilha, new Vector2(-1, 1).normalized, 90);
        pedacos[1].SetInfo(velocidadeEstilha, new Vector2(1, 1).normalized, -90);
        pedacos[2].SetInfo(velocidadeEstilha, new Vector2(-1, -1).normalized, 90);
        pedacos[3].SetInfo(velocidadeEstilha, new Vector2(1, -1).normalized, -90);
        pedacos[4].SetInfo(velocidadeEstilha, new Vector2(0, -1), -90);
        pedacos[5].SetInfo(velocidadeEstilha, new Vector2(1, 0), -90);
        pedacos[6].SetInfo(velocidadeEstilha, new Vector2(-1, 0), -90);
        pedacos[7].SetInfo(velocidadeEstilha, new Vector2(0, 1), -90);

        Destroy(gameObject);
    }

    private IEnumerator EsperaBorda()
    {
        yield return new WaitUntil(() => transform.position.x <= -Mathf.Clamp(Screen.width, camPos.x * -1, camPos.x));
        Destruir();
    }
}
