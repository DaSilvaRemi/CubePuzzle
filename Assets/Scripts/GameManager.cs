using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TimeLeftTXT;
    [SerializeField] private TimerUtils m_TimerUtils;

    public static Tools.GameState GameState { get; set; }

    private void Awake()
    {
        if (m_TimerUtils == null)
        {
            m_TimerUtils = GetComponentInParent<TimerUtils>();
        }
        UpdateTimeLeftTXT();
    }

    private void FixedUpdate()
    {
        UpdateGameState();
        UpdateTimeLeftTXT();
    }

    private void UpdateTimeLeftTXT()
    {
        m_TimeLeftTXT.SetText("Time Left : " + m_TimerUtils.Timer);
    }

    private void UpdateGameState()
    {
        if (m_TimerUtils.IsFinish)
        {
            GameState = Tools.GameState.LOOSE;
        }

        HoldOnVictory();
    }

    private void HoldOnVictory()
    {
        switch (GameState)
        {
            case Tools.GameState.WIN:
            case Tools.GameState.LOOSE:
                SceneManager.LoadScene("VictoryScene");
                break;
        }
    }
}
