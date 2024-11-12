using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    //AttackAreaUnitFind[] m_attackAreas;
    CharacterController m_charCtrl;
    PlayerAnimController m_animCtrl;
    SkillController m_skillCtrl;
    IAttackStrategy m_attackStrategy;

    [Header("���� ����")]
    [SerializeField]
    UIGameInformationMessage m_introduceGame;

    [Header("ī�޶� ����")]
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

    [Header("Player ����")]
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
    //[SerializeField]
    //GameObject m_attackAreaObj;
    [SerializeField]
    UIFollowTarget m_followTarget;
    [SerializeField]
    TextMeshProUGUI m_playerNameText;
    [SerializeField]
    HUD_Controller m_playerHUD;
    [SerializeField]
    UISkillGauge_Controller m_playerSkillGauge;
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

    Vector3 m_dir;
    //List<GameObject> m_enemyList = new List<GameObject>();

    bool m_isSkillActive;
    bool m_isSkillCanUse;
    bool m_isPlayerDead;
    int hash_Speed;
    int m_deathEnemyCnt;
    int m_totalEnemyCnt;
    int m_curHp;
    int m_maxHp;
    float m_curAttack;
    float m_curSkillGauge;
    string m_playerName;
    string m_playerWeapon;

    public Transform Dummy_HUD;
    #endregion Constants and Fields

    #region Public Properties
    public Type GetPlayerType { get { return m_playerType; } set { m_playerType = value; } }

    public PlayerAnimController.Motion GetMotion { get { return m_animCtrl.GetMotion; } }

    public UISkillGauge_Controller GetPlayerSkillGauge { get { return m_playerSkillGauge; } }

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
            m_virtualCamEffect.SetActive(true);

            if (m_curHp <= 0)
            {
                PlayerDie();
            }
        }
        else
        {
            StartCoroutine(CoShieldCameraControl());
        }
    }

    public override void SetDamage(SkillData skill, DamageType type, float damage)
    {
        // �� �޼���� ȣ����� ����
        Debug.LogWarning("���ΰ����� SkillData ��� ������ ������ �õ������� �� �޼���� ������ ����.");
    }

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
        m_attackStrategy.Attack(this);  
    }

    void AnimEvent_AttackFinished()
    {
        bool isCombo = false;
        if (m_skillCtrl != null)
        {
            if (m_skillCtrl.CommandCount > 0)           // �⺻ ������ Ȱ���� �޺� ���� ����
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
        else
        {
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
            "�÷��̾ ����Ͽ� ������ ����Ǿ����ϴ�. \r\n" +
            "\"Ȯ��\" Ŭ�� �� Ÿ��Ʋ ȭ������ �̵��մϴ�. \r\n" +
            "\"����\" Ŭ�� �� ������ �����մϴ�.", () =>
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
            }, "Ȯ��", "����");
    }

    IEnumerator CoShieldCameraControl()
    {
        m_virtualShield.SetActive(true);
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
    void Awake()
    {
        Dummy_HUD = Utility.FindChildObject(gameObject, "Dummy_HUD").transform;
    }

    void Start()
    {
        m_animCtrl = GetComponent<PlayerAnimController>();
        m_charCtrl = GetComponent<CharacterController>();
        //m_attackAreas = m_attackAreaObj.GetComponentsInChildren<AttackAreaUnitFind>();

        hash_Speed = Animator.StringToHash("Speed");
        m_virtualCamEffect.SetActive(false);
        m_virtualShield.SetActive(false);
        m_virtualCamRun.SetActive(false);
        m_skillZCoolTime.SetSkillShadow(true);
 
        m_isSkillActive = false;
        m_isSkillCanUse = false;
        m_isPlayerDead = false;

        switch (m_playerType)
        {
            case Type.Warrior:
                m_playerRange.gameObject.SetActive(false);

                m_skillCtrl = GetComponent<SkillController>();
                m_attackStrategy = GetComponent<WarriorAttack>();

                m_cameraController.SetTarget(m_warriorCameraRoot);
                m_followTarget.SetTarget(Dummy_HUD);

                m_playerName = PlayerPrefs.GetString("PlayerName", "PlayerName");
                m_playerWeapon = PlayerPrefs.GetString("PlayerWeapon", "PlayerWeapon");
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
                m_attackStrategy = GetComponent<RangeAttack>();

                m_cameraController.SetTarget(m_rangeCameraRoot);
                m_followTarget.SetTarget(Dummy_HUD);
                break;
        }

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
        MouseCursorControl();
        RotateCamera();

        // ���� ���� Ȯ���ϱ�
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (!PopupManager.Instance.IsPopupOpened)
            {
                m_introduceGame.IntroduceHowToPlayGame();
            }
        }

        // ���ΰ� ���� Ȯ���ϱ�
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!PopupManager.Instance.IsPopupOpened)
            {
                m_introduceGame.CheckPlayerStat();
            }
        }

        // ���ΰ� ��ų ����
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

        // ���ΰ� �⺻ ����
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

        if (Input.GetKeyDown(KeyCode.L))
        {
            m_animCtrl.Play(PlayerAnimController.Motion.RangeAttack);
        }

        // ���ΰ� ���� ���
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

        // ���ΰ� ������ȯ 
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        m_dir = (forward * Input.GetAxis("Vertical")) + (right * Input.GetAxis("Horizontal"));

        if (m_dir != Vector3.zero && !IsAttack && !IsSkill && !IsShield)
        {
            // ���ΰ� ȸ���ӵ� ����
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

        // ���ΰ� �̵��ӵ�, ī�޶� ����
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
        if (m_charCtrl.enabled && !IsAttack && !IsSkill && !IsShield)
        {
            m_charCtrl.Move(m_dir * m_speed * m_scale * Time.deltaTime);
        }
    }
    #endregion Unity Methods
}