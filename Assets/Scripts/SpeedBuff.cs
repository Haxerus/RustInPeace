using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : Buff
{
    public override void Effect()
    {
        statChanges.Clear();
        statChanges.Add(new Stat { name = "speed", value = 10});
    }
}
