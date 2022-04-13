using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameSpeed : Action
{
    public GameObject speedBuff;
    public override void Effect(BattleActor user, BattleActor target)
    {
        user.AddBuff(speedBuff, true);

        int damage = Mathf.FloorToInt(target.maxHP / 16f);
        target.TakeDamage(damage);
    }
}
