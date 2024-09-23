using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionTriggerZone : MonoBehaviour
{
    [SerializeField]
    GameObject m_nextSceneTriggerZone;

    bool enemiesAllDie = false;

    public void AllEnemiesDie()
    {
        enemiesAllDie = true;
        m_nextSceneTriggerZone.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && enemiesAllDie)
        {
            // ¾À º¯°æ.
            Debug.Log("¾À º¯°æ!");
        }
    }

    void Start()
    {
        m_nextSceneTriggerZone.SetActive(false);
    }
}