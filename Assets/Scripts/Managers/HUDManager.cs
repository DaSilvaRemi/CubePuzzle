using System.Collections;
using System.Collections.Generic;
using TMPro;
using SDD.Events;
using UnityEngine;

public class HUDManager : MonoBehaviour, IEventHandler
{
    [Header("HUD TEXT")]
    [Tooltip("TextMeshPro")]
    [SerializeField] private TextMeshProUGUI m_TimeLeftValueTxt;
    [SerializeField] private TextMeshProUGUI m_ScoreValueTxt;

    private void SetTimeValueText(float time)
    {
        this.m_TimeLeftValueTxt.text = time.ToString("N01");
    }

    private void SetScoreValueText(int score)
    {
        this.m_ScoreValueTxt.text = score.ToString("N01");
    }

    private void OnGameStatisticsChangedEvent(GameStatisticsChangedEvent gameStatisticsChangedEvent)
    {
        this.SetTimeValueText(gameStatisticsChangedEvent.eCountdown);
        this.SetScoreValueText(gameStatisticsChangedEvent.eScore);
    }

    #region MonoBehaviour methods
    private void OnEnable()
    {
        this.SubscribeEvents();
    }

    private void OnDisable()
    {
        this.UnsubscribeEvents();
    }
    #endregion

    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<GameStatisticsChangedEvent>(OnGameStatisticsChangedEvent);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<GameStatisticsChangedEvent>(OnGameStatisticsChangedEvent);
    }
}
