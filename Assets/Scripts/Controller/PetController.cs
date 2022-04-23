using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetController : CharController
{
    [Header("Pet properties")]
    [SerializeField] private float m_DistanceBetweenOwnerRange = 3f;
    private Transform m_PlayerTransform;

    protected override void Move()
    {
        if (Vector3.Distance(this.m_PlayerTransform.position, base.Rigidbody.position) > this.m_DistanceBetweenOwnerRange)
        {
            Vector3 playerTargetPosition = new Vector3(this.m_PlayerTransform.position.x, this.m_PlayerTransform.position.y, this.m_PlayerTransform.position.z);
            Vector3 petNewPosition = Vector3.MoveTowards(base.Rigidbody.position, playerTargetPosition, base.TranslationSpeed * Time.fixedDeltaTime);
            base.Rigidbody.MovePosition(petNewPosition);
        }
    }

    #region MonoBehaviour Methods
    protected override void Awake()
    {
        base.Awake();
        this.m_PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }


    private void FixedUpdate()
    {
        this.Move();
    }
    #endregion
}
