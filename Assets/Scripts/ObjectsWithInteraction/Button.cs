using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class Button : ObjectActivateOthers
{
    /// <summary>
    /// On Button triggered we activate the defined game object if it was present in the list and we send <see cref="ButtonClickedEvent"/> and <see cref="ButtonActivateGOClickedEvent"/>
    /// </summary>
    private void OnButtonTriggered()
    {
        EventManager.Instance.Raise(new ButtonClickedEvent());
        EventManager.Instance.Raise(new ButtonActivateGOClickedEvent() { eGameObject = base.GameObjectToActivate });
        base.OnObjectTriggered();
    }

    /// <summary>
    /// OnTriggerEnter we call <see cref="OnButtonTriggered"/>
    /// </summary>
    /// <param name="other">The collider</param>
    private void OnTriggerEnter(Collider other)
    {
        this.OnButtonTriggered();
    }
}
