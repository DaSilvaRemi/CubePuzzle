using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// When new game button clicked we called <see cref="OnClickButton"/> and send <see cref="NewGameButtonClickedEvent"/>
    /// </summary>
    public void HandleCreateNewGameButton()
    {
        this.OnClickButton();
        EventManager.Instance.Raise(new NewGameButtonClickedEvent());
    }

    /// <summary>
    /// When load game button clicked we called <see cref="OnClickButton"/> and send <see cref="LoadGameButtonClickedEvent"/>
    /// </summary>
    public void HandleLoadGameButton()
    {
        this.OnClickButton();
        EventManager.Instance.Raise(new LoadGameButtonClickedEvent());
    }

    /// <summary>
    /// When help button clicked we called <see cref="OnClickButton"/> and send <see cref="HelpButtonClickedEvent"/>
    /// </summary>
    public void HandleHelpButton()
    {
        this.OnClickButton();
        EventManager.Instance.Raise(new HelpButtonClickedEvent());
    }

    /// <summary>
    /// When credit button clicked we called <see cref="OnClickButton"/> and send <see cref="CreditButtonClickedEvent"/>
    /// </summary>
    public void HandleCreditButton()
    {
        this.OnClickButton();
        EventManager.Instance.Raise(new CreditButtonClickedEvent());
    }

    /// <summary>
    /// When exit game button clicked we called <see cref="OnClickButton"/> and send <see cref="ExitButtonClickedEvent"/>
    /// </summary>
    public void HandleExitButton()
    {
        this.OnClickButton();
        EventManager.Instance.Raise(new ExitButtonClickedEvent());
    }

    /// <summary>
    /// When back to menu button clicked we called <see cref="OnClickButton"/> and send <see cref="MainMenuButtonClickedEvent"/>
    /// </summary>
    public void HandleBackToMenuButton()
    {
        this.OnClickButton();
        EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
    }

    /// <summary>
    /// When a button was clicking we send <see cref="ButtonClickedEvent"/>
    /// </summary>
    public void OnClickButton()
    {
        EventManager.Instance.Raise(new ButtonClickedEvent());
    }
}
