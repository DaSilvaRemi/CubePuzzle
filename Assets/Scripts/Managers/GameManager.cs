using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SDD.Events;
using static Tools;
using UnityEditor;


public class GameManager : Manager<GameManager>, IEventHandler
{
    [Tooltip("Unsigned Int")]
    [SerializeField] private GameScene m_CurrentScene = GameScene.MENUSCENE;

    [SerializeField] private bool m_IsDebugMode = false;

    private IEnumerator m_GameManagerCoroutine;
    private TimerUtils m_TimerUtils;

    #region Time Properties
    /**
     * <summary>The time passed</summary>
     */
    private static float m_TimePassed;

    /**
     * <summary>The best time</summary>
     */
    private static float m_BestTime;
    #endregion

    #region Score properties
    /**
     * <summary>The score</summary>
     */
    private int m_Score;

    /**
     * <summary>The score</summary>
     */
    public int Score
    {
        get => m_Score;
    }
    #endregion

    #region GameState Properties

    /**
     * <summary>The game state</summary>
     */
    private static GameState m_GameState;

    public static bool IsMenu { get => GameManager.m_GameState.Equals(GameState.MENU); }
    public static bool IsPlaying { get => GameManager.m_GameState.Equals(GameState.PLAY); }
    public static bool IsPausing { get => GameManager.m_GameState.Equals(GameState.PAUSE); }
    public static bool IsWinning { get => GameManager.m_GameState.Equals(GameState.WIN); }
    public static bool IsGameOver { get => GameManager.m_GameState.Equals(GameState.GAMEOVER); }
    public static bool IsEndLVL { get => GameManager.m_GameState.Equals(GameState.ENDLVL); }
    public static bool IsOnPlaying { get => GameManager.IsPlaying || GameManager.IsPausing; }

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

    private void OnChooseALevelEvent(ChooseALevelEvent e)
    {
        this.ChooseALevel(e.eGameScene);
    }

    /**
    * <summary>Handle the NewGameButtonClickedEvent</summary>
    * <remarks>Call the new game methods <see cref="NewGame"/></remarks>
    * <param name="e">The event</param> 
    */
    private void OnNewGameButtonClickedEvent(NewGameButtonClickedEvent e)
    {
        this.NewGame();
    }

    /**
    * <summary>Handle the LoadGameButtonClickedEvent</summary>
    * <remarks>Call the load game methods <see cref="LoadGame"/></remarks>
    * <param name="e">The event</param> 
    */
    private void OnLoadGameButtonClickedEvent(LoadGameButtonClickedEvent e)
    {
        this.LoadGame();
    }

    /**
    * <summary>Handle the MainMenuButtonClickedEvent</summary>
    * <remarks>Call the Menu methods <see cref="Menu"/></remarks>
    * <param name="e">The event</param> 
    */
    private void OnMainMenuButtonClickedEvent(MainMenuButtonClickedEvent e)
    {
        this.Menu();
    }

    /**
    * <summary>Handle the HelpButtonClickedEvent</summary>
    * <remarks>Call the help methods <see cref="ExitGame"/></remarks>
    * <param name="e">The event</param> 
    */
    private void OnHelpButtonClickedEvent(HelpButtonClickedEvent e)
    {
        this.Help();
    }

    /**
    * <summary>Handle the CreditButtonClickedEvent</summary>
    * <remarks>Call the credit game methods <see cref="ExitGame"/></remarks>
    * <param name="e">The event</param> 
    */
    private void OnCreditButtonClickedEvent(CreditButtonClickedEvent e)
    {
        this.CreditGame();
    }

    /**
    * <summary>Handle the ExitButtonClickedEvent</summary>
    * <remarks>Call the exit game methods <see cref="ExitGame"/></remarks>
    * <param name="e">The event</param> 
    */
    private void OnExitButtonClickedEvent(ExitButtonClickedEvent e)
    {
        this.ExitGame();
    }

    /**
    * <summary>Handle the LevelGameOverEvent</summary>
    * <remarks>Call the game over methods <see cref="GameOver"/></remarks>
    * <param name="e">The event</param> 
    */
    private void OnLevelGameOverEvent(LevelGameOverEvent e)
    {
        this.GameOver();
    }

    /**
    * <summary>Handle the LevelFinishEvent</summary>
    * <remarks>Call the end game methods <see cref="EndGame"/></remarks>
    * <param name="e">The event</param> 
    */
    private void OnLevelFinishEvent(LevelFinishEvent e)
    {
        this.EndGame();
    }

    /**
    * <summary>Handle the ObjectWillGainTimeEvent</summary>
    * <remarks>If the triggered GO is the player so we earn time</remarks>
    * <param name="e">The event</param> 
    */
    private void OnObjectWillGainTimeEvent(ObjectWillGainTimeEvent e)
    {
        if (e.eOtherGO.CompareTag("Player") && GameManager.IsPlaying) this.EarnTime(e.eThisGameObject);
    }

    /**
    * <summary>Handle the ObjectWillGainScoreEvent</summary>
    * <remarks>If the collided GO is the ThrowableObject so we earn time</remarks>
    * <param name="e">The event</param> 
    */
    private void OnObjectWillGainScoreEvent(ObjectWillGainScoreEvent e)
    {
        bool isThrowableObject = e.eOtherGO.CompareTag("ThrowableObject");
        bool isPlayer = e.eOtherGO.CompareTag("Player");
        if ((isThrowableObject || isPlayer) && GameManager.IsPlaying)
        {
            this.EarnScore(e.eThisGameObject);

            if (isThrowableObject)
            {
                e.eOtherGO.SetActive(false); // Desativate the ThrowableObject when hit ObjectWillGainScore
            } 
        }
    }

    private void OnContinueGameEvent(ContinueGameEvent e)
    {
        this.PlayGame();
    }
    #endregion

    #region  GameMangers Utils Methods
    /**
     * <summary>Init the game</summary> 
     */
    private void InitGame()
    {
        this.m_TimerUtils.StartTimer();
    }

    /// <summary>
    /// Play the game
    /// </summary>
    private void PlayGame()
    {
        Time.timeScale = 1;
        SetGameState(Tools.GameState.PLAY);
    }

    /// <summary>
    /// Pause the game
    /// </summary>
    private void PauseGame()
    {
        Time.timeScale = 0;
        SetGameState(Tools.GameState.PAUSE);
    }

    /**
     * <summary>Go to the victory scene</summary> 
     */
    private void VictoryGame()
    {
        if (!GameManager.IsWinning && !GameManager.IsGameOver) return;

        this.LoadALevel(GameScene.VICTORYSCENE, false);
    }

    /**
     * <summary>Switch to GAMEOVER and go to the victory scene</summary> 
     */
    private void GameOver()
    {
        this.SetGameState(GameState.GAMEOVER);
        this.VictoryGame();
    }

    /**
     * <summary>Start a new Game</summary> 
     * <remarks>Save the game, reset all games variables and load the first LEVEL</remarks>
     */
    private void NewGame()
    {
        this.ChooseALevel(GameScene.FIRSTLEVELSCENE);
    }

    /// <summary>
    /// Choose a level
    /// </summary>
    /// <param name="levelChoosen">The level choosen</param>
    private void ChooseALevel(GameScene levelChoosen)
    {
        SaveData.Save(new SaveData(0f, levelChoosen));
        this.ResetGameVar();
        this.LoadALevel(levelChoosen);
    }

    /**
     * <summary>Load the game with a save</summary> 
     */
    private void LoadGame()
    {
        SaveData saveGame = SaveData.LoadPlayerRefs();
        this.SetGameState(saveGame.GameState);
        this.SetTimePassed(saveGame.Time);
        this.SetBestTime(saveGame.BestTime);
        this.LoadALevel(saveGame.Level);
        this.VictoryGame();
        this.ChangeLevel();
    }

    /**
     *  <summary>Finish the current LVL</summary>
     *  <remarks>Change the game state and the LVL</remarks>
     */
    private void EndGame()
    {
        GameManager.m_TimePassed += this.m_TimerUtils.TimePassed;
        this.SetGameState(GameState.ENDLVL);
        this.ChangeLevel();
    }

    /**
     * <summary>Save the current game</summary>
     * <param name="timePassed">The time passed in the game</param>
     * <param name="gameScene">The game scene</param>
     * <param name="bestTime">The best time in the game</param>
     * <param name="gameState">The game state of the game</param>
     */
    private static void SaveGame(float timePassed, GameScene gameScene, float bestTime, GameState gameState)
    {
        SaveData.Save(new SaveData(timePassed, gameScene, bestTime, gameState));
    }

    /**
     * <summary>Earn time on game</summary>
     * <param name="gameObject">The game object with ITime interface</param>
     */
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

    /**
     * <summary>Earn score on game</summary>
     * <param name="gameObject">The game object with Iscore interface</param>
     */
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

    /**
     * <summary>Reset the game</summary> 
     */
    private void ResetGame()
    {
        this.ResetGameVar();
        this.LoadALevel(this.m_CurrentScene);
    }

    /**
     * <summary>Reset the game variable</summary> 
     */
    private void ResetGameVar()
    {
        this.SetGameState(GameState.PLAY);
        this.SetTimePassed(0);
    }

    /**
     * <summary>Change to the next level when the current LVL is finish</summary>
     * <remarks>If it is the last level so we go on victory scene</remarks>
     */
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
        this.LoadALevel(nextGameScene);
    }

    /**
     * <summary>Load a LVL</summary>
     * <remarks>Start a couroutine to wait a time for event can play</remarks>
     * <param name="gameScene">The gameScene to load</param>
     */
    private void LoadALevel(GameScene gameScene)
    {
        this.LoadALevel(gameScene, true);
    }

    /// <summary>
    /// Load a LVL
    /// </summary>
    /// <param name="gameScene">The gameScene to load</param>
    /// <param name="waitToLoad">If we wait before loading</param>
    private void LoadALevel(GameScene gameScene, bool waitToLoad)
    {
        if (waitToLoad)
        {
            this.m_GameManagerCoroutine = Tools.MyWaitCoroutine(1, null, () => this.SetGameScene(gameScene));
            StartCoroutine(this.m_GameManagerCoroutine);
        }
        else
        {
            this.SetGameScene(gameScene);
        }
    }

    /**
     * <summary>Load the Menu</summary>
     */
    private void Menu()
    {
        this.PlayGame();
        this.LoadALevel(GameScene.MENUSCENE, false);
        this.SetGameState(Tools.GameState.MENU);
    }

    /**
    * <summary>Load the HelpScene</summary>
    */
    private void Help()
    {
        this.LoadALevel(GameScene.HELPSCENE);
    }

    /**
    * <summary>Load the CreditScene</summary>
    */
    private void CreditGame()
    {
        this.LoadALevel(GameScene.CREDITSCENE);
    }

    /**
    * <summary>Exit the game</summary>
    */
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
    /**
     * <summary>Set the game state</summary> 
     * <remarks>Send an event for each gamestate</remarks>
     * <param name="newGameState">The new game state</param>
     */
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

    /**
     * <summary>Set the time passed</summary>
     * <param name="timePassed">The time passed</param>
     */
    private void SetTimePassed(float timePassed)
    {
        if (!this.m_TimerUtils) return;

        GameManager.m_TimePassed = timePassed;
        EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eTime = GameManager.m_TimePassed, eCountdown = this.m_TimerUtils.TimeLeft, eScore = this.m_Score });
    }

    /**
     * <summary>Set the best time</summary>
     * <param name="bestTime">The best time</param>
     */
    private void SetBestTime(float bestTime)
    {
        if (!this.m_TimerUtils) return;

        GameManager.m_BestTime = bestTime;
        EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestTime = GameManager.m_BestTime, eCountdown = this.m_TimerUtils.TimeLeft, eScore = this.m_Score });
    }

    /**
     * <summary>Set the countdown</summary>
     * <param name="countdown">The countdown</param>
     */
    private void SetCountdown(float countdown)
    {
        EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eCountdown = countdown, eScore = this.m_Score });
    }

    /**
     * <summary>Set the score</summary>
     * <param name="score">The score</param>
     */
    private void SetScore(int score)
    {
        this.m_Score = score;
        EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eTime = this.m_TimerUtils.TimePassed, eCountdown = this.m_TimerUtils.TimeLeft, eScore = this.m_Score });
    }

    /**
     * <summary>Set the GameScene</summary>
     * <remarks>Load immediatly the new game scene, pls use <see cref="LoadALevel(GameScene)"/> to wait before load</remarks>
     * <param name="gameScene">The gamescene</param>
     */
    private void SetGameScene(GameScene gameScene)
    {
        this.m_CurrentScene = gameScene;
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
        EventManager.Instance.AddListener<ChooseALevelEvent>(OnChooseALevelEvent);
        EventManager.Instance.AddListener<LoadGameButtonClickedEvent>(OnLoadGameButtonClickedEvent);
        EventManager.Instance.AddListener<HelpButtonClickedEvent>(OnHelpButtonClickedEvent);
        EventManager.Instance.AddListener<CreditButtonClickedEvent>(OnCreditButtonClickedEvent);
        EventManager.Instance.AddListener<ExitButtonClickedEvent>(OnExitButtonClickedEvent);
        EventManager.Instance.AddListener<MainMenuButtonClickedEvent>(OnMainMenuButtonClickedEvent);
        EventManager.Instance.AddListener<LevelGameOverEvent>(OnLevelGameOverEvent);
        EventManager.Instance.AddListener<LevelFinishEvent>(OnLevelFinishEvent);
        EventManager.Instance.AddListener<ObjectWillGainTimeEvent>(OnObjectWillGainTimeEvent);
        EventManager.Instance.AddListener<ObjectWillGainScoreEvent>(OnObjectWillGainScoreEvent);
        EventManager.Instance.AddListener<ContinueGameEvent>(OnContinueGameEvent);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<NewGameButtonClickedEvent>(OnNewGameButtonClickedEvent);
        EventManager.Instance.RemoveListener<ChooseALevelEvent>(OnChooseALevelEvent);
        EventManager.Instance.RemoveListener<LoadGameButtonClickedEvent>(OnLoadGameButtonClickedEvent);
        EventManager.Instance.RemoveListener<HelpButtonClickedEvent>(OnHelpButtonClickedEvent);
        EventManager.Instance.RemoveListener<CreditButtonClickedEvent>(OnCreditButtonClickedEvent);
        EventManager.Instance.RemoveListener<ExitButtonClickedEvent>(OnExitButtonClickedEvent);
        EventManager.Instance.RemoveListener<MainMenuButtonClickedEvent>(OnMainMenuButtonClickedEvent);
        EventManager.Instance.RemoveListener<LevelGameOverEvent>(OnLevelGameOverEvent);
        EventManager.Instance.RemoveListener<LevelFinishEvent>(OnLevelFinishEvent);
        EventManager.Instance.RemoveListener<ObjectWillGainTimeEvent>(OnObjectWillGainTimeEvent);
        EventManager.Instance.RemoveListener<ObjectWillGainScoreEvent>(OnObjectWillGainScoreEvent);
        EventManager.Instance.RemoveListener<ChooseALevelEvent>(OnChooseALevelEvent);
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
        if (GameManager.IsPlaying && Input.GetButton("ResetGame"))
        {
            this.ResetGame();
        }else if (Input.GetButton("PauseGame"))
        {
            if (GameManager.IsPlaying)
            {
                this.PauseGame();
            }else
            {
                this.PlayGame();
            }
        }

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
        Tools.GameState gameState = this.m_IsDebugMode ? Tools.GameState.PLAY : GameManager.m_GameState;

        this.SetGameState(gameState);
        if (GameManager.IsPlaying)
        {
            this.InitGame();
        }
    }

    private void FixedUpdate()
    {
        this.UpdateGame(this.m_TimerUtils);
    }

    private void OnDestroy()
    {
        if (this.m_GameManagerCoroutine != null)
        {
            StopCoroutine(this.m_GameManagerCoroutine);
        }
    }
    #endregion
}
