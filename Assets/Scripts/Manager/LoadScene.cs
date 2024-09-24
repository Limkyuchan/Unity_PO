using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneState
{
    None = -1,
    GameScene01,
    GameScene02,
}

public class LoadScene : SingletonDontDestroy<LoadScene>
{
    [SerializeField]
    Image m_loadingBG;
    [SerializeField]
    Image m_progressBar;
    [SerializeField]
    Image m_progressBarFrame;
    [SerializeField]
    Text m_progressLabel;

    AsyncOperation m_loadingState;              // 로딩 상태를 확인
    SceneState m_state;                         // 현재 씬
    SceneState m_loadState = SceneState.None;   // 현재 로딩하고 있는 씬

    public void ShowUI()
    {
        m_loadingBG.gameObject.SetActive(true);
        m_progressBar.gameObject.SetActive(true);
        m_progressBarFrame.gameObject.SetActive(true);
        m_progressLabel.gameObject.SetActive(true);
    }

    public void HideUI()
    {
        m_loadingBG.gameObject.SetActive(false);
        m_progressBar.gameObject.SetActive(false);
        m_progressBarFrame.gameObject.SetActive(false);
        m_progressLabel.gameObject.SetActive(false);
    }

    public void LoadSceneAsync(SceneState state)
    {
        if (m_loadingState != null) return;
        m_loadState = state;
        StartCoroutine(CoLoadSceneCoroutine((int)state));
    }

    public void LoadSceneAsync(string sceneName)
    {
        if (m_loadingState != null) return;
        m_loadState = (SceneState)Enum.Parse(typeof(SceneState), sceneName);
        StartCoroutine(CoLoadSceneCoroutine(sceneName));
    }

    protected override void OnStart()
    {
        HideUI();
    }

    private IEnumerator CoLoadSceneCoroutine(int sceneIndex)
    {
        ShowUI();
        m_loadingState = SceneManager.LoadSceneAsync(sceneIndex);

        while (!m_loadingState.isDone)
        {
            m_progressBar.fillAmount = m_loadingState.progress;
            m_progressLabel.text = ((int)(m_loadingState.progress * 100)).ToString() + '%';

            if (m_loadingState.progress >= 0.9f)
            {
                m_progressBar.fillAmount = 1f;
                m_progressLabel.text = "100%";
                m_loadingState.allowSceneActivation = true;
            }
            yield return null;
        }

        yield return Utility.GetWaitForSeconds(0.5f);
        HideUI();
        m_loadingState = null;
        m_state = m_loadState;
        m_loadState = SceneState.None;
    }

    private IEnumerator CoLoadSceneCoroutine(string sceneName)
    {
        ShowUI();
        m_loadingState = SceneManager.LoadSceneAsync(sceneName);

        while (!m_loadingState.isDone)
        {
            m_progressBar.fillAmount = m_loadingState.progress;
            m_progressLabel.text = ((int)(m_loadingState.progress * 100)).ToString() + '%';

            if (m_loadingState.progress >= 0.9f)
            {
                m_progressBar.fillAmount = 1f;
                m_progressLabel.text = "100%";
                m_loadingState.allowSceneActivation = true;
            }
            yield return null;
        }

        yield return Utility.GetWaitForSeconds(0.5f);
        HideUI();
        m_loadingState = null;
        m_state = m_loadState;
        m_loadState = SceneState.None;
    }
}