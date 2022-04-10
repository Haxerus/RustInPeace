using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackAction : Action
{
    public int power;

    public override void Effect(BattleActor user, BattleActor target)
    {
        float dmgReduction = 25.0f / (25.0f + target.GetModifiedDefense());
        int damage = Mathf.FloorToInt((user.GetModifiedAttack() + power) * dmgReduction);

        target.TakeDamage(damage);
    }
}
