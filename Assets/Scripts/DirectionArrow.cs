using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    #region Constants and Fields
    [SerializeField]
    Transform m_target;      
    [SerializeField]
    RectTransform m_arrowUI; 
    
    PlayerController m_player;
    int cnt;
    #endregion Constants and Fields

    #region Public Methods
    public void SetPlayer(PlayerController player)
    {
        m_player = player;
    }
    #endregion Public Methods

    #region Methods
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (cnt == 1)
            {
                StartCoroutine(CoDirectionArrow());
            }
            cnt++;
        }
    }

    void ShowDirectionArrow() 
    {
        // 화살표 위치 조정
        Vector3 direction = (m_target.position - m_player.transform.position).normalized;
        Vector2 currentPosition = m_arrowUI.anchoredPosition;
        currentPosition.y += 150f; 
        m_arrowUI.anchoredPosition = currentPosition;

        // 화살표 방향 계산 후 활성화
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        m_arrowUI.rotation = Quaternion.Euler(0, 0, angle);
        m_arrowUI.gameObject.SetActive(true);
    }

    void HideDirectionArrow()
    {
        Vector2 currentPosition = m_arrowUI.anchoredPosition;
        currentPosition.y -= 150f;
        m_arrowUI.anchoredPosition = currentPosition;

        m_arrowUI.gameObject.SetActive(false);
    }
    #endregion Methods

    IEnumerator CoDirectionArrow()
    {
        for (int i = 0; i < 2; i++)
        {
            ShowDirectionArrow();
            yield return new WaitForSeconds(0.5f);
            
            HideDirectionArrow();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void Start()
    {
        cnt = 1;
        m_arrowUI.gameObject.SetActive(false);
    }
}