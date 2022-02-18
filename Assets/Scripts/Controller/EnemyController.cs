using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.Tweening;

public class EnemyController : CharController
{
    [SerializeField] private Tools.MoveDirection m_MoveDirection;
    [SerializeField] private bool m_IsMove;

    private Vector3 m_TranslationDirection;

    private void SetTranslationDirection(Tools.MoveDirection moveDirection)
    {
        switch (moveDirection)
        {
            case Tools.MoveDirection.LEFT:
                this.m_TranslationDirection = Vector3.left;
                break;
            case Tools.MoveDirection.RIGHT:
                this.m_TranslationDirection = Vector3.right;
                break;
            case Tools.MoveDirection.UP:
                this.m_TranslationDirection = Vector3.up;
                break;
            case Tools.MoveDirection.DOWN:
                this.m_TranslationDirection = Vector3.down;
                break;
            case Tools.MoveDirection.FORWARD:
                this.m_TranslationDirection = Vector3.forward;
                break;
            case Tools.MoveDirection.BACK:
                this.m_TranslationDirection = Vector3.back;
                break;
            case Tools.MoveDirection.NONE:
                this.m_TranslationDirection = Vector3.zero;
                break;
        }
    }

    private void Awake()
    {
        this.SetTranslationDirection(m_MoveDirection);
    }

    private void OnEnable()
    {
        if (m_IsMove) StartCoroutine(Tools.MyTranslateCoroutine(base.transform, base.transform.position, new Vector3(0, 0, 0), 200, EasingFunctions.Linear));
    }

    private void OnDisable()
    {
        StopCoroutine(Tools.MyTranslateCoroutine(base.transform, base.transform.position, new Vector3(0, 0, 0), 200, EasingFunctions.Linear));
    }
}