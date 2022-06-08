using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPedra : MonoBehaviour
{
    [SerializeField] private GameObject pedra;
    [SerializeField] private AlienVerde alienVerde;

    private void Start()
    {
        StartCoroutine(Spawna());
    }

    private IEnumerator Spawna()
    {
        Vector3 posAtual = transform.position;

        if(alienVerde.GetEnfurecido())
            yield return new WaitForSeconds(Random.Range(1f, 2f));
        else
            yield return new WaitForSeconds(Random.Range(3f, 6f));

        var objPedra = Instantiate(pedra, posAtual, Quaternion.identity);
        objPedra.GetComponent<ObjMovel>().SetInfo(1, new Vector2(-1, 0), 30);

        float tamanho = Random.Range(0.5f, 2f);
        objPedra.transform.localScale = new Vector3(tamanho, tamanho, tamanho);

        StartCoroutine(Spawna());
    }
}
