using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera virtualCamera;
    [SerializeField]
    CinemachineVirtualCamera virtualEffectCam;
    [SerializeField]
    CinemachineVirtualCamera virtualShieldCam;
    [SerializeField]
    CinemachineVirtualCamera virtualRunCam;

    public void SetTarget(Transform cameraRoot)
    {
        // 메인 카메라는 그대로 설정
        virtualCamera.Follow = cameraRoot;
        virtualCamera.LookAt = cameraRoot;

        virtualShieldCam.Follow = cameraRoot;
        virtualShieldCam.LookAt = cameraRoot;

        virtualRunCam.Follow = cameraRoot;
        virtualRunCam.LookAt = cameraRoot;

        virtualEffectCam.Follow = cameraRoot;
        virtualEffectCam.LookAt = cameraRoot;
    }
}