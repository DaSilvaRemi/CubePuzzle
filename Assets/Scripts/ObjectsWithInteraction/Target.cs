using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class Target : MonoBehaviour
{
    [Header("GO Linked behaviour")]
    [Tooltip("GO Linked to the target")]
    [SerializeField] private GameObject[] m_GamesObjectsLinked;
    [Tooltip("Durations GO will be activate")]
    [SerializeField] private float m_DurationGoActivate;

    [Header("Target behaviour")]
    [Tooltip("Target audio clip")]
    [SerializeField] private AudioClip m_TargetCollisionSfxClip;

    private IEnumerator m_MyActionCoroutine = null;
    private bool m_IsAlreadyCollided = false;

    #region Target Methods
    /**
     * <summary>Changes game objects color linked to the target</summary> 
     * <remarks>Swith to red and green</remarks>
     */
    private void ChangeColorOfGameObjectsLinked()
    {
        foreach (GameObject gameObject in this.m_GamesObjectsLinked)
        {
            Tools.SetColor(gameObject.GetComponentInChildren<MeshRenderer>(), new Color(0, 255, 0));
        }

        this.m_MyActionCoroutine = Tools.MyActionTimedCoroutine(this.m_DurationGoActivate, null, null, this.LambdaResetDefaultGameObjectLinkedColor);
        StartCoroutine(this.m_MyActionCoroutine);
    }

    /**
     * <summary>>Lambda function to set the default game object linked color</summary>
     */
    private void LambdaResetDefaultGameObjectLinkedColor()
    {
        for (int i = 0; i < this.m_GamesObjectsLinked.Length; i++)
        {
            Tools.SetColor(this.m_GamesObjectsLinked[i].GetComponentInChildren<MeshRenderer>(), new Color(255, 0, 0));
        }
    }
    #endregion

    #region MonoBehaviour methods
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if (!this.m_IsAlreadyCollided)
            {
                EventManager.Instance.Raise(new TargetHasCollidedEnterEvent { eTargetGO = this.gameObject, eCollidedGO = collision.gameObject });
                this.m_IsAlreadyCollided = true;
            }
            EventManager.Instance.Raise(new PlaySFXEvent() { eAudioClip = this.m_TargetCollisionSfxClip });
            this.ChangeColorOfGameObjectsLinked();
        }
    }

    private void OnDestroy()
    {
        if (this.m_MyActionCoroutine != null) StopCoroutine(this.m_MyActionCoroutine);
    }
    #endregion
}
