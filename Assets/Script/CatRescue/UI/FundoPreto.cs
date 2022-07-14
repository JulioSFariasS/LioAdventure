using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FundoPreto : MonoBehaviour
{
    [SerializeField] private GameObject fundoPretoInicial;
    [SerializeField] private GameObject fundoPreto;
    [SerializeField] private GameObject fundoPretoMask;
    private Transform alvo;

    private void Start()
    {
        GameController.getInstance().fundoPreto = this;
        fundoPretoInicial.SetActive(true);
    }

    public IEnumerator IniciaCena(Transform obj)
    {
        GameController.getInstance().comecar = false;
        alvo = obj;
        var mask = Instantiate(fundoPretoMask, alvo.position, Quaternion.identity);
        mask.GetComponent<Animator>().SetTrigger("Iniciar");
        yield return new WaitForSeconds(3);
        Destroy(fundoPretoInicial);
        Destroy(mask);
    }

    public IEnumerator AcabaCena(Transform obj, string cenaNome)
    {
        Instantiate(fundoPreto, Vector3.zero, Quaternion.identity, Camera.main.transform);
        alvo = obj;
        var mask = Instantiate(fundoPretoMask, alvo.position, Quaternion.identity);
        mask.transform.SetParent(alvo);
        mask.GetComponent<Animator>().SetTrigger("Fechar");
        yield return new WaitForSeconds(3);
        Destroy(mask);
        yield return new WaitForSeconds(1);
        GameController.getInstance().comecar = false;
        GameController.getInstance().StartCoroutine(GameController.getInstance().MudaCena(cenaNome));
    }
}
