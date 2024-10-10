using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{
    public enum EnemyType
    {
        None = -1,
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
    SceneTransitionTriggerZone m_triggerZone;
    [SerializeField] 
    GameObject m_hudParents;
    [SerializeField]
    GameObject m_hudPrefab;
    [SerializeField]
    GameObject[] m_enemyPrefabs;

    List<EnemyController> m_enemyList = new List<EnemyController>();
    Dictionary<EnemyType, GameObjectPool<EnemyController>> m_enemyPool = new Dictionary<EnemyType, GameObjectPool<EnemyController>>();
    GameObjectPool<HUD_Controller> m_hudPool;

    public List<EnemyController> GetEnemyList() { return m_enemyList; }

    public void CreateEnemy(EnemyType type, PathController path, int count)
    {
        var waypoints = path.Waypoints;
        int spawnCount = Mathf.Min(count, waypoints.Length);

        for (int i = 0; i < spawnCount; i++)
        {
            var enemy = m_enemyPool[type].Get();
            var hud = m_hudPool.Get();
            enemy.SetEnemy(path, hud, i);
            hud.SetHUD(enemy.Dummy_HUD, enemy);
            m_enemyList.Add(enemy);

            enemy.gameObject.SetActive(true);
        }
    } 

    public void RemoveEnemy(EnemyController enemy)
    {
        if (m_enemyList.Contains(enemy))
        {
            m_enemyList.Remove(enemy);
        }

        if (m_enemyList.Count == 0)
        {
            m_triggerZone.AllEnemiesDie();
        }
    }

    protected override void OnStart()
    {
        m_enemyPrefabs = Resources.LoadAll<GameObject>("Prefab/Enemys");

        for (int i = 0; i < m_enemyPrefabs.Length; i++)
        {
            var enemyPrefab = m_enemyPrefabs[i];
            EnemyType type = (EnemyType)(int.Parse(enemyPrefab.name.Split("_")[0]) - 1);

            var pool = new GameObjectPool<EnemyController> (2, () =>
            {
                var obj = Instantiate(enemyPrefab);
                obj.transform.SetParent(transform, false);
                obj.SetActive(false);
                var enemy = obj.GetComponent<EnemyController>();
                enemy.Type = type;
                enemy.InitPlayer(m_player);
                return enemy;
            });
            m_enemyPool.Add(type, pool);
        }

        m_hudPool = new GameObjectPool<HUD_Controller>(5, () =>
        {
            var obj = Instantiate(m_hudPrefab);
            obj.transform.SetParent(m_hudParents.transform, false);
            obj.SetActive(false);
            obj.transform.localScale = Vector3.one;
            var hud = obj.GetComponent<HUD_Controller>();
            return hud;
        });
    }
}