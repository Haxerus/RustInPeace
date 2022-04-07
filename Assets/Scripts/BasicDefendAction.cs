using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDefendAction : Action
{
    public int defense;

    public override void Effect(BattleActor user, BattleActor target)
    {
        user.ModifyDefense(defense);
    }
}
