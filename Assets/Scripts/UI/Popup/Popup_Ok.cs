using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class Popup_Ok : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_titleText;
    [SerializeField]
    TextMeshProUGUI m_bodyText;
    [SerializeField]
    TextMeshProUGUI m_okBtnText;

    Action m_okDel;

    public void SetUI(string title, string body, Action okDel = null, string okBtnText = "Ok")
    {
        m_titleText.text = title;
        m_bodyText.text = body;
        m_okBtnText.text = okBtnText;
        m_okDel = okDel;
    }

    public void OnPressOK()
    {
        if (m_okDel != null)
        {
            m_okDel();
        }
        else
        {
            PopupManager.Instance.Popup_Close();
        }
    }
}