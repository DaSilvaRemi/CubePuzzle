using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

/// <summary>
/// Define when trigger is thrown the <see cref="StartCooldownSpawnEvent"/> it's raised
/// </summary>
public class CooldownSpawnTrigger : SpawnerTrigger
{
    protected override void Awake()
    {
        base.EventToThrown = new StartCooldownSpawnEvent();
    }
}
