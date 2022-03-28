using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class ObjectScoreActivable : MonoBehaviour, IEventHandler
{
    [SerializeField] private int m_TargetScore;

    private void OnGameStatisticsChangedEvent(GameStatisticsChangedEvent e)
    {
        if (e.eScore >= this.m_TargetScore)
        {
            this.gameObject.SetActive(!this.gameObject.activeSelf);
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
