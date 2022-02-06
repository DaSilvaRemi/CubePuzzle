using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Cameras")]
    [Tooltip("Camera")]
    /**
     * <summary>Array of cameras</summary> 
     */
    [SerializeField] private Camera[] m_Cameras;

    private static int m_IndexCameraSelected = 1;

    /**
     * <summary>Handle the camera change UI button</summary> 
     */
    public void HandleCameraChangeUIButton()
    {
        ChangeCamera();
    }

    private void FixedUpdate()
    {
        HandleCameraChangeKey();
    }

    /**
     * <summary>Handle the camera change key</summary> 
     */
    private void HandleCameraChangeKey()
    {
        if (Input.GetButtonDown("ChangeCamera"))
        {
            ChangeCamera();
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
}
