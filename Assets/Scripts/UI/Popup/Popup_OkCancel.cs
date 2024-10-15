using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Popup_OkCancel : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_titleText;
    [SerializeField]
    TextMeshProUGUI m_bodyText;
    [SerializeField]
    TextMeshProUGUI m_okBtnText;
    [SerializeField]
    TextMeshProUGUI m_cancelBtnText;

    Action m_okDel;
    Action m_cancelDel;

    public void SetUI(string title, string body, Action okDel = null, Action cancelDel = null, string okBtnText = "Ok", string cancelBtnText = "Cancel")
    {
        m_titleText.text = title;
        m_bodyText.text = body;
        m_okBtnText.text = okBtnText;
        m_cancelBtnText.text = cancelBtnText;
        m_okDel = okDel;
        m_cancelDel = cancelDel;
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

    public void OnPressCancel()
    {
        if (m_cancelDel != null)
        {
            m_cancelDel();
        }
        else
        {
            PopupManager.Instance.Popup_Close();
        }
    }
}