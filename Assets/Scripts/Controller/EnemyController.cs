using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.Tweening;

public class EnemyController : CharController
{
    [SerializeField] private Vector3 m_TranslationEnd;
    [SerializeField] private bool m_IsMove;

    private IEnumerator m_MyTranslateCoroutine = null;

    private void Awake()
    {
        this.m_MyTranslateCoroutine = Tools.MyTranslateCoroutine(base.transform, base.transform.position, this.m_TranslationEnd, 200, EasingFunctions.Linear);
    }

    private void OnEnable()
    {
        if (this.m_IsMove) StartCoroutine(this.m_MyTranslateCoroutine);
    }

    private void OnDisable()
    {
        if (this.m_MyTranslateCoroutine != null) StopCoroutine(this.m_MyTranslateCoroutine);
        if (this.m_MyTranslateCoroutine != null) StopCoroutine(this.m_MyTranslateCoroutine);
    }
}