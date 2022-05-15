using SDD.Events;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    #region MenuManager Handlers
    /// <summary>
    /// When new game button clicked we called <see cref="OnClickButton"/> and send <see cref="NewGameButtonClickedEvent"/>
    /// </summary>
    public void HandleCreateNewGameButton()
    {
        EventManager.Instance.Raise(new NewGameButtonClickedEvent());
    }

    /// <summary>
    /// When load game button clicked we called <see cref="OnClickButton"/> and send <see cref="LoadGameButtonClickedEvent"/>
    /// </summary>
    public void HandleLoadGameButton()
    {
        EventManager.Instance.Raise(new LoadGameButtonClickedEvent());
    }

    /// <summary>
    /// When help button clicked we called <see cref="OnClickButton"/> and send <see cref="HelpButtonClickedEvent"/>
    /// </summary>
    public void HandleHelpButton()
    {
        EventManager.Instance.Raise(new HelpButtonClickedEvent());
    }

    /// <summary>
    /// When credit button clicked we called <see cref="OnClickButton"/> and send <see cref="CreditButtonClickedEvent"/>
    /// </summary>
    public void HandleCreditButton()
    {
        EventManager.Instance.Raise(new CreditButtonClickedEvent());
    }

    /// <summary>
    /// When exit game button clicked we called <see cref="OnClickButton"/> and send <see cref="ExitButtonClickedEvent"/>
    /// </summary>
    public void HandleExitButton()
    {
        EventManager.Instance.Raise(new ExitButtonClickedEvent());
    }

    /// <summary>
    /// When choose level button 1 clicked we called <see cref="OnClickButton"/> and send <see cref="ChooseALevelEvent"/>
    /// </summary>
    public void HandleChooseLVL1Button()
    {
        EventManager.Instance.Raise(new ChooseALevelEvent() { eGameScene = Tools.GameScene.FIRSTLEVELSCENE});
    }

    /// <summary>
    /// When choose level button 2 clicked we called <see cref="OnClickButton"/> and send <see cref="ChooseALevelEvent"/>
    /// </summary>
    public void HandleChooseLVL2Button()
    {
        EventManager.Instance.Raise(new ChooseALevelEvent() { eGameScene = Tools.GameScene.SECONDLVLSCENE });
    }

    /// <summary>
    /// When choose level button 3 clicked we called <see cref="OnClickButton"/> and send <see cref="ChooseALevelEvent"/>
    /// </summary>
    public void HandleChooseLVL3Button()
    {
        EventManager.Instance.Raise(new ChooseALevelEvent() { eGameScene = Tools.GameScene.THIRDLEVELSCENE });
    }

    /// <summary>
    /// When choose level button 4 clicked we called <see cref="OnClickButton"/> and send <see cref="ChooseALevelEvent"/>
    /// </summary>
    public void HandleChooseLVL4Button()
    {
        EventManager.Instance.Raise(new ChooseALevelEvent() { eGameScene = Tools.GameScene.FOURTHLEVELSCENE });
    }

    /// <summary>
    /// HandleReplayGameButton call <see cref="HandleCreateNewGameButton"/> when <see cref="GameManager.IsWinning"/> is true or else call <see cref="HandleLoadGameButton"/>
    /// </summary>
    public void HandleReplayGameButton()
    {
        if (GameManager.IsWinning)
        {
            this.HandleCreateNewGameButton();
        }
        else
        {
            this.HandleLoadGameButton();
        }
    }
    #endregion
}
