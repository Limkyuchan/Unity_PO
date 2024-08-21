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
    NavMeshAgent m_navAgent;

    [Space(10), Header("AI ���� ����")]
    [SerializeField]
    PathController m_path;
    [SerializeField]
    PatrolType m_patrolType;
    [SerializeField]
    AiState m_state;
    [SerializeField]
    float m_detectDist = 7f;
    [SerializeField]
    bool m_isShowDetectArea;
    [SerializeField]
    float m_attackDist = 3f;
    [SerializeField]
    bool m_isShowAttackArea;
    [SerializeField]
    float m_maxChaseDistance = 15f; // Chase ���¿��� �÷��̾ ������ �ִ� �Ÿ�
    [SerializeField]
    bool m_isShowMaxChaseDistance;
    [SerializeField]
    float m_idleDuration = 5f;      // Idle ���� �����ð�
    [SerializeField]
    float m_idleTime;               // Idle ���� ���� ���� �ð� 
    
    bool m_isPatrol;                // Patrol ������ Ȯ��
    bool m_isReverse;               // Patrol PingPong ��/������ Ȯ��
    int m_curWaypoint;              // ���� �����Ǿ� �ִ� Waypoint
    int m_prevWaypoint;             // ���� �����Ǿ� �ִ� Waypoint
    int m_playerLayer;
    int m_backgroundLayer;

    Coroutine m_coChaseTarget;
    Coroutine m_coSearchTarget;
    #endregion Constants and Fields

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
                m_isPatrol = false;
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

    void SetState(AiState state)
    {
        m_state = state;
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
    }

    void BehaviourProcess()
    {
        switch (m_state)
        {
            case AiState.Idle:
                if (m_idleTime > m_idleDuration)
                {
                    m_idleTime = 0f;
                    // 1) �ν� ���� �ȿ� ������ => Attack / Chase
                    if (FindTarget(m_player.transform, m_detectDist))
                    {
                        // 1-1) ���� ���� �ȿ� ������ => Attack
                        if (CheckArea(m_player.transform, m_attackDist))
                        {
                            SetState(AiState.Attack);
                            return;
                        }
                        // 1-2) ���� ���� �ȿ� ������ ������ => Chase
                        SetState(AiState.Chase);
                        m_navAgent.stoppingDistance = m_attackDist;
                        m_coChaseTarget = StartCoroutine(CoChaseToTarget(m_player.transform, 30));
                        return;
                    }
                    // 2) �ν� ���� �ȿ� ������ ������ => Patrol 
                    SetState(AiState.Patrol);
                    m_navAgent.stoppingDistance = m_navAgent.radius;
                    m_coSearchTarget = StartCoroutine(CoSearchTarget(m_player.transform, 1f));
                    return;
                }
                m_idleTime += Time.deltaTime;
                break;
            case AiState.Attack:
                break;
            case AiState.Chase:
                // 1) ���� �� �÷��̾���� �Ÿ��� �ִ� �Ÿ��� ����� => Idle
                var dir = m_player.transform.position - transform.position;
                if (dir.sqrMagnitude >= m_maxChaseDistance * m_maxChaseDistance)
                {
                    SetIdle(0.5f);
                    StopCoroutine(m_coChaseTarget);
                    return;
                }
                // 2) ���� �� �÷��̾���� �Ÿ��� ��������� => Idle
                if (m_navAgent.remainingDistance <= m_navAgent.stoppingDistance)
                {
                    SetIdle(0.5f);
                    StopCoroutine(m_coChaseTarget);
                    return;
                }
                break;
            case AiState.Patrol:
                // 1) Patrol �ϰ� ���� ������
                if (!m_isPatrol)
                {
                    // 1-1) Patrol Ÿ���� Loop
                    if (m_patrolType == PatrolType.Loop)
                    {
                        m_curWaypoint++;
                        if (m_curWaypoint >= m_path.Points.Length)
                        {
                            m_curWaypoint = 0;
                        }
                    }
                    // 1-2) Patrol Ÿ���� PingPong
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
                    // 1-3) Patrol Ÿ���� Random
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
                    m_isPatrol = true;
                    m_prevWaypoint = m_curWaypoint;
                }
                // 2) Patrol �ϰ� �ִٸ�
                else
                {
                    if (m_navAgent.remainingDistance <= m_navAgent.stoppingDistance)
                    {
                        SetIdle(3f);
                        m_isPatrol = false;
                        StopCoroutine(m_coSearchTarget);
                    }
                }
                break;
        }
    }
    #endregion Methods

    #region Call by Unity
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
        m_navAgent = GetComponent<NavMeshAgent>();

        m_playerLayer = 1 << LayerMask.NameToLayer("Player");
        m_backgroundLayer = 1 << LayerMask.NameToLayer("Background");
    }

    void Update()
    {
        BehaviourProcess();
    }
    #endregion Call by Unity
}