using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PopupManager : SingletonDontDestroy<PopupManager>
{
    [SerializeField]
    GameObject m_popupOkCancelPrefab;
    [SerializeField]
    GameObject m_popupOkPrefab;

    int m_popupDepth;
    List<GameObject> m_popupList = new List<GameObject>();

    public bool IsPopupOpened { get { return m_popupList.Count > 0; } }

    public void Popup_OpenOkCancel(string title, string body, Action okDel = null, Action cancelDel = null, string okBtnText = "Ok", string cancelBtnText = "Cancel")
    {
        var obj = Instantiate(m_popupOkCancelPrefab);
        obj.transform.SetParent(transform, false);

        Canvas popupCanvas = obj.GetComponent<Canvas>();
        if (popupCanvas != null )
        {
            popupCanvas.sortingOrder = m_popupDepth++;
        }

        var popup = obj.GetComponent<Popup_OkCancel>();
        popup.SetUI(title, body, okDel, cancelDel, okBtnText, cancelBtnText);
        m_popupList.Add(obj);
    }

    public void Popup_OpenOk(string title, string body, Action okDel = null, string okBtnText = "Ok")
    {
        var obj = Instantiate(m_popupOkPrefab);
        obj.transform.SetParent(transform, false);

        Canvas popupCanvas = obj.GetComponent<Canvas>();
        if (popupCanvas != null)
        {
            popupCanvas.sortingOrder = m_popupDepth++;
        }

        var popup = obj.GetComponent<Popup_Ok>();
        popup.SetUI(title, body, okDel, okBtnText);
        m_popupList.Add(obj);
    }

    public void Popup_Close()
    {
        if (m_popupList.Count > 0)
        {
            Destroy(m_popupList[m_popupList.Count - 1]);
            m_popupList.RemoveAt(m_popupList.Count - 1);
            m_popupDepth--;
        }
    }

    protected override void OnStart()
    {
        m_popupOkCancelPrefab = Resources.Load<GameObject>("Popup/Popup_OkCancel");
        m_popupOkPrefab = Resources.Load<GameObject>("Popup/Popup_Ok");
        m_popupDepth = 10;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (UnityEngine.Random.Range(0, 100) % 2 == 0)
            {
                Popup_OpenOk("<color=#ff0000>°øÁö»çÇ×</color>", "Â¦¼öÂ¦¼öÂ¦¼ö");
            }
            else
            {
                Popup_OpenOkCancel("<color=#000000>°øÁö»çÇ×</color>", "È¦¼öÈ¦¼öÈ¦¼ö");
            }
        }
    }
}