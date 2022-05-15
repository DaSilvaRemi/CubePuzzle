using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class MovableObjects : MonoBehaviour
{
    /// <summary>
    /// OnTriggerEnter we raise <see cref="SelectGameObjectToInvertEvent"/> and <see cref="ButtonClickedEvent"/>
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other == null || (other != null && !other.gameObject.CompareTag("Player"))) return;
        EventManager.Instance.Raise(new SelectGameObjectToInvertEvent() { eGameObjectToInvert = this.gameObject });
        EventManager.Instance.Raise(new ButtonClickedEvent());
    }
}
