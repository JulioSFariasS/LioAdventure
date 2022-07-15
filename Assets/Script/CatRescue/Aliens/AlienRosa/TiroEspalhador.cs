using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiroEspalhador : MonoBehaviour
{
    [Header("Atributos")]
    
    [SerializeField] private float tempoParaEspalhar;
    [SerializeField] private float velocidade;
    [SerializeField] private float velocidadePart;

    [SerializeField] private GameObject tiroEspalhadorPart;

    private ObjMovel objMovel;

    void Start()
    {
        objMovel = GetComponent<ObjMovel>();
        StartCoroutine(Temporizador());
    }

    private IEnumerator Temporizador()
    {
        objMovel.SetInfo(velocidade, 0);
        yield return new WaitForSeconds(tempoParaEspalhar);

        while (velocidade > 0)
        {
            velocidade -= 0.05f;

            if (velocidade <= 0)
                velocidade = 0;

            objMovel.SetVelocidade(velocidade);
            yield return new WaitForFixedUpdate();
        }

        Espalhar();
    }

    private void Espalhar()
    {
        ObjMovel[] parts = new ObjMovel[8];

        for(int i=0; i<8; i++)
        {
            parts[i] = Instantiate(tiroEspalhadorPart, transform.position, Quaternion.identity).GetComponent<ObjMovel>();
        }

        parts[0].SetInfo(velocidadePart, new Vector2(-1, 1).normalized, 0);
        parts[1].SetInfo(velocidadePart, new Vector2(1, 1).normalized, 0);
        parts[2].SetInfo(velocidadePart, new Vector2(-1, -1).normalized, 0);
        parts[3].SetInfo(velocidadePart, new Vector2(1, -1).normalized, 0);
        parts[4].SetInfo(velocidadePart, new Vector2(0, -1), 0);
        parts[5].SetInfo(velocidadePart, new Vector2(1, 0), 0);
        parts[6].SetInfo(velocidadePart, new Vector2(-1, 0), 0);
        parts[7].SetInfo(velocidadePart, new Vector2(0, 1), 0);

        Destroy(gameObject);
    }
}
