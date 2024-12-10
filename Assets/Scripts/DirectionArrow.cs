using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    [SerializeField]
    Transform m_target;      
    [SerializeField]
    RectTransform m_arrowUI; 
    
    PlayerController m_player;
    int cnt;

    public void SetPlayer(PlayerController player)
    {
        m_player = player;
    }

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
        Vector3 arrowPosition = m_target.position;
        Vector3 direction = (arrowPosition - m_player.transform.position).normalized;

        Vector2 currentPosition = m_arrowUI.anchoredPosition;
        currentPosition.y += 150f; 
        m_arrowUI.anchoredPosition = currentPosition;
        m_arrowUI.rotation = Quaternion.Euler(0, 0, 90);

        m_arrowUI.gameObject.SetActive(true);
    }

    void HideDirectionArrow()
    {
        Vector2 currentPosition = m_arrowUI.anchoredPosition;
        currentPosition.y -= 150f;
        m_arrowUI.anchoredPosition = currentPosition;

        m_arrowUI.gameObject.SetActive(false);
    }

    IEnumerator CoDirectionArrow()
    {
        for (int i = 0; i < 3; i++)
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