using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class ObjectScoreActivable : MonoBehaviour, IEventHandler
{
    [Tooltip("Score to obtain to activate the object")]
    [SerializeField] private int m_TargetScore;
    private bool m_IsAlreadyActivated = false;

    /// <summary>
    /// On game statistic changed event we check the score to activate the object
    /// </summary>
    /// <param name="e"></param>
    private void OnGameStatisticsChangedEvent(GameStatisticsChangedEvent e)
    {
        if (e.eScore >= this.m_TargetScore && !m_IsAlreadyActivated)
        {
            this.gameObject.SetActive(!this.gameObject.activeSelf);
            m_IsAlreadyActivated = true;
        }
    }

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

    #region MonoBehaviour METHODS
    private void Awake()
    {
        this.SubscribeEvents();
    }

    private void OnDestroy()
    {
        this.UnsubscribeEvents();
    }
    #endregion
}
