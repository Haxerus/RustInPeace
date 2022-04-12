using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : MonoBehaviour
{
    public new string name;
    public int duration;
    public BattleActor target { get; set; }
    public Sprite icon;

    protected List<Stat> statChanges = new List<Stat>();
    public abstract void Effect();

    public List<Stat> GetStatChanges()
    {
        return statChanges;
    }
}
