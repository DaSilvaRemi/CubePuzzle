using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] GameObject m_GameObjectToActivate;
    [SerializeField] GameObject[] m_GamesObjectsLinked;

    private void OnTriggerEnter(Collider other)
    {
        this.OnButtonTriggered();
    }

    private void OnButtonTriggered()
    {
        foreach (GameObject item in this.m_GamesObjectsLinked)
        {
            if (item) { 
                item.SetActive(item.Equals(this.m_GameObjectToActivate)); 
            }
        }
    }
}
