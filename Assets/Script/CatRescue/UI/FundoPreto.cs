using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FundoPreto : MonoBehaviour
{
    [SerializeField] private GameObject fundoPretoInicial;
    [SerializeField] private GameObject fundoPreto;
    [SerializeField] private GameObject fundoPretoMask;
    private Transform alvo;
    private Vector3 valores = new Vector3(1f,1f,1f);

    private void Start()
    {
        GameController.getInstance().fundoPreto = this;
        fundoPretoInicial.SetActive(true);
    }

    public IEnumerator IniciaCena(Transform obj)
    {
        alvo = obj;
        var mask = Instantiate(fundoPretoMask, alvo.position, Quaternion.identity);
        mask.transform.localScale = Vector3.zero;

        while(mask.transform.localScale.x <= 200)
        {
            mask.transform.localScale += valores;
            valores += new Vector3(0.01f,0.01f,0.01f);
            yield return new WaitForSeconds(0.001f);
        }
        Destroy(fundoPretoInicial);
        Destroy(mask);
    }

    public IEnumerator AcabaCena(Transform obj, string cenaNome)
    {
        Instantiate(fundoPreto, Vector3.zero, Quaternion.identity, Camera.main.transform);
        alvo = obj;
        var mask = Instantiate(fundoPretoMask, alvo.position, Quaternion.identity);
        mask.transform.SetParent(alvo);
        mask.transform.localScale = new Vector3(200,200,1);
        
        while (mask.transform.localScale.x >= 0)
        {
            mask.transform.localScale -= valores;
            valores += new Vector3(0.01f, 0.01f, 0.01f);
            yield return new WaitForSeconds(0.001f);
        }
        Destroy(mask);
        yield return new WaitForSeconds(1);
        GameController.getInstance().StartCoroutine(GameController.getInstance().MudaCena(cenaNome));
    }
}
