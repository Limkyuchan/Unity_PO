using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSelectLanguage : MonoBehaviour
{
    [SerializeField]
    Toggle toggleKorean;
    [SerializeField]
    Toggle toggleEnglish;

    public void ToggleClick(bool isOn)
    {
        if (LanguageManager.Instance == null) return;

        if (toggleKorean.isOn)
        {
            Debug.Log("Korean");
            LanguageManager.Instance.SetLanguage(LanguageManager.Language.Korean);
        }
        else if (toggleEnglish.isOn)
        {
            Debug.Log("English");
            LanguageManager.Instance.SetLanguage(LanguageManager.Language.English);
        }
        
    }

    void Start()
    {
        toggleKorean.isOn = true;
        toggleEnglish.isOn = false;

        toggleKorean.onValueChanged.AddListener(ToggleClick);
        toggleEnglish.onValueChanged.AddListener(ToggleClick);
        
        ToggleClick(toggleKorean.isOn);
    }
}