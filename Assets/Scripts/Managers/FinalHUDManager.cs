using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalHUDManager : MonoBehaviour
{
    [Header("HUD TEXT")]
    [Tooltip("TextMeshPro")]
    [SerializeField] private TextMeshProUGUI m_VictoryText;
    [Tooltip("TextMeshPro")]
    [SerializeField] private TextMeshProUGUI m_GameOverText;
    [Tooltip("TextMeshPro")]
    [SerializeField] private TextMeshProUGUI m_TimeText;
    [Tooltip("TextMeshPro")]
    [SerializeField] private TextMeshProUGUI m_BestTimeText;

    private void Start()
    {
        UpdateEndScene(GameManager.GameState);
        GameManager.GameState = Tools.GameState.PLAY;
    }

    /**
     * <summary>Update the HUD of the end scene</summary> 
     */
    public void UpdateEndScene(Tools.GameState gameState)
    {
        switch (gameState)
        {
            case Tools.GameState.WIN:
                m_VictoryText.gameObject.SetActive(true);
                break;
            case Tools.GameState.LOOSE:
                m_GameOverText.gameObject.SetActive(true);
                break;
            default:
                break;
        }
        m_TimeText.text = "Time : " + GameManager.GameTimePassed;
        m_BestTimeText.text = "Best Time : " + GameManager.GameBestTime;
    }
}
