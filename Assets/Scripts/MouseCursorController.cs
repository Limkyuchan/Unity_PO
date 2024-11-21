using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorController : MonoBehaviour
{
    [SerializeField]
    GameObject m_AudioSlider;

    void MouseCursorControl()
    {
        if (PopupManager.Instance != null)
        {
            if (PopupManager.Instance.IsPopupOpened || m_AudioSlider.activeSelf)
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
