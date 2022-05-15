using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LioControl : MonoBehaviour
{
    public Animator visualAnim;
    private Vector3 xScaleNormal, xScaleInvertido;

    void Start()
    {
        xScaleNormal = new Vector3(1, transform.localScale.y, transform.localScale.z);
        xScaleInvertido = new Vector3(-1, transform.localScale.y, transform.localScale.z);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {

            transform.localScale = xScaleInvertido;
        }
        else
        if (Input.GetKey(KeyCode.RightArrow))
        {

            transform.localScale = xScaleNormal;
        }

        if (Input.GetKey(KeyCode.A))
        {
            AnimaVisual("parado");
        }
        if (Input.GetKey(KeyCode.S))
        {
            AnimaVisual("corre");
        }
        if (Input.GetKey(KeyCode.D))
        {
            AnimaVisual("atacaChao");
        }
        if (Input.GetKey(KeyCode.F))
        {
            AnimaVisual("atacaAr");
        }
        if (Input.GetKey(KeyCode.G))
        {
            AnimaVisual("pula");
        }
        if (Input.GetKey(KeyCode.H))
        {
            AnimaVisual("cai");
        }
    }

    private void AnimaVisual(string acao)
    {
        switch (acao)
        {
            case "parado": visualAnim.Play("LioParado"); break;
            case "corre": visualAnim.Play("LioCorre"); break;
            case "atacaChao": visualAnim.Play("LioAtacaChao", -1, 0f); break;
            case "atacaAr": visualAnim.Play("LioAtacaAr", - 1, 0f); break;
            case "pula": visualAnim.Play("LioPula"); break;
            case "cai": visualAnim.Play("LioCai"); break;
        }
    }
}
