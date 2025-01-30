using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject meshObj;
    public GameObject effectObj;
    public Rigidbody rigid;

    void Start()
    {
        StartCoroutine(Explosion());

    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3f);

        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        meshObj.SetActive(false);
        effectObj.SetActive(true);

        // ���� ���� �� ��� Collider �˻�
        Collider[] colliders = Physics.OverlapSphere(transform.position, 15, LayerMask.GetMask("Enemy"));

        foreach (Collider collider in colliders)
        {
            EnemyBase enemy = collider.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                Vector3 reactVec = (enemy.transform.position - transform.position).normalized;
                reactVec += Vector3.up * 5; // �������� �߰� �� ����
                enemy.HitByGrenade(reactVec);

            }
        }

        Destroy(gameObject, 5);
    }
}
