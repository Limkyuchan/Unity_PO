using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerController : MonoBehaviour
{
    [SerializeField]
    AudioMixer m_AudioMixer;
    [SerializeField]
    Slider m_MasterSlider;
    [SerializeField]
    Slider m_BGMSlider;
    [SerializeField]
    Slider m_MainSlider;

    public void SetMasterVolume(float volume)
    {
        m_AudioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetBGMVolume(float volume)
    {
        m_AudioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    public void SetMainVolume(float volume)
    {
        m_AudioMixer.SetFloat("Main", Mathf.Log10(volume) * 20);
    }

    void Awake()
    {
        m_MasterSlider.onValueChanged.AddListener(SetMasterVolume);
        m_BGMSlider.onValueChanged.AddListener(SetBGMVolume);
        m_MainSlider.onValueChanged.AddListener(SetMainVolume);
    }
}