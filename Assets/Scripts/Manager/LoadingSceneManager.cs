using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    Image m_progressBar;           
    Text m_progressLabel;           
    float m_minimumLoadTime = 2f;

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

        float totalTime = 0f;
        float loadingProgress = 0f;

        while (!op.isDone)
        {
            totalTime += Time.unscaledDeltaTime;

            if (op.progress < 0.9f)
            {
                m_progressBar.fillAmount = Mathf.Lerp(loadingProgress, op.progress, totalTime / m_minimumLoadTime);
                m_progressLabel.text = $"{(loadingProgress * 100):0}%";
            }
            else
            {
                m_progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, (totalTime - m_minimumLoadTime) / 1f);
                m_progressLabel.text = $"{(Mathf.Lerp(90f, 100f, (totalTime - m_minimumLoadTime) / 1f)):0}%";

                if (m_progressBar.fillAmount >= 1f && totalTime >= m_minimumLoadTime)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
            yield return null;
        }
    }

    void Start()
    {
        StartCoroutine(CoLoadSceneProcess());
    }
}