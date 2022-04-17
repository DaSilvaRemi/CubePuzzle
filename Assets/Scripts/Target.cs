using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class Target : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_GamesObjectsLinked;

    private IEnumerator m_MyActionCoroutine = null;

    private void ChangeGameObjectsLinkedTag()
    {
        foreach (GameObject gameObject in this.m_GamesObjectsLinked)
        {
            Tools.SetColor(gameObject.GetComponentInChildren<MeshRenderer>(), new Color(0, 255, 0));
        }

        this.m_MyActionCoroutine = Tools.MyActionCoroutine(3.0f, null, null, this.LambdaResetDefaultGameObjectLinkedColor);
        StartCoroutine(this.m_MyActionCoroutine);
    }

    private void LambdaResetDefaultGameObjectLinkedColor()
    {
        for (int i = 0; i < this.m_GamesObjectsLinked.Length; i++)
        {
            Tools.SetColor(this.m_GamesObjectsLinked[i].GetComponentInChildren<MeshRenderer>(), new Color(255, 0, 0));
        }
    }

    #region MonoBehaviour methods
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            EventManager.Instance.Raise(new TargetHasCollidedEnterEvent { eTargetGO = this.gameObject, eCollidedGO = collision.gameObject });
            this.ChangeGameObjectsLinkedTag();
        }
    }

    private void OnDestroy()
    {
        if (this.m_MyActionCoroutine != null) StopCoroutine(this.m_MyActionCoroutine);
    }
    #endregion
}
