using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static Tools.GameState GameState { get; set; }

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

        HoldOnVictory();
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
        }
    }
}
