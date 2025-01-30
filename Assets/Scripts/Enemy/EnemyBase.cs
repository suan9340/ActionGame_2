using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected DefineManager.EnemyType enemyType;

    [Space(10)]
    [Tooltip("�� HP"), SerializeField] protected float maxHealth;
    [Tooltip("�� �̵��ӵ�"), SerializeField] protected float speed = 10f;
    [Tooltip("���� ������"), SerializeField] protected float attackDamage = 10f;
    [SerializeField] private Vector2 randAttackTime = Vector2.one;

    [Space(10)]
    [Tooltip("���� ������ ũ��"), SerializeField] protected float targetRadius = 0;
    [Tooltip("���������� �Ÿ�"), SerializeField] protected float targetRange = 0;

    protected bool isChase;
    protected bool isAttack;

    protected float curHealth;
    protected Player followTarget;

    protected Rigidbody myRigid;
    protected Material mat;
    protected NavMeshAgent myNavMesh;
    protected Animator myAnim;

    protected bool isDamage = false;
    public float GetDamageValue => attackDamage;

    protected Coroutine damageCor = null;

    private float attackEndTime = 0f;
    private bool isAttackEnd = false;
    private float randAttackWaitTime = 0f;

    public void Initialize()
    {
        Cashing();

        myNavMesh.speed = speed;
        curHealth = maxHealth;
        randAttackWaitTime = Random.Range(randAttackTime.x, randAttackTime.y);
    }

    protected virtual void Cashing()
    {
        myRigid = GetComponent<Rigidbody>();
        myNavMesh = GetComponent<NavMeshAgent>();

        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        myAnim = GetComponentInChildren<Animator>();

        followTarget = GameManager.Instance.GetPlayer;

        //Invoke(nameof(ChaseStart), 2);
    }

    private void ChaseStart()
    {
        isChase = true;
        myAnim.SetBool("isWalk", true);
    }

    private void Update()
    {
        if (followTarget.GetDie)
        {
            myAnim.SetBool("isWalk", false);
            myAnim.SetBool("isAttack", false);
            isChase = false;
            isAttack = false;
            return;
        }

        if (isAttackEnd)
        {
            myAnim.SetBool("isWalk", false);
            myAnim.SetBool("isAttack", false);
            isChase = false;

            attackEndTime += Time.deltaTime;
            if (attackEndTime > randAttackWaitTime)
            {
                isAttackEnd = false;
            }
            return;
        }
        if (myNavMesh.enabled)
        {
            myNavMesh.SetDestination(followTarget.transform.position);
            if (myNavMesh.isStopped)
            {
                isChase = false;
                myAnim.SetBool("isWalk", false);
            }
            else
            {
                isChase = true;
                myAnim.SetBool("isWalk", true);
            }
        }
        TargetingPlayer();
    }

    void FixedUpdate()
    {
        FreezeVelocity();
    }


    void FreezeVelocity()
    {
        if (isChase)
        {
            myRigid.velocity = Vector3.zero;
            myRigid.angularVelocity = Vector3.zero;
        }
    }

    private void TargetingPlayer()
    {
        if (isAttack) return;   // �̹� �����ϰ� ���� �� Ȯ�� X

        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position, targetRadius,
            transform.forward, targetRange, LayerMask.GetMask("Player"));

        // �ش� �����ɽ�Ʈ�� �÷��̾ �����Ǹ�
        if (rayHits.Length > 0)
        {   // ������ ����
            isChase = false;
            isAttack = true;
            myAnim.SetBool("isAttack", true);
        }
        else
        {

        }
    }

    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, targetRadius); // Draw the sphere at the cast origin

        Gizmos.color = Color.red;
        Vector3 castEnd = transform.position + transform.forward * targetRange;
        Gizmos.DrawLine(transform.position, castEnd); // Draw a line representing the raycast path

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(castEnd, targetRadius); // Draw the sphere at the end position
#endif
    }

    // ������ ���۵� ���� Ÿ�̹�
    public virtual void AttackStart()
    {
        myNavMesh.speed = 0;
    }

    // ������ ���� ���� Ÿ�̹�
    public virtual void AttackEnd()
    {
        isAttackEnd = true;
        isChase = true;
        isAttack = false;
        myAnim.SetBool("isAttack", false);
        myNavMesh.speed = speed;

        randAttackWaitTime = Random.Range(randAttackTime.x, randAttackTime.y);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstantManager.TAG_WEAPON))      // ��������� �浹 ��
        {
            if (other.GetComponent<WeaponBase>() == null) return;

            WeaponBase weapon = other.GetComponent<WeaponBase>();
            Vector3 reactVec = transform.position - other.transform.position;

            Damage(weapon.GetDamage, reactVec, false);
        }

        if (other.CompareTag(ConstantManager.TAG_BULLET))     // �Ѿ˰� �浹 ��
        {
            if (other.GetComponent<Bullet>() == null) return;
            if (isDamage) return;   // �ݺ� �浹 ���� �ڵ�
            isDamage = true;

            Bullet bullet = other.GetComponent<Bullet>();
            Destroy(other.gameObject);
            Vector3 reactVec = transform.position - other.transform.position;

            Damage(bullet.GetDamage, reactVec, false);
        }
    }

    public void HitByGrenade(Vector3 explosionpos)
    {
        curHealth -= 100;
        Vector3 reactVec = transform.position - explosionpos;
        StartCoroutine(OnDamage(reactVec, true));
    }
    private IEnumerator OnDamage(Vector3 reactVec, bool isGrenade)
    {
        mat.color = Color.red;
        transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.1f);
        yield return new WaitForSeconds(0.1f);

        transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f);
        isDamage = false;

        if (curHealth > 0)  // ���� ������ �ƴ�
        {
            mat.color = Color.white;
            yield break;
        }

        // �̰� �׾��� ��
        mat.color = Color.gray;
        isChase = false;
        myNavMesh.enabled = false;
        transform.DOScale(new Vector3(0f, 0f, 0f), 0.5f).SetEase(Ease.OutSine);

        if (isGrenade)
        {
            reactVec = reactVec.normalized;
            reactVec += Vector3.up * 3;

            myRigid.freezeRotation = false;
            myRigid.AddForce(reactVec * 5, ForceMode.Impulse);
            myRigid.AddTorque(reactVec * 15, ForceMode.Impulse);
        }
        else
        {
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            myRigid.AddForce(reactVec * 5, ForceMode.Impulse);
        }
        Destroy(gameObject, 1);
    }

    private void Damage(float _value, Vector3 _reactVec, bool _isGrenade)
    {
        curHealth -= _value;

        if (damageCor != null)
        {
            StopCoroutine(damageCor);
            damageCor = null;
        }

        StartCoroutine(OnDamage(_reactVec, _isGrenade));

    }
}