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
    [SerializeField]
    GameObject m_attackAreaObj;

    AttackAreaUnitFind[] m_attackAreas;
    PlayerAnimController m_animCtrl;
    NavMeshAgent m_navAgent;

    [SerializeField]
    float m_speed = 5f;
    float m_scale;
    int hash_Speed;
    Vector3 m_dir;
    #endregion Constants and Fields

    #region Public Properties
    PlayerAnimController.Motion GetMotion { get { return m_animCtrl.GetMotion; } }
    #endregion Public Properties

    #region Public Methods
    public void SetDamage(EnemyController enemy)
    {
        m_animCtrl.Play(PlayerAnimController.Motion.Hit, false);
    }
    #endregion Public Methods

    #region Animation Event Methods
    void AnimEvent_AttackFinished()
    {
        m_animCtrl.Play(PlayerAnimController.Motion.Idle);
    }

    void AnimEvent_Attack()
    {
        var unitList = m_attackAreas[0].EnemyUnitList;
        for (int i = 0; i < unitList.Count; i++)
        {
            var enemy = unitList[i].GetComponent<EnemyController>();
            enemy.SetDamage();
        }
    }

    void AnimEvent_HitFinished()
    {
        m_animCtrl.Play(PlayerAnimController.Motion.Idle);
    }
    #endregion Animation Event Methods

    #region Unity Methods
    void Start()
    {
        m_animCtrl = GetComponent<PlayerAnimController>();
        m_navAgent = GetComponent<NavMeshAgent>();
        m_attackAreas = m_attackAreaObj.GetComponentsInChildren<AttackAreaUnitFind>();

        m_virtualCamEffect.SetActive(false);
        m_virtualCamBack.SetActive(false);

        hash_Speed = Animator.StringToHash("Speed");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            m_virtualCamBack.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            m_virtualCamBack.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GetMotion == PlayerAnimController.Motion.Idle || GetMotion == PlayerAnimController.Motion.Locomotion)
            {
                m_animCtrl.Play(PlayerAnimController.Motion.Attack1);
            }
        }

        m_dir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        if (m_dir != Vector3.zero)
        {
            transform.forward = m_dir;
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

        m_animCtrl.SetFloat(hash_Speed, m_scale);

        if (m_navAgent.enabled)
        {
            m_navAgent.Move(m_dir * m_speed * m_scale * Time.deltaTime);
        }
    }
    #endregion Unity Methods
}
