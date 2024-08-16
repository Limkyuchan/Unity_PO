using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject m_vcamEffect;
    [SerializeField]
    GameObject m_vcamBack;

    [SerializeField]
    float m_speed = 5f;
    float m_scale;
    Vector3 m_dir;


    void Start()
    {
        m_vcamEffect.SetActive(false);
        m_vcamBack.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            m_vcamBack.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            m_vcamBack.SetActive(false);
        }

        m_dir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        if (m_dir != Vector3.zero)
        {
            if (m_scale < 1f)
            {
                m_scale += Time.deltaTime / 2f;
            }
            else
            {
                m_scale = 1f;
            }
        }
        else
        {
            if (m_scale > 0f)
            {
                m_scale -= Time.deltaTime * 1.5f;
            }
            else
            {
                m_scale = 0f;
            }
        }

        gameObject.transform.position += m_dir * m_speed * m_scale * Time.deltaTime;
    }
}
