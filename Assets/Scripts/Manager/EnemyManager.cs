using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{
    #region Enum Methods
    public enum EnemyType
    {
        None = -1,
        MeleeWalk,
        MeleeWalk2,
        WarriorJump,
        WarriorWalk,
        MageWalk,
        BossMonster,
        Max
    }
    #endregion Enum Methods

    #region Constants and Fields
    [SerializeField]
    PlayerController m_playerWarrior;
    [SerializeField]
    PlayerController m_playerRange;
    [SerializeField]
    EnemySpawnTriggerZone m_spawnZone;
    [SerializeField]
    SceneTransitionTriggerZone m_nextSceneZone;
    [SerializeField] 
    GameObject m_hudParents;
    [SerializeField]
    GameObject m_hudPrefab;
    [SerializeField]
    UIItemManager m_itemManager;
    [SerializeField]
    GameObject[] m_enemyPrefabs;
    PlayerController m_player;

    List<EnemyController> m_enemyList = new List<EnemyController>();
    Dictionary<EnemyType, GameObjectPool<EnemyController>> m_enemyPool = new Dictionary<EnemyType, GameObjectPool<EnemyController>>();
    GameObjectPool<HUD_Controller> m_hudPool;

    string m_playerCharacterType;
    int m_deathEnemyCnt;
    bool m_bossDeath;
    #endregion Constants and Fields

    #region Public Properties
    public List<EnemyController> GetEnemyList() { return m_enemyList; }

    public bool GetBossMonsterDeath { get { return m_bossDeath; } }
    #endregion Public Properties 

    #region Public Methods
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

        m_player.TotalEnemyCnt = m_enemyList.Count;
    } 

    public void RemoveEnemy(EnemyController enemy)
    {
        m_deathEnemyCnt++;

        if (m_enemyList.Contains(enemy))
        {
            m_enemyList.Remove(enemy);
            m_player.DeathEnemyCnt = m_deathEnemyCnt;

            int dropProbability = UnityEngine.Random.Range(0, 100);
            if (dropProbability < 25)
            {
                m_itemManager.SpawnBloodItem(enemy.transform.position);
                m_player.PlayerHpUpgrade();
            }
            else if (dropProbability < 50)
            {
                m_itemManager.SpawnAttackItem(enemy.transform.position);
                m_player.PlayerAttackUpgrade();
            }
            else if (dropProbability < 75)
            {
                m_itemManager.SpawnSkillGaugeItem(enemy.transform.position);
                m_player.PlayerSkillGaugeUpgrade();
            }
        }

        // 보스 몬스터가 사망했을 경우
        if (enemy.Type == EnemyType.BossMonster && !m_bossDeath)
        {
            m_bossDeath = true;
            m_player.DeathEnemyCnt = m_deathEnemyCnt;
            m_nextSceneZone.EndGame();
            return;
        }

        // 모든 적이 죽었을 경우
        if (m_enemyList.Count == 0)
        {
            m_nextSceneZone.AllEnemiesDie();
            m_player.AllEnemiesDie();
            m_spawnZone.CheckEnableBossMonster();
        }
    }

    public void ResetDeathEnemyCnt()
    {
        m_deathEnemyCnt = 0;
        m_player.DeathEnemyCnt = 0;
    }
    #endregion Public Methods

    #region Unity Methods
    protected override void OnStart()
    {
        m_playerCharacterType = PlayerPrefs.GetString("PlayerCharacterType", "PlayerCharacterType");
        if (m_playerCharacterType == "Warrior")
        {
            m_player = m_playerWarrior;
        }
        else if (m_playerCharacterType == "Range")
        {
            m_player = m_playerRange;
        }

        m_deathEnemyCnt = 0;
        m_bossDeath = false;
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
    #endregion Unity Methods
}