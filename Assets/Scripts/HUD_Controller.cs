using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Controller : MonoBehaviour
{
    [SerializeField]
    Slider m_healthSlider;
    [SerializeField]
    Text m_iD;
    //https://www.youtube.com/watch?v=ip0xffLSWlk

    //UIFollowTarget m_followTarget;
    //HUDText[] m_hudText;
    //[SerializeField]
    //UIProgressBar m_hpBar;
    //[SerializeField]
    //UILabel m_id;
    //[SerializeField]
    //float m_duration = 3f;
    //StringBuilder m_sb = new StringBuilder();

    //public void SetHUD(Transform target)
    //{
    //    m_followTarget.target = target;
    //    m_hpBar.value = 1f;
    //    HideUI();
    //}

    //public void UpdateHUD(DamageType type, float damage, float normalizedHp)
    //{
    //    ShowUI();
    //    if (IsInvoking("HideUI"))
    //    {
    //        CancelInvoke("HideUI");
    //    }
    //    Invoke("HideUI", m_duration);

    //    m_hpBar.value = normalizedHp;

    //    switch (type)
    //    {
    //        case DamageType.Normal:
    //            m_sb.Append(Mathf.RoundToInt(damage));
    //            m_hudText[0].Add(m_sb.ToString(), Color.red, 0f);
    //            m_sb.Clear();
    //            break;
    //        case DamageType.Critical:
    //            m_sb.Append(Mathf.RoundToInt(damage));
    //            m_hudText[1].Add(m_sb.ToString(), Color.white, 1f);
    //            m_sb.Clear();
    //            break;
    //        case DamageType.Miss:
    //            m_sb.Append("Miss!");
    //            m_hudText[2].Add(m_sb.ToString(), Color.yellow, 1f);
    //            m_sb.Clear();
    //            break;
    //    }
    //}

    //void ShowUI()
    //{
    //    gameObject.SetActive(true);
    //}

    //void HideUI()
    //{
    //    gameObject.SetActive(false);
    //}

    //void Awake()
    //{
    //    m_followTarget = GetComponent<UIFollowTarget>();
    //    m_hudText = GetComponentsInChildren<HUDText>();

    //    m_followTarget.gameCamera = Camera.main;
    //    m_followTarget.uiCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
    //}
}