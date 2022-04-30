using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class CharController : MonoBehaviour
{
    [Header("Mvt Setup")]
    [Tooltip("unit m/s")]
    [SerializeField] private float m_TranslationSpeed;
    [Tooltip("unit m/s")]
    [SerializeField] private float m_JumpSpeed;
    [Tooltip("unit: �/s")]
    [SerializeField] private float m_RotatingSpeed;

    [Header("SFX")]
    [Tooltip("Audio clip MV3")]
    [SerializeField] private AudioClip m_CharacterWalkClip;
    [SerializeField] private AudioClip m_CharacterShootClip;
    [SerializeField] private AudioClip m_CharacterJumpClip;

    protected Rigidbody Rigidbody { get; set; }

    protected float TranslationSpeed { get => this.m_TranslationSpeed; }
    protected float JumpSpeed { get => this.m_JumpSpeed; }
    protected float RotatingSpeed { get => this.m_RotatingSpeed; }

    #region Character physics controls methods
    /// <summary>
    /// Translate the current object
    /// </summary>
    /// <param name="direction">An direction to translate</param>
    protected virtual void TranslateObject(Vector3 direction)
    {
        this.TranslateObject(1, direction);
    }

    /// <summary>
    /// Translate the object depenfing to an input and a direction
    /// </summary>
    /// <param name="verticalInput">Vlaue of the vertical input</param>
    /// <param name="direction">The vector direction</param>
    protected virtual void TranslateObject(float verticalInput, Vector3 direction)
    {
        Vector3 targetVelocity = this.m_TranslationSpeed * verticalInput * Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
        Vector3 velocityChange = targetVelocity - this.Rigidbody.velocity;
        this.Rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    /// <summary>
    /// Rotate the current object
    /// </summary>
    protected virtual void RotateObject()
    {
        this.RotateObject(1);
    }

    /// <summary>
    /// Rotate the current object depending on the horizontalInput
    /// </summary>
    /// <param name="horizontalInput">The horizontal input</param>
    protected virtual void RotateObject(float horizontalInput)
    {
        Vector3 targetAngularVelocity = horizontalInput * this.m_RotatingSpeed * this.transform.up;
        Vector3 angularVelocityChange = targetAngularVelocity - this.Rigidbody.angularVelocity;
        this.Rigidbody.AddTorque(angularVelocityChange, ForceMode.VelocityChange);
    }

    /// <summary>
    /// Move the current character
    /// </summary>
    protected virtual void Move()
    {
        this.TranslateObject(Vector3.left);
    }

    /// <summary>
    /// Jump the current character <see cref="PlayJumpSound"/>
    /// </summary>
    protected virtual void Jump()
    {
        this.Rigidbody.velocity = this.m_JumpSpeed * new Vector3(0, 1, 0);
        this.PlayJumpSound();
    }
    #endregion

    #region Character action control methods
    /// <summary>
    /// Shoot a current throwable Game object to a transform
    /// </summary>
    /// <param name="thowableGO">The GameObject will be thrown</param>
    /// <param name="position">The position</param>
    /// <param name="direction">The direction to send it</param>
    /// <param name="launchSpeed">The launch speed</param>
    /// <param name="lifeDuration">TH elife duration</param>
    protected virtual void Shoot(GameObject thowableGO, Vector3 position, Vector3 direction, float launchSpeed, float lifeDuration)
    {
        GameObject gameObject = Instantiate(thowableGO);
        gameObject.transform.position = position;

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = direction * launchSpeed;

        Destroy(gameObject, lifeDuration);
    }
    #endregion

    #region Character sound player

    /// <summary>
    /// Play walk Sound <see cref="PlaySFXEvent"/>
    /// </summary>
    /// <remarks>Stop the sound before playing <see cref="StopWalkSound"/></remarks>
    protected void PlayWalkSound()
    {
        this.StopJumpSound();
        EventManager.Instance.Raise(new PlaySFXEvent() { eAudioClip = m_CharacterWalkClip });
    }

    /// <summary>
    /// Stop Wall sound <see cref="StopSFXWithEvent"/>
    /// </summary>
    protected void StopWalkSound()
    {
        EventManager.Instance.Raise(new StopSFXWithEvent() { eAudioClip = this.m_CharacterWalkClip });
    }

    /// <summary>
    /// Play shoot Sound <see cref="PlaySFXEvent"/>
    /// </summary>
    /// <remarks>Stop the sound before playing <see cref="StopShootSound"/></remarks>
    protected void PlayShootSound()
    {
        this.StopShootSound();
        EventManager.Instance.Raise(new PlaySFXEvent() { eAudioClip = this.m_CharacterShootClip });
    }

    /// <summary>
    /// Stop shoot sound
    /// </summary>
    protected void StopShootSound()
    {
        EventManager.Instance.Raise(new StopSFXWithEvent() { eAudioClip = this.m_CharacterShootClip });
    }

    /// <summary>
    /// Play jump Sound <see cref="PlaySFXEvent"/>
    /// </summary>
    /// <remarks>Stop the sound before playing <see cref="StopJumpSound"/></remarks>
    protected void PlayJumpSound()
    {
        this.StopJumpSound();
        EventManager.Instance.Raise(new PlaySFXEvent() { eAudioClip = this.m_CharacterJumpClip });
    }

    /// <summary>
    /// Stop the jump sound
    /// </summary>
    protected void StopJumpSound()
    {
        EventManager.Instance.Raise(new StopSFXWithEvent() { eAudioClip = this.m_CharacterJumpClip });
    }

    /// <summary>
    /// Stop all sounds
    /// </summary>
    protected virtual void StopAllSounds()
    {
        this.StopWalkSound();
        this.StopJumpSound();
        this.StopShootSound();
    }

    #endregion

    #region MonoBehaviour METHODS
    protected virtual void Awake()
    {
        this.Rigidbody = this.GetComponent<Rigidbody>();
    }
    #endregion
}