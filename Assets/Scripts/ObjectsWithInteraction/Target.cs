using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class Target : ObjectWillEarnThings
{
    [Header("Target GO Linked behaviour")]
    [Tooltip("GO Linked to the target to color change")]
    [SerializeField] private GameObject[] m_GOLinkedToColorChange;
    [Tooltip("Time GO will be activate")]
    [SerializeField] private float m_TimeGoActivate;

    private IEnumerator m_MyColorChangedActionCoroutine = null;
    private bool m_IsAlreadyCollided = false;

    #region ObjectWillEarnThings Methods
    protected override void OnInteractionWithTheObjectEarnScore(GameObject gameObject)
    {
        if (!this.m_IsAlreadyCollided)
        {
            this.m_IsAlreadyCollided = true;
            base.OnInteractionWithTheObjectEarnScore(gameObject);
            this.ChangeColorOfGameObjectsLinked();
        }
    }
    #endregion

    #region Target Methods
    /**
     * <summary>Changes game objects color linked to the target</summary> 
     * <remarks>Swith to red and green</remarks>
     */
    private void ChangeColorOfGameObjectsLinked()
    {
        foreach (GameObject gameObject in this.m_GOLinkedToColorChange)
        {
            Tools.SetColor(gameObject.GetComponentInChildren<MeshRenderer>(), new Color(0, 255, 0));
        }

        this.m_MyColorChangedActionCoroutine = Tools.MyActionTimedCoroutine(this.m_TimeGoActivate, null, null, this.LambdaResetDefaultGameObjectLinkedColor);
        StartCoroutine(this.m_MyColorChangedActionCoroutine);
    }

    /**
     * <summary>>Lambda function to set the default game object linked color</summary>
     */
    private void LambdaResetDefaultGameObjectLinkedColor()
    {
        for (int i = 0; i < this.m_GOLinkedToColorChange.Length; i++)
        {
            Tools.SetColor(this.m_GOLinkedToColorChange[i].GetComponentInChildren<MeshRenderer>(), new Color(255, 0, 0));
        }
    }
    #endregion

    #region MonoBehaviour methods
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && collision.gameObject != null)
        {
            this.OnInteractionWithTheObjectEarnScore(collision.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (this.m_MyColorChangedActionCoroutine != null) StopCoroutine(this.m_MyColorChangedActionCoroutine);
    }
    #endregion
}
