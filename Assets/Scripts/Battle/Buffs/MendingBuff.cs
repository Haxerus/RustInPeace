using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MendingBuff : Buff
{
    public override void Effect()
    {
        int healing = Mathf.FloorToInt(target.maxHP / 16f);
        target.currentHP += healing;

        if (target.currentHP > target.maxHP)
        {
            target.currentHP = target.maxHP;
        }
    }
}
