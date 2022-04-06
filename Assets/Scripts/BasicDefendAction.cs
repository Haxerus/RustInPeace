using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDefendAction : Action
{
    public int defense;
    int uses = 0;

    public override void Effect(BattleActor user, BattleActor target)
    {
        if (uses > 6)
            return;

        user.ModifyDefense(defense);
        uses++;
    }
}
