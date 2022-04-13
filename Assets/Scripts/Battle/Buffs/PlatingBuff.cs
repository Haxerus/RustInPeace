using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatingBuff : Buff
{
    public override void Effect()
    {
        statChanges.Clear();
        statChanges.Add(new Stat { name = "defense", value = 500 });
    }
}
