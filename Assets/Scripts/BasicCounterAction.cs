using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCounterAction : Action
{
    public int power;

    public override void Effect()
    {
        float dmgReduction = 25.0f / (25.0f + target.GetModifiedDefense());
        int damage = Mathf.FloorToInt((user.GetModifiedAttack() + power) * 1.5f * dmgReduction);

        target.TakeDamage(damage);
    }


}
