using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleIndicator : MonoBehaviour
{
    [SerializeField]
    GameObject m_enemyIndicatorPrefab;
    [SerializeField]
    Transform m_player;
    [SerializeField]
    Canvas m_uiCanvas;

    Camera m_camera;
    EnemyManager m_enemyManager;

    List<Transform> m_enemies = new List<Transform>();
    List<GameObject> m_activeIndicators = new List<GameObject>();

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

    GameObject GetAvailableIndicator()
    {
        foreach (GameObject indicator in m_activeIndicators)
        {
            if (!indicator.activeInHierarchy)
            {
                return indicator;
            }
        }
        return Instantiate(m_enemyIndicatorPrefab, m_uiCanvas.transform);
    }

    void ShowIndicator(Vector3 screenPosition, Transform enemy)
    {
        GameObject indicator = GetAvailableIndicator();
        RectTransform indicatorRect = indicator.GetComponent<RectTransform>();

        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 direction = (new Vector2(screenPosition.x, screenPosition.y) - screenCenter).normalized;

        Vector2 indicatorPosition = GetEdgePosition(screenCenter, direction);
        indicatorRect.position = indicatorPosition;

        indicator.transform.rotation = Quaternion.identity;
        indicator.SetActive(true);
    }

    void HideIndicator(GameObject indicator)
    {
        indicator.SetActive(false);
    }

    void Start()
    {
        m_camera = Camera.main;
        m_enemyManager = EnemyManager.Instance;

        for (int i = 0; i < 10; i++)
        {
            GameObject indicator = Instantiate(m_enemyIndicatorPrefab, m_uiCanvas.transform);
            indicator.SetActive(false);
            m_activeIndicators.Add(indicator);
        }
    }

    void Update()
    {
        m_enemies = m_enemyManager.GetEnemyList().ConvertAll(enemy => enemy.transform);

        //Reset all indicators to inactive at the start of the frame
        foreach (GameObject indicator in m_activeIndicators)
        {
            HideIndicator(indicator);
        }

        foreach (Transform enemy in m_enemies)
        {
            Vector3 screenPosition = m_camera.WorldToScreenPoint(enemy.position);
            float distanceToPlayer = Vector3.Distance(enemy.position, m_player.position);

            if (distanceToPlayer <= enemy.GetComponent<EnemyController>().GetStatus.detectDist && screenPosition.z > 0 &&
                (screenPosition.x < 0 || screenPosition.x > Screen.width || screenPosition.y < 0 || screenPosition.y > Screen.height))
            {
                ShowIndicator(screenPosition, enemy);
            }
        }
    }
}