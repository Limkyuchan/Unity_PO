using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    #region Constants and Fields
    [SerializeField]
    GameObject m_virtualCamEffect;
    [SerializeField]
    GameObject m_virtualCamBack;

    NavMeshAgent m_navAgent;

    [SerializeField]
    float m_speed = 5f;
    float m_scale;
    Vector3 m_dir;
    #endregion Constants and Fields




    #region Call by Unity
    void Start()
    {
        m_navAgent = GetComponent<NavMeshAgent>();

        m_virtualCamEffect.SetActive(false);
        m_virtualCamBack.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            m_virtualCamBack.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            m_virtualCamBack.SetActive(false);
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

        if (m_navAgent.enabled)
        {
            m_navAgent.Move(m_dir * m_speed * m_scale * Time.deltaTime);
        }
    }
    #endregion Call by Unity
}
