using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIBossSpawn : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_bossSpawnText;
    [SerializeField]
    EnemyManager m_enemyManager;
    [SerializeField]
    PathController m_bossPath;

    public void ShowBossSpawnMessage()
    {
        StartCoroutine(CoBossSpawnRoutine());
    }

    IEnumerator CoBossSpawnRoutine()
    {
        for (int i = 0; i < 3; i++)
        {
            m_bossSpawnText.gameObject.SetActive(true);
            yield return Utility.GetWaitForSeconds(0.5f);

            m_bossSpawnText.gameObject.SetActive(false);
            yield return Utility.GetWaitForSeconds(0.5f);
        }

        m_enemyManager.CreateEnemy(EnemyManager.EnemyType.BossMonster, m_bossPath, 1);
    }

    void Start()
    {
        m_bossSpawnText.gameObject.SetActive(false);
    }
}
