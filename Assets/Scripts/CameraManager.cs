using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera[] m_Cameras;

    private static int indexCameraSelected = 1;

    private void FixedUpdate()
    {
        HandleCameraChangeKey();
    }

    private void HandleCameraChangeKey()
    {
        if (Input.GetButtonDown("ChangeCamera"))
        {
            ChangeCamera();
        }
    }

    private void ChangeCamera()
    {
        int nextCameraWillBeSelected = indexCameraSelected + 1;

        if (nextCameraWillBeSelected >= m_Cameras.Length) nextCameraWillBeSelected = 0;

        m_Cameras[indexCameraSelected].enabled = false;
        m_Cameras[nextCameraWillBeSelected].enabled = true;

        indexCameraSelected = nextCameraWillBeSelected;
    }

    public void HandleCameraChangeUIButton()
    {
        ChangeCamera();
    }
}
