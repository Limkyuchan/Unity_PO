using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameInformationMessage : MonoBehaviour
{
    [SerializeField]
    PlayerController m_player;

    string sceneName;

    public void IntroduceHowToPlayGame()
    {
        PopupManager.Instance.Popup_OpenOk("<color=#000000>게임 안내</color>",
            "적들을 모두 해치우고 보스를 무찌르는 게임입니다.\r\n" +
            "주인공 정보 확인: [Tab]\r\n\n" +
            "<color=#000000><기본 공격 사용 방법></color> \r\n" +
            "[Space]\r\n" +
            " - 콤보 공격(4회 중첩)이 가능합니다.\r\n" +
            " - 적을 해치우면 공격력이 2 증가합니다.\r\n\n" +
            "<color=#000000><스킬 공격 사용 방법></color> \r\n" +
            "[Z] \r\n" +
            " - 기본 공격으로 게이지가 쌓입니다.\r\n" +
            " - Z키를 통해 적을 <color=#ff0000>스턴</color> 시킬 수 있습니다.\r\n" +
            "[X] \r\n" +
            " - 30초의 쿨타임을 가지는 스킬입니다.\r\n" +
            " - X키를 통해 적을 <color=#ff0000>넉백</color> 시킬 수 있습니다.\n\n",
            null, "확인");
    }

    public void CheckPlayerStat()
    {
        PopupManager.Instance.Popup_OpenOk("<color=#000000>플레이어 정보</color>",
           " < 현재 씬: " + sceneName + " >\r\n\n" +
           " 킬 스코어: " + m_player.DeathEnemyCnt + "\r\n" +
           " 최대 체력: " + m_player.GetPlayerMaxHp + "\r\n" +
           " 체력: " + m_player.GetPlayerCurHp + "\r\n" +
           " 공격력: " + m_player.GetPlayerAttack + "\r\n" +
           " 방어력: " + m_player.GetPlayerDefense + "\r\n\n",
           null, "확인");
    }

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }
}
