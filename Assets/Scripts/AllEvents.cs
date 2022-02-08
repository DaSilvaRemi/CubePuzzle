using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

#region GameManager Events
public class GameMenuEvent : SDD.Events.Event
{
}
public class GameInitLevelEvent : SDD.Events.Event
{
}
public class GamePlayEvent : SDD.Events.Event
{
}
public class GamePauseEvent : SDD.Events.Event
{
}
public class GameResumeEvent : SDD.Events.Event
{
}
public class GameOverEvent : SDD.Events.Event
{
}
public class GameVictoryEvent : SDD.Events.Event
{
}

public class GameStatisticsChangedEvent : SDD.Events.Event
{
	public float eBestTime { get; set; }
	public float eTime { get; set; }
	public float eCountdown { get; set; }
}
#endregion

#region MenuManager Events
public class NewGameButtonClickedEvent : SDD.Events.Event
{
}
public class LoadGameButtonClickedEvent : SDD.Events.Event
{
}
public class HelpButtonClickedEvent : SDD.Events.Event
{
}

public class CreditButtonClickedEvent : SDD.Events.Event
{
}

public class ExitButtonClickedEvent : SDD.Events.Event
{ 
}

public class ChangeCameraButtonClickedEvent : SDD.Events.Event
{
}

public class MainMenuButtonClickedEvent : SDD.Events.Event
{
}
#endregion

#region Score Event
public class ScoreHasBeenGainedEvent : SDD.Events.Event
{
	public int eScore;
}
#endregion

#region Level events
public class LevelHasBeenInitializedEvent:SDD.Events.Event
{
	public Transform ePlayerSpawnPoint;
}

public class LevelFinishEvent : SDD.Events.Event
{
}
#endregion

#region Chest events
public class ChestHasTrigerEnterEvent : SDD.Events.Event
{
	public GameObject eTriggeredGO;
}
#endregion