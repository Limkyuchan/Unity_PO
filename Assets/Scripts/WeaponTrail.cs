using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTrail : MonoBehaviour
{
    TrailRenderer m_trailRenderer;

    public void StartTrail()
    {
        if (m_trailRenderer != null)
        {
            m_trailRenderer.Clear();
            m_trailRenderer.enabled = true;
        }
    }

    public void StopTrail()
    {
        if (m_trailRenderer != null)
        {
            m_trailRenderer.enabled = false;
        }
    }

    void Start()
    {
        m_trailRenderer = GetComponentInChildren<TrailRenderer>();

        if (m_trailRenderer == null) return;
        m_trailRenderer.enabled = false;
    }
}