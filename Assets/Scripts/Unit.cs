using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;

    public int attack;
    public int defense;

    public int maxHP;
    public int currentHP;

    public List<Action> actions;

    public bool TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
            return true;

        return false;
    }
}
