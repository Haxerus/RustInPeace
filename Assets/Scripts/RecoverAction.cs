using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RecoverAction : Action
{
    public int healing;

    public override void Effect(BattleActor user, BattleActor target)
    {
        user.currentHP = Math.Min(user.currentHP + healing, user.maxHP);

        if (user.currentHP > user.maxHP)
            user.currentHP = user.maxHP;
    }
}
