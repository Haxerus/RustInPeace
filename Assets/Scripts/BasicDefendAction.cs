using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDefendAction : Action
{
    public int defense;

    public override void Effect()
    {
        user.ModifyDefense(defense);
    }
}
