using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : CharacterBase
{
    #region Enum Methods
    public enum Type
    {
        Warrior,
        Range
    }
    #endregion Enum Methods

    #region Constants and Fields
    CharacterController m_charCtrl;
    PlayerAnimController m_animCtrl;
    SkillController m_skillCtrl;
    IAttackStrategy m_attackStrategy;

    [Header("게임 정보")]
    [SerializeField]
    UIGameInformationMessage m_introduceGame;
    [SerializeField]
    PauseManager m_pauseManager;

    [Header("카메라 정보")]
    [SerializeField]
    CameraController m_cameraController;
    [SerializeField]
    Transform m_warriorCameraRoot;
    [SerializeField]
    Transform m_rangeCameraRoot;
    [SerializeField]
    GameObject m_virtualCamEffect;
    [SerializeField]
    GameObject m_virtualShield;
    [SerializeField]
    GameObject m_virtualCamRun;
    [SerializeField]
    float mouseSensitivity = 100f;

    [Header("UI 정보")]
    [SerializeField]
    IndicatorManager m_indicator;
    [SerializeField]
    UIFollowTarget m_followTarget;
    [SerializeField]
    UIPlayerStat m_playerStat;
    [SerializeField]
    TextMeshProUGUI m_playerNameText;
    [SerializeField]
    HUD_Controller m_playerHUD;
    [SerializeField]
    UISkillGauge_Controller m_playerSkillGauge;
    [SerializeField]
    ToggleCameraShake m_toggleCameraShake;
    [SerializeField]
    FillAmount m_skillZCoolTime;
    [SerializeField]
    UIChangeImage m_skillZImage;
    [SerializeField]
    FillAmount m_skillXCoolTime;
    [SerializeField]
    UIChangeImage m_skillXImage;
    [SerializeField]
    float m_damageToActiveSkill = 100f;

    [Header("Player 정보")]
    [SerializeField]
    Type m_playerType;
    [SerializeField]
    GameObject m_playerWarrior;
    [SerializeField]
    GameObject m_playerRange;
    [SerializeField]
    GameObject m_weaponAxe;
    [SerializeField]
    GameObject m_weaponSword;
    [SerializeField]
    GameObject m_weaponRedFlare;
    [SerializeField]
    GameObject m_weaponBlueBolt;
    [SerializeField]
    float m_movementSpeed;
    [SerializeField]
    float m_scale;

    Vector3 m_dir;
    bool m_isCameraShake = true;
    bool m_isSkillActive;
    bool m_isSkillCanUse;
    bool m_isPlayerDead;
    bool m_isPlayerAttack;
    int hash_Speed;
    int m_deathEnemyCnt;
    int m_totalEnemyCnt;
    int m_curHp;
    int m_maxHp;
    float m_curAttack;
    float m_curSkillGauge;
    float m_curSpeed;
    string m_playerCharacterType;
    string m_playerName;
    string m_playerWeapon;

    public Transform Dummy_HUD;
    #endregion Constants and Fields

    #region Public Properties
    public Type GetPlayerType { get { return m_playerType; } set { m_playerType = value; } }

    public PlayerAnimController GetAnimController { get { return m_animCtrl; } }

    public SkillController GetSkillController { get { return m_skillCtrl; } }

    public PlayerAnimController.Motion GetMotion { get { return m_animCtrl.GetMotion; } }

    public UISkillGauge_Controller GetPlayerSkillGauge { get { return m_playerSkillGauge; } }

    public GameObject GetVirtualEffectCam { get { return m_virtualCamEffect; } }

    public bool IsAttack
    {
        get
        {
            return GetMotion == PlayerAnimController.Motion.Attack1 ||
                GetMotion == PlayerAnimController.Motion.Attack2 ||
                GetMotion == PlayerAnimController.Motion.Attack3 ||
                GetMotion == PlayerAnimController.Motion.Attack4 ||
                GetMotion == PlayerAnimController.Motion.RangeAttack;
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

    public bool IsShield { get { return GetMotion == PlayerAnimController.Motion.Shield; } }

    public bool IsSkillActive { get { return m_isSkillActive; } }

    public bool IsPlayerAttack { get { return m_isPlayerAttack; } set { m_isPlayerAttack = value; } }

    public int PlayerCurHp { get { return m_curHp; } set { m_curHp = value; } }

    public float PlayerMaxHp { get { return m_maxHp; } }

    public float PlayerAttack { get { return m_curAttack; } set { m_curAttack = value; } }

    public float PlayerCurSkillGauge { get { return m_curSkillGauge; } set { m_curSkillGauge = value; } }

    public float PlayerMaxSkillGauge { get { return m_damageToActiveSkill; } }

    public int DeathEnemyCnt { get { return m_deathEnemyCnt; } set { m_deathEnemyCnt = value; } }

    public int TotalEnemyCnt { get { return m_totalEnemyCnt; } set { m_totalEnemyCnt = value; } }
    #endregion Public Properties

    #region Public Methods
    public override void SetDamage(float damage)
    {
        if (!IsShield)
        {
            m_curHp -= Mathf.RoundToInt(damage);
            var type = DamageType.None;
            m_playerHUD.UpdateHUD(type, damage, m_curHp / (float)m_maxHp);

            m_animCtrl.Play(PlayerAnimController.Motion.Hit, false);
            
            if (m_isCameraShake)
            {
                m_virtualCamEffect.SetActive(true);
            }
        }
        else
        {
            StartCoroutine(CoShieldCameraControl());
        }

        if (m_curHp <= 0)
        {
            PlayerDie();
        }
    }

    public override void SetDamage(SkillData skill, DamageType type, float damage) { }

    public override Transform GetTransform()
    {
        return this.transform;
    }

    public DamageType AttackDecision(EnemyController enemy, SkillData skill, StatusData status, out float damage)
    {
        DamageType type = DamageType.Miss;
        damage = 0f;

        if (CalculateDamage.AttackDecision(PlayerStatus.Instance.hitRate + skill.hitRate, status.dodgeRate))
        {
            type = DamageType.Normal;
            damage = CalculateDamage.NormalDamage(m_curAttack, skill.attack, status.defense);

            if (CalculateDamage.CriticalDecision(PlayerStatus.Instance.criRate))
            {
                type = DamageType.Critical;
                damage = CalculateDamage.CriticalDamage(damage, PlayerStatus.Instance.criAttack);
            }
        }
        return type;
    }


    public void SetCameraShake(bool isEnabled)
    {
        m_isCameraShake = isEnabled;

        if (!m_isCameraShake)
        {
            m_virtualCamEffect.SetActive(false);
            m_virtualShield.SetActive(false);
        }

        Debug.Log($"카메라 진동 설정: {(m_isCameraShake ? "On" : "Off")}");
    }

    public void AllEnemiesDie()
    {
        if (PlayerStatus.Instance != null)
        {
            PlayerStatus.Instance.UpdateStatus(m_curHp, m_curAttack, m_curSkillGauge);
        }
    }

    public void EnableSkill()
    {
        m_isSkillCanUse = true;
        m_skillZCoolTime.SetSkillShadow(false);
    }

    public void ResetSkillGauge()
    {
        m_isSkillCanUse = false;
        m_skillZCoolTime.SetSkillShadow(true);
        m_curSkillGauge = 0;
        m_playerSkillGauge.UpdateGauge(m_curSkillGauge);
    }

    public void PlayerAttackUpgrade()
    {
        m_curAttack += 2;
    }

    public void PlayerHpUpgrade()
    {
        m_curHp += 20;
        if (m_curHp >= m_maxHp)
        {
            m_curHp = m_maxHp;
        }
        m_playerHUD.UpdateHUD(m_curHp / (float)m_maxHp);
    }

    public void PlayerSkillGaugeUpgrade()
    {
        m_curSkillGauge += 10;
        if (m_curSkillGauge >= m_damageToActiveSkill)
        {
            m_curSkillGauge = m_damageToActiveSkill;
        }
        m_playerSkillGauge.UpdateGauge(m_curSkillGauge / m_damageToActiveSkill);
    }
    #endregion Public Methods

    #region Animation Event Methods
    void AnimEvent_Attack()
    {
        m_attackStrategy.AnimEvent_Attack(this);  
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
        m_isPlayerAttack = false;
        m_virtualCamEffect.SetActive(false);
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

    IEnumerator CoShieldCameraControl()
    {
        if (m_isCameraShake)
        {
            m_virtualShield.SetActive(true);
        }

        yield return Utility.GetWaitForSeconds(1.5f);
        m_virtualShield.SetActive(false);
    }
    #endregion Coroutine Methods

    #region Methods
    void PlayerDie()
    {
        m_isPlayerDead = true;
        m_animCtrl.Play(PlayerAnimController.Motion.Death);
        m_virtualCamEffect.SetActive(false);
        StartCoroutine(CoShowGameOverPopup());
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
    #endregion Methods

    #region Unity Methods
    void Awake()
    {
        Dummy_HUD = Utility.FindChildObject(gameObject, "Dummy_HUD").transform;
    }

    void Start()
    {
        m_animCtrl = GetComponent<PlayerAnimController>();
        m_charCtrl = GetComponent<CharacterController>();

        hash_Speed = Animator.StringToHash("Speed");

        m_indicator.SetPlayer(this);
        m_toggleCameraShake.SetPlayer(this);
        m_playerStat.SetPlayer(this);
        m_skillZImage.SetPlayer(this);
        m_skillXImage.SetPlayer(this);
        m_introduceGame.SetPlayer(this);
        m_virtualCamEffect.SetActive(false);
        m_virtualShield.SetActive(false);
        m_virtualCamRun.SetActive(false);
        m_skillZCoolTime.SetSkillShadow(true);

        m_isPlayerAttack = false;
        m_isSkillActive = false;
        m_isSkillCanUse = false;
        m_isPlayerDead = false;

        m_playerName = PlayerPrefs.GetString("PlayerName", "PlayerName");
        m_playerWeapon = PlayerPrefs.GetString("PlayerWeapon", "PlayerWeapon");
        m_playerCharacterType = PlayerPrefs.GetString("PlayerCharacterType", "PlayerCharacterType");
        if (m_playerCharacterType == "Warrior")
        {
            m_playerType = Type.Warrior;
        }
        else if (m_playerCharacterType == "Range")
        {
            m_playerType = Type.Range;
        }

        switch (m_playerType)
        {
            case Type.Warrior:
                m_playerRange.gameObject.SetActive(false);
                m_playerWarrior.gameObject.SetActive(true);
                m_skillCtrl = GetComponent<SkillController>();
                m_attackStrategy = GetComponent<WarriorAttack>();

                m_cameraController.SetTarget(m_warriorCameraRoot);
                m_followTarget.SetTarget(Dummy_HUD);
                m_movementSpeed = 1.5f;

                PlayerStatus.Instance.InitializeStatus(m_playerName, m_playerWeapon, PlayerStatus.PlayerType.Warrior);
                m_playerNameText.text = PlayerStatus.Instance.playerName;

                if (m_playerWeapon == "Axe")
                {
                    m_weaponAxe.gameObject.SetActive(true);
                    m_weaponSword.gameObject.SetActive(false);
                }
                else if (m_playerWeapon == "Sword")
                {
                    m_weaponSword.gameObject.SetActive(true);
                    m_weaponAxe.gameObject.SetActive(false);
                }
                break;
            case Type.Range:
                m_playerWarrior.gameObject.SetActive(false);
                m_playerRange.gameObject.SetActive(true);
                m_attackStrategy = GetComponent<RangeAttack>();

                m_cameraController.SetTarget(m_rangeCameraRoot);
                m_followTarget.SetTarget(Dummy_HUD);
                m_movementSpeed = 3f;

                PlayerStatus.Instance.InitializeStatus(m_playerName, m_playerWeapon, PlayerStatus.PlayerType.Range);
                m_playerNameText.text = PlayerStatus.Instance.playerName;

                if (m_playerWeapon == "RedFlare")
                {
                    m_weaponRedFlare.gameObject.SetActive(true);
                    m_weaponBlueBolt.gameObject.SetActive(false);
                }
                else if (m_playerWeapon == "BlueBolt")
                {
                    m_weaponRedFlare.gameObject.SetActive(false);
                    m_weaponBlueBolt.gameObject.SetActive(true);
                }
                break;
        }

        m_curSpeed = m_movementSpeed;

        if (PlayerStatus.Instance != null)
        {
            m_curHp = PlayerStatus.Instance.hp;
            m_maxHp = PlayerStatus.Instance.hpMax;
            m_curAttack = PlayerStatus.Instance.attack;
            m_curSkillGauge = PlayerStatus.Instance.skillGauge;
            m_deathEnemyCnt = PlayerStatus.Instance.deathEnemyCnt;
            m_totalEnemyCnt = PlayerStatus.Instance.totalEnemyCnt;
            m_playerHUD.UpdateHUD(m_curHp / (float)m_maxHp);
            m_playerSkillGauge.UpdateGauge(m_curSkillGauge / m_damageToActiveSkill);

            if (m_curSkillGauge >= 100)
            {
                m_skillZCoolTime.SetSkillShadow(false);
                m_isSkillCanUse = true;
            }
        }
    }

    void Update()
    {
        if (m_pauseManager != null && m_pauseManager.IsPaused)
        {
            return;
        }

        RotateCamera();

        // 주인공 기본 공격
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetMove();
            m_attackStrategy.BasicAttack(this);
            m_isPlayerAttack = true;
        }

        // 주인공 스킬 공격
        if (Input.GetKeyDown(KeyCode.Z) && !m_isSkillActive && m_isSkillCanUse)
        {
            m_isSkillActive = true;
            m_virtualCamEffect.SetActive(false);
            m_attackStrategy.SkillAttack_1(this);
            ResetMove();
        }
        if (Input.GetKeyDown(KeyCode.X) && !m_isSkillActive && !m_skillXCoolTime.IsSkillCoolTime)
        {
            m_isSkillActive = true;
            m_virtualCamEffect.SetActive(false);
            m_skillXCoolTime.StartCoolTime(20f);
            m_attackStrategy.SkillAttack_2(this);
            ResetMove();
        }

        // 주인공 쉴드 방어
        if (Input.GetKeyDown(KeyCode.C))
        {
            ResetMove();
            m_virtualCamEffect.SetActive(false);
            m_animCtrl.Play(PlayerAnimController.Motion.Shield);
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            m_virtualCamEffect.SetActive(false);
            m_virtualShield.SetActive(false);
            m_animCtrl.Play(PlayerAnimController.Motion.Idle);
        }

        // 주인공 방향전환 
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        m_dir = (forward * Input.GetAxis("Vertical")) + (right * Input.GetAxis("Horizontal"));

        if (m_dir != Vector3.zero && !IsAttack && !IsSkill && !IsShield)
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
        else if (!IsAttack && !IsSkill && !IsShield)
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
            m_curSpeed = m_movementSpeed * 2f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && m_dir != Vector3.zero)
        {
            m_virtualCamRun.SetActive(false);
            m_curSpeed = m_movementSpeed;
        }

        m_animCtrl.SetFloat(hash_Speed, m_scale);
        if (m_charCtrl.enabled && !IsAttack && !IsSkill && !IsShield)
        {
            m_charCtrl.Move(m_dir * m_curSpeed * m_scale * Time.deltaTime);
        }
    }
    #endregion Unity Methods
}