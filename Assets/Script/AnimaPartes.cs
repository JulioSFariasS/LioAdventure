using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimaPartes : MonoBehaviour
{
    public Animator boca, olho, guardaChuvaMao;

    public void AnimaBoca(string nome)
    {
        boca.Play(nome);
    }

    public void AnimaOlho(string nome)
    {
        olho.Play(nome);
    }

    public void AnimaGuardaChuvaMao(string nome)
    {
        guardaChuvaMao.Play(nome);
    }
}
