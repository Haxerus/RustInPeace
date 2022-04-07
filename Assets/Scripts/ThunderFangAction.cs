using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderFangAction : Action
{
    public int power;

    public override void Effect(BattleActor user, BattleActor target)
    {
        target.TakeDamage(user.attack + power - target.defense);
    }
}
