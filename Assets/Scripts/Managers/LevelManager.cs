using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : GameManager
{
    private TimerUtils m_TimerUtils;
    [SerializeField] private HUDManager m_HUDManager;

    private void Awake()
    {
        m_TimerUtils = GetComponentInChildren<TimerUtils>();
        m_HUDManager.UpdateTimeLeftTXT(m_TimerUtils);
    }

    // Start is called before the first frame update
    private void Start()
    {
        m_TimerUtils.StartTimer();
    }

    private void FixedUpdate()
    {
        base.UpdateGameState(m_TimerUtils);
        m_HUDManager.UpdateTimeLeftTXT(m_TimerUtils);
    }
}
