using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Tool Class", menuName = "Item/Consumable")]
public class ConsumableClass : ItemClass
{

    [Header("Consumable")]
    public float healthAdded;

    public override ItemClass GetItem() { return this; }
    public override ConsumableClass GetConsumable() { return this; }
    public override ToolClass GetTool() { return null; }
}
