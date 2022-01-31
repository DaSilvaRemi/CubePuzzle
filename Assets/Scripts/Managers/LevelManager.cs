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
        UpdateGame();
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        if (GameState.Equals(Tools.GameState.LVLFINISH))
        {
            GameState = Tools.GameState.PLAY;
            SceneManager.LoadScene((int) CurrentLVL + 1);
        }
    }
}
