using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class SfxManager : Manager<SfxManager>, IEventHandler
{
    [SerializeField] private AudioSource m_SfxAudioSource;
    private List<AudioSource> m_AudioSources;

    public AudioSource SfxAudioSource { get => this.m_SfxAudioSource; }

    #region Event Listeners Methods
    private void OnPlaySFXEvent(PlaySFXEvent e)
    {
        this.StartSFX(e.eAudioSource, e.eAudioClip);
    }

    private void OnStopSFXEvent(StopSFXWithEvent e)
    {
        this.StopSFX(e.eAudioSource);
    }
    #endregion

    #region SFX Manager methods

    /**
     * <summary>Start a SFX sound with audiosource or audioclip or both</summary> 
     * <param name="audioSource">The audio source</param>
     * <param name="audioClip">The audio clip</param>
     */
    public void StartSFX(AudioSource audioSource, AudioClip audioClip)
    {
        if (!audioClip)
        {
            this.StartSFX(audioSource);
        }
        else if (!audioSource)
        {
            this.StartSFX(audioClip);
        }
        else
        {
            this.StopSFX(audioSource);
            audioSource.clip = audioClip;
            this.StartSFX(audioSource);
        }
    }

    /**
     * <summary>Start an sfx with audio clip</summary>
     * <remarks>The audioclip will be played on the SFXManger audio source</remarks>
     * <param name="audioClip">The audio clip to play</param>
     */
    public void StartSFX(AudioClip audioClip)
    {
        if (audioClip)
        {
            if (!Object.Equals(this.SfxAudioSource.clip, audioClip))
            {
                if (!this.SfxAudioSource.isPlaying)
                {
                    this.SfxAudioSource.clip = audioClip;
                    this.StartSFX(this.SfxAudioSource);
                }
                else
                {
                    this.StopSFX();
                }   
            }
            
            
        }
    }

    /**
     * <summary>Start a SFX sound with audiosource</summary> 
     * <param name="audioSource">The audio source</param>
     */
    public void StartSFX(AudioSource audioSource)
    {
        if (audioSource) {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                this.m_AudioSources.Add(audioSource);
            }
            else
            {
                this.StopSFX(audioSource);
            }
        }
        
    }

    /**
     * <summary>Stop SFX of the SFXMangager audio source</summary> 
     */
    public void StopSFX()
    {
        this.StopSFX(this.SfxAudioSource);
    }

    /**
     * <summary>Stop SFX with audio source</summary> 
     * <param name="audioSource">The audio source will stop sound</param>
     */
    public void StopSFX(AudioSource audioSource)
    {
        if (audioSource) { 
            audioSource.Stop();
            audioSource.clip = null;
            this.m_AudioSources.Remove(audioSource);
        }else if (this.SfxAudioSource && this.SfxAudioSource.isPlaying)
        {
            this.StopSFX();
        }
    }

    /**
     * <summary>Stop all save SFX</summary> 
     */
    private void StopAllSaveSFX()
    {
        for (int i = 0; i < this.m_AudioSources.Count; i++)
        {
            this.StopSFX(this.m_AudioSources[i]);
        }
    }

    /**
     * <summary>Remove all audiosource in list which not playing</summary>
     */
    private void RemoveAllUselessSFX()
    {
        this.m_AudioSources.RemoveAll((AudioSource currentAudioSource) => { 
            if (!currentAudioSource.isPlaying) currentAudioSource.clip = null; 
            return !currentAudioSource.isPlaying; 
        });
    }
    #endregion

    #region Events Suscribption
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<PlaySFXEvent>(OnPlaySFXEvent);
        EventManager.Instance.AddListener<StopSFXWithEvent>(OnStopSFXEvent);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<PlaySFXEvent>(OnPlaySFXEvent);
        EventManager.Instance.RemoveListener<StopSFXWithEvent>(OnStopSFXEvent);
    }
    #endregion

    #region MonoBehaviour METHODS

    private void Awake()
    {
        base.InitManager();
        this.m_AudioSources = new List<AudioSource>();
        this.SubscribeEvents();
    }

    private void FixedUpdate()
    {
       this.RemoveAllUselessSFX();
    }

    private void OnDestroy()
    {
        this.UnsubscribeEvents();
        this.StopAllSaveSFX();
    }

    #endregion
}
