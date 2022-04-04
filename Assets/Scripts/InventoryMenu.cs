using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    [SerializeField] private GameObject slotHolder;
    [SerializeField] private ItemClass itemToAdd;
    [SerializeField] private ItemClass itemToRemove;

    public List<SlotClass> items = new List<SlotClass>();

    private GameObject[] slots;

    public void Start()
    {
        //Creating the slots
        slots = new GameObject[slotHolder.transform.childCount];
        
        //Initilizing the slots
        for(int i = 0; i < slotHolder.transform.childCount; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }

        RefreshUI();
        Add(itemToAdd);
        Remove(itemToRemove);
    }

    public void RefreshUI()
    {

        //For every slot
        for(int i = 0; i < slots.Length; i++)
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
    }

    public bool Add(ItemClass item)
    {
        SlotClass slot = Contains(item); //Checks if the slots contain the specified item
        if(slot != null && slot.GetItem().isStackable) //If item exists and is stackable
        {
            slot.AddQuantity(1);
        }
        else
        {
            if (items.Count < slots.Length) //If inventory has space
            {
                items.Add(new SlotClass(item, 1)); //Add new item
            }
            else
            {
                return false;
            }
        }

        RefreshUI();
        return true;
    }
    
    public bool Remove(ItemClass item)
    {
        SlotClass temp = Contains(item); //Checks if the slots contain the specified item
        if (temp != null) //Slot does contain item
        {
            if (temp.GetQuantity() > 1) //If quantity greater than 1
                temp.SubQuantity(1); //Remove 1 from quantity
            else 
            {
                //Remove item
                SlotClass slotToRemove = new SlotClass();
                foreach (SlotClass slot in items)
                {
                    if (slot.GetItem() == item)
                    {
                        slotToRemove = slot;
                        break;
                    }
                }
                items.Remove(slotToRemove);
            }
        }
        else
        {
            return false;
        }

        RefreshUI();
        return true;
    }

    public SlotClass Contains(ItemClass item)
    {
        foreach(SlotClass slot in items)
        {
            if(slot.GetItem() == item)
            {
                return slot;
            }
        }

        return null;
    }
}
