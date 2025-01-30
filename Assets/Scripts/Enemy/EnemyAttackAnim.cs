using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackAnim : MonoBehaviour
{
    // 적이 공격할 때 애니메이션에 Event를 통한 호출함수
    private EnemyBase enemyBase = null;

    private void Awake()
    {
        enemyBase = GetComponentInParent<EnemyBase>();
    }
    public void AttackStart()
    {
        if (enemyBase == null) return;
        enemyBase.AttackStart();
    }

    public void AttackEnd()
    {
        if (enemyBase == null) return;
        enemyBase.AttackEnd();
    }
}
