using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage; // �Ѿ��� ������
    public float destroyDelay = 3f; // �ٴڿ� ����� �� ���� ������
    public float lifetime = 5f; // �Ѿ��� �ִ� ���� �ð�

    public float GetDamage => damage;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // �ٴڰ� �浹���� ��
        if (collision.gameObject.CompareTag("Floor"))
        {
            Debug.Log("Bullet hit the floor.");
            Destroy(gameObject, destroyDelay);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // ���� �浹���� ��
        if (other.CompareTag("Wall"))
        {
            Debug.Log("Bullet hit the wall.");
            Destroy(gameObject);
        }
    }
}
