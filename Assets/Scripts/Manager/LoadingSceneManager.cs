using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField]
    Image m_progressBar;
    [SerializeField]
    Text m_progressLabel;

    static string nextScene;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator CoLoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float time = 0f;
        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                m_progressBar.fillAmount = op.progress;
                m_progressLabel.text = $"{(op.progress * 100):0}%";
            }
            else
            {
                time += Time.unscaledDeltaTime;
                m_progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, time);
                m_progressLabel.text = $"{(Mathf.Lerp(90f, 100f, time)):0}%";

                if (m_progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    void Start()
    {
        StartCoroutine(CoLoadSceneProcess());
    }
}