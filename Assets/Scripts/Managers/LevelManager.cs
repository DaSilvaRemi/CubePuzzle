using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : GameManager
{
    [Header("Managers")]
    [Tooltip("HUDManager")]
    [SerializeField] private HUDManager m_HUDManager;

    private TimerUtils m_TimerUtils;

    private void Awake()
    {
        this.m_TimerUtils = GetComponentInChildren<TimerUtils>();
        this.m_HUDManager.UpdateTimeLeftTXT(m_TimerUtils);
        GameManager.GameTimePassed = 0;
    }

    // Start is called before the first frame update
    private void Start()
    {
        this.m_TimerUtils.StartTimer();
    }

    private void FixedUpdate()
    {
        base.UpdateGameState(m_TimerUtils);
        this.m_HUDManager.UpdateTimeLeftTXT(m_TimerUtils);
        base.UpdateGame(m_TimerUtils);
    }
}
