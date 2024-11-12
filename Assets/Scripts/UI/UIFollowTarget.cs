using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowTarget : MonoBehaviour
{
    Camera m_camera;
    Transform m_target;
    RectTransform m_rect;

    public void SetTarget(Transform target)
    {
        m_target = target;
    }

    void Awake()
    {
        m_camera = Camera.main;
        m_rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (m_target != null)
        {
            m_rect.position = m_camera.WorldToScreenPoint(m_target.position);
        }
    }
}