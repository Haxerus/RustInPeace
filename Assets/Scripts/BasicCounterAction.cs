using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCounterAction : Action
{
    public int power;

    public override void Effect(BattleActor user, BattleActor target)
    {
        float dmgReduction = 25.0f / (25.0f + target.GetModifiedDefense());
        int damage = Mathf.FloorToInt((user.GetModifiedAttack() + power) * 1.5f * dmgReduction);

        Debug.Log("Counterattack damage: " + damage);

        target.TakeDamage(damage);
    }


}
