using System.Collections;
using UnityEditor;
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

    public void EndGame()
    {
        StartCoroutine(CoShowGameEndPopup());
    }

    IEnumerator CoShowGameEndPopup()
    {
        yield return Utility.GetWaitForSeconds(2f);

        PopupManager.Instance.Popup_OpenOkCancel("<color=#ff0000>게임 종료!</color>",
        "보스 몬스터를 포함한 모든 적들을 해치우셨습니다. \r\n" +
        "\"확인\" 클릭 시 타이틀 화면으로 이동합니다. \r\n" +
        "\"종료\" 클릭 시 게임을 종료합니다.", () =>
        {
            LoadSceneManager.Instance.LoadSceneAsync(SceneState.Title);
            PopupManager.Instance.Popup_Close();
        }, () =>
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }, "확인", "종료");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && enemiesAllDie)
        {
            if (sceneName == "GameScene01")
            {
                AudioManager.Instance.StopAllAudio();
                LoadSceneManager.Instance.LoadSceneAsync(SceneState.GameScene02);
            }
        }
    }

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        m_nextSceneTriggerZone.SetActive(false);
    }
}