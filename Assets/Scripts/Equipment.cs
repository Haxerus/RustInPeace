using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "new Equipment Class", menuName = "Item/Equipment")]
public class Equipment : Item
{
    [Header("Equipment")]
    public EquipmentSlot slot;

    public Sprite equippedSprite;

    public enum EquipmentSlot
    {
        HEAD,
        BODY,
        LEGS
    }

    public List<Stat> stats;

    public Action action;
}
