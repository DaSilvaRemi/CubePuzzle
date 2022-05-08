using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class MovableObjects : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other == null || (other != null && !other.gameObject.CompareTag("Player"))) return;
        EventManager.Instance.Raise(new SelectGameObjectToInvertEvent() { eGameObjectToInvert = this.gameObject });
        EventManager.Instance.Raise(new ButtonClickedEvent());
    }
}
