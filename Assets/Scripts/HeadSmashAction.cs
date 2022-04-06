using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadSmashAction : Action
{
    public int power;

    public override void Effect(BattleActor user, BattleActor target)
    {
        int dmg = user.attack + power - target.defense;
        target.TakeDamage(dmg);
        user.TakeDamage(dmg / 8);
    }
}
