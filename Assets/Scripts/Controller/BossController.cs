using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class BossController : EnemyController
{
    #region CharController methods
    protected override void Move()
    {
        this.ControlFollowCharacterDistance();
        Vector3 playerTargetPosition = new Vector3(base.TargetToFollowTransform.position.x, base.TargetToFollowTransform.position.y, base.TargetToFollowTransform.position.z);
        Vector3 petNewPosition = Vector3.MoveTowards(base.Rigidbody.position, playerTargetPosition, base.TranslationSpeed * Time.fixedDeltaTime);
        base.Rigidbody.MovePosition(petNewPosition);
    }
    #endregion

    #region FollowCharacter Methods
    protected override void ControlFollowCharacterDistance()
    {
        if (Vector3.Distance(base.TargetToFollowTransform.position, base.Rigidbody.position) > base.DistanceBetweenTargetRange * 2)
        {
            EventManager.Instance.Raise(new SpawnEachTimeEvent() { eSpawnTime = 1f });
        }
        else
        {
            EventManager.Instance.Raise(new StopEachTimeSpawnEvent());
        }
    }
    #endregion

    #region MonoBehaviour Methods
    protected override void FixedUpdate()
    {
        base.CheckIsAlive();
        this.Move();
        base.RotateObject();
    }
    #endregion
}
