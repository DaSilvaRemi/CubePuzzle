﻿using System.Collections;
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
public class ButtonActivateGOClickedEvent : SDD.Events.Event
{
	public GameObject eGameObject { get; set; }
}


public class CustomButtonClickedEvent : SDD.Events.Event
{
	public AudioClip eOnCustomButtonClick { get; set; }
}

public class ButtonClickedEvent : SDD.Events.Event
{

}

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

#region SFX EVENTS
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
#endregion