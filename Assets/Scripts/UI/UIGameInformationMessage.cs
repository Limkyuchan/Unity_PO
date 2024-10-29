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
        PopupManager.Instance.Popup_OpenOk("<color=#000000>���� �ȳ�</color>",
            "������ ��� ��ġ��� ������ ����� �����Դϴ�.\r\n" +
            "���ΰ� ���� Ȯ��: [Tab]\r\n\n" +
            "<color=#000000><�⺻ ���� ��� ���></color> \r\n" +
            "[Space]\r\n" +
            " - �޺� ����(4ȸ ��ø)�� �����մϴ�.\r\n" +
            " - ���� ��ġ��� ���ݷ��� 2 �����մϴ�.\r\n\n" +
            "<color=#000000><��ų ���� ��� ���></color> \r\n" +
            "[Z] \r\n" +
            " - �⺻ �������� �������� ���Դϴ�.\r\n" +
            " - ZŰ�� ���� ���� <color=#ff0000>����</color> ��ų �� �ֽ��ϴ�.\r\n" +
            "[X] \r\n" +
            " - 30���� ��Ÿ���� ������ ��ų�Դϴ�.\r\n" +
            " - XŰ�� ���� ���� <color=#ff0000>�˹�</color> ��ų �� �ֽ��ϴ�.\n\n",
            null, "Ȯ��");
    }

    public void CheckPlayerStat()
    {
        PopupManager.Instance.Popup_OpenOk("<color=#000000>�÷��̾� ����</color>",
           " < ���� ��: " + sceneName + " >\r\n\n" +
           " ų ���ھ�: " + m_player.DeathEnemyCnt + "\r\n" +
           " �ִ� ü��: " + m_player.GetPlayerMaxHp + "\r\n" +
           " ü��: " + m_player.GetPlayerCurHp + "\r\n" +
           " ���ݷ�: " + m_player.GetPlayerAttack + "\r\n" +
           " ����: " + m_player.GetPlayerDefense + "\r\n\n",
           null, "Ȯ��");
    }

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }
}
