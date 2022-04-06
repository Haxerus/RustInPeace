using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType { ATTACK, DEFENSE, COUNTER, SPECIAL }

public abstract class Action : MonoBehaviour
{
    public string actionName;
    public ActionType type;

    public abstract bool Effect(BattleActor user, BattleActor target);
}
