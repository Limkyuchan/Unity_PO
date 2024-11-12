using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EffectPool : SingletonMonoBehaviour<EffectPool>
{
    [SerializeField]
    int m_presetSize = 1;
    [SerializeField]
    List<string> m_effectNameList = new List<string>();
    Dictionary<string, GameObjectPool<EffectPoolUnit>> m_effectPool = new Dictionary<string, GameObjectPool<EffectPoolUnit>>();
    Dictionary<string, GameObject> m_prefabList = new Dictionary<string, GameObject>();

    public Dictionary<string, GameObject> GetPrefabList { get { return m_prefabList; } }

    public EffectPoolUnit Create(string effectName)
    {
        return Create(effectName, Vector3.zero, Quaternion.identity);
    }

    public EffectPoolUnit Create(string effectName, Vector3 position, Quaternion rotation)
    {
        EffectPoolUnit poolUnit = null;
        GameObjectPool<EffectPoolUnit> pool = null;

        if (!m_effectPool.TryGetValue(effectName, out pool) || pool == null)
        {
            return null;
        }

        for (int i = 0; i < pool.Count; i++)
        {
            poolUnit = pool.Get();
            if (poolUnit == null || !poolUnit.IsReady)
            {
                pool.Set(poolUnit);
                poolUnit = null;
            }
            else
            {
                break;
            }
        }

        if (poolUnit == null)
        {
            poolUnit = pool.New();
            if (poolUnit == null)
            {
                return null;
            }
        }

        poolUnit.transform.position = position;
        poolUnit.transform.rotation = rotation;
        poolUnit.gameObject.SetActive(true);
        return poolUnit;
    }

    public void AddPool(string effectName, EffectPoolUnit poolUnit)
    {
        GameObjectPool<EffectPoolUnit> pool = null;
        if (m_effectPool.TryGetValue(effectName, out pool))
        {
            pool.Set(poolUnit);
        }
    }


    protected override void OnStart()
    {
        m_effectNameList.Clear();
        m_effectPool.Clear();
        m_prefabList.Clear();
        EffectTable.Instance.LoadData();

        foreach (KeyValuePair<int, EffectData> pair in EffectTable.Instance.m_table)
        {
            for (int i = 0; i < pair.Value.Prefabs.Length; i++)
            {
                if (!m_effectNameList.Contains(pair.Value.Prefabs[i]))
                {
                    m_effectNameList.Add(pair.Value.Prefabs[i]);
                }
            }
        }

        for (int i = 0; i < m_effectNameList.Count; i++)
        {
            string effectName = m_effectNameList[i];
            var prefab = Resources.Load<GameObject>("Prefab/Effect/" + effectName);
            m_prefabList.Add(effectName, prefab);
            GameObjectPool<EffectPoolUnit> pool = new GameObjectPool<EffectPoolUnit>();
            m_effectPool.Add(effectName, pool);

            pool.CreatePool(m_presetSize, () =>
            {
                EffectPoolUnit poolUnit = null;
                if (prefab != null)
                {
                    var obj = Instantiate(prefab);
                    poolUnit = obj.GetComponent<EffectPoolUnit>();
                    if (poolUnit == null)
                    {
                        poolUnit = obj.AddComponent<EffectPoolUnit>();
                    }
                    var effectDestroy = obj.GetComponent<EffectAutoDestroy>();
                    if (effectDestroy == null)
                    {
                        effectDestroy = obj.AddComponent<EffectAutoDestroy>();
                    }
                    poolUnit.SetEffectPool(effectName);
                    obj.SetActive(false);
                }
                return poolUnit;
            });
        }
    }
}