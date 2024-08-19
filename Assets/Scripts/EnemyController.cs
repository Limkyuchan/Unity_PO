using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region Constants and Fields
    [SerializeField]
    PlayerController m_player;
    [SerializeField]
    bool m_isShowDetectArea;
    [SerializeField]
    bool m_isShowAttackArea;

    int m_backgroundLayer;
    int m_playerLayer;
    float m_detectDist = 8f;
    float m_attackDist = 5f;
    #endregion Constants and Fields

    #region Methods
    bool FindTarget(Transform target, float distance)
    {
        var start = transform.position + Vector3.up * 0.7f;
        var end = target.position + Vector3.up * 0.7f;

        RaycastHit hit;
        if (Physics.Raycast(start, (end - start).normalized, out hit, distance, m_playerLayer | m_backgroundLayer))
        {
            if (hit.transform.CompareTag("Player"))
            {
                Debug.DrawRay(start, (end - start).normalized * hit.distance, Color.magenta, 1f);
                return true;
            }
        }
        return false;
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
            Gizmos.DrawWireSphere (transform.position, m_attackDist);
        }
    }

    void Start()
    {
        m_playerLayer = 1 << LayerMask.NameToLayer("Player");
        m_backgroundLayer = 1 << LayerMask.NameToLayer("Background");
    }

    void Update()
    {
        FindTarget(m_player.transform, m_detectDist);
    }
    #endregion Call by Unity
}