using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionTriggerZone : MonoBehaviour
{
    [SerializeField]
    GameObject m_nextSceneTriggerZone;
    bool enemiesAllDie;

    public void AllEnemiesDie()
    {
        enemiesAllDie = true;
        m_nextSceneTriggerZone.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && enemiesAllDie)
        {
            LoadingSceneManager.LoadScene("GameScene02");
        }
    }

    void Start()
    {
        enemiesAllDie = false;
        m_nextSceneTriggerZone.SetActive(false);
    }
}