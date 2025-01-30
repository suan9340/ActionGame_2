using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimAction : MonoBehaviour
{
    private Player playerBase = null;

    private void Awake()
    {
        playerBase = GetComponentInParent<Player>();
    }

    public void JumpEnd()
    {
        if (playerBase != null)
            playerBase.JumpOut();
    }

    public void DodgeEnd()
    {
        if (playerBase != null)
            playerBase.DodgeOut();
    }

    public void AttackStart()
    {
        if (playerBase != null)
            playerBase.AttackStart();
    }

    public void AttackEnd()
    {
        if (playerBase != null)
            playerBase.AttackEnd();
    }

    public void AttackEffect()
    {
        if (playerBase != null)
            playerBase.AttackEffectStart();
    }

}
