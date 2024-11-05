using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemManager : MonoBehaviour
{
    [SerializeField]
    GameObject m_itemBloodPrefab;
    [SerializeField]
    GameObject m_itemAttackPrefab;
    [SerializeField]
    GameObject m_itemParents;

    Camera m_camera;
    GameObjectPool<RectTransform> m_bloodPool;
    GameObjectPool<RectTransform> m_attackPool;

    public void SpawnBloodItem(Vector3 position)
    {
        var bloodItem = m_bloodPool.Get();
        Vector3 screenPosition = m_camera.WorldToScreenPoint(position);
        bloodItem.position = screenPosition;
        bloodItem.gameObject.SetActive(true);
        StartCoroutine(CoMoveAndDestroy(bloodItem));
    }

    public void SpawnAttackItem(Vector3 position)
    {
        var attackItem = m_attackPool.Get();
        Vector3 screenPosition = m_camera.WorldToScreenPoint(position);
        attackItem.position = screenPosition;
        attackItem.gameObject.SetActive(true);
        StartCoroutine(CoMoveAndDestroy(attackItem));
    }

    IEnumerator CoMoveAndDestroy(RectTransform item)
    {
        float time = 0f;
        Vector3 initialPosition = item.position;
        Vector3 targetPosition = initialPosition + new Vector3(0, 50, 0);

        while (time < 1.5f)
        {
            item.position = Vector3.Lerp(initialPosition, targetPosition, (time / 1f));
            time += Time.deltaTime;
            yield return null;
        }

        item.gameObject.SetActive(false); 
        item.position = initialPosition;
    }

    void Start()
    {
        m_camera = Camera.main;

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