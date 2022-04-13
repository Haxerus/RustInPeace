using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertDefend : Action
{
    public GameObject defend;
    public GameObject plating;

    public override void Effect(BattleActor user, BattleActor target)
    {
        user.AddBuff(defend, true);
        user.AddBuff(plating, true);
    }
}
