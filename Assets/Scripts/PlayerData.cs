using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    // Globally persistent stats
    public int money { get; set; }
    public int experience { get; set; }
    public int level { get; set; }

    // Battle stats
    public int health { get; set; }
    public int attack { get; set;  }
    public int defense { get; set; }
    public int speed { get; set; }

    // TODO: Replace int with Item object
    private List<int> inventory = new List<int>();
    private int[] equipped = new int[4];
        
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        health = 20;
        attack = 5;
        defense = 5;
        speed = 5;
    }

    void AddItem(int item)
    {
        inventory.Add(item);
    }

    void EquipItem(int item, int slot)
    {
        if (slot < 0 || slot > equipped.Length)
            return;

        equipped[slot] = item;
    }
}
