using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMelee : WeaponBase
{
    [SerializeField] private ParticleSystem particleObj = null;
    private BoxCollider meleeArea;

    private void Start()
    {
        meleeArea = GetComponent<BoxCollider>();
        meleeArea.enabled = false;
        particleObj.gameObject.SetActive(false);
    }

    public override void AttackStart(bool _isActive)
    {
        meleeArea.enabled = _isActive;
    }

    public override void EffectStart()
    {
        particleObj.gameObject.SetActive(true);
        particleObj.Play();
    }
}
