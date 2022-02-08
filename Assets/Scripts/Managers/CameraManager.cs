using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Cameras")]
    [Tooltip("Camera")]
    [SerializeField] private Camera[] m_Cameras;
    [Tooltip("Unit : s")]
    [SerializeField] private float m_CooldownDuration;

    private int m_IndexCameraSelected = 1;

    private float m_NextCameraChangedTime;

    /**
     * <summary>Handle the camera change UI button</summary> 
     */
    public void HandleCameraChangeUIButton()
    {
        ChangeCamera();
    }

    /**
     * <summary>Handle the camera change key</summary> 
     */
    private void HandleCameraChangeKey()
    {
        if (Input.GetButton("ChangeCamera") && Time.time > m_NextCameraChangedTime)
        {
            ChangeCamera();
            m_NextCameraChangedTime = Time.time + m_CooldownDuration;
        }
    }

    /**
     * <summary>Change the camera</summary>
     */
    private void ChangeCamera()
    {
        int nextCameraWillBeSelected = m_IndexCameraSelected + 1;

        if (nextCameraWillBeSelected >= m_Cameras.Length) nextCameraWillBeSelected = 0;

        m_Cameras[m_IndexCameraSelected].enabled = false;
        m_Cameras[nextCameraWillBeSelected].enabled = true;

        m_IndexCameraSelected = nextCameraWillBeSelected;
    }

    private void Start()
    {
        m_NextCameraChangedTime = Time.time;
    }

    private void FixedUpdate()
    {
        HandleCameraChangeKey();
    }
}
