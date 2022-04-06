using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    // Globally persistent stats
    public int money { get; set; }
    public int experience { get; set; }
    public int level { get; set; }
    public string playerName { get; set; }

    // Base battle stats
    public int health { get; set; }
    public int attack { get; set;  }
    public int defense { get; set; }
    public int speed { get; set; }

    private static GameObject instance;
        
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
        {
            instance = this.gameObject;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        playerName = "Default Name";
        money = 100;
        experience = 0;
        level = 1;

        health = 20;
        attack = 5;
        defense = 5;
        speed = 5;
    }
}
