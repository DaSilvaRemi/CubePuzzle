using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class Button : MonoBehaviour
{
    [SerializeField] GameObject m_GameObjectToActivate;
    [SerializeField] GameObject[] m_GamesObjectsLinked;

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

    private void OnTriggerEnter(Collider other)
    {
        this.OnButtonTriggered();
    }
}
