using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] protected DefineManager.WeaponType type;
    [SerializeField] protected int damage;
    [SerializeField] protected float rate;



    public DefineManager.WeaponType GetWeaponType => type;
    public int GetDamage => damage;
    public float GetRate => rate;

    public virtual void AttackStart(bool _isActive)
    { }

    public void Use()
    { }

    public virtual int GetMaxAmo()
    {
        return 0;
    }

    public virtual void EffectStart()
    {

    }

    public virtual void SetCurAmo(int _value)
    {

    }
}
