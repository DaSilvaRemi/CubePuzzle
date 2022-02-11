using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using SDD.Events;
using static Tools;
using UnityEditor;


public class GameManager : Manager<GameManager>, IEventHandler
{
    [Tooltip("Unsigned Int")]
    [SerializeField] private GameScene m_CurrentScene = GameScene.MENUSCENE;

    private TimerUtils m_TimerUtils;

    private static float m_TimePassed;

    private static float m_BestTime;

    private int m_Score;

    #region GameState Properties

    private static GameState m_GameState;

    public static bool IsMenu { get => m_GameState.Equals(GameState.MENU); }
    public static bool IsPlaying { get => m_GameState.Equals(GameState.PLAY); }
    public static bool IsPausing { get => m_GameState.Equals(GameState.PAUSE); }
    public static bool IsWinning { get => m_GameState.Equals(GameState.WIN); }
    public static bool IsGameOver { get => m_GameState.Equals(GameState.GAMEOVER); }
    public static bool IsEndLVL { get => m_GameState.Equals(GameState.ENDLVL); }

    #endregion

    #region GameScene Properties
    public bool IsMenuScene { get => m_CurrentScene.Equals(GameScene.MENUSCENE); }
    public bool IsFirstLevelScene { get => m_CurrentScene.Equals(GameScene.FIRSTLEVELSCENE); }
    public bool IsSecondLevelScene { get => m_CurrentScene.Equals(GameScene.SECONDLVLSCENE); }
    public bool IsThirdLevelScene { get => m_CurrentScene.Equals(GameScene.THIRDLEVELSCENE); }
    public bool IsFourthLevelScene { get => m_CurrentScene.Equals(GameScene.FOURTHLEVELSCENE); }
    public bool IsHelpScene { get => m_CurrentScene.Equals(GameScene.HELPSCENE); }
    public bool IsCreditScene { get => m_CurrentScene.Equals(GameScene.CREDITSCENE); }
    public bool IsLastLevel { get => IsFourthLevelScene; }
    public bool IsShootableScene { get => IsThirdLevelScene || IsFourthLevelScene; }
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

    private void OnLevelGameOverEvent(LevelGameOverEvent e)
    {
        this.GameOver();
    }

    private void OnLevelFinishEvent(LevelFinishEvent e)
    {
        GameManager.m_TimePassed += this.m_TimerUtils.TimePassed;
        this.SetGameState(GameState.ENDLVL);
        this.ChangeLevel();
    }

    private void OnMainMenuButtonClickedEvent(MainMenuButtonClickedEvent e)
    {
        this.SetGameScene(GameScene.MENUSCENE);
    }

    private void OnChestHasTrigerEnterEvent(ChestHasTrigerEnterEvent e)
    {
        if (e.eTriggeredGO.CompareTag("Player") && GameManager.IsPlaying) this.EarnTime(e.eChestGO);
    }

    private void OnTargetHasCollidedEnterEvent(OnTargetHasCollidedEnterEvent e)
    {
        if (e.eCollidedGO.CompareTag("ThrowableObject") && GameManager.IsPlaying) this.EarnScore(e.eTargetGO);
    }
    #endregion

    #region  GameMangers Utils Methods
    private void PlayGame()
    {
        this.m_TimerUtils.StartTimer();
    }

    private void VictoryGame()
    {
        if (!GameManager.IsWinning && !GameManager.IsGameOver) return;

        this.SetGameScene(GameScene.VICTORYSCENE);
    }

    private void GameOver()
    {
        this.SetGameState(GameState.GAMEOVER);
        this.SetGameScene(GameScene.VICTORYSCENE);
    }

    private void NewGame()
    {
        SaveData.Save(new SaveData());
        this.ResetGameVar();
        this.SetGameScene(GameScene.FIRSTLEVELSCENE);
    }

    private void LoadGame()
    {
        SaveData saveGame = SaveData.Load();
        this.SetGameState(saveGame.GameState);
        this.SetTimePassed(saveGame.Time);
        this.SetBestTime(saveGame.BestTime);
        this.SetGameScene(saveGame.Level);
        this.VictoryGame();
        this.ChangeLevel();
    }

    private void SaveGame()
    {
        GameManager.SaveGame(GameManager.m_TimePassed, this.m_CurrentScene, GameManager.m_TimePassed, GameManager.m_GameState);
    }

    private static void SaveGame(float timePassed, GameScene gameScene, float bestTime, GameState gameState)
    {
        SaveData.Save(new SaveData(timePassed, gameScene, bestTime, gameState));
    }

    private void EarnTime(GameObject gameObject)
    {
        if (!gameObject) return;

        float totalNewlyGainedTime = 0;
        ITime[] times = gameObject.GetComponentsInChildren<ITime>();

        for (int i = 0; i < times.Length; i++)
        {
            totalNewlyGainedTime += times[i].Time;
        }

        this.m_TimerUtils.LateTime(totalNewlyGainedTime);
    }

    private void EarnScore(GameObject gameObject)
    {
        if (!gameObject) return;

        int totalNewlyGainedScore = this.m_Score;
        IScore[] scores = gameObject.GetComponentsInChildren<IScore>();

        for (int i = 0; i < scores.Length; i++)
        {
            totalNewlyGainedScore += scores[i].Score;
        }

        SetScore(totalNewlyGainedScore);
    }

    private void ResetGameVar()
    {
        this.SetGameState(GameState.PLAY);
        this.SetTimePassed(0);
    }

    private void ChangeLevel()
    {
        if (!GameManager.IsEndLVL) return;

        GameState nextGameState = GameState.PLAY;
        GameScene nextGameScene = m_CurrentScene + 1;
        if (this.IsLastLevel)
        {
            nextGameState = GameState.WIN;
            nextGameScene = GameScene.VICTORYSCENE;
        }

        GameManager.SaveGame(GameManager.m_TimePassed, nextGameScene, GameManager.m_TimePassed, nextGameState);
        this.SetGameState(nextGameState);
        this.SetGameScene(nextGameScene);
    }

    private void Help()
    {
        this.SetGameScene(GameScene.HELPSCENE);
    }

    private void CreditGame()
    {
        this.SetGameScene(GameScene.CREDITSCENE);
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

    #region Setters
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
            case GameState.ENDLVL:
                EventManager.Instance.Raise(new GameEndLVLEvent());
                break;
            default:
                break;
        }
    }

    private void SetTimePassed(float timePassed)
    {
        GameManager.m_TimePassed = timePassed;
        EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eTime = timePassed, eCountdown = this.m_TimerUtils.TimeLeft });
    }

    private void SetBestTime(float bestTime)
    {
        GameManager.m_BestTime = bestTime;
        EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestTime = bestTime, eCountdown = this.m_TimerUtils.TimeLeft });
    }

    private void SetCountdown(float countdown)
    {
        EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eCountdown = countdown });
    }

    private void SetScore(int score)
    {
        this.m_Score = score;
        EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eTime = this.m_TimerUtils.TimePassed, eCountdown = this.m_TimerUtils.TimeLeft, eScore = this.m_Score });
    }

    private void SetGameScene(GameScene gameScene)
    {
        m_CurrentScene = gameScene;
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
    #endregion

    #region Events Suscribption
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<NewGameButtonClickedEvent>(OnNewGameButtonClickedEvent);
        EventManager.Instance.AddListener<LoadGameButtonClickedEvent>(OnLoadGameButtonClickedEvent);
        EventManager.Instance.AddListener<HelpButtonClickedEvent>(OnHelpButtonClickedEvent);
        EventManager.Instance.AddListener<CreditButtonClickedEvent>(OnCreditButtonClickedEvent);
        EventManager.Instance.AddListener<ExitButtonClickedEvent>(OnExitButtonClickedEvent);
        EventManager.Instance.AddListener<MainMenuButtonClickedEvent>(OnMainMenuButtonClickedEvent);
        EventManager.Instance.AddListener<LevelGameOverEvent>(OnLevelGameOverEvent);
        EventManager.Instance.AddListener<LevelFinishEvent>(OnLevelFinishEvent);
        EventManager.Instance.AddListener<ChestHasTrigerEnterEvent>(OnChestHasTrigerEnterEvent);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<NewGameButtonClickedEvent>(OnNewGameButtonClickedEvent);
        EventManager.Instance.RemoveListener<LoadGameButtonClickedEvent>(OnLoadGameButtonClickedEvent);
        EventManager.Instance.RemoveListener<HelpButtonClickedEvent>(OnHelpButtonClickedEvent);
        EventManager.Instance.RemoveListener<CreditButtonClickedEvent>(OnCreditButtonClickedEvent);
        EventManager.Instance.RemoveListener<ExitButtonClickedEvent>(OnExitButtonClickedEvent);
        EventManager.Instance.RemoveListener<MainMenuButtonClickedEvent>(OnMainMenuButtonClickedEvent);
        EventManager.Instance.RemoveListener<LevelGameOverEvent>(OnLevelGameOverEvent);
        EventManager.Instance.RemoveListener<LevelFinishEvent>(OnLevelFinishEvent);
        EventManager.Instance.RemoveListener<ChestHasTrigerEnterEvent>(OnChestHasTrigerEnterEvent);
    }
    #endregion

    #region GameManagers OWN UPDATE METHODS

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
            this.GameOver();
        }

        this.UpdateCountdown(timerUtils);
    }
    #endregion

    #region MonoBehaviour METHODS
    private void OnEnable()
    {
        this.SubscribeEvents();
    }

    private void OnDisable()
    {
        this.UnsubscribeEvents();
    }

    private void Awake()
    {
        base.InitManager();
        this.m_TimerUtils = GetComponent<TimerUtils>();
    }

    private void Start()
    {
        this.SetGameState(m_GameState);
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
        this.SetGameScene(this.m_CurrentScene);
    }
    #endregion
}
