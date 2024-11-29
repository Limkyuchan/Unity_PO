using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorController : MonoBehaviour
{
    [SerializeField]
    GameObject m_gameOption;

    void MouseCursorControl()
    {
        if (PopupManager.Instance != null)
        {
            if (PopupManager.Instance.IsPopupOpened || m_gameOption.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    void Update()
    {
        MouseCursorControl();
    }
}