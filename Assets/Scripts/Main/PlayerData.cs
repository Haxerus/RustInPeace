using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    // Globally persistent stats
    public int money;
    public int experience;
    public int level;
    public string playerName;
    public int moneyEarned { get; set; }
    public int battlesWon { get; set; }

    public bool refreshShop { get; set; } = true;

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
        moneyEarned += m;
    }

    public bool GainEXP(int xp)
    {
        experience += xp;

        bool lvl = false;

        while (experience >= EXPToNextLevel())
        {
            level++;
            lvl = true;
        }

        return lvl;
    }

    public int EXPToNextLevel()
    {
        return level * 100;
    }
}
