using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    UIGameInformationMessage m_introduceGame;
    [SerializeField]
    UIGameOptionController m_gameOptionController;

    bool m_isPaused = false;
    bool m_prevPopupState = false;
    bool m_prevOptionState = false;

    public bool IsPaused { get { return m_isPaused; } }

    void CheckState()
    {
        bool curOptionState = m_gameOptionController.IsGameOptionOpen();
        bool curPopupState = PopupManager.Instance.IsPopupOpened;

        if (curOptionState != m_prevOptionState)
        {
            if (curOptionState)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

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
        m_prevOptionState = curOptionState;
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
        CheckState();

        // 게임 정보 확인하기
        if (Input.GetKeyDown(KeyCode.H) && !PopupManager.Instance.IsPopupOpened && !m_gameOptionController.IsGameOptionOpen())
        {
            m_introduceGame.IntroduceHowToPlayGame();
        }

        // 주인공 스탯 확인하기
        if (Input.GetKeyDown(KeyCode.Tab) && !PopupManager.Instance.IsPopupOpened && !m_gameOptionController.IsGameOptionOpen())
        {
            m_introduceGame.CheckPlayerStat();
        }
    }
}