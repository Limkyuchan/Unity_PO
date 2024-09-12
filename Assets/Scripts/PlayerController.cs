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

    AttackAreaUnitFind[] m_attackAreas;
    PlayerAnimController m_animCtrl;
    SkillController m_skillCtrl;
    CharacterController m_charCtrl;
    //NavMeshAgent m_navAgent;

    [Header("Player 관련 정보")]
    [SerializeField]
    GameObject m_attackAreaObj;
    [SerializeField]
    float m_speed = 0f;
    [SerializeField]
    float m_scale;

    int hash_Speed;
    Vector3 m_dir;
    #endregion Constants and Fields

    #region Public Properties
    PlayerAnimController.Motion GetMotion { get { return m_animCtrl.GetMotion; } }
    #endregion Public Properties

    #region Public Methods
    public void SetDamage()
    {
        m_animCtrl.Play(PlayerAnimController.Motion.Hit, false);
    }

    public bool IsAttack
    {
        get
        {
            if (GetMotion == PlayerAnimController.Motion.Attack1 ||
                GetMotion == PlayerAnimController.Motion.Attack2 ||
                GetMotion == PlayerAnimController.Motion.Attack3 ||
                GetMotion == PlayerAnimController.Motion.Attack4)
                return true;
            return false;
        }

    }
    #endregion Public Methods

    #region Animation Event Methods
    void AnimEvent_AttackFinished()
    {
        bool isCombo = false;
        if (m_skillCtrl.CommandCount > 0)
        {
            var command = m_skillCtrl.GetCommand();
            if (command == KeyCode.Space)
            {
                isCombo = true;
            }

            if (m_skillCtrl.CommandCount > 0)
            {
                m_skillCtrl.ReleaseKeyBuffer();
                isCombo = false;
            }
        }

        if (isCombo)
        {
            m_animCtrl.Play(m_skillCtrl.GetCombo());
        }
        else
        {
            m_skillCtrl.ResetCombo();
            m_animCtrl.Play(PlayerAnimController.Motion.Idle);
        }
    }

    void AnimEvent_Attack()
    {
        var skill = SkillTable.Instance.GetSkillData(GetMotion);
        var unitList = m_attackAreas[skill.attackArea].EnemyUnitList;
        for (int i = 0; i < unitList.Count; i++)
        {
            var enemy = unitList[i].GetComponent<EnemyController>();
            enemy.SetDamage(skill);
        }
    }

    void AnimEvent_HitFinished()
    {
        ResetMove();
        m_animCtrl.Play(PlayerAnimController.Motion.Idle);
    }
    #endregion Animation Event Methods

    #region Methods
    void ResetMove()
    {
        //m_navAgent.ResetPath();
        m_scale = 0f;
        m_animCtrl.SetFloat(hash_Speed, m_scale);
    }
    #endregion Methods

    #region Unity Methods
    void Start()
    {
        m_animCtrl = GetComponent<PlayerAnimController>();
        m_skillCtrl = GetComponent<SkillController>();
        m_charCtrl = GetComponent<CharacterController>();
        //m_navAgent = GetComponent<NavMeshAgent>();
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
            ResetMove();
            if (GetMotion == PlayerAnimController.Motion.Idle || GetMotion == PlayerAnimController.Motion.Locomotion)
            {
                m_animCtrl.Play(PlayerAnimController.Motion.Attack1);
            }
            else
            {
                m_skillCtrl.AddCommand(KeyCode.Space);
            }
        }

        m_dir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        if (m_dir != Vector3.zero && !IsAttack)
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
        else if (!IsAttack)
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
        if (m_charCtrl.enabled && !IsAttack)
        {
            m_charCtrl.Move(m_dir * m_speed * m_scale * Time.deltaTime);
        }
        //if (m_navAgent.enabled && !IsAttack)
        //{
        //    m_navAgent.Move(m_dir * m_speed * m_scale * Time.deltaTime);
        //}
    }
    #endregion Unity Methods
}