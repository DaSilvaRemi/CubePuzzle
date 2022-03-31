using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class Target : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_GamesObjectsLinked;

    private IEnumerator m_MyActionCoroutine = null;

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
                gameObject.tag = "Dalle";
            }

            Tools.SetColor(gameObject.GetComponentInChildren<MeshRenderer>(), new Color(0, 255, 0));
        }

        this.m_MyActionCoroutine = Tools.MyActionCoroutine(3.0f, null, null, () =>
        {
            for (int i = 0; i < m_GamesObjectsLinked.Length; i++)
            {
                Tools.SetColor(m_GamesObjectsLinked[i].GetComponentInChildren<MeshRenderer>(), new Color(255, 0, 0));
            }
        });
        StartCoroutine(this.m_MyActionCoroutine);
    }

    private void OnDestroy()
    {
        if (this.m_MyActionCoroutine != null) StopCoroutine(this.m_MyActionCoroutine);
    }

}
