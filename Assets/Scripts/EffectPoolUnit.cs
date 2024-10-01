using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPoolUnit : MonoBehaviour
{
    [SerializeField]
    float m_delay = 0.5f;       // 이팩트 딜레이 시간
    float m_inactiveTime;       // 이팩트 꺼지는 시간
    string m_effectName;

    public bool IsReady
    {
        get
        {
            if (!gameObject.activeSelf)                     // 이팩트가 꺼져 있다면
            {
                if (Time.time > m_inactiveTime + m_delay)   // 이팩트 꺼진시간 + 딜레이시간보다 시간이 지났다면 이팩트 사용 가능
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

    void OnDisable()    // 이팩트가 꺼졌을 때
    {
        m_inactiveTime = Time.time;
        EffectPool.Instance.AddPool(m_effectName, this);
    }
}