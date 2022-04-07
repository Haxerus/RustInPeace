using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    [Header("Item")]
    public new string name;
    public Sprite sprite;

    public int price;
}
