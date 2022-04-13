using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDefend : Action
{
    public GameObject defenseBuff;

    public override void Effect(BattleActor user, BattleActor target)
    {
        user.AddBuff(defenseBuff, true);
    }
}
