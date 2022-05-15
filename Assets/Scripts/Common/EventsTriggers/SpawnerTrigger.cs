using SDD.Events;
using UnityEngine;

/// <summary>
/// Abstract class throw an <see cref="SDD.Events.Event"/> when player enter in defined children trigger
/// </summary>
public abstract class SpawnerTrigger : MonoBehaviour
{
    private bool m_IsAlreadyTriggered = false;
    private SDD.Events.Event m_EventToThrown;

    #region SpawnerTrigger properties
    /// <summary>
    /// The event to thrown, need to be defined by childrens 
    /// </summary>
    protected SDD.Events.Event EventToThrown { get => this.m_EventToThrown; set => this.m_EventToThrown = value; }
    #endregion 

    #region SpawnerTrigger methods
    /// <summary>
    /// When trigger is thrown we checked if it's not already triggered and if other is not null and equal to player
    /// </summary>
    /// <param name="other">The other collider</param>
    protected virtual void SpawnTriggerThrown(Collider other)
    {
        if (this.m_IsAlreadyTriggered || other == null || (other != null && !other.gameObject.CompareTag("Player"))) return;

        EventManager.Instance.Raise(this.EventToThrown);
        this.m_IsAlreadyTriggered = true;
    }
    #endregion

    #region MonoBehaviour methods
    protected abstract void Awake();

    /// <summary>
    /// OnTriggerEnter we throw <see cref="EventToThrown"/> and set <seealso cref="m_IsAlreadyTriggered"/>
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        this.SpawnTriggerThrown(other);
    }
    #endregion 
}
