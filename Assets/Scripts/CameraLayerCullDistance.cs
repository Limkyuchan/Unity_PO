using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLayerCullDistance : MonoBehaviour
{
    [SerializeField]
    float[] layerCallDist = new float[32];

    Camera m_camera;

    void Start()
    {
        m_camera = GetComponent<Camera>();
        m_camera.layerCullDistances = layerCallDist;
    }
}