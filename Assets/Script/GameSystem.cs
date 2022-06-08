using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameSystem : Singleton<GameSystem>
{
    public int vidas;
    public int alienQuantidade;
    public int estrelasPegas;
    public LioControl lioCtrl;
    public Hud hud;
    public ParticleSystem particulasVitoria;
    private Animator escuroAnim; 

    void Start()
    {
        if (SingletonStart())
        {
            AjeitaCena();
        }
    }

    public void ChamaVitoria()
    {
        hud.Vitoria();
        particulasVitoria.Play();
    }

    public IEnumerator MudaCena(string cena)
    {
        //AsyncOperation op = SceneManager.LoadSceneAsync("LoadingCena");

        //while (!op.isDone)
        //    yield return new WaitForEndOfFrame();
        AsyncOperation op = SceneManager.LoadSceneAsync(cena);

        while (!op.isDone)
            yield return new WaitForEndOfFrame();

        AjeitaCena();
    }

    public void AjeitaCena()
    {
        switch(SceneManager.GetActiveScene().name)
        {
            case "SampleScene":
                QuantidadeDeAliens();
                particulasVitoria = Camera.main.transform.GetChild(0).GetComponent<ParticleSystem>();
                hud = GameObject.Find("HUD").GetComponent<Hud>();
                escuroAnim = GameObject.Find("Escuro").GetComponent<Animator>();
                estrelasPegas = 0;
                vidas = 3;
                break;
        }   
    }

    public void SomaEstrelasPegas()
    {
        estrelasPegas++;
        if (estrelasPegas == 3)
        {
            StartCoroutine(lioCtrl.VitoriaDeFase());
            estrelasPegas = 3;
        }
    }

    public void QuantidadeDeAliens()
    {
        List<GameObject> aliens = new List<GameObject>(GameObject.FindGameObjectsWithTag("Inimigo"));
        alienQuantidade = aliens.Count;
    }

    public void QuantidadeDeAlienDiminui()
    {
        alienQuantidade--;
        if (alienQuantidade < 0)
        {
            alienQuantidade = 0;
        }
    }

    public void QuantidadeDeAlienAumenta()
    {
        alienQuantidade++;
    }

    public void LevarDano(LioControl lioCtrl ,int dano)
    {
        Controle.SetVibra(1, 1, 0.05f);
        vidas -= dano;
        if (vidas < 0)
        {
            vidas = 0;
        }
        StartCoroutine(lioCtrl.PiscaBranco());
        //StartCoroutine(SacodeCamera());
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

    public void Escuro(bool escuro)
    {
        escuroAnim.SetBool("Escuro", escuro);
    }
}
