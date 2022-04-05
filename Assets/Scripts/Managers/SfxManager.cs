using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class SfxManager : Manager<SfxManager>, IEventHandler
{
    [SerializeField] private AudioSource[] m_SfxAudioSources;
    private List<AudioSource> m_SaveAudioSources;

    public AudioSource[] SfxAudioSource { get => this.m_SfxAudioSources; }

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
    private void OnAwakeSFXManager()
    {
        base.InitManager();
        this.m_SaveAudioSources = new List<AudioSource>();
        this.SubscribeEvents();

        if (this.m_SfxAudioSources.Length < 2)
        {
            Tools.LogWarning(this, "You should be set 2 audio sources");
        }
    }


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
            for (int i = 0; i < this.m_SfxAudioSources.Length; i++)
            {
                AudioSource audioSource = this.m_SfxAudioSources[i];
                if (!Object.Equals(audioSource.clip, audioClip) && !audioSource.isPlaying)
                {
                    audioSource.clip = audioClip;
                    this.StartSFX(audioSource);
                    break;
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
                this.m_SaveAudioSources.Add(audioSource);
            }
            else
            {
                this.StopSFX(audioSource);
            }
        }
        
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
            this.m_SaveAudioSources.Remove(audioSource);
        }
    }

    /**
     * <summary>Stop all save SFX</summary> 
     */
    private void StopAllSaveSFX()
    {
        for (int i = 0; i < this.m_SaveAudioSources.Count; i++)
        {
            this.StopSFX(this.m_SaveAudioSources[i]);
        }
    }

    /**
     * <summary>Remove all audiosource in list which not playing</summary>
     */
    private void RemoveAllUselessSFX()
    {
        this.m_SaveAudioSources.RemoveAll((AudioSource currentAudioSource) => { 
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
        this.OnAwakeSFXManager();
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
