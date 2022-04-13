using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameDefend : Action
{
    public GameObject defend;
    public GameObject burn;

    public override void Effect(BattleActor user, BattleActor target)
    {
        bool burned = Random.Range(0.0f, 1.0f) <= 0.1f;

        user.AddBuff(defend, true);

        int damage = Mathf.FloorToInt(target.maxHP / 16f);
        target.TakeDamage(damage);

        if (burned)
            target.AddBuff(burn, false);
    }
}
