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
                LoadSceneManager.Instance.LoadSceneAsync(SceneState.GameScene02);
            }

            if (sceneName == "GameScene02")
            {
                LoadSceneManager.Instance.LoadSceneAsync(SceneState.GameScene03);
            }

            if (sceneName == "GameScene03")
            {
                Debug.Log("GameOver!");
            }
        }
    }

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        m_nextSceneTriggerZone.SetActive(false);
    }
}