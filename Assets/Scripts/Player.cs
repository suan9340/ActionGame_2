using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponClass
{
    public WeaponBase weaponObj = null;
    public bool isHasWeapon = false;
}
public class Player : MonoBehaviour
{
    [SerializeField] private float defaultSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private int maxAttackCount = 0;
    [SerializeField] private float attackCheckDelayTime = 0.5f;

    [SerializeField] private List<WeaponClass> weapons = new List<WeaponClass>();
    public GameObject[] grenades;
    public int hasGrenades;
    public GameObject GrenadeObj;
    public Camera followCamera;

    public int ammo;
    public int coin;

    public int maxAmmo;
    public int maxCoin;
    public int maxhasGrenades;

    private float maxHp;        // 최대 HP
    private float currentHp;    // 현재 HP

    private float maxMp;        // 최대 MP
    private float currentMp;    // 현재 Mp

    private float currentSpeed;     // 현재 속도

    private float hAxis;
    private float vAxis;

    private bool isWalkIP;
    private bool isJumpIP;
    private bool isAttackIP;
    private bool isThrowIP;
    private bool isReloadIP;
    private bool idown;
    private bool isSwap1IP;
    private bool isSwap2IP;
    private bool isSwap3IP;


    private bool isJump;
    private bool isDodge;
    private bool isSwap;
    private bool isReload;
    private bool isFireReady = true;
    private bool isBorder;
    private bool isDamage;


    Vector3 moveVec;
    Vector3 dodgeVec;

    private Rigidbody myRigid;
    private Animator myAnim;
    private MeshRenderer[] meshs;

    GameObject nearobject;
    WeaponBase equipWeapon;
    int equipWeaponIndex = -1;
    float fireDelay;

    private float refiilRemainTime = 0f;
    private float mpRefillTime = 0;
    private float mpRefillValue = 0;

    private float currentAttackTime = 0;
    public int currentAttackCount = 0;
    private int attackAnimTime = 0;

    private bool isDie = false;

    public bool GetDie => isDie;
    void Start()
    {
        Cashing();
        Initialize();
    }

    private void Cashing()
    {
        myRigid = GetComponent<Rigidbody>();
        myAnim = GetComponentInChildren<Animator>();
        meshs = GetComponentsInChildren<MeshRenderer>();
    }
    private void Initialize()
    {
        maxHp = GameConfigManager.Instance.GetPlayerData.maxHp;

        maxMp = GameConfigManager.Instance.GetPlayerData.maxMP;
        mpRefillTime = GameConfigManager.Instance.GetPlayerData.mpTime;
        mpRefillValue = GameConfigManager.Instance.GetPlayerData.mpValue;

        currentSpeed = defaultSpeed;

        currentHp = maxHp;
        currentMp = maxMp;

        SelectWeapon();
    }


    void Update()
    {
        if (isDie || isDamage)
        {
            return;
        }

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.GetRate < fireDelay;

        if (isAttackIP)
        {
            // 공격하고 n초가 안지났을 때
            if (currentAttackTime > 0.1f && currentAttackTime < attackCheckDelayTime)
            {
                if (Input.GetButton("Fire1"))
                {
                    currentAttackTime = 0;
                    currentAttackCount++;
                }
            }
            currentAttackTime += Time.deltaTime;
        }

        Rotaion();
        Move();
        GetInput();

        //Grenade();
        //Attack();
        //Reload();
        //Dodge();
        //Swap();
        //Interation();

        CheckRefillMP();
    }

    void FixedUpdate()
    {
        myRigid.angularVelocity = Vector3.zero;
    }

    private void CheckRefillMP()
    {
        refiilRemainTime += Time.deltaTime;
        if (refiilRemainTime >= mpRefillTime)
        {
            float _value = Mathf.Min(currentMp + mpRefillValue, maxHp);
            currentMp = _value;
            refiilRemainTime = 0;
            UIManager.Instance.GetUIMain.StartPlayerMpCoroutine(currentMp);

        }
    }

    #region Collision
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            myAnim.SetBool("isJump", false);
            isJump = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isDamage) return;

        if (other.CompareTag(ConstantManager.TAG_ITEM))   // 아이템과 충돌 시
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Ammo:
                    {
                        ammo += item.value;
                        if (ammo > maxAmmo)
                            ammo = maxAmmo;
                    }
                    break;
                case Item.Type.Coin:
                    {
                        if (coin > maxCoin)
                            coin = maxCoin;
                    }
                    break;
                case Item.Type.Heart:
                    {
                        if (currentHp > maxHp)
                            currentHp = maxHp;
                    }
                    break;
                case Item.Type.Grenade:
                    {
                        grenades[hasGrenades].SetActive(true);
                        hasGrenades += item.value;
                        if (hasGrenades > maxhasGrenades)
                            hasGrenades = maxhasGrenades;
                    }
                    break;
            }
            Destroy(other.gameObject);
        }

        if (other.CompareTag(ConstantManager.TAG_ENEMY_ATTACK))       // 적의 공격에 맞았을 때
        {
            if (other.GetComponentInParent<EnemyBase>() == null) return;

            var _enemy = other.GetComponentInParent<EnemyBase>();
            OnDamagePlayer(_enemy.GetDamageValue);  // 유저 hp 감소
        }

        if (other.CompareTag(ConstantManager.TAG_ENEMY_BULLET))     // 적 총알에 맞았을 때
        {
            if (other.GetComponent<Bullet>() == null) return;

            var _bullet = other.GetComponent<Bullet>();
            OnDamagePlayer(_bullet.GetDamage);

            Destroy(_bullet.gameObject);
        }

    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            nearobject = other.gameObject;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            nearobject = null;
        }

    }
    #endregion

    private void OnDamagePlayer(float _value)
    {
        isDamage = true;
        if (isAttackIP)
        {
            AttackEnd(true);
        }

        currentHp -= _value; // 유저 hp 감소

        myAnim.SetTrigger("doDamage");
        UIManager.Instance.GetUIMain.OnDamageStart(currentHp);
        if (currentHp <= 0)
        {
            isDie = true;
            myAnim.SetTrigger("doDie");
        }
    }

    public void DamagedEnd()
    {
        isDamage = false;
    }

    void GetInput()
    {
        if (Input.GetButton("Fire1"))
            Attack();
        isThrowIP = Input.GetButtonDown("Fire2");
        isReloadIP = Input.GetButtonDown("Reload");

        idown = Input.GetButtonDown("Interaction");

        isSwap1IP = Input.GetButtonDown("Swap1");
        isSwap2IP = Input.GetButtonDown("Swap2");
        isSwap3IP = Input.GetButtonDown("Swap3");
    }

    void Move()
    {
        isWalkIP = Input.GetButton("Walk");
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        if (hAxis == 0 && vAxis == 0 && isAttackIP == false)
        {
            myAnim.SetBool("isWalk", false);
            myAnim.SetBool("isRun", false);
            return;
        }

        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        // 벽 감지
        isBorder = Physics.Raycast(transform.position, transform.forward,
            5, LayerMask.GetMask("Wall"));

        if (isBorder) return;

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (Input.GetButtonDown("Jump"))
            Dodge();

        if (isDodge)
            moveVec = dodgeVec;
        if (isSwap || isReload || !isFireReady)
            moveVec = Vector3.zero;

        if (isAttackIP) return;

        if (isWalkIP)    // 걷기 키 눌렀을 때
        {
            myRigid.MovePosition(transform.position + moveVec * walkSpeed * Time.deltaTime);
            //transform.position += moveVec * walkSpeed * Time.deltaTime;
        }
        else
        {
            myRigid.MovePosition(transform.position + moveVec * defaultSpeed * Time.deltaTime);
            //transform.position += moveVec * defaultSpeed * Time.deltaTime;
        }

        myAnim.SetBool("isWalk", isWalkIP);
        myAnim.SetBool("isRun", true);
    }
    void Rotaion()
    {
        //#1. 키보드에 의한 회전 값
        transform.LookAt(transform.position + moveVec);

        //#2. 마우스에 의한 회전 값
        if (isAttackIP)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }
    }

    void Attack()
    {
        if (equipWeapon == null)
            return;

        switch (equipWeapon.GetWeaponType)
        {
            case DefineManager.WeaponType.Melee:
                myAnim.applyRootMotion = true;
                myAnim.SetBool("isAttack", true);
                if (currentAttackCount == 0)
                    myAnim.SetTrigger("doAttack");
                break;
            case DefineManager.WeaponType.Range:
                myAnim.SetTrigger("doShot");
                break;
        }
        if (isAttackIP == false)
            currentAttackCount++;
        isAttackIP = true;
        fireDelay = 0;
    }

    public void AttackStart()
    {
        equipWeapon.AttackStart(true);
    }

    public void AttackEnd(bool _isEnd = false)
    {
        equipWeapon.AttackStart(false);
        attackAnimTime += 1;
        if (currentAttackCount - 1 <= 0 || attackAnimTime >= maxAttackCount || _isEnd)
        {
            attackAnimTime = 0;
            currentAttackCount = 0;
            currentAttackTime = 0;
            isAttackIP = false;
            myAnim.SetBool("isAttack", false);
            myAnim.applyRootMotion = false;
        }
        else
        {
            currentAttackCount--;
        }
    }

    public void AttackEffectStart()
    {
        equipWeapon.EffectStart();
    }
    void Reload()
    {
        // 장착중인 무기가 없거나 총알이 필요없는 무기라면
        if (equipWeapon == null || equipWeapon.GetWeaponType == DefineManager.WeaponType.Melee || ammo == 0) return;
        // 이미 재장전중이거나, 점프중이거나, 구르고있거나, 바꾸고있거나, 재장전중일땐 리턴
        if (isReloadIP || isJump || isDodge || isSwap || isReload) return;

        myAnim.SetTrigger("doReload");
        isReload = true;
        Invoke(nameof(ReloadOut), 2f);

    }
    void ReloadOut()
    {
        int reAmmo = ammo < equipWeapon.GetMaxAmo() ? ammo : equipWeapon.GetMaxAmo();
        equipWeapon.SetCurAmo(reAmmo);
        ammo -= reAmmo;
        isReload = false;
    }

    public void JumpOut()
    {
        isJump = false;
    }

    void Grenade()
    {
        if (hasGrenades == 0)
            return;
        if (isThrowIP && !isReload && !isSwap)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 10;

                GameObject instantGrenade = Instantiate(GrenadeObj, transform.position, transform.rotation);
                Rigidbody rigidGrenade = instantGrenade.GetComponent<Rigidbody>();
                rigidGrenade.AddForce(nextVec, ForceMode.Impulse);
                rigidGrenade.AddTorque(Vector3.back * 10, ForceMode.Impulse);

                hasGrenades--;
                grenades[hasGrenades].SetActive(false);
            }
        }
    }
    void Dodge()    // 구르기
    {
        if (isDodge) return;
        if (currentMp < GameConfigManager.Instance.GetPlayerData.mpUseValue) return;

        if (moveVec == Vector3.zero) return;
        if (isAttackIP)
        {
            AttackEnd(true);
        }

        dodgeVec = moveVec;
        currentSpeed *= 1.5f;
        myAnim.SetTrigger("doDodge");
        isDodge = true;

        currentMp -= GameConfigManager.Instance.GetPlayerData.mpUseValue;
        UIManager.Instance.GetUIMain.StartPlayerMpCoroutine(currentMp);
    }

    public void DodgeOut()
    {
        currentSpeed = defaultSpeed;
        isDodge = false;
    }

    void Swap()
    {
        int weaponIndex = -1;
        if (isSwap1IP) weaponIndex = 0;
        if (isSwap2IP) weaponIndex = 1;
        if (isSwap3IP) weaponIndex = 2;

        if (equipWeaponIndex == weaponIndex) return;
        if (weaponIndex + 1 > weapons.Count) return;
        if (isSwap1IP == false && isSwap2IP == false && isSwap3IP == false) return; // 세개 다 안했으면
        if (isJump || isDodge) return;

        if (equipWeapon != null)
            equipWeapon.gameObject.SetActive(false);

        equipWeaponIndex = weaponIndex;
        equipWeapon = weapons[weaponIndex].weaponObj;
        equipWeapon.gameObject.SetActive(true);

        switch (weaponIndex)
        {
            case 0: // 근접무기
                myAnim.SetTrigger("doSwapKnife");
                break;
            case 1: // 활
                myAnim.SetTrigger("doSwapArrow");
                break;
            case 2:
                break;
        }

        isSwap = true;

        Invoke("SwapOut", 0.4f);

    }
    void SwapOut()
    {
        isSwap = false;
    }

    private void SelectWeapon()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].isHasWeapon)
            {
                equipWeaponIndex = i;
                equipWeapon = weapons[i].weaponObj;
                equipWeapon.gameObject.SetActive(true);
                fireDelay = equipWeapon.GetRate;
            }
            else
                weapons[i].weaponObj.gameObject.SetActive(false);
        }
    }

    void Interation()
    {
        //if (idown && nearobject != null && !isJump && !isDodge)
        //{
        //    if (nearobject.tag == "Weapon")
        //    {
        //        Item item = nearobject.GetComponent<Item>();
        //        int weaponIndex = item.value;
        //        hasWeapons[weaponIndex] = true;
        //        Destroy(nearobject);
        //    }
        //}
    }
    void FreezeRotation()
    {
    }
}
