using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : SingletonDontDestroy<AudioManager>
{
    [Header("Audio Source")]
    [SerializeField]
    AudioSource m_BGMSource;
    [SerializeField]
    AudioSource m_SFXSource;

    [Header("Audio Clip")]
    public AudioClip m_BGM;
    public AudioClip m_Victory;
    public AudioClip m_warriorAttack;
    public AudioClip m_rangeAttack;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float m_BGMVolume = 0.5f;
    [Range(0f, 1f)] public float m_MainVolume = 0.5f;
    [Range(0f, 1f)] public float m_SFXVolume = 0.5f;

    static readonly HashSet<string> m_scenesWithBGM = new HashSet<string> { "Title", "GameSettingScene", "GameScene01", "GameScene02" };

    public void PlayBGM()
    {
        StopAllAudio();
        m_BGMSource.volume = m_BGMVolume;
        m_BGMSource.clip = m_BGM;
        m_BGMSource.Play();
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
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (m_scenesWithBGM.Contains(scene.name))
        {
            PlayBGM();
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