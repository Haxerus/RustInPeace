using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActor : MonoBehaviour
{
    public string displayName;

    // Stats
    public int maxHP { get; set; }
    public int currentHP { get; set; }

    public List<Action> actions;
    public List<Stat> stats;

    List<Buff> buffs = new List<Buff>();

    bool dead;

    void Start()
    {

    }

    public void LoadStats(List<Stat> _stats)
    {
        foreach (Stat s in _stats)
        {
            if (stats.Exists(x => x.name == s.name))
                continue;

            Stat newStat = new Stat { name = s.name, value = s.value };
            stats.Add(newStat);
        }

        maxHP = 1;
        currentHP = 1;

        Stat hp = stats.Find(x => x.name == "health");

        if (hp != null)
        {

            maxHP = hp.value;
            currentHP = hp.value;
        }
    }

    public void UpdateHealth()
    {
        maxHP = 1;
        currentHP = 1;

        Stat hp = stats.Find(x => x.name == "health");

        if (hp != null)
        {
            maxHP = hp.value;
            currentHP = hp.value;
        }
    }

    public int GetStat(string statName)
    {
        Stat stat = stats.Find(x => x.name == statName);
        if (stat == null)
            return 0;

        Stat modStat = new Stat
        {
            name = stat.name,
            value = stat.value
        };
        
        foreach (Buff b in buffs)
        {
            foreach (Stat s in b.GetStats())
            {
                if (modStat.name == s.name)
                {
                    modStat.value += s.value;
                }
            }
        }

        return modStat.value;
    }

    public float GetCritChance()
    {
        float chance = 0f;

        int crit = stats.Find(x => x.name == "crit").value;
        int speed = stats.Find(x => x.name == "speed").value;

        chance += 1.0f - 1.0f / (1.0f + 0.01f * speed);
        chance += crit;

        return chance;
    }

    public void AddBuff(GameObject buffObj, bool immediate)
    {
        if (buffs.Exists(x => x.name == buffObj.GetComponent<Buff>().name))
            return;

        GameObject obj = Instantiate(buffObj, gameObject.transform);
        Buff b = obj.GetComponent<Buff>();

        b.target = this;
        buffs.Add(b);

        if (immediate)
        {
            b.Effect();
        }
    }

    public void ProcessBuffs()
    {
        List<Buff> removed = new List<Buff>();

        foreach (Buff b in buffs)
        {
            b.duration--;

            if (b.duration == 0)
            {
                removed.Add(b);
                continue;
            }

            b.Effect();
        }

        foreach (Buff b in removed)
        {
            Destroy(b.gameObject);
            buffs.Remove(b);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
            dead = true;
    }

    public bool IsDead()
    {
        return dead;
    }
}
