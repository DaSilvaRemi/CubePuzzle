using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour, IDamage
{
    [Header("Time to earn")]
    [SerializeField] private int m_DamagePoint;

    public int DamagePoint => this.m_DamagePoint;
}
