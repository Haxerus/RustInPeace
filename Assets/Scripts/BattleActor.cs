using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActor : MonoBehaviour
{
    public string displayName;

    // Stats
    public int maxHP { get; set; }
    public int currentHP { get; set; }

    public int level;

    public List<Action> actions;
    public List<Stat> stats;
    

    public List<Buff> buffs { get; set; }
    public List<Stat> statChanges { get; set; }

    bool dead;

    void Start()
    {
        buffs = new List<Buff>();
    }

    public void LoadStats(List<Stat> _stats)
    {
        foreach (Stat s in _stats)
        {
            if (stats.Exists(x => x.name == s.name))
                continue;

            stats.Add(new Stat { name = s.name, value = s.value });
        }
    }

    public void UpdateHealth()
    {
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
        {
            // Try finding it in the buffs
            foreach (Buff b in buffs)
            {
                foreach (Stat s in b.GetStatChanges())
                {
                    if (s.name == statName)
                    {
                        return s.value;
                    }
                }
            }

            return 0;
        }

        Stat modStat = new Stat
        {
            name = stat.name,
            value = stat.value
        };
        
        foreach (Buff b in buffs)
        {
            foreach (Stat s in b.GetStatChanges())
            {
                if (modStat.name == s.name)
                {
                    modStat.value += s.value;
                }
            }
        }

        return modStat.value;
    }

    public void AddLevelBonus()
    {
        Stat hp = stats.Find(x => x.name == "health");
        Stat attack = stats.Find(x => x.name == "attack");
        Stat defense = stats.Find(x => x.name == "defense");
        Stat speed = stats.Find(x => x.name == "speed");

        hp.value += 10 * level;
        attack.value += 2 * level;
        defense.value += 2 * level;
        speed.value += 2 * level;
    }

    public float GetCritChance()
    {
        float chance = 0f;

        int crit = stats.Find(x => x.name == "crit").value;
        int speed = stats.Find(x => x.name == "speed").value;

        foreach (Buff b in buffs)
        {
            Stat critBuff = b.GetStatChanges().Find(s => s.name == "crit");
            Stat spdBuff = b.GetStatChanges().Find(s => s.name == "speed");

            if (critBuff != null)
                crit += critBuff.value;

            if (spdBuff != null)
                speed += spdBuff.value;
                speed += spdBuff.value;
        }

        float spdMod = 1.0f - 1.0f / (1.0f + 0.001f * speed);

        Debug.Log(spdMod);

        chance += spdMod;
        chance += crit / 100f;

        Debug.Log(chance);

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
