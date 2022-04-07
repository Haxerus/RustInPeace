using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    Slot[] slots;

    Slot head;
    Slot body;
    Slot legs;

    void Start()
    {
        int numSlots = GameObject.Find("Slots").transform.childCount;
        slots = new Slot[numSlots];

        for (int i = 0; i < slots.Length; i++)
        {
            GameObject slotObj = GameObject.Find("Slots").transform.GetChild(i).gameObject;
            slots[i] = slotObj.GetComponent<Slot>();
            slots[i].AddClickListener(SlotListener);
        }

        head = GameObject.Find("HeadSlot").GetComponent<Slot>();
        head.AddClickListener(SlotListener);

        body = GameObject.Find("BodySlot").GetComponent<Slot>();
        body.AddClickListener(SlotListener);

        legs = GameObject.Find("LegSlot").GetComponent<Slot>();
        legs.AddClickListener(SlotListener);
    }

    void SlotListener(int id)
    {
        Debug.Log("Received click from " + id);
    }
}
