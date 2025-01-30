using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_B : EnemyBase
{
    [Tooltip("공격범위"), SerializeField] protected BoxCollider attackCollider;

    public override void AttackStart()
    {
        myRigid.AddForce(transform.forward * 20, ForceMode.Impulse);
        attackCollider.enabled = true;
    }

    public override void AttackEnd()
    {
        base.AttackEnd();

        myRigid.velocity = Vector3.zero;
        attackCollider.enabled = false;
    }
}
