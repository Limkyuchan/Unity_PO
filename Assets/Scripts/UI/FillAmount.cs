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
    //[SerializeField]
    //Button m_button;
    [SerializeField]
    Image m_imgSkill;
    [SerializeField]
    TextMeshProUGUI m_textCoolTime;
    [SerializeField]
    float m_coolTime = 7f;

    bool m_isSkillCoolTime;
    Coroutine m_coCoolTime;

    public bool IsSkillCoolTime { get { return m_isSkillCoolTime;} }

    public void Init()
    {
        m_skillCoolTime.gameObject.SetActive(false);
        m_imgSkill.fillAmount = 0f;
        m_isSkillCoolTime = false;
    }

    public void StartCoolTime()
    {
        if (m_coCoolTime == null)
        {
            m_coCoolTime = StartCoroutine(CoCoolTime());
        }
    }

    IEnumerator CoCoolTime()
    {
        m_isSkillCoolTime = true;
        m_skillCoolTime.gameObject.SetActive(true);
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

    void Start()
    {
        //m_button = GetComponent<Button>();
        //m_button.onClick.AddListener(() => 
        //{
        //    if (m_coCoolTime != null)
        //    {
        //        Debug.Log("쿨타임 중입니다..");
        //    }
        //    else
        //    {
        //        m_coCoolTime = StartCoroutine(CoCoolTime());
        //    }
        //});

        Init();
    }
}
