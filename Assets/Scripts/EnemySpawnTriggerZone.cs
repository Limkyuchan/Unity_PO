using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    string sceneName;

    public void CheckEnableBossMonster()
    {
        if (sceneName == "GameScene03")
        {
            m_enemyManager.CreateEnemy(EnemyManager.EnemyType.BossMonster, m_pathC, 1);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !enemySpawn)
        {
            if (sceneName == "GameScene01")
            {
                m_enemyManager.CreateEnemy(EnemyManager.EnemyType.BossMonster, m_pathA, 2);
                m_enemyManager.CreateEnemy(EnemyManager.EnemyType.WarriorWalk, m_pathB, 1);
                m_enemyManager.CreateEnemy(EnemyManager.EnemyType.WarriorJump, m_pathC, 1);
            }
            else if (sceneName == "GameScene02")
            {
                m_enemyManager.CreateEnemy(EnemyManager.EnemyType.MeleeWalk, m_pathA, 1);
                m_enemyManager.CreateEnemy(EnemyManager.EnemyType.MeleeWalk, m_pathB, 1);
                m_enemyManager.CreateEnemy(EnemyManager.EnemyType.MageWalk, m_pathC, 1);
            }
            else if (sceneName == "GameScene03")
            {
                m_enemyManager.CreateEnemy(EnemyManager.EnemyType.MeleeWalk2, m_pathA, 1);
                m_enemyManager.CreateEnemy(EnemyManager.EnemyType.MeleeWalk2, m_pathB, 1);
            }

            enemySpawn = true;
        }
    }

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }
}