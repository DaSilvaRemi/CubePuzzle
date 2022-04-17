using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

/**
 * <summary>Manager the SFX of all buttons after a click</summary> 
 */
public class ButtonManager : Manager<ButtonManager>, IEventHandler
{
    [Tooltip("Button audio clip")]
    [SerializeField] private AudioClip m_ButtonClickSfxClip;

    #region Handler
    /**
     * <summary>Handle a button click</summary> 
     */
    public void HandleDefaultClickButton()
    {
        this.HandleCustomDefaultClickButton(this.m_ButtonClickSfxClip);
    }

    /**
     * <summary>Handle a custom button click</summary> 
     * <param name="audioClip">The audio clip to play after clicking</param>
     */
    public void HandleCustomDefaultClickButton(AudioClip audioClip)
    {
        EventManager.Instance.Raise(new PlaySFXEvent() { eAudioClip = audioClip });
    }
    #endregion

    #region Events Handler
    /**
     * <summary>Handle the CustomButtonClickedEvent</summary>
     * <param name="e">The event</param> 
     */
    public void OnCustomButtonClickedEven(CustomButtonClickedEvent e)
    {
        this.HandleCustomDefaultClickButton(e.eOnCustomButtonClick);
    }

    /**
     * <summary>Handle the ButtonClickedEvent</summary>
     * <param name="e">The event</param> 
     */
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
        this.SubscribeEvents();
    }

    private void OnDestroy()
    {
        this.UnsubscribeEvents();
    }
    #endregion
}
