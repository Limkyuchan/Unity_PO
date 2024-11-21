using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioController : MonoBehaviour
{
    [SerializeField]
    GameObject m_AudioSlider;
    bool m_isClick;

    void Start()
    {
        m_isClick = false;
        m_AudioSlider.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (m_isClick)
            {
                m_AudioSlider.SetActive(false);
                m_isClick = false;
            }
            else if (!m_isClick)
            {
                m_AudioSlider.SetActive(true);
                m_isClick = true;
            }
        }
    }
}