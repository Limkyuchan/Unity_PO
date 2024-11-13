using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameInformationMessage : MonoBehaviour
{
    PlayerController m_player;
    string sceneName;

    public void SetPlayer(PlayerController player)
    {
        m_player = player;
    }

    public void IntroduceHowToPlayGame()
    { 
        PopupManager.Instance.Popup_OpenOk("<color=#000000>게임 안내</color>",
            "< 게임 정보 > \r\n" +
            "모든 적과 보스를 해치우는 게임입니다.\r\n" +
            "맵이 변경되도 정보는 저장됩니다.\r\n" +
            "적을 해치우면 랜덤으로 효과를 얻습니다.\r\n" +
            " ▶ 25% 확률: <color=#ff0000>체력</color> 증가 (+ 20) \r\n" +
            " ▶ 25% 확률: <color=#ff0000>스킬 게이지</color> 증가 (+ 10) \r\n" +
            " ▶ 25% 확률: <color=#ff0000>공격력</color> 증가 (+ 2) \r\n\n" +
            "< 기본 키 > \r\n" +
            "[Tab] \r\n" +
            " ▶ 플레이어의 정보를 확인할 수 있습니다.\r\n" +
            "[Left Shift] \r\n" +
            " ▶ 플레이어의 이동속도가 증가합니다.\r\n" +
            "[C] \r\n" +
            " ▶ 플레이어가 무기를 들어 방어합니다.\r\n" +
            "[Space] \r\n" +
            " ▶ <color=#ff0000>콤보 공격</color>(4회 중첩)이 가능합니다.\r\n\n" +
            "< 스킬 공격 > \r\n" +
            "[Z] \r\n" +
            " ▶ 기본 공격으로 게이지가 쌓입니다.\r\n" +
            " ▶ Z키를 통해 적을 <color=#ff0000>스턴</color> 시킬 수 있습니다.\r\n" +
            "[X] \r\n" +
            " ▶ 30초의 쿨타임을 가지는 스킬입니다.\r\n" +
            " ▶ X키를 통해 적을 <color=#ff0000>넉백</color> 시킬 수 있습니다.\n\n",
            null, "확인");
    }

    public void CheckPlayerStat()
    {
        PopupManager.Instance.Popup_OpenOk("<color=#000000>플레이어 정보</color>",
           " < 현재 씬: " + sceneName + " >\r\n\n" +
           " 킬 스코어: " + m_player.DeathEnemyCnt + " / " + m_player.TotalEnemyCnt + "\r\n" +
           " 공격력: " + m_player.PlayerAttack + "\r\n" +
           " 현재 체력: " + m_player.PlayerCurHp + " / " + m_player.PlayerMaxHp + "\r\n" +
           " 스킬 게이지: " + m_player.PlayerCurSkillGauge + " / " + m_player.PlayerMaxSkillGauge + "\r\n\n",
           null, "확인");
    }

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }
}