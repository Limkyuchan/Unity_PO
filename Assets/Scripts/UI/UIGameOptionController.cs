using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGameOptionController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_gameSettingText;
    [SerializeField]
    GameObject m_gameOption;

    bool m_isOpen;

    public bool IsGameOptionOpen()
    {
        return m_isOpen;
    }

    public void CloseGameOption()
    {
        if (m_isOpen)
        {
            m_gameOption.SetActive(false);
            m_isOpen = false;
        }
    }

    void UpdateTexts()
    {
        m_gameSettingText.text = LanguageManager.Instance.SetUITextLanguage("GameSetting");
    }

    void ToggleGameOption()
    {
        m_isOpen = !m_isOpen;
        m_gameOption.SetActive(m_isOpen);
    }

    void OnEnable()
    {
        if (LanguageManager.Instance == null) return;

        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.OnLanguageChanged += UpdateTexts;
            UpdateTexts();
        }
    }

    void OnDisable()
    {
        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.OnLanguageChanged -= UpdateTexts;
        }
    }

    void Start()
    {
        m_isOpen = false;
        m_gameOption.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ToggleGameOption();
        }
    }
}