using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class ButtonManager : Manager<CameraManager>, IEventHandler
{
    [Tooltip("Button audio clip")]
    [SerializeField] private AudioClip m_ButtonClickSfxClip;

    #region Handler
    public void HandleDefaultClickButton()
    {
        this.HandleCustomDefaultClickButton(this.m_ButtonClickSfxClip);
    }
    #endregion

    #region Events Handler
    public void HandleCustomDefaultClickButton(AudioClip audioClip)
    {
        EventManager.Instance.Raise(new PlaySFXEvent() { eAudioClip = audioClip });
    }

    public void OnCustomButtonClickedEven(CustomButtonClickedEvent e)
    {
        this.HandleCustomDefaultClickButton(e.eOnCustomButtonClick);
    }

    public void OnButtonClickedEvent(ButtonClickedEvent e)
    {
        this.HandleDefaultClickButton();
    }
    #endregion

    #region Events Suscribption
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<ButtonClickedEvent>(OnButtonClickedEvent);
        EventManager.Instance.AddListener<CustomButtonClickedEvent>(OnCustomButtonClickedEven);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<ButtonClickedEvent>(OnButtonClickedEvent);
        EventManager.Instance.RemoveListener<CustomButtonClickedEvent>(OnCustomButtonClickedEven);
    }
    #endregion

    #region MonoBehaviour METHODS
    private void Awake()
    {
        base.InitManager();
    }
    #endregion
}
