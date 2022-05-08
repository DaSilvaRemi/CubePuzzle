using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActivateOthers : MonoBehaviour
{
    [Header("GO linkeds to the object")]
    [Tooltip("GO to activate when object triggered")]
    [SerializeField] GameObject m_GameObjectToActivate;
    [Tooltip("GO linked to the object")]
    [SerializeField] GameObject[] m_GamesObjectsLinked;

    protected GameObject GameObjectToActivate { get { return this.m_GameObjectToActivate; } }
    protected GameObject[] GamesObjectsLinked { get { return this.m_GamesObjectsLinked; } }

    protected virtual void OnObjectTriggered()
    {
        foreach (GameObject item in this.m_GamesObjectsLinked)
        {
            if (item)
            {
                item.SetActive(item.Equals(this.m_GameObjectToActivate));
            }
        }
    }

    /// <summary>
    /// OnTriggerEnter we call <see cref="OnObjectTriggered"/>
    /// </summary>
    /// <param name="other">The collider</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("ThrowableObject")))
        {
            this.OnObjectTriggered();
        }
    }
}
