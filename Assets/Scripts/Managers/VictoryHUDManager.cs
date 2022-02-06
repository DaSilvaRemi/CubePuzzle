using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using SDD.Events;

public class VictoryHUDManager : MonoBehaviour
{
    [Header("Victory / GameOver txt")]
    [Tooltip("TextMeshPro")]
    [SerializeField] private GameObject m_WinPanel;
    [Tooltip("TextMeshPro")]
    [SerializeField] private GameObject m_GameOverPanel;

    [Header("Time and Best Time values txt")]
    [Tooltip("TextMeshPro")]
    [SerializeField] private TextMeshProUGUI m_TimeValueText;
    [Tooltip("TextMeshPro")]
    [SerializeField] private TextMeshProUGUI m_BestTimeValueText;

    private List<GameObject> m_Panels = new List<GameObject>();

    private void OpenPanel(GameObject panel)
    {
        m_Panels.ForEach(item => { if (item) { item.SetActive(item == panel); } });
    }

    private void OnGameWinEvent(GameVictoryEvent e)
    {
        OpenPanel(m_WinPanel);
    }

    private void OnGameOverEvent(GameOverEvent e)
    {
        OpenPanel(m_GameOverPanel);
    }

    private void Awake()
    {
        m_Panels.AddRange(new GameObject[] { m_WinPanel, m_GameOverPanel });
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
}
