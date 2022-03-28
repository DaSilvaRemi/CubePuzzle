using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarnTime : MonoBehaviour, ITime
{
    [SerializeField] int m_Time;

    public float Time { get => this.m_Time; }

    public float BestTime { get => this.m_Time; }
}
