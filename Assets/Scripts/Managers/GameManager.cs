using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Tooltip("Unsigned Int")]
    [SerializeField] private uint m_CurrentLvl = 0;

    public static Tools.GameState GameState { get; set; }

    private void FixedUpdate()
    {
        if (GameState.Equals(Tools.GameState.PLAY))
        {
            if (Input.GetButton("ResetGame")) Reset();

            UpdateGameState();
        }
    }


    protected void UpdateGameState()
    {
        HoldOnVictory();
    }

    /**
     * 
     */
    protected void UpdateGameState(TimerUtils timerUtils)
    {
        if (timerUtils.IsFinish)
        {
            timerUtils.StopTimer();
            GameState = Tools.GameState.LOOSE;
        }

        UpdateGameState();
    }

    private void Reset()
    {
        Debug.Log("Reset");

        GameState = Tools.GameState.PLAY;
        SceneManager.LoadScene((int) m_CurrentLvl);
    }

    /**
     * 
     */
    private void HoldOnVictory()
    {
        switch (GameState)
        {
            case Tools.GameState.WIN:
            case Tools.GameState.LOOSE:
                SceneManager.LoadScene("VictoryScene");
                break;
            case Tools.GameState.LVLFINISH:
                Debug.Log("Trigger !");
                if (m_CurrentLvl == 4)
                {
                    GameState = Tools.GameState.WIN;
                    HoldOnVictory();
                }
                else
                {
                    GameState = Tools.GameState.PLAY;
                    SceneManager.LoadScene((int)m_CurrentLvl + 1);
                }
                break;
        }
    }
}
