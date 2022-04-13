using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpeed : Action
{
    public GameObject speedBuff;

    public override void Effect(BattleActor user, BattleActor target)
    {
        user.AddBuff(speedBuff, true);
    }
}
