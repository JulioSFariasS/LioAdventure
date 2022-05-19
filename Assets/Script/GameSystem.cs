using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameSystem : Singleton<GameSystem>
{
    public int vidas;

    void Start()
    {
        if (base.SingletonStart())
        {
        }
    }

    public void LevarDano(LioControl lioCtrl ,int dano)
    {
        vidas -= dano;
        if (vidas < 0)
        {
            vidas = 0;
        }
        StartCoroutine(lioCtrl.PiscaBranco());
        StartCoroutine(SacodeCamera());
    }

    public IEnumerator SacodeCamera()
    {
        CinemachineVirtualCamera cineCam = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        CinemachineFramingTransposer cineFraming = cineCam.GetCinemachineComponent<CinemachineFramingTransposer>();

        for(int i = 0; i<5; i++)
        {
            if(cineFraming.m_ScreenX == 0.5f)
            {
                cineFraming.m_ScreenX = 0.51f;
            }
            else
            if (cineFraming.m_ScreenX == 0.51f)
            {
                cineFraming.m_ScreenX = 0.49f;
            }
            else
            if (cineFraming.m_ScreenX == 0.49f)
            {
                cineFraming.m_ScreenX = 0.51f;
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }
        cineFraming.m_ScreenX = 0.5f;
    }
}
