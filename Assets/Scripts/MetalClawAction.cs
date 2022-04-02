using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalClawAction : Action
{
    public int power;

    public override bool Effect(BattleActor user, BattleActor target)
    {
        return target.TakeDamage(user.attack + power - target.defense);
    }
}
