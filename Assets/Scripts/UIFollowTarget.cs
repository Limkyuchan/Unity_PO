using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowTarget : MonoBehaviour
{
    RectTransform m_rect;
    
    void Awake()
    {
        m_rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        //m_rect.position = Camera.main.WorldToScreenPoint("해당 적의 포지션");
    }
}
