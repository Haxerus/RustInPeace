using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Consumable Class", menuName = "Item/Consumable")]
public class Consumable : Item
{

    [Header("Consumable")]
    public List<Stat> stats;
}
