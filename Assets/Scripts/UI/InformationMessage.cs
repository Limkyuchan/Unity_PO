using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InformationMessage : MonoBehaviour
{
    public void IntroduceHowToPlayGame()
    {
        PopupManager.Instance.Popup_OpenOk("<color=#000000>���� �ȳ�</color>",
            " ������ ��� ��ġ��� ���������� �̵��ϸ� ������ ��� �����Դϴ�.\r\n\n" +
            "<color=#708090><�⺻ ���� ��� ���></color> \r\n" +
            "[Space] 4ȸ���� �޺� ������ �����մϴ�.\r\n" +
            "<color=#708090><��ų ���� ��� ���></color> \r\n" +
            "[Z] �⺻ �������� �������� ���Դϴ�.\r\n" +
            "[X] 30���� ��Ÿ���� ������ ��ų�Դϴ�.\n\n",
            null, "Ȯ��");
    }

    public void PlayerDieGameOver()
    {
        PopupManager.Instance.Popup_OpenOkCancel("<color=#ff0000>���� ����!</color>", 
            "�÷��̾ ����Ͽ� ������ ����Ǿ����ϴ�. \r\n" +
            "\"Ȯ��\" Ŭ�� �� Ÿ��Ʋ ȭ������ �̵��մϴ�. \r\n" +
            "\"����\" Ŭ�� �� ������ �����մϴ�.", () => 
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
            }, "Ȯ��", "���");
    }
}