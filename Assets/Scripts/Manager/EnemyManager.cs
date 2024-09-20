using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{
    public enum EnemyType
    {
        MeleeWalk,
        MeleeWalk2,
        WarriorJump,
        WarriorWalk,
        MageWalk,
        Max
    }

    [SerializeField]
    PlayerController m_player;
    [SerializeField]
    GameObject[] m_enemyPrefabs;

    List<EnemyController> m_enemyList = new List<EnemyController>();
    Dictionary<GameObject, EnemyController> m_enemyComponentList = new Dictionary<GameObject, EnemyController>();
    Dictionary<EnemyType, GameObjectPool<EnemyController>> m_enemyPool = new Dictionary<EnemyType, GameObjectPool<EnemyController>>();

    public void CreateEnemy(EnemyType type, PathController path, int count)
    {
        var waypoints = path.Waypoints;
        int spawnCount = Mathf.Min(count, waypoints.Length);

        for (int i = 0; i < spawnCount; i++)
        {
            var enemy = m_enemyPool[type].Get();
            enemy.SetEnemy(path, i);
            enemy.gameObject.SetActive(true);
        }
    }

    protected override void OnStart()
    {
        m_enemyPrefabs = Resources.LoadAll<GameObject>("Prefabs/Enemys");

        for (int i = 0; i < m_enemyPrefabs.Length; i++)
        {
            var enemyPrefab = m_enemyPrefabs[i];
            EnemyType type = (EnemyType)(int.Parse(enemyPrefab.name.Split("_")[0]) - 1);

            var pool = new GameObjectPool<EnemyController> (3, () =>
            {
                var obj = Instantiate(enemyPrefab);
                obj.transform.SetParent(transform, false);
                obj.SetActive(false);
                var enemy = obj.GetComponent<EnemyController>();
                enemy.Type = type;
                enemy.InitPlayer(m_player);
                m_enemyComponentList.Add(enemy.gameObject, enemy);
                return enemy;
            });
            m_enemyPool.Add(type, pool);
        }
    }
}
