using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMove : MonoBehaviour
{
    public void GoGameSettingScene()
    {
        LoadSceneManager.Instance.LoadSceneAsync(SceneState.GameSettingScene);
    }

    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}