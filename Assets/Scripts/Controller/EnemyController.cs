using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharController
{
    private Transform m_PlayerTransform;

    #region Character physics controls methods

    protected override void Move()
    {
        Vector3 playerTargetPosition = new Vector3(this.m_PlayerTransform.position.x, this.m_PlayerTransform.position.y, this.m_PlayerTransform.position.z);
        Vector3 petNewPosition = Vector3.MoveTowards(base.Rigidbody.position, playerTargetPosition, base.TranslationSpeed * Time.fixedDeltaTime);
        base.Rigidbody.MovePosition(petNewPosition);
    }

    #endregion

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
