using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Follow an character (in this case only player)
/// </summary>
public class FollowCharacterController : CharController
{
    [Header("Follow Controller Controller properties")]
    [Tooltip("Distance to follow")]
    [SerializeField] private float m_DistanceBetweenTargetRange = 0f;

    #region FollowCharacterController properties
    /// <summary>
    /// The distance between the target to reach
    /// </summary>
    protected float DistanceBetweenTargetRange { get => this.m_DistanceBetweenTargetRange; }

    /// <summary>
    /// The follow character animator
    /// </summary>
    protected Animator FollowCharacterAnimator { get; private set; }

    /// <summary>
    /// The target to follow transform
    /// </summary>
    protected Transform TargetToFollowTransform { get; private set; }
    #endregion

    #region CharController methods
    /// <summary>
    /// Move until this object have reached the distance
    /// </summary>
    protected override void Move()
    {
        Vector3 playerTargetPosition = new Vector3(this.TargetToFollowTransform.position.x, this.TargetToFollowTransform.position.y, this.TargetToFollowTransform.position.z);
        Vector3 followCharacterNewPosition = Vector3.MoveTowards(base.Rigidbody.position, playerTargetPosition, base.TranslationSpeed * Time.fixedDeltaTime);
        base.Rigidbody.MovePosition(followCharacterNewPosition);
    }

    protected virtual void SecureMove()
    {
        if (Vector3.Distance(this.TargetToFollowTransform.position, base.Rigidbody.position) > this.DistanceBetweenTargetRange)
        {
            this.Move();
        }
    }

    /// <summary>
    /// Rotate the current object
    /// </summary>
    protected override void RotateObject()
    {
        /*-------OLD VERSION-------*/
        //this.Rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, this.TargetToFollowTransform.rotation, this.RotatingSpeed * Time.deltaTime));
        //this.Rigidbody.rotation = Quaternion.RotateTowards(this.Rigidbody.rotation, this.TargetToFollowTransform.rotation, 360);

        /*------NEW VERSION-------*/
        //Target the current position to the target transform position
        Vector3 targetDirection = Vector3.Normalize(this.TargetToFollowTransform.position - base.Rigidbody.position);
        //Look at that direction
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        //Rotate toward this rotation
        base.Rigidbody.MoveRotation(Quaternion.RotateTowards(base.Rigidbody.rotation, targetRotation, Time.fixedDeltaTime * this.RotatingSpeed));

    }
    #endregion

    #region FollowCharacterController Methods

    /**
     * <summary>Control the follow character distance</summary>
     * <remarks>If the pet is twice more faraway than the player pos so we TP the pet to the player</remarks>
     */
    protected virtual void ControlFollowCharacterDistance()
    {
        if (Vector3.Distance(this.TargetToFollowTransform.position, base.Rigidbody.position) > this.DistanceBetweenTargetRange * 2)
        {
            Vector3 playerTargetPosition = new Vector3(this.TargetToFollowTransform.position.x, this.TargetToFollowTransform.position.y, this.TargetToFollowTransform.position.z);
            base.transform.position = playerTargetPosition;
        }
    }
    #endregion

    #region FollowCharacterController methods
    /// <summary>
    /// On awake we called parent awake, and init variables
    /// </summary>
    protected virtual void OnAwake()
    {
        base.Awake();
        this.FollowCharacterAnimator = this.GetComponent<Animator>();
        this.TargetToFollowTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    /// <summary>
    /// Start walk the character with trigger
    /// </summary>
    protected virtual void StartWalk()
    {
        this.FollowCharacterAnimator.SetBool("Walk", true);
    }
    #endregion

    #region MonoBehaviour methods
    protected virtual void Start()
    {
        this.StartWalk();
    }

    protected override void Awake()
    {
        this.OnAwake();
    }

    protected virtual void FixedUpdate()
    {
        this.ControlFollowCharacterDistance();
        this.Move();
        this.RotateObject();
    }
    #endregion
}
