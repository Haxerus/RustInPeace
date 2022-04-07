using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public GameObject cursorSlotObj;

    public GameObject tooltipObj;

    Slot[] slots;

    Slot head;
    Slot body;
    Slot legs;

    Slot cursorSlot;
    Item cursor;

    Tooltip tooltip;

    InventoryData invData;

    void Start()
    {
        GameObject invObj = GameObject.Find("InventoryData");
        if (invObj)
            invData = invObj.GetComponent<InventoryData>();

        cursorSlot = cursorSlotObj.GetComponent<Slot>();
        tooltip = tooltipObj.GetComponent<Tooltip>();

        int numSlots = GameObject.Find("Slots").transform.childCount;
        slots = new Slot[numSlots];

        for (int i = 0; i < slots.Length; i++)
        {
            GameObject slotObj = GameObject.Find("Slots").transform.GetChild(i).gameObject;
            slots[i] = slotObj.GetComponent<Slot>();
            slots[i].AddClickListener(ClickListener);
            slots[i].AddHoverListener(HoverListener);
        }

        head = GameObject.Find("HeadSlot").GetComponent<Slot>();
        head.AddClickListener(ClickListener);
        head.AddHoverListener(HoverListener);

        body = GameObject.Find("BodySlot").GetComponent<Slot>();
        body.AddClickListener(ClickListener);
        body.AddHoverListener(HoverListener);

        legs = GameObject.Find("LegSlot").GetComponent<Slot>();
        legs.AddClickListener(ClickListener);
        legs.AddHoverListener(HoverListener);

        RefreshUI();
    }

    void Update()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = 10;
        cursorSlotObj.transform.position = Camera.main.ScreenToWorldPoint(screenPos);
        tooltipObj.transform.position = Camera.main.ScreenToWorldPoint(screenPos);

        RefreshUI();
    }

    void RefreshUI()
    {
        cursorSlot.RemoveItem();
        if (cursor)
            cursorSlot.AddItem(cursor);

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
            if (invData.GetItem(i))
                slots[i].AddItem(invData.GetItem(i));
        }

        head.RemoveItem();
        if (invData.GetEquipment(0))
            head.AddItem(invData.GetEquipment(0));

        body.RemoveItem();
        if (invData.GetEquipment(1))
            body.AddItem(invData.GetEquipment(1));

        legs.RemoveItem();
        if (invData.GetEquipment(2))
            legs.AddItem(invData.GetEquipment(2));
    }

    void ClickListener(int id)
    {
        // Regular Inventory 0 - 24
        // Equipment 100 - 102
        if (id < 100)
        {
            TransferInventory(id);
            RefreshUI();
        }
        else
        {
            switch (id)
            {
                case 100:
                    TransferEquipment(0);
                    RefreshUI();
                    break;
                case 101:
                    TransferEquipment(1);
                    RefreshUI();
                    break;
                case 102:
                    TransferEquipment(2);
                    RefreshUI();
                    break;
            }
        }
    }

    void HoverListener(int id, bool entered)
    {
        tooltipObj.SetActive(false);
        if (id < 100)
        {
            if (invData.GetItem(id) != null)
            {
                tooltipObj.SetActive(entered);
                tooltip.UpdateItem(cursor);
            }
        }
        else
        {
            if (invData.GetEquipment(id - 100) != null)
            {
                tooltipObj.SetActive(entered);
                tooltip.UpdateItem(cursor);
            }
        }
    }

    private void TransferInventory(int other)
    {
        if (cursor != null && invData.GetItem(other) != null)
        {
            Item temp = invData.GetItem(other);

            invData.RemoveItem(other);
            invData.AddItem(cursor, other);

            cursor = temp;
        }
        else if (cursor == null && invData.GetItem(other) != null)
        {
            cursor = invData.GetItem(other);
            invData.RemoveItem(other);
        }
        else if (cursor != null && invData.GetItem(other) == null)
        {
            invData.AddItem(cursor, other);
            cursor = null;
        }
    }

    private void TransferEquipment(int slot)
    {
        if (cursor != null && invData.GetEquipment(slot) != null)
        {
            if (cursor is Equipment && (int)(cursor as Equipment).slot == slot)
            {
                Item temp = invData.GetEquipment(slot);

                invData.UnequipItem(slot);
                invData.EquipItem(cursor, slot);

                cursor = temp;
            }
        }
        else if (cursor == null && invData.GetEquipment(slot) != null)
        {
            cursor = invData.GetEquipment(slot);
            invData.UnequipItem(slot);
        }
        else if (cursor != null && invData.GetEquipment(slot) == null)
        {
            if (cursor is Equipment && (int)(cursor as Equipment).slot == slot)
            {
                invData.EquipItem(cursor, slot);
                cursor = null;
            }
        }
    }

    public void BackToMain()
    {
        SceneManager.LoadScene("MainScene");
    }
}
