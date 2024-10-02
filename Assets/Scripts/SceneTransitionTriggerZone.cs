using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTriggerZone : MonoBehaviour
{
    [SerializeField]
    GameObject m_nextSceneTriggerZone;
    string sceneName;
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
            if (sceneName == "GameScene01")
            {
                LoadingSceneManager.LoadScene("GameScene02");
            }
        }
    }

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        m_nextSceneTriggerZone.SetActive(false);
    }
}