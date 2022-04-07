using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public GameObject[] slots;

    void Start()
    {
        foreach (GameObject slotObj in slots)
        {
            Slot slot = slotObj.GetComponent<Slot>();
            slot.AddClickListener(SlotListener);
        }
        
    }

    void SlotListener(int id)
    {
        Debug.Log("Received click from " + id);
    }
}
