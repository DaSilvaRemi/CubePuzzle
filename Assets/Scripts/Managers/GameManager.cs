using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Tooltip("Unsigned Int")]
    [SerializeField] private uint m_CurrentLvl = 0;

    protected uint CurrentLVL { get => m_CurrentLvl; }

    /**
     * <summary>The game state</summary> 
     */
    public static Tools.GameState GameState { get; set; }

    /**
     * <summary>The game time passed</summary> 
     */
    public static float GameTimePassed { get; protected set; }

    /**
     * <summary>The best time</summary>
     */
    public static float GameBestTime { get; protected set; }

    /**
     * <summary>Load the game</summary> 
     */
    public static void LoadGame()
    {
        SaveGame saveGame = SaveGame.Load();
        GameManager.GameState = saveGame.GameState;
        GameManager.GameTimePassed = 0;
        GameManager.GameBestTime = saveGame.BestTime;

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
        }
    }

    /**
     * <summary>Update the Game</summary>
     * <remarks>It run only if the game state is PLAY</remarks>
     * 
     * <param name="timerUtils">The TimerUtils object</param>
     */
    protected void UpdateGame(TimerUtils timerUtils)
    {
        if (GameManager.GameState.Equals(Tools.GameState.PLAY))
        {
            if (Input.GetButton("ResetGame")) Reset();

            UpdateGameState(timerUtils);
        }
    }

    /**
     * <summary>Update the GameTime</summary>
     * <remarks>If GameState is not equal to PLAY so we save the time passed</remarks>
     * 
     * <param name="timerUtils">The TimerUtils object</param>
     */
    protected void UpdateGameTime(TimerUtils timerUtils)
    {
        if (!GameManager.GameState.Equals(Tools.GameState.PLAY))
        {
            GameManager.GameTimePassed = timerUtils.FormatedTimePassed;
        }
    }

    /**
     * <summary>Update the GameState</summary>
     * <remarks>If the timer is finish the game state is set to LOOSE. Also we call everytime UpdateGameTime and CheckGameState</remarks>
     * 
     * <param name="timerUtils">The TimerUtils object</param>
     */
    protected void UpdateGameState(TimerUtils timerUtils)
    {
        if (timerUtils.IsFinish)
        {
            timerUtils.StopTimer();
            GameManager.GameState = Tools.GameState.LOOSE;
        }

        UpdateGameTime(timerUtils);
        CheckGameState();
    }

    /**
     * <summary>Reset the game</summary> 
     */
    private void Reset()
    {
        GameManager.GameState = Tools.GameState.PLAY;
        GameManager.GameTimePassed = 0;
        SceneManager.LoadScene((int)CurrentLVL);
    }

    /**
     * <summary>Check the game state and do appropriate action according to them</summary>
     */
    private void CheckGameState()
    {
        switch (GameState)
        {
            case Tools.GameState.WIN:
            case Tools.GameState.LOOSE:
                SceneManager.LoadScene("VictoryScene");
                break;
            case Tools.GameState.LVLFINISH:
                SaveGame.Save(new SaveGame(GameTimePassed, CurrentLVL, GameTimePassed, GameState));
                if (m_CurrentLvl == 4) GameManager.GameState = Tools.GameState.WIN;
                break;
        }
    }
}
