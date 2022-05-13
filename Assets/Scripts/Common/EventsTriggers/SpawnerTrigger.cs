using SDD.Events;
using UnityEngine;

public abstract class SpawnerTrigger : MonoBehaviour
{
    private bool m_IsAlreadyTriggered = false;
    private SDD.Events.Event m_EventToThrown;

    #region SpawnerTrigger properties
    protected bool IsAlreadyTriggered { get => this.m_IsAlreadyTriggered; }

    protected SDD.Events.Event EventToThrown { get => this.m_EventToThrown; set => this.m_EventToThrown = value; }
    #endregion 

    #region SpawnerTrigger methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
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
