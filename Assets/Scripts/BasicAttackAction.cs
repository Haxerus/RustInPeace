using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackAction : Action
{
    public int power;

    public override void Effect(BattleActor user, BattleActor target)
    {
        /*float dmgReduction = 1.0f - (target.GetModifiedDefense() / 100.0f + Mathf.Abs(target.GetModifiedDefense())); 
        int damage = Mathf.FloorToInt((user.GetModifiedAttack() + power) * dmgReduction);

        Debug.Log(damage);*/
        
        target.TakeDamage(1);
    }
}
