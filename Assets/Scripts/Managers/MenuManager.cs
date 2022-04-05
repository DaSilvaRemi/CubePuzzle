using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class MenuManager : MonoBehaviour
{
    public void HandleCreateNewGameButton()
    {
        this.OnClickButton();
        EventManager.Instance.Raise(new NewGameButtonClickedEvent());
    }

    public void HandleLoadGameButton()
    {
        this.OnClickButton();
        EventManager.Instance.Raise(new LoadGameButtonClickedEvent());
    }

    public void HandleHelpButton()
    {
        this.OnClickButton();
        EventManager.Instance.Raise(new HelpButtonClickedEvent());
    }

    public void HandleCreditButton()
    {
        this.OnClickButton();
        EventManager.Instance.Raise(new CreditButtonClickedEvent());
    }

    public void HandleExitButton()
    {
        this.OnClickButton();
        EventManager.Instance.Raise(new ExitButtonClickedEvent());
    }

    public void HandleBackToMenuButton()
    {
        this.OnClickButton();
        EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
    }

    public void OnClickButton()
    {
        EventManager.Instance.Raise(new ButtonClickedEvent());
    }
}
