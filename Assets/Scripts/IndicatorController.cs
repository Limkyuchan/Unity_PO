using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IndicatorController : MonoBehaviour
{
    Camera m_camera;
    EnemyManager m_enemyManager;
    PlayerController m_player;

    List<Transform> m_enemiesList = new List<Transform>();

    public void InitPlayer(PlayerController player)
    {
        m_player = player;
    }

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
        IndicatorManager.Instance.CreateIndicatorList();
        foreach (GameObject indicator in IndicatorManager.Instance.GetIndicatorList)
        {
            if (!indicator.activeInHierarchy)
            {
                return indicator;
            }
        }
        return null;
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
    }

    void Update()
    {
        m_enemiesList = m_enemyManager.GetEnemyList().ConvertAll(enemy => enemy.transform);

        foreach (GameObject indicator in IndicatorManager.Instance.GetIndicatorList)
        {
            Debug.Log("ÀÎµðÄÉÀÌÅÍ ¸ðµÎ ¼û±è");
            HideIndicator(indicator);
        }

        foreach (Transform enemy in m_enemiesList)
        {
            Vector3 screenPosition = m_camera.WorldToScreenPoint(enemy.position);
            float distanceToPlayer = Vector3.Distance(enemy.position, m_player.transform.position);
            Debug.Log("Ãâ·Â ¾ÈµÊ");

            if (distanceToPlayer <= enemy.GetComponent<EnemyController>().GetStatus.detectDist && screenPosition.z > 0 &&
                (screenPosition.x < 0 || screenPosition.x > Screen.width || screenPosition.y < 0 || screenPosition.y > Screen.height))
            {
                Debug.Log("Ãâ·Â µÊ");
                ShowIndicator(screenPosition, enemy);
            }
        }
    }
}