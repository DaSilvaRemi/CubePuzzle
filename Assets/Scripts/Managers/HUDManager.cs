using System.Collections;
using System.Collections.Generic;
using TMPro;
using SDD.Events;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [Header("HUD TEXT")]
    [Tooltip("TextMeshPro")]
    [SerializeField] private TextMeshProUGUI m_TimeLeftValueTxt;

    private void SetTimeValueText(float time)
    {
        m_TimeLeftValueTxt.text = time.ToString("N01");
    }

    private void OnGameStatisticsChangedEvent(GameStatisticsChangedEvent gameStatisticsChangedEvent)
    {
        SetTimeValueText(gameStatisticsChangedEvent.eCountdown);
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener<GameStatisticsChangedEvent>(OnGameStatisticsChangedEvent);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener<GameStatisticsChangedEvent>(OnGameStatisticsChangedEvent);
    }
}
