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
    Slider m_BGMSlider;
    [SerializeField]
    Slider m_SFXSlider;

    public void SetBGMVolume(float volume)
    {
        m_AudioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    public void SetMainVolume(float volume)
    {
        m_AudioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }

    void Awake()
    {
        m_BGMSlider.onValueChanged.AddListener(SetBGMVolume);
        m_SFXSlider.onValueChanged.AddListener(SetMainVolume);
    }
}