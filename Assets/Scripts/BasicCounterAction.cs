using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCounterAction : Action
{
    public int power;

    public override void Effect(BattleActor user, BattleActor target)
    {
        /*float spdMod = 1.0f + Mathf.Log10(1.0f + (user.GetModifiedSpeed() / target.GetModifiedSpeed()));
        float halfDef =  target.GetModifiedDefense() >= 0 ? target.GetModifiedDefense() / 2.0f  : target.GetModifiedDefense() * 1.5f;
        float dmgReduction = 1.0f - (halfDef / 100.0f + Mathf.Abs(halfDef));
        int damage = Mathf.FloorToInt((user.GetModifiedAttack() + power) * spdMod * dmgReduction);*/
        
        target.TakeDamage(1);
    }


}
