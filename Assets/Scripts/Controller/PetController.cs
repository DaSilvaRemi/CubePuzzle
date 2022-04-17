using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetController : CharController
{
    [Header("Pet Setup")]
    [Tooltip("The Game object transfrom the pet will follow")]
    [SerializeField] private Transform m_GameObjectTransformToFollow;
    private Vector3 m_DeltaPosition;

    protected override void Move()
    {
        if (m_GameObjectTransformToFollow != null)
        {
            this.transform.position = this.m_GameObjectTransformToFollow.position - this.m_DeltaPosition;
        }
    }

    private void Awake()
    {
        this.m_DeltaPosition = this.transform.position;
    }


    private void FixedUpdate()
    {
        this.Move();
    }
}
