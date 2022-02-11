using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarnScore : MonoBehaviour, IScore
{
    [SerializeField] private int m_Score;

    public int Score { get => m_Score; }
}
