using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionHUD : MonoBehaviour
{

    public List<Button> buttons;

    public void SetActions(List<Action> actions)
    {
        for (int i = 0; i < actions.Count; i++)
        {
            Text buttonText = buttons[i].GetComponentInChildren<Text>();
            buttonText.text = actions[i].actionName;
        }
    }

    public void SetEnabled(bool enabled)
    {
        foreach (Button b in buttons)
        {
            b.interactable = enabled;
        }
    }
}
