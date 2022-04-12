using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    // Globally persistent stats
    public int money;
    public int experience;
    public int level;
    public string playerName;

    // Base battle stats
    public List<Stat> baseStats;

    private static GameObject instance;
        
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = gameObject;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GainMoney(int m)
    {
        money += m;
    }

    public bool GainEXP(int xp)
    {
        experience += xp;

        if (experience >= EXPToNextLevel())
        {
            level++;
            return true;
        }

        return false;
    }

    public int EXPToNextLevel()
    {
        return level * 100;
    }
}
