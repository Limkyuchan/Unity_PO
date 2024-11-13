using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : MonoBehaviour
{
    [SerializeField]
    EnemyManager m_enemyManager;
    [SerializeField]
    GameObject m_indicatorPrefab;
    [SerializeField]
    GameObject m_indicatorParents;

    Camera m_camera;
    PlayerController m_player;

    List<Transform> m_enemies = new List<Transform>();
    GameObjectPool<RectTransform> m_indicatorPool;
    Dictionary<Transform, RectTransform> m_enemyIndicatorList = new Dictionary<Transform, RectTransform>();

    public void SetPlayer(PlayerController player)
    {
        m_player = player;
    }

    // Indicator 위치 설정
    Vector2 GetEdgePosition(Vector2 screenCenter, Vector2 direction)
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Vector2 edgePosition = screenCenter;

        float margin = 20f;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            edgePosition.x = (direction.x > 0) ? screenWidth - margin : margin;
            edgePosition.y = screenCenter.y + direction.y * (screenWidth / 2);
        }
        else
        {
            edgePosition.x = screenCenter.x + direction.x * (screenHeight / 2);
            edgePosition.y = (direction.y > 0) ? screenHeight - margin : margin;
        }

        return edgePosition;
    }

    void ShowIndicator(Vector3 screenPosition, Transform enemy)
    {
        if (m_enemyIndicatorList.Count >= 10)
        {
            return;
        }

        if (!m_enemyIndicatorList.ContainsKey(enemy))
        {
            RectTransform indicatorRect = m_indicatorPool.Get();
            m_enemyIndicatorList[enemy] = indicatorRect;
        }

        RectTransform activeIndicator = m_enemyIndicatorList[enemy];
        activeIndicator.gameObject.SetActive(true);

        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 direction = (new Vector2(screenPosition.x, screenPosition.y) - screenCenter).normalized;

        Vector2 indicatorPosition = GetEdgePosition(screenCenter, direction);
        activeIndicator.position = indicatorPosition;
        activeIndicator.rotation = Quaternion.identity;
    }

    void HideIndicator(Transform enemy)
    {
        if (m_enemyIndicatorList.ContainsKey(enemy))
        {
            RectTransform indicator = m_enemyIndicatorList[enemy];
            indicator.gameObject.SetActive(false);
            m_indicatorPool.Set(indicator);
            m_enemyIndicatorList.Remove(enemy);
        }
    }

    void Start()
    {
        m_camera = Camera.main;

        m_indicatorPool = new GameObjectPool<RectTransform>(10, () =>
        {
            var obj = Instantiate(m_indicatorPrefab);
            obj.transform.SetParent(m_indicatorParents.transform, false);
            obj.SetActive(false);
            var indicator = obj.GetComponent<RectTransform>();
            return indicator;
        });
    }

    void Update()
    {
        m_enemies = m_enemyManager.GetEnemyList().ConvertAll(enemy => enemy.transform);

        foreach (Transform enemy in m_enemies)
        {
            Vector3 screenPosition = m_camera.WorldToScreenPoint(enemy.position);
            float distanceToPlayer = Vector3.Distance(enemy.position, m_player.transform.position);

            if (distanceToPlayer <= enemy.GetComponent<EnemyController>().GetStatus.detectDist && screenPosition.z > 0 &&
                (screenPosition.x < 0 || screenPosition.x > Screen.width || screenPosition.y < 0 || screenPosition.y > Screen.height))
            {
                ShowIndicator(screenPosition, enemy);
            }
            else
            {
                if (m_enemyIndicatorList.ContainsKey(enemy))
                {
                    HideIndicator(enemy);
                }
            }
        }
    }
}