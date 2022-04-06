using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActor : MonoBehaviour
{
    public string displayName { get; set; }

    // Stats
    public int maxHP { get; set; }
    public int currentHP { get; set; }

    public int attack { get; set; }
    public int defense { get; set; }
    public int speed { get; set; }

    public List<Action> actions { get; set; }

    public bool TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
            return true;

        return false;
    }
}
