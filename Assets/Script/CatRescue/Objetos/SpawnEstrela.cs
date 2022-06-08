using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEstrela : MonoBehaviour
{
    private int cor; //0-Rosa 1-Preta
    [SerializeField] private GameObject estrelaRosa;
    [SerializeField] private GameObject estrelaPreta;

    private void Start()
    {
        StartCoroutine(Spawna());
    }

    private IEnumerator Spawna()
    {
        int quantidade = 3;//Random.Range(1, 4);
        Vector3 posAtual = transform.position;
        yield return new WaitForSeconds(Random.Range(1f, 3f));

        for (int i = 0; i < quantidade; i++)
        {
            cor = 1;//GameController.getInstance().lioController.GetSuper() ? 0 : 1;

            switch (GameController.getInstance().GetNomeDaCena())
            {
                case "AlienVerde":
                    switch (cor)
                    {
                        case 0:
                            var objRosa = Instantiate(estrelaRosa, posAtual, Quaternion.identity);
                            objRosa.GetComponent<Estrela>().movimento.x = -1;
                            break;
                        case 1:
                            var objPreto = Instantiate(estrelaPreta, posAtual, Quaternion.identity);
                            objPreto.GetComponent<Estrela>().movimento.x = -1;
                            break;
                    }
                    break;
            }

            if (i + 1 < quantidade)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }

        StartCoroutine(Spawna());
    }
}
