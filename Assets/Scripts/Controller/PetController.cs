using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetController : CharController
{
    [Header("Pet properties")]
    [SerializeField] private float m_DistanceBetweenOwnerRange = 3f;
    private Transform m_PlayerTransform;
    //The pet animator
    private Animator m_PetAnimator;

    #region CharController methods
    protected override void Move()
    {
        if (Vector3.Distance(this.m_PlayerTransform.position, base.Rigidbody.position) > this.m_DistanceBetweenOwnerRange)
        {
            Vector3 playerTargetPosition = new Vector3(this.m_PlayerTransform.position.x, this.m_PlayerTransform.position.y, this.m_PlayerTransform.position.z);
            Vector3 petNewPosition = Vector3.MoveTowards(base.Rigidbody.position, playerTargetPosition, base.TranslationSpeed * Time.fixedDeltaTime);
            base.Rigidbody.MovePosition(petNewPosition);
        }
    }

    protected override void RotateObject()
    {
        this.Rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, this.m_PlayerTransform.rotation, this.RotatingSpeed * Time.deltaTime));
    }
    #endregion

    #region PetController Methods

    /**
     * <summary>Control the pet distance</summary>
     * <remarks>If the pet is twice more faraway than the player pos so we TP the pet to the player</remarks>
     */
    private void ControlPetDistance()
    {
        if(Vector3.Distance(this.m_PlayerTransform.position, base.Rigidbody.position) > this.m_DistanceBetweenOwnerRange * 2){
            Vector3 playerTargetPosition = new Vector3(this.m_PlayerTransform.position.x, this.m_PlayerTransform.position.y, this.m_PlayerTransform.position.z);
            base.transform.position = playerTargetPosition;
        }
    }
    #endregion

    #region MonoBehaviour Methods
    protected override void Awake()
    {
        base.Awake();
        this.m_PlayerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        this.m_PetAnimator = base.GetComponent<Animator>();
    }

    private void Start()
    {
        this.m_PetAnimator.SetBool("Walk", true);
    }

    private void FixedUpdate()
    {
        this.ControlPetDistance();
        this.Move();
        this.RotateObject();
    }
    #endregion
}
