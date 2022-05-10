using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : ObjectWillEarnThings
{
    #region MonoBehaviour methods
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            base.OnInteractionWithTheObjectEarnScore(collision.gameObject);
        }
    }
    #endregion
}
