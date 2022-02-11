using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class PlayerController : MonoBehaviour
{
    [Header("Mvt Setup")]
    [Tooltip("unit m/s")]
    [SerializeField] private float m_TranslationSpeed;
    [Tooltip("unit m/s")]
    [SerializeField] private float m_JumpSpeed;
    [Tooltip("unit: °/s")]
    [SerializeField] private float m_RotatingSpeed;

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
    private Rigidbody m_Rigidbody;

    /**
     * <summary>Move the player according to the Vertical and Horizontal input</summary> 
     */
    private void Move()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        if (this.m_IsOnGround)
        {
            //MODE VELOCITY
            Vector3 targetVelocity = m_TranslationSpeed * Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized * verticalInput;
            Vector3 velocityChange = targetVelocity - m_Rigidbody.velocity;
            m_Rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

            Vector3 targetAngularVelocity = horizontalInput * m_RotatingSpeed * transform.up;
            Vector3 angularVelocityChange = targetAngularVelocity - m_Rigidbody.angularVelocity;
            m_Rigidbody.AddTorque(angularVelocityChange, ForceMode.VelocityChange);

        }
    }

    private void Jump()
    {
        if (this.m_IsOnGround && Input.GetButton("Jump"))
        {
            m_IsOnGround = false;
            Vector3 targetVelocity = m_TranslationSpeed * Vector3.ProjectOnPlane(transform.up, Vector3.up).normalized;
            Vector3 velocityChange = targetVelocity - m_Rigidbody.velocity;
            m_Rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }

    private void Shoot()
    {
        GameObject newBallGO = Instantiate(m_ThrowableGOPrefab);
        newBallGO.transform.position = m_ThrowableGOSpawnTransform.position;

        Rigidbody rb = newBallGO.GetComponent<Rigidbody>();
        rb.velocity = m_ThrowableGOSpawnTransform.forward * m_ThrowableGOInitSpeed;

        Destroy(newBallGO, m_ThrowableGOLifeDuration);
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
            Tools.Log(this, "in collision");
            m_IsOnGround = true;
        }
        
        if (collision.gameObject.CompareTag("Ennemy"))
        {
            EventManager.Instance.Raise(new GameOverEvent());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Ground>())
        {
            m_IsOnGround = false;
        }
    }

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Tools.Log(this, m_IsOnGround.ToString());

        Move();
        Jump();

        if ((GameManager.Instance && GameManager.Instance.IsShootableScene) && (Input.GetButton("Fire1") && Time.time > m_NextShootTime))
        {
            Shoot();
        }
    }
    #endregion
}
