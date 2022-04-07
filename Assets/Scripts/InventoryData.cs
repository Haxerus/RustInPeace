using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryData : MonoBehaviour
{
    [SerializeField] private SlotClass[] inventoryItems;
    [SerializeField] private SlotClass[] selectedItems;
    [SerializeField] private SlotClass[] selectedUpgrades;

    private static GameObject instance;

    void Start()
    {
        InventoryController inventoryController = new InventoryController();
        inventoryItems = inventoryController.GetInventoryItems();
        //Debug.Log(inventoryItems);
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
}
