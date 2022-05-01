using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class SpawnTriggerScript : MonoBehaviour
{
    private bool m_IsAlreadyTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (this.m_IsAlreadyTriggered || other == null || (other != null && !other.gameObject.CompareTag("Player"))) return;

        EventManager.Instance.Raise(new StartCooldownSpawnEvent());
    }
}
