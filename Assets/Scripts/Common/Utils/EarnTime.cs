using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarnTime : MonoBehaviour, ITime
{
    [SerializeField] int m_Time;

    public float Time { get => m_Time; }

    public float BestTime { get => m_Time; }
}
