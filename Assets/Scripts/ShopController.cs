using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public GameObject[] shopSlotObjs;
    public GameObject tooltipObj;
    public GameObject moneyWarning;

    public Text playerCurrency;

    Slot[] slots;
    Text[] prices;
    Button[] buyButtons;

    PlayerData playerData;
    InventoryData invData;
    Registry registry;

    Tooltip tooltip;

    void Start()
    {
        GameObject pDataObj = GameObject.Find("PlayerData");
        if (pDataObj)
            playerData = pDataObj.GetComponent<PlayerData>();

        GameObject invObj = GameObject.Find("InventoryData");
        if (invObj)
            invData = invObj.GetComponent<InventoryData>();

        GameObject regObj = GameObject.Find("Registry");
        if (regObj)
            registry = regObj.GetComponent<Registry>();

        slots = new Slot[shopSlotObjs.Length];
        prices = new Text[shopSlotObjs.Length];
        buyButtons = new Button[shopSlotObjs.Length];

        for (int i = 0; i < shopSlotObjs.Length; i++)
        {
            slots[i] = shopSlotObjs[i].transform.GetChild(0).gameObject.GetComponent<Slot>();
            slots[i].AddHoverListener(HoverListener);

            prices[i] = shopSlotObjs[i].transform.GetChild(1).gameObject.GetComponent<Text>();
            buyButtons[i] = shopSlotObjs[i].transform.GetChild(2).gameObject.GetComponent<Button>();
        }

        tooltipObj.SetActive(false);
        tooltip = tooltipObj.GetComponent<Tooltip>();

        moneyWarning.SetActive(false);

        GenerateItems();
        RefreshUI();
    }

    void Update()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = 10;
        tooltipObj.transform.position = Camera.main.ScreenToWorldPoint(screenPos);
    }

    void RefreshUI()
    {
        playerCurrency.text = String.Format("${0}", playerData.money);

        for (int i = 0; i < invData.shopItems.Length; i++)
        {
            slots[i].RemoveItem();
            prices[i].text = "---";
            if (invData.shopItems[i])
            {
                prices[i].text = String.Format("${0}", invData.shopItems[i].price);
                slots[i].AddItem(invData.shopItems[i]);
            }
        }
    }

    void GenerateItems()
    {
        if (playerData.refreshShop)
        {
            playerData.refreshShop = false;

            List<int> itemIds = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                int id = UnityEngine.Random.Range(0, registry.items.Count);

                while (itemIds.Contains(id))
                {
                    id = UnityEngine.Random.Range(0, registry.items.Count);
                }

                itemIds.Add(id);
                invData.shopItems[i] = Instantiate(registry.items[id]);
            }
        }
    }

    public void BuyItem(int slot)
    {
        if (playerData.money < invData.shopItems[slot].price)
        {
            moneyWarning.SetActive(true);
            return;
        }

        playerData.money -= invData.shopItems[slot].price;

        invData.AddItem(invData.shopItems[slot]);
        invData.shopItems[slot] = null;
        buyButtons[slot].interactable = false;

        moneyWarning.SetActive(false);
        RefreshUI();
    }

    void HoverListener(int id, bool entered)
    {
        if (entered)
        {
            tooltip.UpdateItem(invData.shopItems[id]);
        }
        else
        {
            tooltip.UpdateItem(null);
        }
    }

    public void BackToMain()
    {
        SceneManager.LoadScene("MainScene");
    }
}
