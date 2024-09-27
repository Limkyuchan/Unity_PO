using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTriggerZone : MonoBehaviour
{
    [SerializeField]
    EnemyManager m_enemyManager;
    [SerializeField]
    PathController m_pathA;
    [SerializeField]
    PathController m_pathB;
    [SerializeField]
    PathController m_pathC;

    bool enemySpawn = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !enemySpawn)
        {
            m_enemyManager.CreateEnemy(EnemyManager.EnemyType.MeleeWalk, m_pathA, 2);
            m_enemyManager.CreateEnemy(EnemyManager.EnemyType.WarriorWalk, m_pathB, 2);
            m_enemyManager.CreateEnemy(EnemyManager.EnemyType.WarriorJump, m_pathC, 1);

            enemySpawn = true;
        }
    }
}