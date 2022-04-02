using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class MenuManager : MonoBehaviour
{
    public void HandleCreateNewGameButton()
    {
        EventManager.Instance.Raise(new NewGameButtonClickedEvent());
    }

    public void HandleLoadGameButton()
    {
        EventManager.Instance.Raise(new LoadGameButtonClickedEvent());
    }

    public void HandleHelpButton()
    {
        EventManager.Instance.Raise(new HelpButtonClickedEvent());
    }

    public void HandleCreditButton()
    {
        EventManager.Instance.Raise(new CreditButtonClickedEvent());
    }

    public void HandleExitButton()
    {
        EventManager.Instance.Raise(new ExitButtonClickedEvent());
    }

    public void HandleBackToMenuButton()
    {
        EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
    }
}
