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
    /// <summary>
    /// Set the time value text
    /// </summary>
    /// <param name="time">The time</param>
    private void SetTimeValueText(float time)
    {
        if (this.m_TimeLeftValueTxt)
        {
            this.m_TimeLeftValueTxt.text = time.ToString("N01");
        }
    }

    /// <summary>
    /// Set score value text
    /// </summary>
    /// <param name="score">The score</param>
    private void SetScoreValueText(int score)
    {
        if (this.m_ScoreValueTxt)
        {
            this.m_ScoreValueTxt.text = score.ToString();
        }
    }
    #endregion

    #region Event Listeners
    /// <summary>
    /// On gameStatisticsChangedEvent we calls <see cref="SetTimeValueText"/> and <see cref="SetScoreValueText"/>
    /// </summary>
    /// <param name="gameStatisticsChangedEvent"></param>
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
