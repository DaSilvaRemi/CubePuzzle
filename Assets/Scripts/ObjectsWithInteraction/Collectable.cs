using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : ObjectWillEarnThings
{
    #region MonoBehaviour methods
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && collision.gameObject != null && collision.gameObject.CompareTag("Player"))
        {
            base.OnInteractionWithTheObjectEarnScore(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
    #endregion
}
