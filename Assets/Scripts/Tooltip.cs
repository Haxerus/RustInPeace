using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public GameObject statGroup;
    public GameObject modifierText;
    
    Item item;

    void RefreshUI()
    {
        ClearStatGroup();
        
        for (int i = 0; i < 5; i++)
        {
            GameObject mod = Instantiate(modifierText, statGroup.transform);
        }
    }

    public void UpdateItem(Item i)
    {
        item = i;
        RefreshUI();
    }

    private void ClearStatGroup()
    {
        foreach (Transform child in statGroup.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
