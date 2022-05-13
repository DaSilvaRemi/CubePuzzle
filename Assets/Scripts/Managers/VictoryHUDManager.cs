using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using SDD.Events;

public class VictoryHUDManager : PanelHUDManager
{
    [Header("Victory / GameOver panels")]
    [Tooltip("Game Object")]
    [SerializeField] private GameObject m_WinPanel;
    [Tooltip("Game Object")]
    [SerializeField] private GameObject m_GameOverPanel;

    [Header("Time and Best Time values txt")]
    [Tooltip("TextMeshPro")]
    [SerializeField] private TextMeshProUGUI m_TimeValueText;
    [Tooltip("TextMeshPro")]
    [SerializeField] private TextMeshProUGUI m_BestTimeValueText;

    [Header("Score and Best Score values txt")]
    [Tooltip("TextMeshPro")]
    [SerializeField] private TextMeshProUGUI m_ScoreValueText;
    [Tooltip("TextMeshPro")]
    [SerializeField] private TextMeshProUGUI m_BestScoreValueText;

    #region VictoryHUDManager listeners
    /// <summary>
    /// OnGameWinEvent we open the WinPanel
    /// </summary>
    /// <param name="e"></param>
    private void OnGameWinEvent(GameVictoryEvent e)
    {
        this.OpenPanel(this.m_WinPanel);
    }

    /// <summary>
    /// OnGameOverEvent we open the GameOverPanel
    /// </summary>
    /// <param name="e"></param>
    private void OnGameOverEvent(GameOverEvent e)
    {
        this.OpenPanel(this.m_GameOverPanel);
    }
    #endregion

    private void SetTimeValueText(float time)
    {
        this.m_TimeValueText.text = time.ToString();
    }

    private void SetBestTimeValueText(float bestTime)
    {
        this.m_BestTimeValueText.text = bestTime.ToString();
    }

    private void SetScoreValueText(int score)
    {
        this.m_ScoreValueText.text = score.ToString();
    }

    private void SetBestScoreText(int bestScore)
    {
        this.m_BestScoreValueText.text = bestScore.ToString();
    }

    #region MonoBehaviour Methods
    protected override void Awake()
    {
        base.Awake();
        base.Panels.AddRange(new GameObject[] { this.m_WinPanel, this.m_GameOverPanel });
    }

    private void Start()
    {
        SaveData save = SaveData.LoadPlayerRefs();
        this.SetTimeValueText(save.Time);
        this.SetBestTimeValueText(save.BestTime);
        this.SetScoreValueText(save.Score);
        this.SetBestScoreText(save.BestScore);
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener<GameVictoryEvent>(OnGameWinEvent);
        EventManager.Instance.AddListener<GameOverEvent>(OnGameOverEvent);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener<GameVictoryEvent>(OnGameWinEvent);
        EventManager.Instance.RemoveListener<GameOverEvent>(OnGameOverEvent);
    }
    #endregion
}
