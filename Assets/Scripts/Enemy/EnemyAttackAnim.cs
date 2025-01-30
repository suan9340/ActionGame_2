using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackAnim : MonoBehaviour
{
    // ���� ������ �� �ִϸ��̼ǿ� Event�� ���� ȣ���Լ�
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
