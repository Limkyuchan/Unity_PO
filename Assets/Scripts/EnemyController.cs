using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

// Melee : AttackDist = 2.5f;
// Zombie : AttackDist = 1.5f;
// Warrior : AttackDist = 2f;
// Mage : AttackDist = 5f;

public class EnemyController : MonoBehaviour
{
    #region Enum Methods
    public enum MovementType
    {
        Walk,
        Roll,
        Jump,
    }

    public enum AttackType
    {
        Melee,
        Range,
    }

    public enum AiState
    {
        Idle,
        Attack,
        Chase,
        Patrol,
        Jump,
        Damaged,
        Max
    }

    public enum PatrolType
    {
        Loop,
        PingPong,
        Random
    }
    #endregion

    #region Constants and Fields
    [SerializeField]
    PlayerController m_player;
    [SerializeField]
    GameObject m_attackAreaObj;
    EnemyAnimController m_animCtrl;
    AttackAreaUnitFind m_attackArea;
    NavMeshAgent m_navAgent;
    MoveTween m_moveTween;
    Transform m_dummyFire;

    [Space(10), Header("AI 관련 정보")]
    [SerializeField]
    MovementType m_movementType;
    IMovementStrategy m_movementStrategy;
    [SerializeField]
    AttackType m_attackType;
    IAttackStrategy m_attackStrategy;
    [SerializeField]
    PathController m_path;
    [SerializeField]
    PatrolType m_patrolType;
    [SerializeField]
    AiState m_state;
    [SerializeField]
    float m_detectDist;
    [SerializeField]
    bool m_isShowDetectArea;
    [SerializeField]
    float m_attackDist;
    [SerializeField]
    bool m_isShowAttackArea;
    [SerializeField]
    float m_maxChaseDistance = 15f; // Chase 상태에서 플레이어를 추적할 최대 거리
    [SerializeField]
    bool m_isShowMaxChaseDistance;
    [SerializeField]
    float m_idleDuration = 5f;      // Idle 상태 유지시간
    [SerializeField]
    float m_idleTime;               // Idle 상태 진입 후의 시간 

    bool m_isChase;                 // AiState 현재 상태 확인
    bool m_isPatrol;                // AiState 현재 상태 확인
    bool m_isEnemyAttack;
    bool m_isInvokeJumpPatrolMove;  // Invoke가 호출된 상태인지 확인
    bool m_isEnemyPatrol;           // Patrol 중인지 확인
    bool m_isReverse;               // Patrol PingPong 정/역주행 확인
    int m_curWaypoint;              // 현재 지정되어 있는 Waypoint
    int m_prevWaypoint;             // 이전 지정되어 있는 Waypoint
    int m_playerLayer;
    int m_backgroundLayer;

    Coroutine m_coChaseTarget;
    Coroutine m_coSearchTarget;
    #endregion Constants and Fields

    #region Public Properties
    public AiState GetMotion { get { return m_state; } }

    public PlayerController GetPlayer { get { return m_player; } }

    public AttackAreaUnitFind GetUnitFind { get { return m_attackArea; } }

    public float GetAttackDist { get { return m_attackDist; } }

    public bool IsChase { get { return m_isChase; } set { m_isChase = value; } }

    public bool IsPatrol { get { return m_isPatrol; } set { m_isPatrol = value; } }

    public bool IsEnemyAttack { get { return m_isEnemyAttack; } set { m_isEnemyAttack = value; } }
    #endregion Public Properties

    #region Public Methods
    public void SetDamage()
    {
        SetState(AiState.Damaged);
        m_animCtrl.Play(EnemyAnimController.Motion.Hit, false);     // Hit 모션 바로 재생(Blend X)

        Vector3 from = transform.position;
        Vector3 dir = transform.position - m_player.transform.position;
        dir.y = 0f;
        Vector3 to = from + dir.normalized * 0.5f;
        float duration = 0.5f;
        m_moveTween.Play(from, to, duration);
    }

    public void SetState(AiState state)
    {
        m_state = state;
    }

    public NavMeshAgent GetNavMeshAgent()
    {
        return m_navAgent;
    }

    public EnemyAnimController GetAnimator()
    {
        return m_animCtrl;
    }
    #endregion Public Methods

    #region Animation Event Methods
    void AnimEvent_HitFinished()
    {
        SetIdle(1.5f);
    }

    void AnimEvent_AttackFinished()
    {
        m_isEnemyAttack = false;
        SetIdle(1.5f);
    }

    void AnimEvent_Attack()
    {
        if (!m_isEnemyAttack)
        {
            m_isEnemyAttack = true;
            m_attackStrategy.Attack(this);
        }
    }

    void AnimEvent_RollFinished()
    {
        SetIdle(2f);
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
            if (FindTarget(target, m_detectDist))
            {
                SetIdle(0.5f);
                m_isEnemyPatrol = false;
                yield break;
            }
            yield return Utility.GetWaitForSeconds(sec);
        }
    }
    #endregion Coroutine Methods

    #region Methods
    bool FindTarget(Transform target, float distance)
    {
        var start = transform.position + Vector3.up * 0.7f;
        var end = target.position + Vector3.up * 0.7f;

        RaycastHit hit;
        if (Physics.Raycast(start, (end - start).normalized, out hit, distance, m_playerLayer | m_backgroundLayer))
        {
            Debug.DrawRay(start, (end - start).normalized * hit.distance, Color.magenta, 0.5f);
            if (hit.transform.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    bool CheckArea(Transform target, float distance)
    {
        var dir = target.position - transform.position;
        if (dir.sqrMagnitude <= distance * distance)
        {
            return true;
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
                    if (FindTarget(m_player.transform, m_detectDist))
                    {
                        // 1-1) 공격 범위 안에 들어오면 => Attack
                        if (CheckArea(m_player.transform, m_attackDist))    
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
                        if (m_movementType == MovementType.Walk || m_movementType == MovementType.Roll)
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
                    StopCoroutine(m_coChaseTarget);
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
                    // 1-1) Patrol 타입이 Loop
                    if (m_patrolType == PatrolType.Loop)            
                    {
                        m_curWaypoint++;
                        if (m_curWaypoint >= m_path.Points.Length)
                        {
                            m_curWaypoint = 0;
                        }
                    }
                    // 1-2) Patrol 타입이 PingPong
                    else if (m_patrolType == PatrolType.PingPong)   
                    {
                        if (!m_isReverse)
                        {
                            m_curWaypoint++;
                            if (m_curWaypoint >= m_path.Points.Length)
                            {
                                m_isReverse = true;
                                m_curWaypoint = m_path.Points.Length - 2;
                            }
                        }
                        else
                        {
                            m_curWaypoint--;
                            if (m_curWaypoint < 0)
                            {
                                m_isReverse = false;
                                m_curWaypoint = 1;
                            }
                        }
                    }
                    // 1-3) Patrol 타입이 Random       
                    else if (m_patrolType == PatrolType.Random)           
                    {
                        int point = 0;
                        do
                        {
                            point = Random.Range(0, m_path.Points.Length);
                        } while (point == m_curWaypoint || point == m_prevWaypoint);
                        m_curWaypoint = point;
                    }
                    m_navAgent.SetDestination(m_path.Points[m_curWaypoint]);
                    m_isEnemyPatrol = true;
                    m_prevWaypoint = m_curWaypoint;
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
                if (FindTarget(m_player.transform, m_detectDist))
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
        if (m_isShowDetectArea)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, m_detectDist);
        }

        if (m_isShowAttackArea)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, m_attackDist);
        }

        if (m_isShowMaxChaseDistance)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_maxChaseDistance);
        }
    }

    void Start()
    {
        m_animCtrl = GetComponent<EnemyAnimController>();
        m_attackArea = m_attackAreaObj.GetComponentInChildren<AttackAreaUnitFind>();
        m_navAgent = GetComponent<NavMeshAgent>();
        m_moveTween = GetComponent<MoveTween>();

        m_playerLayer = 1 << LayerMask.NameToLayer("Player");
        m_backgroundLayer = 1 << LayerMask.NameToLayer("Background");

        m_isInvokeJumpPatrolMove = false;
        m_isEnemyAttack = false;

        switch (m_movementType)
        {
            case MovementType.Walk:
                m_movementStrategy = new WalkMovement();
                m_detectDist = 7f;
                break;
            case MovementType.Roll:
                m_movementStrategy = new RollMovement();
                m_detectDist = 8f;
                break;
            case MovementType.Jump:
                m_movementStrategy = new JumpMovement();
                m_detectDist = 10f;
                break;
        }

        switch (m_attackType)
        {
            case AttackType.Melee:
                m_attackStrategy = new MeleeAttack();
                m_attackDist = 2f;
                break;
            case AttackType.Range:
                m_attackStrategy = new RangeAttack();
                m_attackDist = 5f;
                break;
        }

        // 원거리 공격
        //m_dummyFire = Utility.FindChildObject(gameObject, "Dummy_Fire").transform;
    }

    void Update()
    {
        BehaviourProcess();
    }
    #endregion Unity Methods
}