using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Constants and Fields
    AttackAreaUnitFind[] m_attackAreas;
    CharacterController m_charCtrl;
    PlayerAnimController m_animCtrl;
    SkillController m_skillCtrl;

    [Header("게임 정보")]
    [SerializeField]
    InformationMessage m_introduceGame;

    [Header("카메라 관련 정보")]
    [SerializeField]
    GameObject m_virtualCamEffect;
    [SerializeField]
    GameObject m_virtualCamRun;
    [SerializeField]
    float mouseSensitivity = 100f;

    [Header("Player 관련 정보")]
    [SerializeField]
    StatusData m_statusData;
    [SerializeField]
    GameObject m_attackAreaObj;
    [SerializeField]
    HUD_Controller m_playerHUD;
    [SerializeField]
    SkillGauge_Controller m_playerSkillGauge;
    [SerializeField]
    FillAmount m_skillZCoolTime;
    [SerializeField]
    FillAmount m_skillXCoolTime;
    [SerializeField]
    float m_damageToActiveSkill = 100f;
    [SerializeField]
    float m_speed = 1.5f;
    [SerializeField]
    float m_scale;

    bool m_isSkillActive;
    bool m_isSkillCanUse;
    bool m_isPlayerDead;

    int m_deathEnemyCnt;
    float m_currentHp;
    float m_maxHp;
    float m_attack;
    float m_defense;
    float m_curSkillGauge = 0f;
    int hash_Speed;
    Vector3 m_dir;
    List<GameObject> m_enemyList = new List<GameObject>();
    #endregion Constants and Fields

    #region Public Properties
    PlayerAnimController.Motion GetMotion { get { return m_animCtrl.GetMotion; } }

    public float GetPlayerCurHp { get { return m_currentHp; } }

    public float GetPlayerMaxHp {  get { return m_maxHp; } }

    public float GetPlayerAttack {  get { return m_attack; } }

    public float GetPlayerDefense {  get { return m_defense; } }

    public int DeathEnemyCnt { get { return m_deathEnemyCnt; } set { m_deathEnemyCnt = value; } }
    #endregion Public Properties

    #region Public Methods
    public void SetDamage(float damage)
    {
        m_currentHp -= Mathf.RoundToInt(damage);
        var type = DamageType.None;
        m_playerHUD.UpdateHUD(type, damage, m_currentHp / (float)m_maxHp);

        m_animCtrl.Play(PlayerAnimController.Motion.Hit, false);
        m_virtualCamEffect.SetActive(true);

        if (m_currentHp <= 0)
        {
            Die();
        }
    }

    public void PlayerAttackUpgrade()
    {
        m_attack += 2;
    }

    public bool IsAttack
    {
        get
        {
            return GetMotion == PlayerAnimController.Motion.Attack1 ||
                GetMotion == PlayerAnimController.Motion.Attack2 ||
                GetMotion == PlayerAnimController.Motion.Attack3 ||
                GetMotion == PlayerAnimController.Motion.Attack4;
        }
    }

    public bool IsSkill
    {
        get
        {
            return m_isSkillActive ||
                   GetMotion == PlayerAnimController.Motion.Skill1 ||
                   GetMotion == PlayerAnimController.Motion.Skill2;
        }
    }
    #endregion Public Methods

    #region Animation Event Methods
    void AnimEvent_Attack()
    {
        var skill = SkillTable.Instance.GetSkillData(GetMotion);
        var unitList = m_attackAreas[skill.attackArea].EnemyUnitList;
        var effectData = EffectTable.Instance.GetData(skill.effectId);

        if (skill.attack == 0) return;      // skill Data 못 가져오면 return
        m_enemyList.Clear();
        foreach (var unit in unitList)      // unitList에 적 추가
        {
            if (unit != null)
            {
                m_enemyList.Add(unit);
            }
        }

        DamageType type = DamageType.Miss;
        float damage = 0f;

        for (int i = m_enemyList.Count - 1; i >= 0; i--)
        {
            var enemyObj = m_enemyList[i];
            if (enemyObj == null)
            {
                m_enemyList.RemoveAt(i);
                continue;
            }

            var enemy = m_enemyList[i].GetComponent<EnemyController>();
            if (enemy == null) continue;

            var status = StatusTable.Instance.GetStatusData(enemy.Type);
            type = AttackDecision(enemy, skill, status, out damage);
            enemy.SetDamage(skill, type, damage);

            if (type != DamageType.Miss)
            {
                // 공격 데미지에 따른 Z스킬 게이지 계산
                m_curSkillGauge += damage;
                m_playerSkillGauge.UpdateGauge(m_curSkillGauge / m_damageToActiveSkill);
                if (m_curSkillGauge >= m_damageToActiveSkill)
                {
                    EnableSkill();
                }

                // 공격 이팩트 효과 적용
                var effect = EffectPool.Instance.Create(effectData.Prefabs[type == DamageType.Normal ? 0 : 1]);
                effect.transform.position = enemy.transform.position + Vector3.up * 0.6f;
                var dir = transform.position - effect.transform.position;
                dir.y = 0f;
                effect.transform.rotation = Quaternion.FromToRotation(effect.transform.forward, dir.normalized);
            }
        }
    }

    void AnimEvent_AttackFinished()
    {
        bool isCombo = false;
        if (m_skillCtrl.CommandCount > 0)           // 기본 공격을 활용한 콤보 공격 구현
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

    void AnimEvent_SkillFinished()
    {
        m_isSkillActive = false;
        m_animCtrl.Play(PlayerAnimController.Motion.Idle);
    }

    void AnimEvent_HitFinished()
    {
        ResetMove();
        m_virtualCamEffect.SetActive(false);
        m_animCtrl.Play(PlayerAnimController.Motion.Idle);
    }
    #endregion Animation Event Methods

    #region Coroutine Methods
    IEnumerator CoShowGameOverPopup()
    {
        yield return Utility.GetWaitForSeconds(1.5f);

        PopupManager.Instance.Popup_OpenOkCancel("<color=#ff0000>GameOver!</color>",
            "플레이어가 사망하여 게임이 종료되었습니다. \r\n" +
            "\"확인\" 클릭 시 타이틀 화면으로 이동합니다. \r\n" +
            "\"종료\" 클릭 시 게임을 종료합니다.", () =>
            {
                LoadSceneManager.Instance.LoadSceneAsync(SceneState.Title);
                PopupManager.Instance.Popup_Close();
            }, () =>
            {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }, "확인", "종료");
    }
    #endregion Coroutine Methods

    #region Methods
    DamageType AttackDecision(EnemyController enemy, SkillData skill, StatusData status, out float damage)
    {
        DamageType type = DamageType.Miss;
        damage = 0f;

        if (CalculateDamage.AttackDecision(m_statusData.hitRate + skill.hitRate, status.dodgeRate))
        {
            type = DamageType.Normal;
            damage = CalculateDamage.NormalDamage(m_statusData.attack, skill.attack, status.defense);

            if (CalculateDamage.CriticalDecision(m_statusData.criRate))
            {
                type = DamageType.Critical;
                damage = CalculateDamage.CriticalDamage(damage, m_statusData.criAttack);
            }
        }
        return type;
    }

    void Die()
    {
        m_isPlayerDead = true;
        m_animCtrl.Play(PlayerAnimController.Motion.Death);
        m_virtualCamEffect.SetActive(false);
        StartCoroutine(CoShowGameOverPopup());
    }

    void EnableSkill()
    {
        m_isSkillCanUse = true;
        m_skillZCoolTime.SetSkillShadow(false);
    }

    void ResetSkillGauge()
    {
        m_isSkillCanUse = false;
        m_skillZCoolTime.SetSkillShadow(true);
        m_curSkillGauge = 0;
        m_playerSkillGauge.UpdateGauge(m_curSkillGauge);
    }

    void ResetMove()
    {
        m_scale = 0f;
        m_animCtrl.SetFloat(hash_Speed, m_scale);
    }

    void RotateCamera()
    {
        if (!m_isPlayerDead)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            transform.Rotate(Vector3.up * mouseX);
        }
    }

    void MouseCursorControl()
    {
        if (PopupManager.Instance.IsPopupOpened)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    #endregion Methods

    #region Unity Methods
    void Start()
    {
        m_animCtrl = GetComponent<PlayerAnimController>();
        m_skillCtrl = GetComponent<SkillController>();
        m_charCtrl = GetComponent<CharacterController>();
        m_attackAreas = m_attackAreaObj.GetComponentsInChildren<AttackAreaUnitFind>();

        hash_Speed = Animator.StringToHash("Speed");
        m_virtualCamEffect.SetActive(false);
        m_virtualCamRun.SetActive(false);
        m_skillZCoolTime.SetSkillShadow(true);

        m_isSkillActive = false;
        m_isSkillCanUse = false;
        m_isPlayerDead = false;

        m_currentHp = m_statusData.hp;
        m_maxHp = m_statusData.hpMax;
        m_attack = m_statusData.attack;
        m_defense = m_statusData.defense;
    }

    void Update()
    {
        MouseCursorControl();
        RotateCamera();

        // 게임 정보 확인하기
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (!PopupManager.Instance.IsPopupOpened)
            {
                m_introduceGame.IntroduceHowToPlayGame();
            }
        }

        // 주인공 스탯 확인하기
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!PopupManager.Instance.IsPopupOpened)
            {
                m_introduceGame.CheckPlayerStat();
            }
        }

        if (Input.GetKeyDown(KeyCode.Z) && !m_isSkillActive && m_isSkillCanUse)
        {
            m_isSkillActive = true;
            m_animCtrl.Play(PlayerAnimController.Motion.Skill1, false);
            m_virtualCamEffect.SetActive(false);
            ResetSkillGauge();
            ResetMove();
        }
        if (Input.GetKeyDown(KeyCode.X) && !m_isSkillActive && !m_skillXCoolTime.IsSkillCoolTime)
        {
            m_isSkillActive = true;
            m_animCtrl.Play(PlayerAnimController.Motion.Skill2, false);
            m_virtualCamEffect.SetActive(false);
            m_skillXCoolTime.StartCoolTime(30f);
            ResetMove();
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

        // 주인공 방향전환 
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        m_dir = (forward * Input.GetAxis("Vertical")) + (right * Input.GetAxis("Horizontal"));

        if (m_dir != Vector3.zero && !IsAttack && !IsSkill)
        {
            // 주인공 회전속도 조절
            Quaternion targetRotation = Quaternion.LookRotation(m_dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);

            if (m_scale < 1f)
            {
                m_scale += Time.deltaTime / 2f;
            }
            else
            {
                m_scale = 1f;
            }
        }
        else if (!IsAttack && !IsSkill)
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

        // 주인공 이동속도, 카메라 조절
        if (Input.GetKeyDown(KeyCode.LeftShift) && m_dir != Vector3.zero)
        {
            m_virtualCamRun.SetActive(true);
            m_speed = 3f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && m_dir != Vector3.zero)
        {
            m_virtualCamRun.SetActive(false);
            m_speed = 1.5f;
        }

        m_animCtrl.SetFloat(hash_Speed, m_scale);
        if (m_charCtrl.enabled && !IsAttack && !IsSkill)
        {
            m_charCtrl.Move(m_dir * m_speed * m_scale * Time.deltaTime);
        }
    }
    #endregion Unity Methods
}