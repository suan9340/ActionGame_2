using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRange : WeaponBase
{
    [SerializeField] private int maxAmmo;
    [SerializeField] private int curAmmo;

    private Coroutine shotCor = null;

    public Transform bulletPos;
    public GameObject bullet;
    public Transform bulletCasePos;
    public GameObject bulletCase;

    public override int GetMaxAmo()
    {
        return maxAmmo;
    }

    public override void SetCurAmo(int _value)
    {
        curAmmo = _value;
    }

    public override void AttackStart(bool _isActive)
    {
        if (_isActive)
        {
            if (curAmmo > 0)
            {
                if (shotCor != null)
                {
                    StopCoroutine(shotCor);
                    shotCor = null;
                }
                curAmmo--;
                shotCor = StartCoroutine(Shot());
            }
        }
    }

    IEnumerator Shot()
    {
        //#1. ÃÑ¾Ë ¹ß»ç
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50; //ÃÑ¾Ë ¼Óµµ

        yield return null;

        //#2. ÅºÇÇ ¹èÃâ
        GameObject intantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = intantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(2, 3) + Vector3.up;
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
    }
}
