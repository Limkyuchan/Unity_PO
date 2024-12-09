using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionIndicatorManager : MonoBehaviour
{
    [SerializeField]
    GameObject m_indicatorPrefab;
    [SerializeField]
    GameObject m_indicatorParents;

    GameObjectPool<RectTransform> m_indicatorPool;

    public RectTransform ShowIndicator()
    {
        var indicator = m_indicatorPool.Get();
        indicator.gameObject.SetActive(true);
        return indicator;
    }

    public void HideIndicator(RectTransform indicator)
    {
        indicator.gameObject.SetActive(false);
        m_indicatorPool.Set(indicator);
    }

    void Start()
    {
        m_indicatorPool = new GameObjectPool<RectTransform>(3, () =>
        {
            var obj = Instantiate(m_indicatorPrefab);
            obj.transform.SetParent(m_indicatorParents.transform, false);
            obj.SetActive(false);
            var indicator = obj.GetComponent<RectTransform>();
            return indicator;
        });
    }
}