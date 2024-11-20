using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    UIGameInformationMessage m_introduceGame;

    bool m_isPaused = false;
    bool m_prevPopupState = false;

    public bool IsPaused { get { return m_isPaused; } }

    void CheckPopupState()
    {
        bool curPopupState = PopupManager.Instance.IsPopupOpened;

        if (curPopupState != m_prevPopupState)
        {
            if (curPopupState)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        m_prevPopupState = curPopupState;
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        m_isPaused = true;
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        m_isPaused = false;
    }

    void Update()
    {
        CheckPopupState();

        // ���� ���� Ȯ���ϱ�
        if (Input.GetKeyDown(KeyCode.H) && !PopupManager.Instance.IsPopupOpened)
        {
            m_introduceGame.IntroduceHowToPlayGame();
        }

        // ���ΰ� ���� Ȯ���ϱ�
        if (Input.GetKeyDown(KeyCode.Tab) && !PopupManager.Instance.IsPopupOpened)
        {
            m_introduceGame.CheckPlayerStat();
        }
    }
}