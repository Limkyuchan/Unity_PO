using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillGauge_Controller : MonoBehaviour
{
    [SerializeField]
    Slider m_skillGaugeBar;

    public void UpdateGauge(float gauge)
    {
        m_skillGaugeBar.value = gauge;
    }

    void Start()
    {
        m_skillGaugeBar.value = 0;
    }
}