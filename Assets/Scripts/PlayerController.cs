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

    private bool m_IsOnGround = true;
    private Rigidbody m_Rigidbody;


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
        Jump();

        if (m_IsOnGround)
        {
            ResetRbVelocity();
        }
    }

    /**
     * <summary>Move the player according to the Vertical and Horizontal input</summary> 
     */
    private void Move()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        //VERTICAL INPUT
        Vector3 worldmovVect = m_TranslationSpeed * Time.fixedDeltaTime * transform.forward * verticalInput;
        m_Rigidbody.MovePosition(transform.position + worldmovVect);

        //HORIZONTAL INPUT
        float deltaAngle = m_RotatingSpeed * Time.fixedDeltaTime * horizontalInput;
        Quaternion qRot = Quaternion.AngleAxis(deltaAngle, transform.up);
        m_Rigidbody.MoveRotation(qRot * transform.rotation);
    }

    private void Jump()
    {
        if (m_IsOnGround && Input.GetButton("Jump"))
        {
            m_IsOnGround = false;
            Vector3 worldmovVect = m_JumpSpeed * Time.fixedDeltaTime * transform.up;
            m_Rigidbody.MovePosition(transform.position + worldmovVect);
        }
    }

    private void ResetRbVelocity()
    {
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Finish"))
        {
            EventManager.Instance.Raise(new LevelFinishEvent());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<CanJump>())
        {
            m_IsOnGround = true;
        }
        else if (collision.gameObject.tag.Equals("Ennemy"))
        {
            EventManager.Instance.Raise(new GameOverEvent());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<CanJump>())
        {
            m_IsOnGround = false;
        }
    }
}
