using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarnTime : MonoBehaviour, ITime
{
    [Header("Time to earn")]
    [SerializeField] private int m_Time;
    
    /// <summary>
    /// The timme
    /// </summary>
    public float Time { get => this.m_Time; }

    /// <summary>
    /// The best time
    /// </summary>
    public float BestTime { get => this.m_Time; }
}
