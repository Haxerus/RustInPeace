using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnBuff : Buff
{
    public override void Effect()
    {
        int damage = Mathf.FloorToInt(target.maxHP / 16f);
        target.TakeDamage(damage);
    }
}
