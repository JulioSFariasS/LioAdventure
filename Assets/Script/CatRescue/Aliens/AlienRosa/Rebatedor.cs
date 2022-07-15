using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rebatedor : MonoBehaviour
{
    [SerializeField] private ObjMovel objMovel;
    [SerializeField] private Vector2 boundaries;
    [SerializeField] private float tempoDeVida;

    private bool rebateu;

    private void Start()
    {
        boundaries = GetComponent<Boundaries>().GetScreenBounds();
        objMovel = GetComponent<ObjMovel>();

        Destroy(gameObject, tempoDeVida);
    }

    private void Update()
    {
        if((transform.position.x == boundaries.x || transform.position.x == boundaries.x *-1) && !rebateu)
        {
            rebateu = true;
            objMovel.SetMovimento(new Vector2(objMovel.GetMovimento().x * -1, objMovel.GetMovimento().y));
        }

        if ((transform.position.y == boundaries.y || transform.position.y == boundaries.y * -1) && !rebateu)
        {
            rebateu = true;
            objMovel.SetMovimento(new Vector2(objMovel.GetMovimento().x, objMovel.GetMovimento().y * -1));
        }
    }

    private void LateUpdate()
    {
        rebateu = false;
    }

}
