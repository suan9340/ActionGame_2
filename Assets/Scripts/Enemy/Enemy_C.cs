using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_C : EnemyBase
{
    [Space(10), SerializeField] private GameObject bullet; //원거리 몬스터

    public override void AttackStart()
    {
        GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);
        Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
        rigidBullet.velocity = transform.forward * 20;
    }

    public override void AttackEnd()
    {
        base.AttackEnd();
    }
}
