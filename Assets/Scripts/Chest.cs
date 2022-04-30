using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class Chest : MonoBehaviour
{
    /// <summary>
    /// On Trigger enter we send an <see cref="ChestHasTrigerEnterEvent"/>
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        EventManager.Instance.Raise(new ChestHasTrigerEnterEvent() { eChestGO = this.gameObject, eTriggeredGO = other.gameObject });
    }
}
