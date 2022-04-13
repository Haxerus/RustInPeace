using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverdriveBuff : Buff
{
    public override void Effect()
    {
        statChanges.Clear();
        statChanges.Add(new Stat { name = "speed", value = 100 });
        statChanges.Add(new Stat { name = "crit", value = 20 });
    }
}
