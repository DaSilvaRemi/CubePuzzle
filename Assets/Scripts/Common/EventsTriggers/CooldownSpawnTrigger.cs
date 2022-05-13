using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class CooldownSpawnTrigger : SpawnerTrigger
{
    protected override void Awake()
    {
        base.EventToThrown = new StartCooldownSpawnEvent();
    }
}
