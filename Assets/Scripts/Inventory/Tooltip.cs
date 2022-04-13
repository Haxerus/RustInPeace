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

            if (eq.action != null)
                AddActionModText(eq.action);
        }
    }

    public void UpdateItem(Item i)
    {
        item = i;
        gameObject.SetActive(false);

        if (item)
        {
            RefreshUI();
            gameObject.SetActive(true);
        }
    }

    private void ClearStatGroup()
    {
        foreach (Transform child in statGroup.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void AddActionModText(Action a)
    {
        GameObject mod = Instantiate(modifierText, statGroup.transform);

        Text text = mod.GetComponent<Text>();

        string modText = String.Format("Grants {0} action", a.name);
        text.text = modText;
    }
}
