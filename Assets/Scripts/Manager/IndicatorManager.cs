using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : SingletonMonoBehaviour<IndicatorManager>
{
    [SerializeField]
    PlayerController m_player;
    [SerializeField]
    GameObject m_indicatorPrefab;
    [SerializeField]
    GameObject m_indicatorParents;

    List<GameObject> m_indicatorList = new List<GameObject>();
    GameObjectPool<IndicatorController> m_indicatorPool;

    public List<GameObject> GetIndicatorList { get { return m_indicatorList; } }

    public void CreateIndicatorList()
    {
        for (int i = 0; i < m_indicatorPool.Count; i++)
        {
            var indicator = m_indicatorPool.Get();
            indicator.gameObject.SetActive(false);
            m_indicatorList.Add(indicator.gameObject);
        }
    }

    protected override void OnStart()
    {
        m_indicatorPool = new GameObjectPool<IndicatorController>(7, () =>
        {
            var obj = Instantiate(m_indicatorPrefab);
            obj.transform.SetParent(m_indicatorParents.transform, false);
            obj.SetActive(false);
            //obj.transform.localScale = Vector3.one;
            var indicator = obj.GetComponent<IndicatorController>();
            indicator.InitPlayer(m_player);
            return indicator;
        });
    }
}