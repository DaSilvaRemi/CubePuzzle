using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using SDD.Events;
using static Tools;
using UnityEditor;



public class GameManager : MonoBehaviour
{
    [Tooltip("Unsigned Int")]
    [SerializeField] private GameScene m_CurrentScene = GameScene.MENUSCENE;

    [SerializeField] private TimerUtils m_TimerUtils;

    public static float GameTimePassed { get; protected set; }

    public static float GameBestTime { get; protected set; }

    #region GameState

    private static GameState m_GameState;

    public static bool IsMenu { get => m_GameState.Equals(GameState.MENU) ;}
    public static bool IsPlaying { get => m_GameState.Equals(GameState.PLAY) ;}
    public static bool IsPausing { get => m_GameState.Equals(GameState.PAUSE) ;}
    public static bool IsWinning { get => m_GameState.Equals(GameState.WIN) ;}
    public static bool IsGameOver { get => m_GameState.Equals(GameState.GAMEOVER) ;}
    public static bool IsEndLVL { get => m_GameState.Equals(GameState.ENDLVL) ;}

    #endregion

    #region GameScene
    public bool IsMenuScene { get => m_CurrentScene.Equals(GameScene.MENUSCENE); }
    public bool IsFirstLevelScene { get => m_CurrentScene.Equals(GameScene.FIRSTLEVELSCENE); }
    public bool IsSecondLevelScene { get => m_CurrentScene.Equals(GameScene.SECONDLVLSCENE); }
    public bool IsThirdLevelScene { get => m_CurrentScene.Equals(GameScene.THIRDLEVELSCENE); }
    public bool IsFourthLevelScene { get => m_CurrentScene.Equals(GameScene.FOURTHLEVELSCENE); }
    public bool IsHelpScene { get => m_CurrentScene.Equals(GameScene.HELPSCENE); }
    public bool IsCreditScene { get => m_CurrentScene.Equals(GameScene.CREDITSCENE); }
    #endregion

    #region Event Listeners Methods

    private void OnNewGameButtonClickedEvent(NewGameButtonClickedEvent e)
    {
        NewGame();
    }

    private void OnLoadGameButtonClickedEvent(LoadGameButtonClickedEvent e)
    {
        LoadGame();
    }

    private void OnHelpButtonClickedEvent(HelpButtonClickedEvent e)
    {
        Help();
    }

    private void OnCreditButtonClickedEvent(CreditButtonClickedEvent e)
    {
        CreditGame();
    }

    private void OnExitButtonClickedEvent(ExitButtonClickedEvent e)
    {
        ExitGame();
    }

    private void OnLevelFinishEvent(LevelFinishEvent e)
    {
        ChangeLevel();
    }

    private void NewGame()
    {
        SaveData.Save(new SaveData());
        ChangeScene(GameScene.FIRSTLEVELSCENE);
    }

    private void LoadGame()
    {
        Debug.Log("Load Game");
        SaveData saveGame = SaveData.Load();
        SetGameState(saveGame.GameState);
        GameManager.GameTimePassed = saveGame.Time;
        GameManager.GameBestTime = saveGame.BestTime;
    }

    private void ChangeLevel()
    {
        if (IsEndLVL)
        {
            SetGameState(GameState.WIN);
        }
        SaveGame();
    }

    private void Help()
    {
        ChangeScene(GameScene.HELPSCENE);
    }

    private void CreditGame()
    {
        ChangeScene(GameScene.CREDITSCENE);
    }

    private void ExitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    #endregion

    private void SaveGame()
    {
        //SaveData.Save(new SaveData(GameTimePassed, CurrentScene, GameTimePassed, m_GameState));
    }

    private void SetGameState(GameState newGameState)
    {
        m_GameState = newGameState;
        switch (m_GameState)
        {
            case GameState.MENU:
                EventManager.Instance.Raise(new GameMenuEvent());
                break;
            case GameState.PLAY:
                EventManager.Instance.Raise(new GamePlayEvent());
                break;
            case GameState.PAUSE:
                EventManager.Instance.Raise(new GamePauseEvent());
                break;
            case GameState.WIN:
                EventManager.Instance.Raise(new GameVictoryEvent());
                break;
            case GameState.GAMEOVER:
                EventManager.Instance.Raise(new GameOverEvent());
                break;
            default:
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
        if (IsPlaying && Input.GetButton("ResetGame")) Reset();

        UpdateGameState(timerUtils);
    }

    /**
     * <summary>Update the GameTime</summary>
     * <remarks>If GameState is not equal to PLAY so we save the time passed</remarks>
     * 
     * <param name="timerUtils">The TimerUtils object</param>
     */
    protected void UpdateGameTime(TimerUtils timerUtils)
    {
        if (IsPlaying)
        {
            GameManager.GameTimePassed += timerUtils.TimePassed;
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
            SetGameState(GameState.GAMEOVER);
        }

        UpdateGameTime(timerUtils);
    }

    private void ChangeScene(GameScene gameScene)
    {

        switch (gameScene)
        {
            case GameScene.MENUSCENE:
                SceneManager.LoadScene("MenuScene");
                break;
            case GameScene.FIRSTLEVELSCENE:
                SceneManager.LoadScene("FirstLevelScene");
                break;
            case GameScene.SECONDLVLSCENE:
                SceneManager.LoadScene("SecondLevelScene");
                break;
            case GameScene.THIRDLEVELSCENE:
                SceneManager.LoadScene("ThirdLevelScene");
                break;
            case GameScene.FOURTHLEVELSCENE:
                SceneManager.LoadScene("FourthLevelScene");
                break;
            case GameScene.HELPSCENE:
                SceneManager.LoadScene("HelpScene");
                break;
            case GameScene.CREDITSCENE:
                SceneManager.LoadScene("CreditScene");
                break;
            default:
                break;
        }
    }

    #region UNITY METHODS
    private void OnEnable()
    {
        EventManager.Instance.AddListener<NewGameButtonClickedEvent>(OnNewGameButtonClickedEvent);
        EventManager.Instance.AddListener<LoadGameButtonClickedEvent>(OnLoadGameButtonClickedEvent);
        EventManager.Instance.AddListener<HelpButtonClickedEvent>(OnHelpButtonClickedEvent);
        EventManager.Instance.AddListener<CreditButtonClickedEvent>(OnCreditButtonClickedEvent);
        EventManager.Instance.AddListener<ExitButtonClickedEvent>(OnExitButtonClickedEvent);
        EventManager.Instance.AddListener<LevelFinishEvent>(OnLevelFinishEvent);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener<NewGameButtonClickedEvent>(OnNewGameButtonClickedEvent);
        EventManager.Instance.RemoveListener<LoadGameButtonClickedEvent>(OnLoadGameButtonClickedEvent);
        EventManager.Instance.RemoveListener<HelpButtonClickedEvent>(OnHelpButtonClickedEvent);
        EventManager.Instance.RemoveListener<CreditButtonClickedEvent>(OnCreditButtonClickedEvent);
        EventManager.Instance.RemoveListener<ExitButtonClickedEvent>(OnExitButtonClickedEvent);
        EventManager.Instance.RemoveListener<LevelFinishEvent>(OnLevelFinishEvent);
    }

    private void FixedUpdate()
    {
        UpdateGame(m_TimerUtils);
    }

    private void Reset()
    {
        SetGameState(GameState.PLAY);
        GameManager.GameTimePassed = 0;
        ChangeScene(m_CurrentScene);
    }
    #endregion
}
