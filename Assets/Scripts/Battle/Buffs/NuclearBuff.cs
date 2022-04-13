using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuclearBuff : Buff
{
    public override void Effect()
    {
        statChanges.Clear();
        statChanges.Add(new Stat { name = "attack", value = 100 });
        statChanges.Add(new Stat { name = "defense", value = -100 });
        statChanges.Add(new Stat { name = "speed", value = 50 });
    }
}
