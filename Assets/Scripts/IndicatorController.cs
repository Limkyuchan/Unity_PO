using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorController : MonoBehaviour
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
    List<GameObject> m_activeIndicators = new List<GameObject>(); // List to manage active indicators

    Vector2 GetEdgePosition(Vector2 screenCenter, Vector2 direction)
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Vector2 edgePosition = screenCenter;

        // Calculate the safe margin to prevent cutting off the indicator
        float margin = 20f; // You can adjust this value to control how far inside the screen edge the indicator appears

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            edgePosition.x = (direction.x > 0) ? screenWidth - margin : margin;
            edgePosition.y = screenCenter.y + direction.y * (screenWidth / 2);
        }
        else
        {
            edgePosition.y = (direction.y > 0) ? screenHeight - margin : margin;
            edgePosition.x = screenCenter.x + direction.x * (screenHeight / 2);
        }

        return edgePosition;
    }

    GameObject GetAvailableIndicator()
    {
        // Find an inactive indicator to reuse
        foreach (GameObject indicator in m_activeIndicators)
        {
            if (!indicator.activeInHierarchy)
            {
                return indicator;
            }
        }
        // If none are available, create a new one (if necessary)
        return Instantiate(m_enemyIndicatorPrefab, m_uiCanvas.transform);
    }

    void ShowIndicator(Vector3 screenPosition, Transform enemy)
    {
        GameObject indicator = GetAvailableIndicator(); // Get an available indicator from the pool
        RectTransform indicatorRect = indicator.GetComponent<RectTransform>();

        // Calculate screen center and direction
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 direction = (new Vector2(screenPosition.x, screenPosition.y) - screenCenter).normalized;

        // Position the indicator at the screen edge
        Vector2 indicatorPosition = GetEdgePosition(screenCenter, direction);
        indicatorRect.position = indicatorPosition;

        // Set the indicator to the default rotation (no rotation)
        indicator.transform.rotation = Quaternion.identity; // Reset rotation to ensure it's upright

        // Activate the indicator
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

        // Preload indicators
        for (int i = 0; i < 10; i++) // You can adjust the number of indicators
        {
            GameObject indicator = Instantiate(m_enemyIndicatorPrefab, m_uiCanvas.transform);
            indicator.SetActive(false); // Initially hide the indicator
            m_activeIndicators.Add(indicator); // Add to the active indicators list
        }
    }

    void Update()
    {
        m_enemies = m_enemyManager.GetEnemyList().ConvertAll(enemy => enemy.transform);

        // Reset all indicators to inactive at the start of the frame
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