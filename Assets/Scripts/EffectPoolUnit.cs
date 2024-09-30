using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPoolUnit : MonoBehaviour
{
    [SerializeField]
    float m_delay = 0.5f;
    float m_inactiveTime;
    string m_effectName;

    public bool IsReady
    {
        get
        {
            if (!gameObject.activeSelf)
            {
                if (Time.time > m_inactiveTime + m_delay)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public void SetEffectPool(string effectName)
    {
        m_effectName = effectName;
        transform.SetParent(EffectPool.Instance.transform);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    void OnDisable()
    {
        m_inactiveTime = Time.time;
        EffectPool.Instance.AddPool(m_effectName, this);
    }
}