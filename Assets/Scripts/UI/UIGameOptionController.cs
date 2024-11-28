using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameOptionController : MonoBehaviour
{
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

    void ToggleGameOption()
    {
        m_isOpen = !m_isOpen;
        m_gameOption.SetActive(m_isOpen);
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