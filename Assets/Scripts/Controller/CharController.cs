using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    [Header("Mvt Setup")]
    [Tooltip("unit m/s")]
    [SerializeField] private float m_TranslationSpeed;
    [Tooltip("unit m/s")]
    [SerializeField] private float m_JumpSpeed;
    [Tooltip("unit: °/s")]
    [SerializeField] private float m_RotatingSpeed;

    protected Rigidbody Rigidbody { get; set; }

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

    protected virtual void Jump()
    {
        this.Rigidbody.velocity = this.m_JumpSpeed * new Vector3(0, 1, 0);
    }

    protected virtual void Shoot(GameObject thowableGO, Vector3 position, Vector3 direction, float launchSpeed, float lifeDuration)
    {
        GameObject gameObject = Instantiate(thowableGO);
        gameObject.transform.position = position;

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = direction * launchSpeed;

        Destroy(gameObject, lifeDuration);
    }

    #region MonoBehaviour METHODS
    private void Awake()
    {
        this.Rigidbody = GetComponent<Rigidbody>();
    }
    #endregion
}