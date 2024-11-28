using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGameOption : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_gameOptionText;
    [SerializeField]
    TextMeshProUGUI m_soundText;
    [SerializeField]
    TextMeshProUGUI m_cameraShakeText;
    [SerializeField]
    TextMeshProUGUI m_languageText;

    void OnEnable()
    {
        if (LanguageManager.Instance == null) return;

        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.OnLanguageChanged += UpdateTexts;
            UpdateTexts();      // 초기 한글로 설정
        }
    }

    void OnDisable()
    {
        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.OnLanguageChanged -= UpdateTexts;
        }
    }

    void UpdateTexts()
    {
        m_gameOptionText.text = LanguageManager.Instance.SetUITextLanguage("GameOption");
        m_soundText.text = LanguageManager.Instance.SetUITextLanguage("Sound");
        m_cameraShakeText.text = LanguageManager.Instance.SetUITextLanguage("CameraShake");
        m_languageText.text = LanguageManager.Instance.SetUITextLanguage("Language");
    }
}