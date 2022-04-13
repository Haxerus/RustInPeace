using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public GameObject cursorSlotObj;
    public GameObject tooltipObj;
    public GameObject playerModel;

    public GameObject statsContainer;
    public GameObject actionsContainer;

    public GameObject modifierText;

    public Sprite defaultHeadSprite;
    public Sprite defaultBodySprite;
    public Sprite defaultLegsSprite;

    Slot[] slots;

    Slot head;
    Slot body;
    Slot legs;

    Image headImage;
    Image bodyImage;
    Image legsImage;

    Slot cursorSlot;
    Item cursor;

    Tooltip tooltip;

    PlayerData playerData;
    InventoryData invData;

    void Start()
    {
        GameObject playerDataObj = GameObject.Find("PlayerData");
        if (playerDataObj)
            playerData = playerDataObj.GetComponent<PlayerData>();

        GameObject invObj = GameObject.Find("InventoryData");
        if (invObj)
            invData = invObj.GetComponent<InventoryData>();

        tooltipObj.SetActive(false);

        cursorSlot = cursorSlotObj.GetComponent<Slot>();
        tooltip = tooltipObj.GetComponent<Tooltip>();

        headImage = playerModel.transform.GetChild(0).gameObject.GetComponent<Image>();
        bodyImage = playerModel.transform.GetChild(1).gameObject.GetComponent<Image>();
        legsImage = playerModel.transform.GetChild(2).gameObject.GetComponent<Image>();

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
        headImage.sprite = defaultHeadSprite;
        bodyImage.sprite = defaultBodySprite;
        legsImage.sprite = defaultLegsSprite;

        DisplayStats();
        DisplayActions();

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
        {
            Equipment eq = invData.GetEquipment(0) as Equipment;
            head.AddItem(eq);
            headImage.sprite = eq.equippedSprite;
        }
            

        body.RemoveItem();
        if (invData.GetEquipment(1))
        {
            Equipment eq = invData.GetEquipment(1) as Equipment;
            body.AddItem(eq);
            bodyImage.sprite = eq.equippedSprite;
        }

        legs.RemoveItem();
        if (invData.GetEquipment(2))
        {
            Equipment eq = invData.GetEquipment(2) as Equipment;
            legs.AddItem(eq);
            legsImage.sprite = eq.equippedSprite;
        }
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
        if (entered)
        {
            if (id < 100)
            {
                tooltip.UpdateItem(invData.GetItem(id));
            }
            else
            {
                tooltip.UpdateItem(invData.GetEquipment(id - 100));
            }
        }
        else
        {
            tooltip.UpdateItem(null);
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
        if (cursor != null)
            return;

        SceneManager.LoadScene("MainScene");
    }

    private void DisplayStats()
    {
        for (int i = 0; i < statsContainer.transform.childCount; i++)
        {
            Destroy(statsContainer.transform.GetChild(i).gameObject);
        }

        for (int j = 0; j < playerData.baseStats.Count; j++)
        {
            Stat newStat = new Stat { name = playerData.baseStats[j].name, value = playerData.baseStats[j].value };

            for (int i = 0; i < 3; i++)
            {
                Equipment e = invData.GetEquipment(i) as Equipment;

                if (e != null)
                {
                    foreach (Stat eqs in e.stats)
                    {
                        if (newStat.name == eqs.name)
                        {
                            newStat.value += eqs.value;
                        }
                    }
                }
            }

            string statText = string.Format("{0}: {1}", newStat.name, newStat.value);
            AddText(statText, statsContainer.transform);
        }
    }

    private void DisplayActions()
    {
        for (int i = 0; i < actionsContainer.transform.childCount; i++)
        {
            Destroy(actionsContainer.transform.GetChild(i).gameObject);
        }

        string[] actions = { "Attack", "Defend", "Speed Boost" };

        for (int i = 0; i < actions.Length; i++)
        {
            Equipment e = invData.GetEquipment(i) as Equipment;

            if (e != null)
            {
                actions[i] = e.action.name;
            }

            AddText(actions[i], actionsContainer.transform);
        }
    }
    private void AddText(string s, Transform parent)
    {
        GameObject mod = Instantiate(modifierText, parent);

        Text text = mod.GetComponent<Text>();
        text.text = s;
    }
}
