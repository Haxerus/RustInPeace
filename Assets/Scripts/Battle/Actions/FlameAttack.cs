using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameAttack : Action
{
    public float power;
    public GameObject burn;

    public override void Effect(BattleActor user, BattleActor target)
    {
        float critChance = user.GetCritChance();
        bool crit = Random.Range(0.0f, 1.0f) <= critChance;
        float critMulti = 1f;

        if (crit)
            critMulti = 2f;

        bool burned = Random.Range(0.0f, 1.0f) <= 0.3f;

        int armor = target.GetStat("armor");

        float dmgReduction = armor / 100.0f;

        if (armor == 0)
        {
            dmgReduction = 1.0f;
        }

        float defense = 100f / (100f + target.GetStat("defense"));

        int damage = Mathf.FloorToInt((user.GetStat("attack") * power) * critMulti * dmgReduction * defense);

        if (burned)
            target.AddBuff(burn, false);

        target.TakeDamage(damage);
    }
}
