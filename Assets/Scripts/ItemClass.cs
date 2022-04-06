using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemClass : ScriptableObject
{
    [Header("Item")]
    public string Name;
    public Sprite Sprite;
    public bool isStackable = true;
    public int Price;

    public abstract ConsumableClass GetConsumable();
    public abstract ItemClass GetItem();
    public abstract ToolClass GetTool();
}
