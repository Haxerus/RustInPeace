using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechDefend : Action
{
    public GameObject defenseBuff;
    public GameObject mendingBuff;

    public override void Effect(BattleActor user, BattleActor target)
    {
        user.AddBuff(defenseBuff, true);
        user.AddBuff(mendingBuff, true);
    }
}
