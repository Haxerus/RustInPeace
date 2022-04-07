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

    // Temporary stat modifiers, cleared after each turn
    int attMod = 0;
    int defMod = 0;
    int spdMod = 0;

    public List<Action> actions;

    bool dead;

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
            dead = true;
    }

    public void ModifyAttack(int mod)
    {
        attMod = mod;
    }

    public void ModifyDefense(int mod)
    {
        defMod = mod;
    }

    public void ModifySpeed(int mod)
    {
        spdMod = mod;
    }

    public int GetModifiedAttack()
    {
        return attack + attMod;
    }

    public int GetModifiedDefense()
    {
        return defense + defMod;
    }

    public int GetModifiedSpeed()
    {
        return speed + spdMod;
    }
    public void ClearStatModifiers()
    {
        attMod = 0;
        defMod = 0;
        spdMod = 0;
    }

    public bool IsDead()
    {
        return dead;
    }
}
