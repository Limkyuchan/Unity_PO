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

        PopupManager.Instance.Popup_OpenOkCancel("<color=#ff0000>���� ����!</color>",
        "���� ���͸� ������ ��� ������ ��ġ��̽��ϴ�. \r\n" +
        "\"Ȯ��\" Ŭ�� �� Ÿ��Ʋ ȭ������ �̵��մϴ�. \r\n" +
        "\"����\" Ŭ�� �� ������ �����մϴ�.", () =>
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
        }, "Ȯ��", "����");
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