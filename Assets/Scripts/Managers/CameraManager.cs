using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Manager<CameraManager>
{
    [Header("Cameras")]
    [Tooltip("Camera")]
    [SerializeField] private Camera[] m_Cameras;
    [Tooltip("Unit : s")]
    [SerializeField] private float m_CooldownDuration;

    private int m_IndexCameraSelected = 1;
    private float m_NextCameraChangedTime;

    #region Handler
    /**
     * <summary>Handle the camera change UI button</summary> 
     */
    public void HandleCameraChangeUIButton()
    {
        this.ChangeCamera();
    }

    /**
     * <summary>Handle the camera change key</summary> 
     */
    private void HandleCameraChangeKey()
    {
        if (Input.GetButton("ChangeCamera") && Time.time > this.m_NextCameraChangedTime)
        {
            this.ChangeCamera();
            this.m_NextCameraChangedTime = Time.time + this.m_CooldownDuration;
        }
    }
    #endregion

    /**
     * <summary>Change the camera</summary>
     */
    private void ChangeCamera()
    {
        int nextCameraWillBeSelected = this.m_IndexCameraSelected + 1;

        if (nextCameraWillBeSelected >= this.m_Cameras.Length) nextCameraWillBeSelected = 0;

        this.m_Cameras[this.m_IndexCameraSelected].enabled = false;
        this.m_Cameras[nextCameraWillBeSelected].enabled = true;

        this.m_IndexCameraSelected = nextCameraWillBeSelected;
    }

    #region MonoBehaviour methods

    private void Awake()
    {
        base.InitManager();
    }

    private void Start()
    {
        this.m_NextCameraChangedTime = Time.time;
    }

    private void FixedUpdate()
    {
        this.HandleCameraChangeKey();
    }
    #endregion
}
