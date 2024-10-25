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
            PopupManager.Instance.Popup_OpenOk("<color=#000000>���� �ȳ�</color>",
            " ������ ��� ��ġ��� ���������� �̵��ϸ� ������ ��� �����Դϴ�.\r\n\n" +
            "<color=#000000><�⺻ ���� ��� ���></color> \r\n" +
            "[Space]\r\n" +
            " - �޺� ����(4ȸ ��ø)�� �����մϴ�.\r\n\n" +
            "<color=#000000><��ų ���� ��� ���></color> \r\n" +
            "[Z] \r\n" +
            " - �⺻ �������� �������� ���Դϴ�.\r\n" +
            " - ZŰ�� ���� ���� <color=#ff0000>����</color> ��ų �� �ֽ��ϴ�.\r\n" +
            "[X] \r\n" +
            " - 30���� ��Ÿ���� ������ ��ų�Դϴ�.\r\n" +
            " - XŰ�� ���� ���� <color=#ff0000>�˹�</color> ��ų �� �ֽ��ϴ�.\n\n",
            null, "Ȯ��");
        }
        else
        {
            return;
        }
    }
}