using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Define when trigger is thrown the <see cref="SpawnEachTimeLinearEvent"/> it's raised
/// </summary>
public class EachTimeLinearSpawnTrigger : SpawnerTrigger
{
    [Header("EachTimeLinearEvent properties")]
    [Tooltip("Duration to spawn other object")]
    [SerializeField] private float m_TimeToSpawn = 0.2f;
     
    protected override void Awake()
    {
        this.EventToThrown = new SpawnEachTimeLinearEvent() { eSpawnTime = this.m_TimeToSpawn };
    }
}
