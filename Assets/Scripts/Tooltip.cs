using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public GameObject statGroup;
    public GameObject modifierText;

    public Text itemName;
    
    Item item;

    void RefreshUI()
    {
        itemName.text = "---";
        ClearStatGroup();

        if (item == null)
            return;

        itemName.text = item.name;

        if (item is Equipment)
        {
            Equipment eq = item as Equipment;
            foreach (Stat s in eq.stats)
            {
                GameObject mod = Instantiate(modifierText, statGroup.transform);

                Text text = mod.GetComponent<Text>();

                string sign = s.value >= 0 ? "+" : "-";
                string modText = String.Format("{0}: {1}{2}", s.name, sign, s.value);
                text.text = modText;
            }
        }
    }

    public void UpdateItem(Item i)
    {
        item = i;

        if (item)
        {
            RefreshUI();
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void ClearStatGroup()
    {
        foreach (Transform child in statGroup.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
