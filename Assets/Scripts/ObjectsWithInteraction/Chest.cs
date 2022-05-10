using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class Chest : ObjectWillEarnThings
{
    #region ObjectWillEarnThings Methods
    protected override void OnInteractionWithTheObjectEarnTime(GameObject gameObject)
    {
        base.OnInteractionWithTheObjectEarnTime(gameObject);
    }
    #endregion

    #region MonoBehaviour Methods

    /// <summary>
    /// On Trigger enter we call <see cref="OnInteractionWithTheObjectEarnTime"/>
    /// </summary>
    /// <param name="other">The other element</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.gameObject != null)
        {
            this.OnInteractionWithTheObjectEarnTime(other.gameObject);
        }
    }
    #endregion
}
