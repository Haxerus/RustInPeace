using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderFangAction : Action
{
    public int power;

    public override bool Effect(Unit user, Unit target)
    {
        return target.TakeDamage(user.attack + power - target.defense);
    }
}
