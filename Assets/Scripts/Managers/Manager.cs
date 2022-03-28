using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; private set; }

    protected void InitManager()
    {
        if (!Manager<T>.Instance)
        {
            Manager<T>.Instance = this as T;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
