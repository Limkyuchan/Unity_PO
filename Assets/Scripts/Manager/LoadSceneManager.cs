using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneState
{
    None = -1,
    Title,
    GameSettingScene,
    GameScene01,
    GameScene02
}

public class LoadSceneManager : SingletonDontDestroy<LoadSceneManager>
{
    [SerializeField]
    Image m_loadingBG;
    [SerializeField]
    GameObject m_progressBarParents;
    [SerializeField]
    TextMeshProUGUI m_backgroundText;
    [SerializeField]
    Image m_progressBar;
    [SerializeField]
    Text m_progressLabel;
    [SerializeField]
    float m_minimumLoadTime = 2f;

    AsyncOperation m_loadingState;              // 로딩 상태 확인
    UIGameOptionController m_gameOptionController;

    SceneState m_state;                         // 현재 씬
    SceneState m_loadState = SceneState.None;   // 현재 로딩중인 씬

    public void ShowUI()
    {
        m_loadingBG.gameObject.SetActive(true);
        m_progressBarParents.gameObject.SetActive(true);
    }

    public void HideUI()
    {
        m_loadingBG.gameObject.SetActive(false);
        m_progressBarParents.gameObject.SetActive(false);
    }

    public void LoadSceneAsync(SceneState state)
    {
        if (m_loadingState != null) return;
        m_loadState = state;
        DisablePreviousCanvas();
        StartCoroutine(CoLoadSceneProcess((int)state));
    }

    public void LoadSceneAsync(string sceneName)
    {
        if (m_loadingState != null) return;
        m_loadState = (SceneState)Enum.Parse(typeof(SceneState), sceneName);
        DisablePreviousCanvas();
        StartCoroutine(CoLoadSceneProcess(sceneName));
    }

    void DisablePreviousCanvas()
    {
        Canvas[] allCanvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in allCanvases)
        {
            if (canvas.gameObject != LoadSceneManager.Instance.GetComponent<Canvas>().gameObject)
            {
                canvas.gameObject.SetActive(false);     // 로딩 Canvas 제외한 나머지 Canvas 비활성화
            }
        }
    }

    void UpdateCursorState(SceneState state)
    {
        if (state == SceneState.GameScene01 || state == SceneState.GameScene02)
        {
            Cursor.lockState = CursorLockMode.Locked;  // 게임 씬에서 커서 고정
            Cursor.visible = false;                    // 커서 숨기기
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;    // 타이틀 등 UI 씬에서 커서 고정 해제
            Cursor.visible = true;                     // 커서 보이기
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        m_gameOptionController = FindObjectOfType<UIGameOptionController>();
    }

    void UpdateTexts()
    {
        m_backgroundText.text = LanguageManager.Instance.SetUITextLanguage("PlayerKeyText");
    }

    IEnumerator CoLoadSceneProcess(int sceneIndex)
    {
        ShowUI();
        m_loadingState = SceneManager.LoadSceneAsync(sceneIndex);
        m_loadingState.allowSceneActivation = false;        // 자동으로 씬 전환하지 않음

        float totalTime = 0f;
        float loadingProgress = 0f;

        while (!m_loadingState.isDone)
        {
            totalTime += Time.unscaledDeltaTime;

            if (m_loadingState.progress < 0.9f)
            {
                m_progressBar.fillAmount = Mathf.Lerp(loadingProgress, m_loadingState.progress, totalTime / m_minimumLoadTime);
                m_progressLabel.text = $"{(m_progressBar.fillAmount * 100):0}%";
            }
            else
            {
                m_progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, (totalTime - m_minimumLoadTime) / 1f);
                m_progressLabel.text = $"{Mathf.Lerp(90f, 100f, (totalTime - m_minimumLoadTime) / 1f):0}%";

                if (m_progressBar.fillAmount >= 1f && totalTime >= m_minimumLoadTime)
                {
                    m_loadingState.allowSceneActivation = true;
                    yield return null;
                }
            }
            yield return null;
        }

        m_state = m_loadState;              // 현재 상태를 로드한 상태로 설정
        m_loadState = SceneState.None;      // 로드 상태 초기화
        m_loadingState = null;
        UpdateCursorState(m_state);
        HideUI();
    }

    IEnumerator CoLoadSceneProcess(string sceneName)
    {
        ShowUI();
        m_loadingState = SceneManager.LoadSceneAsync(sceneName);
        m_loadingState.allowSceneActivation = false;

        float totalTime = 0f;
        float loadingProgress = 0f;

        while (!m_loadingState.isDone)
        {
            totalTime += Time.unscaledDeltaTime;

            if (m_loadingState.progress < 0.9f)
            {
                m_progressBar.fillAmount = Mathf.Lerp(loadingProgress, m_loadingState.progress, totalTime / m_minimumLoadTime);
                m_progressLabel.text = $"{(m_progressBar.fillAmount * 100):0}%";
            }
            else
            {
                m_progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, (totalTime - m_minimumLoadTime) / 1f);
                m_progressLabel.text = $"{Mathf.Lerp(90f, 100f, (totalTime - m_minimumLoadTime) / 1f):0}%";

                if (m_progressBar.fillAmount >= 1f && totalTime >= m_minimumLoadTime)
                {
                    m_loadingState.allowSceneActivation = true;
                    yield return null;
                }
            }
            yield return null;
        }

        m_state = m_loadState;
        m_loadState = SceneState.None;
        m_loadingState = null;
        UpdateCursorState(m_state);
        HideUI();
    }

    void OnEnable()
    {
        if (LanguageManager.Instance == null) return;

        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.OnLanguageChanged += UpdateTexts;
            UpdateTexts();
        }
    }

    void OnDisable()
    {
        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.OnLanguageChanged -= UpdateTexts;
        }
    }


    protected override void OnAwake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected override void OnStart()
    {
        HideUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_gameOptionController != null && m_gameOptionController.IsGameOptionOpen())
            {
                m_gameOptionController.CloseGameOption();
            }
            else if (PopupManager.Instance.IsPopupOpened)
            {
                PopupManager.Instance.Popup_Close();
            }
            else 
            {
                switch (m_state)
                {
                    case SceneState.Title:
                        string notice = LanguageManager.Instance.GetLocalizedText("Notice");
                        string exitGame = LanguageManager.Instance.GetLocalizedText("ExitGame");
                        PopupManager.Instance.Popup_OpenOkCancel(notice, exitGame, () =>
                        {
#if UNITY_EDITOR
                            EditorApplication.isPlaying = false;
#else
                            Application.Quit();
#endif
                        }, null, LanguageManager.Instance.GetLocalizedText("OkButton"), LanguageManager.Instance.GetLocalizedText("CancelButton"));
                        break;
                    case SceneState.GameSettingScene:
                    case SceneState.GameScene01:
                    case SceneState.GameScene02:
                        notice = LanguageManager.Instance.GetLocalizedText("Notice");
                        string exitMessage = LanguageManager.Instance.GetLocalizedText("ExitMessage");
                        PopupManager.Instance.Popup_OpenOkCancel(notice, exitMessage, () =>
                            {
                                LoadSceneAsync(SceneState.Title);
                                PopupManager.Instance.Popup_Close();
                            }, null, LanguageManager.Instance.GetLocalizedText("OkButton"), LanguageManager.Instance.GetLocalizedText("CancelButton"));
                        break;
                }
            }
        }
    }
}