using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryData : MonoBehaviour
{
    [SerializeField] private SlotClass[] inventoryItems;
    [SerializeField] private SlotClass[] selectedItems;
    [SerializeField] private SlotClass[] selectedUpgrades;

    public void Start()
    {
        InventoryController inventoryController = new InventoryController();
        inventoryItems = inventoryController.GetInventoryItems();
        //Debug.Log(inventoryItems);
    }

}
