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
    List<Transform> m_enemies;                 
    [SerializeField]
    Canvas m_uiCanvas;
    Camera m_camera;

    void ShowIndicator(Vector3 screenPosition, Transform enemy)
    {
        GameObject indicator = Instantiate(m_enemyIndicatorPrefab, m_uiCanvas.transform); 
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 direction = (new Vector2(screenPosition.x, screenPosition.y) - screenCenter).normalized;

        // 인디케이터를 화면 가장자리에 배치
        indicator.GetComponent<RectTransform>().position = screenCenter + direction * (Screen.height / 2);

        // 회전 처리
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        indicator.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void HideIndicator(Transform enemy)
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
        m_camera = Camera.main;
    }

    void Update()
    {
        Debug.Log(m_enemies.Count);

        foreach (Transform enemy in m_enemies)
        {
            Vector3 screenPosition = m_camera.WorldToScreenPoint(enemy.position);

            if (screenPosition.z > 0 && (screenPosition.x < 0 || screenPosition.x > Screen.width || screenPosition.y < 0 || screenPosition.y > Screen.height))
            {
                ShowIndicator(screenPosition, enemy);
            }
            else
            {
                HideIndicator(enemy);
            }
        }
    }
}