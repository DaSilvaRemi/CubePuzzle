using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : CharController
{
    [Header("CollectableController properties")]
    [SerializeField] private bool m_IsAnimated = false;
    [SerializeField] private bool m_IsRotating = false;
    [SerializeField] private bool m_IsFloating = false;
    [SerializeField] private float m_CooldownJumpDuration = 0.2f;

    private float m_NextJumpTime;

    #region
    protected override void Move()
    {
        if (!this.m_IsAnimated)
        {
            return;
        }

        if (this.m_IsRotating)
        {
            base.RotateObject();
        }

        if (this.m_IsFloating && Time.time > this.m_NextJumpTime)
        {
            this.m_NextJumpTime += m_CooldownJumpDuration;
            base.Jump();
        }
    }
    #endregion


    #region MonoBehaviour methods
    protected override void Awake()
    {
        base.Awake();
        m_NextJumpTime = Time.time;
    }


    private void FixedUpdate()
    {
        this.Move();
    }
    #endregion
}
