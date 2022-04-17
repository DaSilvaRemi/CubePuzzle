using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class PlayerController : CharController
{
    [Header("Throwable Gameobjects Settings")]
    [Tooltip("Prefab")]
    [SerializeField] private GameObject m_ThrowableGOPrefab;
    [Tooltip("Unit: m/s")]
    [SerializeField] private float m_ThrowableGOInitSpeed;
    [Tooltip("Transform component")]
    [SerializeField] private Transform m_ThrowableGOSpawnTransform;
    [Tooltip("Unit : s")]
    [SerializeField] private float m_ThrowableGOLifeDuration;
    [Tooltip("Unit : s")]
    [SerializeField] private float m_CooldownDuration;
    
    private bool m_IsOnGround = false;
    private float m_NextShootTime;

    /**
     * <summary>Move the player according to the Vertical and Horizontal input</summary> 
     */
    private void Move()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        if (this.m_IsOnGround)
        {
            if(verticalInput == 0 || horizontalInput == 0) base.PlayWalkSound();

            base.TranslateObject(verticalInput, transform.forward);
            base.RotateObject(horizontalInput);
        }
    }

    protected override void Jump()
    {
        if (this.m_IsOnGround && Input.GetButton("Jump"))
        {
            this.m_IsOnGround = false;
            base.Jump();
        }
    }

    private void Shoot()
    {
        base.Shoot(this.m_ThrowableGOPrefab, this.m_ThrowableGOSpawnTransform.position, this.m_ThrowableGOSpawnTransform.forward, this.m_ThrowableGOInitSpeed, this.m_ThrowableGOLifeDuration);
    }

    private void SetIsOnGround(bool isOnGround)
    {
        this.m_IsOnGround = isOnGround;
    }

    #region MonoBehaviour METHODS

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            EventManager.Instance.Raise(new LevelFinishEvent());
            base.StopAllSounds();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EventManager.Instance.Raise(new LevelGameOverEvent());
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<Ground>())
        {
            this.SetIsOnGround(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Ground>())
        {
            this.SetIsOnGround(false);
        }
    }

    private void FixedUpdate()
    {
        this.Move();
        this.Jump();

        if ((GameManager.Instance && GameManager.Instance.IsShootableScene) && (Input.GetButton("Fire1") && Time.time > this.m_NextShootTime))
        {
            this.Shoot();
            this.m_NextShootTime = Time.time + this.m_CooldownDuration;
        }
    }
    #endregion
}
