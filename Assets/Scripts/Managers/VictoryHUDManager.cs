using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    private void OnGameWinEvent()
    {

    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    /**
     * <summary>Update the HUD of the end scene</summary> 
     */
   /* public void UpdateEndScene(Tools.GameState gameState)
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
        m_TimeText.text = GameManager.GameTimePassed;
        m_BestTimeText.text = GameManager.GameBestTime;
    }*/
}
