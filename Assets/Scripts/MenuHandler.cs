using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MenuHandler : MonoBehaviour
{
    public void HandleCreateNewGameButton()
    {
        SaveGame.Save(new SaveGame(0, 0));
        SceneManager.LoadScene("FirstLevelScene");
    }

    public void HandleLoadGameButton()
    {

    }

    public void HandleHelpButton()
    {
        SceneManager.LoadScene("HelpScene");
    }

    public void HandleCreditButton()
    {
        SceneManager.LoadScene("CreditScene");
    }

    public void HandleBackToMenuButton()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void HandleExitButton()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
