using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationMessage : MonoBehaviour
{
    int m_popupCount = 0;

    public void IntroduceHowToPlayGame()
    {
        m_popupCount++;
        if (m_popupCount == 1)
        {
            PopupManager.Instance.Popup_OpenOk("<color=#000000>게임 안내</color>",
            " 적들을 모두 해치우고 스테이지를 이동하며 보스를 잡는 게임입니다.\r\n\n" +
            "<color=#000000><기본 공격 사용 방법></color> \r\n" +
            "[Space]\r\n" +
            " - 콤보 공격(4회 중첩)이 가능합니다.\r\n\n" +
            "<color=#000000><스킬 공격 사용 방법></color> \r\n" +
            "[Z] \r\n" +
            " - 기본 공격으로 게이지가 쌓입니다.\r\n" +
            " - Z키를 통해 적을 <color=#ff0000>스턴</color> 시킬 수 있습니다.\r\n" +
            "[X] \r\n" +
            " - 30초의 쿨타임을 가지는 스킬입니다.\r\n" +
            " - X키를 통해 적을 <color=#ff0000>넉백</color> 시킬 수 있습니다.\n\n",
            null, "확인");
        }
        else
        {
            return;
        }
    }
}