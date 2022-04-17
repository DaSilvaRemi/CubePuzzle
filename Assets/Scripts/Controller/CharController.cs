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
    protected virtual void TranslateObject(Vector3 direction)
    {
        this.TranslateObject(1, direction);
    }

    protected virtual void TranslateObject(float verticalInput, Vector3 direction)
    {
        Vector3 targetVelocity = this.m_TranslationSpeed * verticalInput * Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
        Vector3 velocityChange = targetVelocity - this.Rigidbody.velocity;
        this.Rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    protected virtual void RotateObject()
    {
        this.RotateObject(1);
    }

    protected virtual void RotateObject(float horizontalInput)
    {
        Vector3 targetAngularVelocity = horizontalInput * this.m_RotatingSpeed * this.transform.up;
        Vector3 angularVelocityChange = targetAngularVelocity - this.Rigidbody.angularVelocity;
        this.Rigidbody.AddTorque(angularVelocityChange, ForceMode.VelocityChange);
    }

    protected virtual void Move()
    {
        this.TranslateObject(Vector3.left);
    }

    protected virtual void Jump()
    {
        this.Rigidbody.velocity = this.m_JumpSpeed * new Vector3(0, 1, 0);
        this.PlayJumpSound();
    }
    #endregion

    #region Character action control methods
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

    protected void PlayWalkSound()
    {
        this.StopJumpSound();
        EventManager.Instance.Raise(new PlaySFXEvent() { eAudioClip = m_CharacterWalkClip });
    }

    protected void StopWalkSound()
    {
        EventManager.Instance.Raise(new StopSFXWithEvent() { eAudioClip = this.m_CharacterWalkClip });
    }

    protected void PlayShootSound()
    {
        EventManager.Instance.Raise(new PlaySFXEvent() { eAudioClip = this.m_CharacterShootClip });
    }

    protected void StopShootSound()
    {
        EventManager.Instance.Raise(new StopSFXWithEvent() { eAudioClip = this.m_CharacterShootClip });
    }

    protected void PlayJumpSound()
    {
        this.StopWalkSound();
        EventManager.Instance.Raise(new PlaySFXEvent() { eAudioClip = this.m_CharacterJumpClip });
    }

    protected void StopJumpSound()
    {
        EventManager.Instance.Raise(new StopSFXWithEvent() { eAudioClip = this.m_CharacterJumpClip });
    }

    protected virtual void StopAllSounds()
    {
        this.StopWalkSound();
        this.StopJumpSound();
        this.StopShootSound();
    }

    #endregion

    #region MonoBehaviour METHODS
    private void Awake()
    {
        this.Rigidbody = this.GetComponent<Rigidbody>();
    }
    #endregion
}