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

    protected uint CurrentLVL { get => m_CurrentLvl;  }

    public static void LoadGame()
    {
        SaveGame saveGame = SaveGame.Load();
        GameState = saveGame.GameState;

        switch (saveGame.Level)
        {
            case 1:
                SceneManager.LoadScene("FirstLevelScene");
                break;
            case 2:
                SceneManager.LoadScene("SecondLevelScene");
                break;
            case 3:
                SceneManager.LoadScene("ThirdLevelScene");
                break;
            case 4:
                SceneManager.LoadScene("FourthLevelScene");
                break;
            default:
                break;
        }
    }

    protected void UpdateGame()
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

    private void FixedUpdate()
    {
        UpdateGame();
    }

    private void Reset()
    {
        GameState = Tools.GameState.PLAY;
        SceneManager.LoadScene((int)CurrentLVL);
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
                if (m_CurrentLvl == 4)
                {
                    GameState = Tools.GameState.WIN;
                    HoldOnVictory();
                }
                break;
        }
    }
}
