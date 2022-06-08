using UnityEngine.Audio;
using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Som[] sons;
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(Som s in sons)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string nome)
    {
        Som s = Array.Find(sons, som => som.nome == nome);

        if (s == null)
        {
            Debug.LogWarning("Som: " + nome + " não encontrado!");
            return;
        }

        s.source.Play();
    }

    public void TocaEDestroi(AudioSource source)
    {
        AudioSource audio = gameObject.AddComponent<AudioSource>();
        audio.clip = source.clip;
        audio.volume = source.volume;
        audio.pitch = source.pitch;
        audio.loop = source.loop;
        StartCoroutine(PlayAndDestroy(audio));
    }

    private IEnumerator PlayAndDestroy(AudioSource audio)
    {
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        Destroy(audio);
    }

    public void CriaTocaEDestroi(AudioClip clip, float volume, float pitch, bool loop)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;
        StartCoroutine(PlayAndDestroy(source));
    }
}
