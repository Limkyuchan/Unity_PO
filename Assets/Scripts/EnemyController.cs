using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    #region Enum Methods
    public enum AiState
    {
        Idle,
        Attack,
        Chase,
        Patrol,
        Jump,
        Damaged,
        Death,
        Max
    }
    #endregion

    #region Constants and Fields
    PlayerController m_player;
    EnemyAnimController m_animCtrl;
    HittedFeedback m_hittedFeedback;
    AttackAreaUnitFind m_attackArea;
    NavMeshAgent m_navAgent;
    PathController m_path;
    HUD_Controller m_hudCtrl;
    GameObject m_rangeAttackEffect;
    Transform m_dummyFire;
    EnemyManager.EnemyType m_enemyType;
    IMovementStrategy m_movementStrategy;
    IAttackStrategy m_attackStrategy;

    [Header("Enemy 관련 정보")]
    [SerializeField] 
    AiState m_state;
    [SerializeField]
    GameObject m_attackAreaObj;
    [SerializeField]
    int m_maxHp;                    // 생성된 적 최대 Hp
    [SerializeField]
    int m_currentHp;                // 생성된 적 현재 Hp
    [SerializeField]
    float m_maxChaseDistance = 15f; // Chase 상태에서 플레이어를 추적할 최대 거리
    [SerializeField]
    float m_idleDuration = 5f;      // Idle 상태 유지시간
    [SerializeField]
    float m_idleTime;               // Idle 상태 진입 후의 시간 

    bool m_isChase;                 // AiState 현재 상태 확인
    bool m_isPatrol;                // AiState 현재 상태 확인
    bool m_isEnemyAttack;
    bool m_isInvokeJumpPatrolMove;  // Invoke가 호출된 상태인지 확인
    bool m_isEnemyPatrol;           // Patrol 중인지 확인
    int m_curWaypoint;              // 현재 지정되어 있는 Waypoint
    int m_curWaypointIndex;
    int m_playerLayer;
    int m_backgroundLayer;

    Coroutine m_coChaseTarget;
    Coroutine m_coSearchTarget;
    Coroutine m_coDestroy;
    public Transform Dummy_HUD;
    #endregion Constants and Fields
  
    #region Public Properties
    public AiState GetMotion { get { return m_state; } }

    public EnemyManager.EnemyType Type { get { return m_enemyType; } set { m_enemyType = value; } }

    public StatusData GetStatus
    {
        get
        {
            var status = StatusTable.Instance.GetStatusData(this.Type);
            return status;
        }
    }

    public PlayerController GetPlayer { get { return m_player; } }

    public NavMeshAgent GetNavMeshAgent { get { return m_navAgent; } }

    public EnemyAnimController GetAnimator { get { return m_animCtrl; } }

    public AttackAreaUnitFind GetUnitFind { get { return m_attackArea; } }

    public GameObject GetRangeAttackEffect { get { return m_rangeAttackEffect; } }

    public Transform GetDummyFire { get { return m_dummyFire; } }

    public float GetAttackDist { get { return GetStatus.attackDist; } }

    public bool IsChase { get { return m_isChase; } set { m_isChase = value; } }

    public bool IsPatrol { get { return m_isPatrol; } set { m_isPatrol = value; } }

    public bool IsEnemyAttack { get { return m_isEnemyAttack; } set { m_isEnemyAttack = value; } }
    #endregion Public Properties

    #region Public Methods
    public void SetEnemy(PathController path, HUD_Controller hud, int waypointIndex)
    {
        m_path = path;
        m_hudCtrl = hud;
        m_curWaypointIndex = waypointIndex;
        transform.position = m_path.Points[m_curWaypointIndex];
    }

    public void SetDamage(SkillData skill, DamageType type, float damage)
    {
        m_currentHp -= Mathf.RoundToInt(damage);
        Debug.Log("적 체력: " + m_currentHp);
        m_hudCtrl.UpdateHUD(type, damage, m_currentHp / (float)m_maxHp);

        if (type == DamageType.Miss) return;
        SetState(AiState.Damaged);
        m_animCtrl.Play(EnemyAnimController.Motion.Hit, false);

        Vector3 from = transform.position;
        Vector3 dir = transform.position - m_player.transform.position;
        dir.y = 0f;
        Vector3 to = from + dir.normalized * skill.knockback;
        float duration = skill.knockbackDuration;
        m_hittedFeedback.Play(from, to, duration);

        if (m_currentHp <= 0)
        {
            SetState(AiState.Death);
            m_animCtrl.Play(EnemyAnimController.Motion.Death);
            EnemyManager.Instance.RemoveEnemy(this);

            if (m_coDestroy != null)
            {
                StopCoroutine(m_coDestroy);
            }
            m_coDestroy = StartCoroutine(CoDestroyGameObject(3f));
        }
    }

    public void SetState(AiState state)
    {
        m_state = state;
    }

    public void InitPlayer(PlayerController player)
    {
        m_player = player;
    }

    public bool CheckArea(Transform target, float distance)
    {
        var dir = target.position - transform.position;
        if (dir.sqrMagnitude <= distance * distance)
        {
            return true;
        }
        return false;
    }
    #endregion Public Methods

    #region Animation Event Methods
    void AnimEvent_Attack()
    {
        if (!m_isEnemyAttack)
        {
            m_isEnemyAttack = true;
            m_attackStrategy.Attack(this);
        }
    }

    void AnimEvent_AttackFinished()
    {
        m_isEnemyAttack = false;
        SetIdle(1.5f);
    }

    void AnimEvent_HitFinished()
    {
        SetIdle(1.5f);
    }

    void AnimEvent_JumpFinished()
    {
        SetState(AiState.Jump);
    }
    #endregion Animation Event Methods

    #region Coroutine Methods
    IEnumerator CoChaseToTarget(Transform target, int frame)
    {
        while (m_state == AiState.Chase)
        {
            m_navAgent.SetDestination(target.position);
            for (int i = 0; i < frame; i++)
            {
                yield return null;
            }
        }
    }

    IEnumerator CoSearchTarget(Transform target, float sec)
    {
        while (m_state == AiState.Patrol)
        {
            if (FindTarget(target, GetStatus.detectDist))
            {
                SetIdle(0.5f);
                m_isEnemyPatrol = false;
                yield break;
            }
            yield return Utility.GetWaitForSeconds(sec);
        }
    }

    IEnumerator CoDestroyGameObject(float sec)
    {
        yield return Utility.GetWaitForSeconds(sec);
        Destroy(gameObject);
    }
    #endregion Coroutine Methods

    #region Methods
    bool FindTarget(Transform target, float distance)
    {
        var start = transform.position + Vector3.up * 0.3f;
        var end = target.position + Vector3.up * 0.3f;

        RaycastHit hit;
        if (Physics.Raycast(start, (end - start).normalized, out hit, distance, m_playerLayer | m_backgroundLayer))
        {
            if (hit.transform.CompareTag("Player"))
            {
                Debug.DrawRay(start, (end - start).normalized * hit.distance, Color.magenta, 0.5f);
                return true;
            }
            else
            {
                Debug.DrawRay(start, (end - start).normalized * hit.distance, Color.cyan, 0.5f);
            }
        }
        return false;
    }
    
    void InvokeJumpPatrolMove()
    {
        m_movementStrategy.Move(this);
        SetState(AiState.Jump);
        m_isInvokeJumpPatrolMove = false;
    }

    void SetIdleDuration(float duration)
    {
        m_idleTime = m_idleDuration - duration;
    }

    void SetIdle(float duration)
    {
        m_navAgent.ResetPath();
        SetState(AiState.Idle);
        SetIdleDuration(duration);
        m_animCtrl.Play(EnemyAnimController.Motion.Idle);
    }

    void BehaviourProcess()
    {
        switch (m_state)
        {
            case AiState.Idle:
                if (m_idleTime >= m_idleDuration)
                {
                    m_idleTime = 0f;
                    // 1) 인식 범위 안에 들어오면 => Attack / Chase
                    if (FindTarget(m_player.transform, GetStatus.detectDist))
                    {
                        // 1-1) 공격 범위 안에 들어오면 => Attack
                        if (CheckArea(m_player.transform, GetStatus.attackDist))    
                        {
                            m_isEnemyAttack = false;
                            m_attackStrategy.Attack(this);
                        }
                        // 1-2) 공격 범위 안에 들어오지 않으면 => Chase
                        else
                        {
                            m_isChase = true;
                            m_movementStrategy.Move(this);
                            m_coChaseTarget = StartCoroutine(CoChaseToTarget(m_player.transform, 30));
                        }
                        return;
                    }
                    // 2) 인식 범위 안에 들어오지 않으면 => Patrol
                    else 
                    {
                        m_isPatrol = true;
                        m_movementStrategy.Move(this);
                        if (Type != EnemyManager.EnemyType.WarriorJump)
                        {
                            m_coSearchTarget = StartCoroutine(CoSearchTarget(m_player.transform, 1f));
                        }
                        return;
                    }
                }
                m_idleTime += Time.deltaTime;
                break;
            case AiState.Attack:
                break;
            case AiState.Chase:
                // 1) 추적 중 플레이어와의 거리가 최대 거리를 벗어나면 => Idle
                var dir = m_player.transform.position - transform.position;
                if (dir.sqrMagnitude >= m_maxChaseDistance * m_maxChaseDistance)
                {
                    SetIdle(0.5f);
                    if (m_coChaseTarget != null)
                    {
                        StopCoroutine(m_coChaseTarget);
                    }
                    return;
                }
                // 2) 추적 중 플레이어와의 거리가 가까워지면 => Idle
                if (m_navAgent.remainingDistance <= m_navAgent.stoppingDistance)
                {
                    SetIdle(0.5f);
                    if (m_coChaseTarget != null)
                    {
                        StopCoroutine(m_coChaseTarget);
                    }
                    return;
                }
                break;
            case AiState.Patrol:
                // 1) Patrol 하고 있지 않으면
                if (!m_isEnemyPatrol)    
                {
                    m_coSearchTarget = StartCoroutine(CoSearchTarget(m_player.transform, 1f));
                    m_curWaypoint++;
                    if (m_curWaypoint >= m_path.Points.Length)
                    {
                        m_curWaypoint = 0;
                    }
                    m_navAgent.SetDestination(m_path.Points[m_curWaypoint]);
                    m_isEnemyPatrol = true;
                }
                // 2) Patrol 하고 있다면        
                else
                {
                    if (m_navAgent.remainingDistance <= m_navAgent.stoppingDistance)
                    {
                        m_isEnemyPatrol = false;
                        StopCoroutine(m_coSearchTarget);
                    }
                }
                break;
            case AiState.Jump:
                if (FindTarget(m_player.transform, GetStatus.detectDist))
                {
                    CancelInvoke("InvokeJumpPatrolMove");
                    m_isInvokeJumpPatrolMove = false;
                    m_navAgent.enabled = true;
                    SetState(AiState.Chase);
                }
                else if (!m_isInvokeJumpPatrolMove)
                {
                    Invoke("InvokeJumpPatrolMove", 3f);
                    m_isInvokeJumpPatrolMove = true;
                }
                break;
        }
    }
    #endregion Methods

    #region Unity Methods
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, GetStatus.detectDist);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, GetStatus.attackDist);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_maxChaseDistance);
    }

    void Awake()
    {
        Dummy_HUD = Utility.FindChildObject(gameObject, "Dummy_HUD").transform;
    }

    void Start()
    {
        m_animCtrl = GetComponent<EnemyAnimController>();
        m_navAgent = GetComponent<NavMeshAgent>();
        m_hittedFeedback = GetComponent<HittedFeedback>();
        m_attackArea = m_attackAreaObj.GetComponentInChildren<AttackAreaUnitFind>();
        m_maxHp = StatusTable.Instance.GetStatusData(this.Type).hpMax;
        m_currentHp = StatusTable.Instance.GetStatusData(this.Type).hp;

        m_playerLayer = 1 << LayerMask.NameToLayer("Player");
        m_backgroundLayer = 1 << LayerMask.NameToLayer("Background");
        m_isInvokeJumpPatrolMove = false;
        m_isEnemyAttack = false;

        switch (Type)
        {
            case EnemyManager.EnemyType.MeleeWalk:
            case EnemyManager.EnemyType.MeleeWalk2:
                m_attackStrategy = GetComponent<MeleeAttack>();
                m_movementStrategy = GetComponent<WalkMovement>();
                break;
            case EnemyManager.EnemyType.WarriorJump:
                m_attackStrategy = GetComponent<WarriorAttack>();
                m_movementStrategy = GetComponent<JumpMovement>();
                break;
            case EnemyManager.EnemyType.WarriorWalk:
                m_attackStrategy = GetComponent<WarriorAttack>();
                m_movementStrategy = GetComponent<WalkMovement>();
                break;
            case EnemyManager.EnemyType.MageWalk:
                m_attackStrategy = GetComponent<RangeAttack>();
                m_movementStrategy = GetComponent<WalkMovement>();
                m_dummyFire = Utility.FindChildObject(gameObject, "Dummy_Fire").transform;
                m_rangeAttackEffect = Resources.Load<GameObject>("FX/FX_Fireball_Shooting_Straight");
                break;
        }
    }

    void Update()
    {
        BehaviourProcess();
    }
    #endregion Unity Methods
}