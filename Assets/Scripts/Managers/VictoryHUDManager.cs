using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using SDD.Events;

public class VictoryHUDManager : Manager<VictoryHUDManager>
{
    [Header("Victory / GameOver txt")]
    [Tooltip("Game Object")]
    [SerializeField] private GameObject m_WinPanel;
    [Tooltip("Game Object")]
    [SerializeField] private GameObject m_GameOverPanel;

    [Header("Time and Best Time values txt")]
    [Tooltip("TextMeshPro")]
    [SerializeField] private TextMeshProUGUI m_TimeValueText;
    [Tooltip("TextMeshPro")]
    [SerializeField] private TextMeshProUGUI m_BestTimeValueText;

    private readonly List<GameObject> m_Panels = new List<GameObject>();

    /// <summary>
    /// Open a panel in the list
    /// </summary>
    /// <param name="panel">The panel to open</param>
    private void OpenPanel(GameObject panel)
    {
        this.m_Panels.ForEach(item => { if (item) { item.SetActive(item.Equals(panel)); } });
    }

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

    #region MonoBehaviour Methods
    private void Awake()
    {
        base.InitManager();
        this.m_Panels.AddRange(new GameObject[] { this.m_WinPanel, this.m_GameOverPanel });
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
