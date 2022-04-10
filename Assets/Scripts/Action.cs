using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    public new string name;
    public ActionType type;

    public enum ActionType { ATTACK, DEFENSE, COUNTER, SPECIAL }

    public abstract void Effect(BattleActor user, BattleActor target);
}
