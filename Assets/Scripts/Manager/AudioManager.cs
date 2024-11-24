using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : SingletonDontDestroy<AudioManager>
{
    [Header("Audio Source")]
    [SerializeField]
    AudioSource m_MainSource;
    [SerializeField]
    AudioSource m_BGMSource;
    [SerializeField]
    AudioSource m_SFXSource;

    [Header("Audio Clip")]
    public AudioClip m_Main;
    public AudioClip m_BGM;
    public AudioClip m_Victory;
    public AudioClip m_warriorAttack;
    public AudioClip m_rangeAttack;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float m_BGMVolume = 0.5f;
    [Range(0f, 1f)] public float m_MainVolume = 0.5f;
    [Range(0f, 1f)] public float m_SFXVolume = 0.5f;

    public void PlayBGM()
    {
        StopAllAudio();
        m_BGMSource.volume = m_BGMVolume;
        m_BGMSource.clip = m_BGM;
        m_BGMSource.Play();
    }

    public void PlayMain()
    {
        StopAllAudio();
        m_MainSource.volume = m_MainVolume;
        m_MainSource.clip = m_Main;
        m_MainSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        m_SFXSource.volume = m_SFXVolume;
        m_SFXSource.PlayOneShot(clip);
    }

    public void PlaySFX(AudioClip clip, float sec)
    {
        m_SFXSource.volume = m_SFXVolume;
        StartCoroutine(CoPlaySFX(clip, sec));
    }

    public void StopAllAudio()
    {
        m_BGMSource.Stop();
        m_MainSource.Stop();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Title":
            case "GameSettingScene":
                PlayBGM();
                break;
            case "GameScene01":
            case "GameScene02":
                PlayMain();
                break;
        }
    }

    IEnumerator CoPlaySFX(AudioClip clip, float sec)
    {
        yield return Utility.GetWaitForSeconds(sec);
        m_SFXSource.PlayOneShot(clip);
    }

    protected override void OnAwake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
}