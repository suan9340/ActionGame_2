using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage; // 총알의 데미지
    public float destroyDelay = 3f; // 바닥에 닿았을 때 제거 딜레이
    public float lifetime = 5f; // 총알의 최대 생존 시간

    public float GetDamage => damage;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // 바닥과 충돌했을 때
        if (collision.gameObject.CompareTag("Floor"))
        {
            Debug.Log("Bullet hit the floor.");
            Destroy(gameObject, destroyDelay);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // 벽과 충돌했을 때
        if (other.CompareTag("Wall"))
        {
            Debug.Log("Bullet hit the wall.");
            Destroy(gameObject);
        }
    }
}
