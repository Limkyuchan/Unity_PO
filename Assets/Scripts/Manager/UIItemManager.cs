using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIItemManager : MonoBehaviour
{
    [SerializeField]
    GameObject m_itemBloodPrefab;
    [SerializeField]
    GameObject m_itemAttackPrefab;
    [SerializeField]
    GameObject m_itemParents;

    GameObjectPool<RectTransform> m_bloodPool;
    GameObjectPool<RectTransform> m_attackPool;

    void Start()
    {
        m_bloodPool = new GameObjectPool<RectTransform>(2, () =>
        {
            var obj = Instantiate(m_itemBloodPrefab);
            obj.transform.SetParent(m_itemParents.transform, false);
            obj.SetActive(false);
            var blood = obj.GetComponent<RectTransform>();
            return blood;
        });

        m_attackPool = new GameObjectPool<RectTransform>(2, () =>
        {
            var obj = Instantiate(m_itemAttackPrefab);
            obj.transform.SetParent(m_itemParents.transform, false);
            obj.SetActive(false);
            var attack = obj.GetComponent<RectTransform>();
            return attack;
        });
    }
}