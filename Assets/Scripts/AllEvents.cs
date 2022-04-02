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

public class GameEndLVLEvent : SDD.Events.Event
{
}

public class GameStatisticsChangedEvent : SDD.Events.Event
{
	public int eScore { get; set; }
	public float eBestTime { get; set; }
	public float eTime { get; set; }
	public float eCountdown { get; set; }
}
#endregion

#region MenuManager Events
public class CustomButtonClickedEvent : SDD.Events.Event
{
	public AudioClip eOnCustomButtonClick { get; set; }
}

public class ButtonClickedEvent : CustomButtonClickedEvent
{
}

public class NewGameButtonClickedEvent : ButtonClickedEvent
{
}
public class LoadGameButtonClickedEvent : ButtonClickedEvent
{
}
public class HelpButtonClickedEvent : ButtonClickedEvent
{
}

public class CreditButtonClickedEvent : ButtonClickedEvent
{
}

public class ExitButtonClickedEvent : ButtonClickedEvent
{ 
}

public class ChangeCameraButtonClickedEvent : ButtonClickedEvent
{
}

public class MainMenuButtonClickedEvent : ButtonClickedEvent
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

public class LevelGameOverEvent : SDD.Events.Event
{
}
#endregion

#region Trigger/Collider events
public class ChestHasTrigerEnterEvent : SDD.Events.Event
{
	public GameObject eChestGO;
	public GameObject eTriggeredGO;
}

public class TargetHasCollidedEnterEvent : SDD.Events.Event
{
	public GameObject eTargetGO;
	public GameObject eCollidedGO;
}

#endregion

public class PlaySFXEvent : SDD.Events.Event
{
	public AudioSource eAudioSource { get; set; }
	public AudioClip eAudioClip { get; set; }
}

public class StopSFXWithEvent : SDD.Events.Event
{
	public AudioSource eAudioSource { get; set; }
	public AudioClip eAudioClip { get; set; }
}