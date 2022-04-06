using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject itemCursor;

    [SerializeField] private GameObject itemSlot;
    [SerializeField] private GameObject upgradeSlot;
    [SerializeField] private GameObject slotHolder;
    [SerializeField] private ItemClass itemToAdd;
    [SerializeField] private ItemClass itemToRemove;

    [SerializeField] private ItemClass item;
    [SerializeField] private ItemClass upgrade;

    [SerializeField] private SlotClass[] startingItems;

    public SlotClass[] items;
    public SlotClass[] itemSelected;
    public SlotClass[] upgradeSelected;

    public SlotClass[] allItems;

    private GameObject[] slots;
    private GameObject[] itemSlots;
    private GameObject[] upgradeSlots;

    private GameObject[] allSlots;

    private SlotClass movingSlot;
    private SlotClass tempSlot;
    private SlotClass originalSlot;

    bool isMovingItem;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void Start()
    {
        //Creating the slots
        slots = new GameObject[slotHolder.transform.childCount];
        itemSlots = new GameObject[itemSlot.transform.childCount];
        upgradeSlots = new GameObject[upgradeSlot.transform.childCount];

        int totLength = slots.Length + itemSlots.Length + upgradeSlots.Length;
        allSlots = new GameObject[totLength];

        items = new SlotClass[slots.Length];
        itemSelected = new SlotClass[itemSlots.Length];
        upgradeSelected = new SlotClass[upgradeSlots.Length];

        int numItems = items.Length + itemSelected.Length + upgradeSelected.Length;
        allItems = new SlotClass[numItems];

        int l, j, k = 0;
        for (l = 0; l < items.Length; l++)
        {
            items[l] = new SlotClass();
            allItems[l] = items[l];
        }
        for (int i = 0; i < startingItems.Length; i++)
        {
            items[i] = startingItems[i];

        }

        for(j = 0; j < itemSelected.Length; j++)
        {
            itemSelected[j] = new SlotClass();
        }

        for (k = 0; k < upgradeSelected.Length; k++)
        {
            upgradeSelected[k] = new SlotClass();
        }


        //Initilizing the slots
        for (l = 0; l < slotHolder.transform.childCount; l++)
        {
            slots[l] = slotHolder.transform.GetChild(l).gameObject;
        }
        for (j = 0; j < itemSlot.transform.childCount; j++)
        {
            itemSlots[j] = itemSlot.transform.GetChild(j).gameObject;
        }
        for (k = 0; k < upgradeSlot.transform.childCount; k++)
        {
            upgradeSlots[k] = upgradeSlot.transform.GetChild(k).gameObject;
        }

        RefreshUI();
        Add(itemToAdd, 1);
        Remove(itemToRemove);

        AddItemToSelection(item, 1);
        AddUpgradeToSelection(upgrade, 1);

    }

    private void Update()
    {
        itemCursor.SetActive(isMovingItem);
        itemCursor.transform.position = Input.mousePosition;
        if (isMovingItem)
        {
            itemCursor.GetComponent<Image>().sprite = movingSlot.GetItem().Sprite;
        }

        if (Input.GetMouseButtonDown(0)) //We clicked!
        {
            //Find the closest slot over the slot we clicked on
            if (isMovingItem)
            {
                //End Item Move
                EndItemMove();
            }
            else
            {
                BeginItemMove();
            }
        }
    }

    #region Inventory Utils
    public void RefreshUI()
    {

        //For every slot
        for (int i = 0; i < slots.Length; i++)
        {
            try
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true; //Enable the image
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetItem().Sprite; //Set the image to the sprite

                if (items[i].GetItem().isStackable) //Check if item is stackable
                    slots[i].transform.GetChild(1).GetComponent<Text>().text = items[i].GetQuantity() + ""; //If so set the quantity to the number of items
                else
                    slots[i].transform.GetChild(1).GetComponent<Text>().text = ""; //If not set the quantity to blank
            }
            catch
            {
                //Empty slot
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
        }

        // For every item slot
        for (int i = 0; i < itemSlots.Length; i++)
        {
            try
            {
                itemSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true; //Enable the image
                itemSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = itemSelected[i].GetItem().Sprite; //Set the image to the sprite

                if (itemSelected[i].GetItem().isStackable) //Check if item is stackable
                    itemSlots[i].transform.GetChild(1).GetComponent<Text>().text = itemSelected[i].GetQuantity() + ""; //If so set the quantity to the number of items
                else
                    itemSlots[i].transform.GetChild(1).GetComponent<Text>().text = ""; //If not set the quantity to blank
            }
            catch
            {
                //Empty slot
                itemSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                itemSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                itemSlots[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
        }

        //for every upgrade slot
        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            try
            {
                upgradeSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true; //Enable the image
                upgradeSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = upgradeSelected[i].GetItem().Sprite; //Set the image to the sprite

                if (upgradeSelected[i].GetItem().isStackable) //Check if item is stackable
                    upgradeSlots[i].transform.GetChild(1).GetComponent<Text>().text = upgradeSelected[i].GetQuantity() + ""; //If so set the quantity to the number of upgradeSelected
                else
                    upgradeSlots[i].transform.GetChild(1).GetComponent<Text>().text = ""; //If not set the quantity to blank
            }
            catch
            {
                //Empty slot
                upgradeSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                upgradeSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                upgradeSlots[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
        }

        Array.Copy(slots, allSlots, slots.Length);
        Array.Copy(itemSlots, 0, allSlots, slots.Length, itemSlots.Length);
        Array.Copy(upgradeSlots, 0, allSlots, slots.Length + itemSlots.Length, upgradeSlots.Length);

        Array.Copy(items, allItems, items.Length);
        Array.Copy(itemSelected, 0, allItems, items.Length, itemSelected.Length);
        Array.Copy(upgradeSelected, 0, allItems, items.Length + itemSelected.Length, upgradeSelected.Length);
    }

    public bool AddItemToSelection(ItemClass item, int quantity)
    {
        SlotClass slot = Contains(item, 1); //Checks if the slots contain the specified item
        if (slot != null && slot.GetItem().isStackable) //If item exists and is stackable
        {
            slot.AddQuantity(quantity);
        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (itemSelected[i].GetItem() == null)
                {
                    itemSelected[i].AddItem(item, 1);
                    break;
                }
            }
        }

        RefreshUI();
        return true;
    }

    public bool AddUpgradeToSelection(ItemClass item, int quantity)
    {
        SlotClass slot = Contains(item, 2); //Checks if the slots contain the specified item
        if (slot != null && slot.GetItem().isStackable) //If item exists and is stackable
        {
            slot.AddQuantity(quantity);
        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (upgradeSelected[i].GetItem() == null)
                {
                    upgradeSelected[i].AddItem(item, 1);
                    break;
                }
            }
        }

        RefreshUI();
        return true;
    }

    public bool Add(ItemClass item, int quantity)
    {
        SlotClass slot = Contains(item, 0); //Checks if the slots contain the specified item
        if (slot != null && slot.GetItem().isStackable) //If item exists and is stackable
        {
            slot.AddQuantity(quantity);
        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].GetItem() == null)
                {
                    items[i].AddItem(item, 1);
                    break;
                }
            }
        }

        RefreshUI();
        return true;
    }

    public bool Remove(ItemClass item)
    {
        SlotClass temp = Contains(item, 0); //Checks if the slots contain the specified item
        if (temp != null) //Slot does contain item
        {
            if (temp.GetQuantity() > 1) //If quantity greater than 1
                temp.SubQuantity(1); //Remove 1 from quantity
            else
            {
                //Remove item
                int slotToRemoveIndex = 0;

                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].GetItem() == item)
                    {
                        slotToRemoveIndex = i;
                        break;
                    }
                }
                items[slotToRemoveIndex].Clear();
            }
        }
        else
        {
            return false;
        }

        RefreshUI();
        return true;
    }

    public bool RemoveFromItemSelection(ItemClass item)
    {
        SlotClass temp = Contains(item, 1); //Checks if the slots contain the specified item
        if (temp != null) //Slot does contain item
        {
            if (temp.GetQuantity() > 1) //If quantity greater than 1
                temp.SubQuantity(1); //Remove 1 from quantity
            else
            {
                //Remove item
                int slotToRemoveIndex = 0;

                for (int i = 0; i < itemSelected.Length; i++)
                {
                    if (itemSelected[i].GetItem() == item)
                    {
                        slotToRemoveIndex = i;
                        break;
                    }
                }
                itemSelected[slotToRemoveIndex].Clear();
            }
        }
        else
        {
            return false;
        }

        RefreshUI();
        return true;
    }

    public bool RemoveFromUpgradeSelection(ItemClass item)
    {
        SlotClass temp = Contains(item, 2); //Checks if the slots contain the specified item
        if (temp != null) //Slot does contain item
        {
            if (temp.GetQuantity() > 1) //If quantity greater than 1
                temp.SubQuantity(1); //Remove 1 from quantity
            else
            {
                //Remove item
                int slotToRemoveIndex = 0;

                for (int i = 0; i < upgradeSelected.Length; i++)
                {
                    if (upgradeSelected[i].GetItem() == item)
                    {
                        slotToRemoveIndex = i;
                        break;
                    }
                }
                upgradeSelected[slotToRemoveIndex].Clear();
            }
        }
        else
        {
            return false;
        }

        RefreshUI();
        return true;
    }

    public SlotClass Contains(ItemClass item, int type)
    {
        if (type == 0)
        {
            foreach (SlotClass slot in items)
            {
                if (slot.GetItem() == item)
                {
                    return slot;
                }
            }
        }
        else if (type == 1)
        {
            foreach (SlotClass slot in itemSelected)
            {
                if (slot.GetItem() == item)
                {
                    return slot;
                }
            }
        }
        else if (type == 2)
        {
            foreach (SlotClass slot in upgradeSelected)
            {
                if (slot.GetItem() == item)
                {
                    return slot;
                }
            }
        }

        return null;
    }
    #endregion

    #region Moving Stuff
    private bool BeginItemMove()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null || originalSlot.GetItem() == null)
        {
            return false;
        }

        movingSlot = new SlotClass(originalSlot);
        originalSlot.Clear();
        RefreshUI();
        isMovingItem = true;
        return true;
    }

    private bool EndItemMove()
    {
        originalSlot = GetClosestSlot();

        if (originalSlot == null)
        {
            Add(movingSlot.GetItem(), movingSlot.GetQuantity());
            Debug.Log(movingSlot.GetItem());
            movingSlot.Clear();
        }
        else
        {
            if (originalSlot.GetItem() != null)
            {
                if (originalSlot.GetItem() == movingSlot.GetItem()) // They're the same time (they should stack)
                {
                    if (originalSlot.GetItem().isStackable)
                    {
                        originalSlot.AddQuantity(movingSlot.GetQuantity());
                        movingSlot.Clear();
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    tempSlot = new SlotClass(originalSlot); // a = b
                    originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity()); // b = c
                    movingSlot.AddItem(tempSlot.GetItem(), tempSlot.GetQuantity()); // a = c
                    RefreshUI();
                    return true;
                }
            }
            else // Place Item as Usual
            {
                originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                movingSlot.Clear();
            }
        }

        isMovingItem = false;
        RefreshUI();
        return true;

    }

    private SlotClass GetClosestSlot()
    {
        for (int i = 0; i < allSlots.Length; i++)
        {
            if (Vector2.Distance(allSlots[i].transform.position, Input.mousePosition) <= 32)
            {
                return allItems[i];
            }
        }
        return null;
    }
    #endregion

    #region Information Transfer
    public SlotClass[] GetInventoryItems()
    {
        return items;
    }

    public SlotClass[] GetSelectedItems()
    {
        return itemSelected;
    }

    public SlotClass[] GetSelectedUpgrades()
    {
        return upgradeSelected;
    }
    #endregion
}
