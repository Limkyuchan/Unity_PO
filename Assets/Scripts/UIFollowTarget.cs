using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowTarget : MonoBehaviour
{
    [SerializeField]
    Transform m_target;

    Camera m_camera;
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
        m_rect.position = m_camera.WorldToScreenPoint(m_target.position);
    }
}