using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameInformationMessage : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_showInfo;

    PlayerController m_player;
    string sceneName;

    public void SetPlayer(PlayerController player)
    {
        m_player = player;
    }

    public void IntroduceHowToPlayGame()
    {
        string commonInfo = LanguageManager.Instance.GetLocalizedText("CommonGameInfo");
        string basicKeyInfo = LanguageManager.Instance.GetLocalizedText("BasicKeyInfo");
        string skillInfo = GetSkillInfo();

        string message = commonInfo + basicKeyInfo + skillInfo;

        PopupManager.Instance.Popup_OpenOk(
            LanguageManager.Instance.GetLocalizedText("GameGuideTitle"),
            message,
            null, 
            LanguageManager.Instance.GetLocalizedText("OkButton"));
    }

    public void CheckPlayerStat()
    {
        string title = LanguageManager.Instance.GetLocalizedText("PlayerInfoTitle");
        string message = $" < {LanguageManager.Instance.GetLocalizedText("CurrentScene")}: {sceneName} >\r\n" +
                         $" {LanguageManager.Instance.GetLocalizedText("PlayerType")}: {m_player.GetPlayerType}\r\n" +
                         $" {LanguageManager.Instance.GetLocalizedText("KillScore")}: {m_player.DeathEnemyCnt} / {m_player.TotalEnemyCnt}\r\n" +
                         $" {LanguageManager.Instance.GetLocalizedText("AttackPower")}: {m_player.PlayerAttack}\r\n" +
                         $" {LanguageManager.Instance.GetLocalizedText("CurrentHealth")}: {m_player.PlayerCurHp} / {m_player.PlayerMaxHp}\r\n" +
                         $" {LanguageManager.Instance.GetLocalizedText("SkillGauge")}: {m_player.PlayerCurSkillGauge} / {m_player.PlayerMaxSkillGauge}\r\n\n";

        PopupManager.Instance.Popup_OpenOk(
            title,
            message,
            null,
            LanguageManager.Instance.GetLocalizedText("OkButton"));
    }

    public void PlayerDieGameOver()
    {
        PopupManager.Instance.Popup_OpenOkCancel(
        LanguageManager.Instance.GetLocalizedText("GameOver"),
        LanguageManager.Instance.GetLocalizedText("GameOverText"), () =>
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
        }, LanguageManager.Instance.GetLocalizedText("OkButton"), LanguageManager.Instance.GetLocalizedText("EndButton"));
    }

    string GetSkillInfo()
    {
        if (m_player.GetPlayerType == PlayerController.Type.Warrior)
        {
            return LanguageManager.Instance.GetLocalizedText("WarriorSkillInfo");
        }
        else if (m_player.GetPlayerType == PlayerController.Type.Range)
        {
            return LanguageManager.Instance.GetLocalizedText("RangeSkillInfo");
        }
        return "";
    }

    void UpdateTexts()
    {
        m_showInfo.text = LanguageManager.Instance.SetUITextLanguage("ShowInfo");
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

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }
}