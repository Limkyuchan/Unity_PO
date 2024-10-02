using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Controller : MonoBehaviour
{
    UIFollowTarget m_followTarget;

    [SerializeField]
    HUD_Text m_hudText;
    [SerializeField]
    Slider m_hpBar;
    [SerializeField]
    Text m_iD;
    [SerializeField]
    float m_duration = 3f;

    StringBuilder m_sb = new StringBuilder();

    public void SetHUD(Transform target, EnemyController enemy)
    {
        if (m_followTarget == null)
        {
            m_followTarget = GetComponent<UIFollowTarget>();
        }

        if (m_hudText != null)
        {
            m_hudText.SetText("");
        }

        m_followTarget.SetTarget(target);
        m_hpBar.value = 1f;
        m_iD.text = enemy.GetStatus.type.ToString();
        HideUI();
    }

    public void UpdateHUD(DamageType type, float damage, float normalizedHp)
    {
        if (type != DamageType.None)
        {
            ShowUI();
            if (IsInvoking("HideUI"))
            {
                CancelInvoke("HideUI");
            }
            Invoke("HideUI", m_duration);
        }

        m_hpBar.value = normalizedHp;

        switch (type)
        {
            case DamageType.None:
                m_sb.Append(Mathf.RoundToInt(damage));
                m_hudText.SetText(m_sb.ToString());
                m_hudText.SetColor(Color.red);
                m_sb.Clear();
                break;
            case DamageType.Normal:
                m_sb.Append(Mathf.RoundToInt(damage));
                m_hudText.SetText(m_sb.ToString());
                m_hudText.SetColor(Color.red);
                m_sb.Clear();
                break;
            case DamageType.Critical:
                m_sb.Append(Mathf.RoundToInt(damage));
                m_hudText.SetText(m_sb.ToString());
                m_hudText.SetColor(Color.yellow);
                m_sb.Clear();
                break;
            case DamageType.Miss:
                m_sb.Append("Miss");
                m_hudText.SetText(m_sb.ToString());
                m_hudText.SetColor(Color.white);
                m_sb.Clear();
                break;
        }
    }

    void ShowUI()
    {
        gameObject.SetActive(true);
    }

    void HideUI()
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
        m_followTarget = GetComponent<UIFollowTarget>();
    }
}