using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechSpeed : Action
{
    public GameObject overdrive;
    public override void Effect(BattleActor user, BattleActor target)
    {
        user.AddBuff(overdrive, true);
    }
}
