using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    public string actionName;

    public abstract bool Effect(BattleActor user, BattleActor target);
}
