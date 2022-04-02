using System.Collections;
using System.Collections.Generic;
using TMPro;
using SDD.Events;
using UnityEngine;

public class HUDManager : Manager<HUDManager>, IEventHandler
{
    [Header("HUD TEXT")]
    [Tooltip("TextMeshPro")]
    [SerializeField] private TextMeshProUGUI m_TimeLeftValueTxt;
    [SerializeField] private TextMeshProUGUI m_ScoreValueTxt;

    #region Setters
    private void SetTimeValueText(float time)
    {
        this.m_TimeLeftValueTxt.text = time.ToString("N01");
    }

    private void SetScoreValueText(int score)
    {
        this.m_ScoreValueTxt.text = score.ToString("N01");
    }
    #endregion

    #region Event Listeners
    private void OnGameStatisticsChangedEvent(GameStatisticsChangedEvent gameStatisticsChangedEvent)
    {
        this.SetTimeValueText(gameStatisticsChangedEvent.eCountdown);
        this.SetScoreValueText(gameStatisticsChangedEvent.eScore);
    }
    #endregion

    #region Events Suscribption
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<GameStatisticsChangedEvent>(OnGameStatisticsChangedEvent);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<GameStatisticsChangedEvent>(OnGameStatisticsChangedEvent);
    }
    #endregion

    #region MonoBehaviour methods
    private void Awake()
    {
        base.InitManager();
    }

    private void OnEnable()
    {
        this.SubscribeEvents();
    }

    private void OnDisable()
    {
        this.UnsubscribeEvents();
    }
    #endregion
}
