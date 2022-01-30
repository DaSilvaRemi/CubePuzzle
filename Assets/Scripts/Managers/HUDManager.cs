using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [Header("GameObjects")]
    [Tooltip("TextMeshPro")]
    [SerializeField] private TextMeshProUGUI m_TimeLeftTXT;

    public void UpdateTimeLeftTXT(TimerUtils timerUtils)
    {
        m_TimeLeftTXT.SetText("Time : " + Mathf.Round(timerUtils.Timer));
    }
}
