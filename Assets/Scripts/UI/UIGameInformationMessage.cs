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
        PopupManager.Instance.Popup_OpenOk("<color=#000000>���� �ȳ�</color>",
            "< ���� ���� > \r\n" +
            "��� ���� ������ ��ġ��� �����Դϴ�.\r\n" +
            "���� ����ǵ� ������ ����˴ϴ�.\r\n" +
            "���� ��ġ��� �������� ȿ���� ����ϴ�.\r\n" +
            " �� 25% Ȯ��: <color=#ff0000>ü��</color> ���� (+ 20) \r\n" +
            " �� 25% Ȯ��: <color=#ff0000>��ų ������</color> ���� (+ 10) \r\n" +
            " �� 25% Ȯ��: <color=#ff0000>���ݷ�</color> ���� (+ 2) \r\n\n" +
            "< �⺻ Ű > \r\n" +
            "[Tab] \r\n" +
            " �� �÷��̾��� ������ Ȯ���� �� �ֽ��ϴ�.\r\n" +
            "[Left Shift] \r\n" +
            " �� �÷��̾��� �̵��ӵ��� �����մϴ�.\r\n" +
            "[C] \r\n" +
            " �� �÷��̾ ���⸦ ��� ����մϴ�.\r\n" +
            "[Space] \r\n" +
            " �� <color=#ff0000>�޺� ����</color>(4ȸ ��ø)�� �����մϴ�.\r\n\n" +
            "< ��ų ���� > \r\n" +
            "[Z] \r\n" +
            " �� �⺻ �������� �������� ���Դϴ�.\r\n" +
            " �� ZŰ�� ���� ���� <color=#ff0000>����</color> ��ų �� �ֽ��ϴ�.\r\n" +
            "[X] \r\n" +
            " �� 30���� ��Ÿ���� ������ ��ų�Դϴ�.\r\n" +
            " �� XŰ�� ���� ���� <color=#ff0000>�˹�</color> ��ų �� �ֽ��ϴ�.\n\n",
            null, "Ȯ��");
    }

    public void CheckPlayerStat()
    {
        PopupManager.Instance.Popup_OpenOk("<color=#000000>�÷��̾� ����</color>",
           " < ���� ��: " + sceneName + " >\r\n\n" +
           " ų ���ھ�: " + m_player.DeathEnemyCnt + " / " + m_player.TotalEnemyCnt + "\r\n" +
           " ���ݷ�: " + m_player.PlayerAttack + "\r\n" +
           " ���� ü��: " + m_player.PlayerCurHp + " / " + m_player.PlayerMaxHp + "\r\n" +
           " ��ų ������: " + m_player.PlayerCurSkillGauge + " / " + m_player.PlayerMaxSkillGauge + "\r\n\n",
           null, "Ȯ��");
    }

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }
}