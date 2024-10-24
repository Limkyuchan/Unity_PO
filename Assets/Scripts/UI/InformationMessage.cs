using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InformationMessage : MonoBehaviour
{
    public void IntroduceHowToPlayGame()
    {
        PopupManager.Instance.Popup_OpenOk("<color=#000000>게임 안내</color>",
            " 적들을 모두 해치우고 스테이지를 이동하며 보스를 잡는 게임입니다.\r\n\n" +
            "<color=#708090><기본 공격 사용 방법></color> \r\n" +
            "[Space] 4회까지 콤보 공격이 가능합니다.\r\n" +
            "<color=#708090><스킬 공격 사용 방법></color> \r\n" +
            "[Z] 기본 공격으로 게이지가 쌓입니다.\r\n" +
            "[X] 30초의 쿨타임을 가지는 스킬입니다.\n\n",
            null, "확인");
    }

    public void PlayerDieGameOver()
    {
        PopupManager.Instance.Popup_OpenOkCancel("<color=#ff0000>게임 종료!</color>", 
            "플레이어가 사망하여 게임이 종료되었습니다. \r\n" +
            "\"확인\" 클릭 시 타이틀 화면으로 이동합니다. \r\n" +
            "\"종료\" 클릭 시 게임을 종료합니다.", () => 
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
            }, "확인", "취소");
    }
}