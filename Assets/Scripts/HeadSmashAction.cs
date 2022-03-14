using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadSmashAction : Action
{
    public int power;

    public override bool Effect(Unit user, Unit target)
    {
        int dmg = user.attack + power - target.defense;
        bool targetDead = target.TakeDamage(dmg);
        user.TakeDamage(dmg / 8);

        return targetDead;
    }
}
