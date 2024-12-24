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
    [SerializeField]
    UIBossSpawn m_bossSpawnUI;

    bool enemySpawn = false;
    int bossCnt;
    string sceneName;

    public void CheckEnableBossMonster()
    {
        if (sceneName == "GameScene02" && !m_enemyManager.GetBossMonsterDeath && bossCnt == 1)
        {
            bossCnt++;
            m_enemyManager.ResetDeathEnemyCnt();
            m_bossSpawnUI.ShowBossSpawnMessage();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !enemySpawn)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player == null)
            {
                return;
            }

            if (sceneName == "GameScene01")
            {
                if (player.GetPlayerType == PlayerController.Type.Warrior)
                {
                    m_enemyManager.CreateEnemy(EnemyManager.EnemyType.MeleeWalk2, m_pathA, 2);
                    m_enemyManager.CreateEnemy(EnemyManager.EnemyType.WarriorWalk, m_pathB, 2);
                    m_enemyManager.CreateEnemy(EnemyManager.EnemyType.WarriorJump, m_pathC, 1);
                }
                else if (player.GetPlayerType == PlayerController.Type.Range)
                {
                    m_enemyManager.CreateEnemy(EnemyManager.EnemyType.MeleeWalk2, m_pathA, 1);
                    m_enemyManager.CreateEnemy(EnemyManager.EnemyType.WarriorWalk, m_pathB, 1);
                    m_enemyManager.CreateEnemy(EnemyManager.EnemyType.MeleeWalk2, m_pathC, 1);
                }
            }
            else if (sceneName == "GameScene02")
            {
                if (player.GetPlayerType == PlayerController.Type.Warrior)
                {
                    m_enemyManager.CreateEnemy(EnemyManager.EnemyType.MageWalk, m_pathA, 1);
                    m_enemyManager.CreateEnemy(EnemyManager.EnemyType.MageWalk, m_pathB, 1);
                }
                else if (player.GetPlayerType == PlayerController.Type.Range)
                {
                    m_enemyManager.CreateEnemy(EnemyManager.EnemyType.MageWalk, m_pathA, 1);
                    m_enemyManager.CreateEnemy(EnemyManager.EnemyType.WarriorWalk, m_pathB, 2);
                }
            }
            enemySpawn = true;
        }
    }

    void Start()
    {
        bossCnt = 1;
        sceneName = SceneManager.GetActiveScene().name;
    }
}