using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : GameManager
{
    private TimerUtils m_TimerUtils;
    [SerializeField] private HUDManager m_HUDManager;

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
        this.LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        if (GameManager.GameState.Equals(Tools.GameState.LVLFINISH))
        {
            GameManager.GameState = Tools.GameState.PLAY;
            GameManager.GameTimePassed += this.m_TimerUtils.TimePassed;
            SceneManager.LoadScene((int) base.CurrentLVL + 1);
        }
    }
}
