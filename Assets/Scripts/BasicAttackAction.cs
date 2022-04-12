using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackAction : Action
{
    public int power;

    public override void Effect(BattleActor user, BattleActor target)
    {
        int crit = Random.Range(0f, 1f) <= user.GetCritChance() ? 2 : 1;
        float dmgReduction = target.GetStat("dmgReduction") != 0 ? target.GetStat("dmgReduction") / 100.0f : 1f;
        float defense = 100f / (100f + target.GetStat("defense"));
        int damage = Mathf.FloorToInt((user.GetStat("attack") + power) * crit * dmgReduction * defense);

        target.TakeDamage(damage);
    }
}
