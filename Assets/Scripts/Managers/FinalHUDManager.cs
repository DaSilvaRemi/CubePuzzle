using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalHUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_VictoryText;
    [SerializeField] private TextMeshProUGUI m_GameOverText;

    private void Start()
    {
        UpdateEndScene(GameManager.GameState);
        GameManager.GameState = Tools.GameState.PLAY;
    }

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
    }
}
