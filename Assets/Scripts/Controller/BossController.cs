using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : CharController
{
    [SerializeField] private float m_DistanceBetweenTargetRange = 3f;
    private Transform m_TargetTransform;

    protected override void Move()
    {
        if (Vector3.Distance(this.m_TargetTransform.position, base.Rigidbody.position) < this.m_DistanceBetweenTargetRange)
        {
           
        }

        Vector3 playerTargetPosition = new Vector3(this.m_TargetTransform.position.x, this.m_TargetTransform.position.y, this.m_TargetTransform.position.z);
        Vector3 petNewPosition = Vector3.MoveTowards(base.Rigidbody.position, playerTargetPosition, base.TranslationSpeed * Time.fixedDeltaTime);
        base.Rigidbody.MovePosition(petNewPosition);
    }

    #region MonoBehaviour Methods
    void FixedUpdate()
    {
        
    }
    #endregion
}
