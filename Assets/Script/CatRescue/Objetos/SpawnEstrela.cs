using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEstrela : MonoBehaviour
{
    [SerializeField] private GameObject estrelaPreta;

    private void Start()
    {
        StartCoroutine(Spawna());
    }

    private IEnumerator Spawna()
    {
        yield return new WaitUntil(() => GameController.getInstance().comecar);
        int quantidade = 0;
        switch (GameController.getInstance().GetNomeDaCena())
        {
            case "AlienVerde": quantidade = 3; break;
            case "AlienEletrico":
                if (estrelaPreta.name == "BolaEletrica")
                    quantidade = 1;
                else
                    quantidade = 2;
                break;
        }
        
        Vector3 posAtual = transform.position;
        yield return new WaitForSeconds(Random.Range(1f, 2f));

        for (int i = 0; i < quantidade; i++)
        {
            switch (GameController.getInstance().GetNomeDaCena())
            {
                case "AlienVerde":

                    var objPreto = Instantiate(estrelaPreta, posAtual, Quaternion.identity);
                    objPreto.GetComponent<Flutuantes>().posBase.GetComponent<ObjMovel>().SetInfo(1, new Vector2(-1, 0), 360);
                    break;

                case "AlienEletrico":
                    InicioPontoDeMovimentoAleatorio pontos = GetComponent<InicioPontoDeMovimentoAleatorio>();
                    objPreto = Instantiate(estrelaPreta, posAtual, Quaternion.identity);
                    objPreto.GetComponent<MovimentoEntrePontos>().pontoFuturo = pontos.pontosDeMovimento[Random.Range(0, pontos.pontosDeMovimento.Length)].transform;
                    objPreto.GetComponent<MovimentoEntrePontos>().movendo = true;
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
