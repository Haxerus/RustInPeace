using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    public new string name;
    public ActionType type;
    public BattleActor user { get; set; }
    public BattleActor target { get; set; }
    public enum ActionType { ATTACK, DEFENSE, SPEED, SPECIAL }

    public abstract void Effect();
}
