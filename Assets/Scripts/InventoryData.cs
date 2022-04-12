using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryData : MonoBehaviour
{
    [SerializeField] private Item[] inventory;
    [SerializeField] private Item[] equipment;

    private static GameObject instance;

    void Start()
    {
        inventory = new Item[25];
        equipment = new Item[3];
    }

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

    public bool AddItem(Item item, int index)
    {
        if (inventory[index] == null)
        {
            inventory[index] = item;
            return true;
        }
        return false;
    }

    public Item GetItem(int i)
    {
        return inventory[i];
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = item;
                return true;
            }
        }

        return false;
    }

    public bool RemoveItem(int index)
    {
        if (inventory[index] == null)
        {
            return false;
        }

        inventory[index] = null;
        return true;
    }

    public Item GetEquipment(int slot)
    {
        return equipment[slot];
    }

    public bool EquipItem(Item item, int slot)
    {
        if (equipment[slot] == null)
        {
            equipment[slot] = item;
            return true;
        }
        return false;
    }

    public bool UnequipItem(int slot)
    {
        if (equipment[slot] == null)
        {
            return false;
        }

        equipment[slot] = null;
        return true;
    }
}
