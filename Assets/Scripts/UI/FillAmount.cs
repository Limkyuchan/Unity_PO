using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FillAmount : MonoBehaviour
{
    [SerializeField]
    GameObject m_skillCoolTime;
    [SerializeField]
    Image m_imgSkill;
    [SerializeField]
    TextMeshProUGUI m_textCoolTime;
    [SerializeField]
    float m_coolTime;
    [SerializeField]
    GameObject m_skillShadow;

    bool m_isSkillCoolTime;
    Coroutine m_coCoolTime;

    public bool IsSkillCoolTime { get { return m_isSkillCoolTime; } }

    public void SetSkillShadow(bool result)
    {
        m_skillCoolTime.SetActive(false);
        m_skillShadow.SetActive(result);
    }

    public void Init()
    {
        m_skillCoolTime.gameObject.SetActive(false);
        m_imgSkill.fillAmount = 0f;
        m_isSkillCoolTime = false;
        m_skillShadow.SetActive(false);
    }

    public void StartCoolTime(float coolTimeDuration)
    {
        if (m_coCoolTime == null)
        {
            m_skillShadow.SetActive(false);
            m_coolTime = coolTimeDuration;
            m_coCoolTime = StartCoroutine(CoCoolTime());
        }
    }

    IEnumerator CoCoolTime()
    {
        m_isSkillCoolTime = true;
        m_skillCoolTime.gameObject.SetActive(true);
        m_skillShadow.SetActive(false);
        var time = m_coolTime;

        while (true)
        {
            time -= Time.deltaTime;
            m_textCoolTime.text = Mathf.FloorToInt(time).ToString();

            var per = time / m_coolTime;
            m_imgSkill.fillAmount = per;

            if (time <= 0)
            {
                m_skillCoolTime.gameObject.SetActive(false);
                m_isSkillCoolTime = false;
                break;
            }
            yield return null;
        }

        m_coCoolTime = null;
        Init();
    }

    void Awake()
    {
        Init();
    }
}