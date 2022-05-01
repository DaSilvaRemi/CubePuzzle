using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

/// <summary>
/// Control an enemy
/// </summary>
public class EnemyController : FollowCharacterController
{
    [SerializeField] private int m_EnemyInitLifeMax = 3;
    protected int EnemyLife { get; set; }

    protected bool IsAlive
    {
        get
        {
            return this.EnemyLife > 0;
        }
    }

    #region EnemyController methods

    protected bool CheckIsAlive()
    {
        if (!this.IsAlive)
        {
            EventManager.Instance.Raise(new SpawnedGameObjectToDestroyEvent() { eGameObjectToDestroy = this.gameObject });
        }

        return this.IsAlive;
    }

    protected virtual void TakeDamage(GameObject gameObject)
    {
        if (gameObject == null || !this.CheckIsAlive())
        {
            return;
        }

        int totalDamagesTake = 0;
        IDamage[] damages = gameObject.GetComponentsInChildren<IDamage>();

        foreach (IDamage damage in damages)
        {
            totalDamagesTake += damage.DamagePoint;
        }
        this.EnemyLife -= totalDamagesTake;
        this.ChangeColorOnDamage();
    }

    protected virtual void ChangeColorOnDamage()
    {
        if (this.EnemyLife == this.m_EnemyInitLifeMax)
        {
            return;
        }

        if (this.EnemyLife <= this.m_EnemyInitLifeMax * 0.20)
        {
            Tools.SetColor(this.GetComponentInChildren<MeshRenderer>(), Color.yellow);
            Tools.SetColor(this.GetComponentInChildren<SkinnedMeshRenderer>(), Color.yellow);
        }
        else if (this.EnemyLife <= this.m_EnemyInitLifeMax * 0.50)
        {
            Tools.SetColor(this.GetComponentInChildren<MeshRenderer>(), new Color(255, 165, 0));
            Tools.SetColor(this.GetComponentInChildren<SkinnedMeshRenderer>(), new Color(255, 165, 0));
        }
        else {
            Tools.SetColor(this.GetComponentInChildren<MeshRenderer>(), Color.red);
            Tools.SetColor(this.GetComponentInChildren<SkinnedMeshRenderer>(), Color.red);
        }
    }
    #endregion

    #region MonoBehaviour Methods
    protected override void OnAwake()
    {
        base.OnAwake();
        this.EnemyLife = this.m_EnemyInitLifeMax; 
    }

    protected override void Awake()
    {
        this.OnAwake();
    }

    protected override void FixedUpdate()
    {
        this.CheckIsAlive();
        if (this.IsAlive)
        {
            base.Move();
            base.RotateObject();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            EventManager.Instance.Raise(new LevelGameOverEvent());
        }

        if (collision.gameObject.CompareTag("ThrowableObject"))
        {
            this.TakeDamage(collision.gameObject);
        }
    }
    #endregion
}
