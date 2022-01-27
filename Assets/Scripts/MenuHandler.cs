using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MenuHandler : MonoBehaviour
{
    public void HandleCreateNewGameButton()
    {
        SceneManager.LoadScene("MainScene");
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

    public void HandleExitButton()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
