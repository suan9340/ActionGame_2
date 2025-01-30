using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_A : EnemyBase
{
    [Tooltip("공격범위"), SerializeField] protected BoxCollider attackCollider;

    public override void AttackStart()
    {
        base.AttackStart();

        if (attackCollider != null)
            attackCollider.enabled = true;
    }

    public override void AttackEnd()
    {
        base.AttackEnd();

        if (attackCollider != null)
            attackCollider.enabled = false;
    }
}
