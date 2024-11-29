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

    PlayerController playerController;

    public void SetPlayer(PlayerController player)
    {
        playerController = player;
    }

    public void ToggleClick(bool isOn)
    {
        if (playerController == null) return;

        if (toggleOn.isOn)
        {
            playerController.SetCameraShake(true);
        }
        else if (toggleOff.isOn)
        {
            playerController.SetCameraShake(false);
        }
    }

    void Start()
    {
        toggleOn.isOn = true;
        toggleOff.isOn = false;

        toggleOn.onValueChanged.AddListener(ToggleClick);
        toggleOff.onValueChanged.AddListener(ToggleClick);

        if (playerController != null)
        {
            playerController.SetCameraShake(toggleOn.isOn);
        }
    }
}