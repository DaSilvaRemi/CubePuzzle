using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.Tweening;
using SDD.Events;

public class LinearGOController : CharController, IEventHandler
{
    [Header("Enemy Setup")]
    [Tooltip("The final position of object")]
    [SerializeField] private Transform m_TransformEnd;
    [Tooltip("If the object move or not ?")]
    [SerializeField] private bool m_IsMove;
    [Tooltip("If the object move in cycle ?")]
    [SerializeField] private bool m_IsCycle;

    private Transform m_StartTransform; 
    private IEnumerator m_MyTranslateCoroutine = null;
    private IEnumerator m_MyTranslateForwardCoroutine = null;
    private IEnumerator m_MyTranslateBackwardCoroutine = null;

    protected bool IsMove { get => m_IsMove; set => this.m_IsMove = value; }

    #region Events handlers
    private void OnButtonActivateGOClickedEvent(ButtonActivateGOClickedEvent e)
    {
        if(e.eGameObject != null && e.eGameObject.Equals(this.gameObject))
        {
            this.IsMove = true;
            this.Move();
        }
    }
    #endregion

    #region CharController methods
    protected override void Move()
    {
        if (this.IsMove) StartCoroutine(this.m_MyTranslateCoroutine);
    }
    #endregion

    #region LinearGOController Methods
    private void UpdateLinearGOController()
    {
        if (!this.m_IsCycle)
        {
            return;
        }
    }
    #endregion

    #region Events suscribtions
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<ButtonActivateGOClickedEvent>(OnButtonActivateGOClickedEvent);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<ButtonActivateGOClickedEvent>(OnButtonActivateGOClickedEvent);
    }
    #endregion

    #region MonoBehaviour methods
    protected override void Awake()
    {
        base.Awake();
        this.m_MyTranslateCoroutine = Tools.MyTranslateCoroutine(base.transform, base.transform.position, this.m_TransformEnd.position, 200, EasingFunctions.Linear, TranslationSpeed);
        this.SubscribeEvents();
    }

    protected virtual void OnEnable()
    {
        this.Move();
    }

    private void FixedUpdate()
    {
        
    }

    protected virtual void OnDisable()
    {
        if (this.m_MyTranslateCoroutine != null) StopCoroutine(this.m_MyTranslateCoroutine);
    }

    private void OnDestroy()
    {
        this.UnsubscribeEvents();
    }
    #endregion
}