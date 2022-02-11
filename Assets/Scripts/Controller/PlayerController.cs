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
    [Tooltip("Element")]
    [SerializeField] private Transform m_ThrowableGOSpawnTransform;
    [Tooltip("Unit : s")]
    [SerializeField] private float m_ThrowableGOLifeDuration;
    [Tooltip("Unit : s")]
    [SerializeField] private float m_CooldownDuration;

    private bool m_IsOnGround = true;
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
            base.TranslateObject(verticalInput, transform.forward);
            base.RotateObject(horizontalInput);

        }
    }

    protected override void Jump()
    {
        if (this.m_IsOnGround && Input.GetButton("Jump"))
        {
            m_IsOnGround = false;
            base.Jump();
        }
    }

    private void Shoot()
    {
        base.Shoot(m_ThrowableGOPrefab, m_ThrowableGOSpawnTransform.position, m_ThrowableGOSpawnTransform.forward, m_ThrowableGOInitSpeed, m_ThrowableGOLifeDuration);
    }

    #region MonoBehaviour METHODS

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            EventManager.Instance.Raise(new LevelFinishEvent());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Ground>())
        {
            m_IsOnGround = true;
        }
        
        if (collision.gameObject.CompareTag("Ennemy"))
        {
            EventManager.Instance.Raise(new LevelGameOverEvent());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Ground>())
        {
            m_IsOnGround = false;
        }
    }

    private void FixedUpdate()
    {
        Move();
        Jump();

        if ((GameManager.Instance && GameManager.Instance.IsShootableScene) && (Input.GetButton("Fire1") && Time.time > m_NextShootTime))
        {
            Shoot();
        }
    }
    #endregion
}
