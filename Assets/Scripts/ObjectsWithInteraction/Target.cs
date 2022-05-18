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

    [Header("Target GO color change")]
    [Tooltip("Before Color (RGB)")]
    [SerializeField] private Color m_GOLinkedColorBeforeCollision = Color.red;
    [Tooltip("After Color (RGB)")]
    [SerializeField] private Color m_GOLinkedColorAfterCollision = Color.green;

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
        this.SetColorToAllGOLinked(this.m_GOLinkedColorAfterCollision);
        this.m_MyColorChangedActionCoroutine = Tools.MyActionTimedCoroutine(this.m_TimeGoActivate, null, null, this.LambdaResetDefaultGameObjectLinkedColor);
        StartCoroutine(this.m_MyColorChangedActionCoroutine);
    }

    /**
     * <summary>>Lambda function to set the default game object linked color</summary>
     */
    private void LambdaResetDefaultGameObjectLinkedColor()
    {
        this.SetColorToAllGOLinked(this.m_GOLinkedColorBeforeCollision);
    }

    /// <summary>
    /// Set a color to all linked game object
    /// </summary>
    /// <param name="newColor">The new color</param>
    private void SetColorToAllGOLinked(Color newColor)
    {
        for (int i = 0; i < this.m_GOLinkedToColorChange.Length; i++)
        {
            Tools.SetColor(this.m_GOLinkedToColorChange[i].GetComponentInChildren<MeshRenderer>(), newColor);
        }
    }
    #endregion

    #region MonoBehaviour methods
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && collision.gameObject != null && collision.gameObject.CompareTag("ThrowableObject"))
        {
            this.OnInteractionWithTheObjectEarnScore(collision.gameObject);
            EventManager.Instance.Raise(new SpawnedGameObjectToDestroyEvent() { eGameObjectToDestroy = this.gameObject });
        }
    }

    private void OnDestroy()
    {
        if (this.m_MyColorChangedActionCoroutine != null) StopCoroutine(this.m_MyColorChangedActionCoroutine);
    }
    #endregion
}
