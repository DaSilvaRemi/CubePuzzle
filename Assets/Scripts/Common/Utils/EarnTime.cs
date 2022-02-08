using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarnTime : MonoBehaviour, ITime
{
    [SerializeField] int m_Time;

    public float Time => m_Time;

    public float BestTime => m_Time;
}
