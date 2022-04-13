using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertSpeed : Action
{
    public GameObject critical;
    public override void Effect(BattleActor user, BattleActor target)
    {
        user.AddBuff(critical, true);
    }
}
