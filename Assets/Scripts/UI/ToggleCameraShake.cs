using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleCameraShake : MonoBehaviour
{
    [SerializeField]
    Toggle toggleOn;
    [SerializeField]
    Toggle toggleOff;

    [SerializeField]
    PlayerController playerController;

    public void ToggleClick(bool isOn)
    {
        if (toggleOn.isOn)
        {
            Debug.Log("카메라 진동 On");
            //playerController.SetCameraShake(true);
        }
        else if (toggleOff.isOn)
        {
            Debug.Log("카메라 진동 Off");
            //playerController.SetCameraShake(false);
        }
    }

    void Start()
    {
        toggleOn.isOn = true;
        toggleOff.isOn = false;

        toggleOn.onValueChanged.AddListener(ToggleClick);
        toggleOff.onValueChanged.AddListener(ToggleClick);

        //playerController.SetCameraShake(toggleOn.isOn);
    }
}