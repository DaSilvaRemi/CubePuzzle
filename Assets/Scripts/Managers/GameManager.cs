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

    private TimerUtils m_TimerUtils;

    private static float m_TimePassed;

    private static float m_BestTime;

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
        this.NewGame();
    }

    private void OnLoadGameButtonClickedEvent(LoadGameButtonClickedEvent e)
    {
        this.LoadGame();
    }

    private void OnHelpButtonClickedEvent(HelpButtonClickedEvent e)
    {
        this.Help();
    }

    private void OnCreditButtonClickedEvent(CreditButtonClickedEvent e)
    {
        this.CreditGame();
    }

    private void OnExitButtonClickedEvent(ExitButtonClickedEvent e)
    {
        this.ExitGame();
    }

    private void OnLevelFinishEvent(LevelFinishEvent e)
    {
       GameManager.m_TimePassed += this.m_TimerUtils.TimePassed;
       this.ChangeLevel();
    }

    private void OnMainMenuButtonClickedEvent(MainMenuButtonClickedEvent e)
    {
        ChangeScene(GameScene.MENUSCENE);
    }

    private void NewGame()
    {
        SaveData.Save(new SaveData());
        this.ResetGameVar();
        this.ChangeScene(GameScene.FIRSTLEVELSCENE);
    }

    private void LoadGame()
    {
        Debug.Log("Load Game");
        SaveData saveGame = SaveData.Load();
        this.SetGameState(saveGame.GameState);
        this.SetTimePassed(saveGame.Time);
        this.SetBestTime(saveGame.BestTime);
        this.ChangeScene(saveGame.Level);
    }

    private void ChangeLevel()
    {
        m_CurrentScene += 1;
        if (this.IsFourthLevelScene && GameManager.IsEndLVL)
        {
            this.SetGameState(GameState.WIN);
            m_CurrentScene = GameScene.VICTORYSCENE;
        }

        this.SaveGame();
        this.ChangeScene(m_CurrentScene);
    }

    private void Help()
    {
        this.ChangeScene(GameScene.HELPSCENE);
    }

    private void CreditGame()
    {
        this.ChangeScene(GameScene.CREDITSCENE);
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
        SaveData.Save(new SaveData(GameManager.m_TimePassed, this.m_CurrentScene, GameManager.m_TimePassed, GameManager.m_GameState));
    }

    private void SetGameState(GameState newGameState)
    {
        GameManager.m_GameState = newGameState;
        switch (GameManager.m_GameState)
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

    private void SetTimePassed(float timePassed)
    {
        GameManager.m_TimePassed = timePassed;
        EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eTime = timePassed, eCountdown = m_TimerUtils.TimeLeft });
    }

    private void SetBestTime(float bestTime)
    {
        GameManager.m_BestTime = bestTime;
        EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestTime = bestTime, eCountdown = m_TimerUtils.TimeLeft });
    }

    private void SetCountdown(float countdown)
    {
        EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eCountdown = countdown });
    }

    private void PlayGame()
    {
        this.m_TimerUtils.StartTimer();
    }

    /**
     * <summary>Update the Game</summary>
     * <remarks>It run only if the game state is PLAY</remarks>
     * 
     * <param name="timerUtils">The TimerUtils object</param>
     */
    protected void UpdateGame(TimerUtils timerUtils)
    {
        if (GameManager.IsPlaying && Input.GetButton("ResetGame")) this.Reset();

        this.UpdateGameState(timerUtils);
    }

    /**
     * <summary>Update the GameTime</summary>
     * <remarks>If GameState is not equal to PLAY so we save the time passed</remarks>
     * 
     * <param name="timerUtils">The TimerUtils object</param>
     */
    protected void UpdateCountdown(TimerUtils timerUtils)
    {
        if (GameManager.IsPlaying)
        {
            this.SetCountdown(timerUtils.TimeLeft);
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
        if (GameManager.IsPlaying && timerUtils.IsFinish)
        {
            timerUtils.StopTimer();
            this.SetGameState(GameState.GAMEOVER);
            this.ChangeScene(GameScene.VICTORYSCENE);
        }

        UpdateCountdown(timerUtils);
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
            case GameScene.VICTORYSCENE:
                SceneManager.LoadScene("VictoryScene");
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
        EventManager.Instance.AddListener<MainMenuButtonClickedEvent>(OnMainMenuButtonClickedEvent);
        EventManager.Instance.AddListener<LevelFinishEvent>(OnLevelFinishEvent);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener<NewGameButtonClickedEvent>(OnNewGameButtonClickedEvent);
        EventManager.Instance.RemoveListener<LoadGameButtonClickedEvent>(OnLoadGameButtonClickedEvent);
        EventManager.Instance.RemoveListener<HelpButtonClickedEvent>(OnHelpButtonClickedEvent);
        EventManager.Instance.RemoveListener<CreditButtonClickedEvent>(OnCreditButtonClickedEvent);
        EventManager.Instance.RemoveListener<ExitButtonClickedEvent>(OnExitButtonClickedEvent);
        EventManager.Instance.RemoveListener<MainMenuButtonClickedEvent>(OnMainMenuButtonClickedEvent);
        EventManager.Instance.RemoveListener<LevelFinishEvent>(OnLevelFinishEvent);
    }

    private void Awake()
    {
        this.m_TimerUtils = GetComponent<TimerUtils>();
    }

    private void Start()
    {
        SetGameState(m_GameState);
        if (GameManager.IsPlaying)
        {
            this.PlayGame();
        }
    }

    private void FixedUpdate()
    {
        this.UpdateGame(this.m_TimerUtils);
    }

    private void Reset()
    {
        this.ResetGameVar();
        this.ChangeScene(this.m_CurrentScene);
    }
    #endregion

    private void ResetGameVar()
    {
        this.SetGameState(GameState.PLAY);
        this.SetTimePassed(0);
    }
}
