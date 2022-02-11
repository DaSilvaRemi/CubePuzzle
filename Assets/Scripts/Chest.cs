using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class Chest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        EventManager.Instance.Raise(new ChestHasTrigerEnterEvent() { eChestGO = this.gameObject, eTriggeredGO = other.gameObject });
    }
}
