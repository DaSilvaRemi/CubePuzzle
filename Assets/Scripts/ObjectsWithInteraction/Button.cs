using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class Button : MonoBehaviour
{
    [Header("GO linkeds to the button")]
    [Tooltip("GO to activate when button clicked")]
    [SerializeField] GameObject m_GameObjectToActivate;
    [Tooltip("GO linked to the button")]
    [SerializeField] GameObject[] m_GamesObjectsLinked;

    /// <summary>
    /// On Button triggered we acitvate the defined game object if it was present in the list and we send <see cref="ButtonClickedEvent"/> and <see cref="ButtonActivateGOClickedEvent"/>
    /// </summary>
    private void OnButtonTriggered()
    {
        EventManager.Instance.Raise(new ButtonClickedEvent());
        EventManager.Instance.Raise(new ButtonActivateGOClickedEvent() { eGameObject = this.m_GameObjectToActivate });

        foreach (GameObject item in this.m_GamesObjectsLinked)
        {
            if (item) { 
                item.SetActive(item.Equals(this.m_GameObjectToActivate)); 
            }
        }
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
