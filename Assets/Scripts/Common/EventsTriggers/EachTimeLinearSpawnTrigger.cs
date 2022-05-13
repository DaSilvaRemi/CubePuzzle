using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
