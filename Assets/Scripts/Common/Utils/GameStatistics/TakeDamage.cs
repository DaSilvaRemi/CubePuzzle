using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour, IDamage
{
    [Header("Time to earn")]
    [SerializeField] private int m_DamagePoint;

    /// <summary>
    /// The damage point will be given to the object interact with this interface
    /// </summary>
    public int DamagePoint => this.m_DamagePoint;
}
