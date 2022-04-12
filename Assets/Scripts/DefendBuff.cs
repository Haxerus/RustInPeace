using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendBuff : Buff
{
    public override void Effect()
    {
        statChanges.Clear();
        statChanges.Add(new Stat { name = "dmgReduction", value = 50 });
    }
}
