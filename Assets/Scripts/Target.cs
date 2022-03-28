using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class Target : MonoBehaviour
{
    [SerializeField] GameObject[] m_GamesObjectsLinked;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && collision.gameObject.CompareTag("ThrowableObject"))
        {
            EventManager.Instance.Raise(new OnTargetHasCollidedEnterEvent { eTargetGO = this.gameObject, eCollidedGO = collision.gameObject });
            this.ChangeGameObjectsLinkedTag();
        } 
    }

    private void ChangeGameObjectsLinkedTag()
    {
        foreach (GameObject gameObject in this.m_GamesObjectsLinked)
        {
            if (gameObject.CompareTag("Enemy"))
            {
                gameObject.tag = "Untagged";
            }

            Tools.SetColor(gameObject.GetComponentInChildren<MeshRenderer>(), new Color(0, 255, 0));
        }
    }
}
