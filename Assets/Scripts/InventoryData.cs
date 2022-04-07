using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryData : MonoBehaviour
{
    [SerializeField] private Item[] inventoryItems;
    [SerializeField] private Item[] selectedItems;
    [SerializeField] private Item[] selectedUpgrades;

    private static GameObject instance;

    void Start()
    {
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
