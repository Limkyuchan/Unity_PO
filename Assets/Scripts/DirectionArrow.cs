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

    public void ShowArrowToTarget(Vector3 targetPosition)
    {
        StartCoroutine(CoShowBossDirectionArrow(targetPosition));
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

    void ShowDirectionArrow(Vector3 targetPosition)
    {
        // 화살표 방향 계산
        Vector3 direction = (targetPosition - m_player.transform.position).normalized;
        Vector2 currentPosition = m_arrowUI.anchoredPosition;
        currentPosition.y += 150f; 
        m_arrowUI.anchoredPosition = currentPosition;

        // 화살표 UI 방향 설정
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

    #region Coroutine Methods
    IEnumerator CoShowBossDirectionArrow(Vector3 targetPosition)
    {
        for (int i = 0; i < 2; i++)
        {
            ShowDirectionArrow(targetPosition);
            yield return new WaitForSeconds(0.5f);

            HideDirectionArrow();
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator CoDirectionArrow()
    {
        for (int i = 0; i < 2; i++)
        {
            ShowDirectionArrow(m_target.position);
            yield return new WaitForSeconds(0.5f);
            
            HideDirectionArrow();
            yield return new WaitForSeconds(0.5f);
        }
    }
    #endregion Coroutine Methods

    #region Unity Method
    void Start()
    {
        cnt = 1;
        m_arrowUI.gameObject.SetActive(false);
    }
    #endregion Unity Method
}