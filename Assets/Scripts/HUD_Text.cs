using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD_Text : MonoBehaviour
{
    [SerializeField]
    float m_moveSpeed = 10f;
    [SerializeField]
    float m_alphaSpeed = 1f;

    TextMeshProUGUI m_text;
    //Vector3 m_initPos;
    Color m_alpha;

    public void SetText(string text)
    {
        m_text.text = text;
    }

    public void SetColor(Color color)
    {
        m_alpha = color;
    }

    //void ResetPosition()
    //{
    //    transform.position = m_initPos;
    //}

    void ShowData()
    {
        transform.localPosition += new Vector3(0, m_moveSpeed * Time.deltaTime, 0);
        m_alpha.a = Mathf.Lerp(m_alpha.a, 0, Time.deltaTime * m_alphaSpeed);
        m_text.color = m_alpha;

        //Invoke("ResetPosition", 2f);
    }

    void Start()
    {
        m_text = GetComponent<TextMeshProUGUI>();

        //m_initPos = transform.position;
        m_alpha = m_text.color;
        m_alpha.a = 1;
    }

    void Update()
    {
        ShowData();
    }
}