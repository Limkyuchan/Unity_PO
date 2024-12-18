using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : SingletonDontDestroy<AudioManager>
{
    #region Constants and Fields
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
    public AudioClip m_dragonScream;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float m_BGMVolume = 0.5f;
    [Range(0f, 1f)] public float m_MainVolume = 0.5f;
    [Range(0f, 1f)] public float m_SFXVolume = 0.5f;

    static readonly HashSet<string> m_scenesWithBGM = new HashSet<string> { "Title", "GameSettingScene", "GameScene01", "GameScene02" };
    //bool m_allowSFX = true;
    #endregion  Constants and Fields

    #region Public Methods
    public void PlayBGM()
    {
        StopBGMAudio();
        m_BGMSource.volume = m_BGMVolume;
        m_BGMSource.clip = m_BGM;
        m_BGMSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        //if (!m_allowSFX) return;
        m_SFXSource.volume = m_SFXVolume;
        m_SFXSource.PlayOneShot(clip);
    }

    public void PlaySFX(AudioClip clip, float sec)
    {
        //if (!m_allowSFX) return;
        m_SFXSource.volume = m_SFXVolume;
        StartCoroutine(CoPlaySFX(clip, sec));
    }

    public void StopSFX()
    {
        m_SFXSource.Stop();
    }

    //public void EnableSFX(bool enable)
    //{
    //    m_allowSFX = enable;
    //}

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (m_scenesWithBGM.Contains(scene.name))
        {
            PlayBGM();
        }
    }

    public void StopAllAudio()
    {
        m_BGMSource.Stop();
        m_SFXSource.Stop();
        m_SFXSource.clip = null;
    }
    #endregion Public Methods

    #region Coroutine Methods
    IEnumerator CoPlaySFX(AudioClip clip, float sec)
    {
        yield return Utility.GetWaitForSeconds(sec);

        //if (!m_allowSFX)
        //{
        //    yield break;
        //}
        m_SFXSource.PlayOneShot(clip);
    }
    #endregion Coroutine Methods

    #region Methods
    void StopBGMAudio()
    {
        m_BGMSource.Stop();
    }
    #endregion Methods

    #region Unity Methods
    protected override void OnAwake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    #endregion Unity Methods
}