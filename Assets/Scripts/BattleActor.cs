using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActor : MonoBehaviour
{
    public string displayName;

    // Stats
    public int maxHP;
    public int currentHP;

    public int attack;
    public int defense;
    public int speed;
    
    public List<Action> actions;

    public bool TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
            return true;

        return false;
    }
}
