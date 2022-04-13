using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionHUD : MonoBehaviour
{

    public List<Button> buttons;

    bool special = false;

    public void SetActions(List<Action> actions)
    {
        for (int i = 0; i < actions.Count; i++)
        {
            if (actions[i])
            {
                Text buttonText = buttons[i].GetComponentInChildren<Text>();
                buttonText.text = actions[i].name;
            }
        }
    }

    public void SetSpecial(bool s)
    {
        special = s;
    }

    public void SetEnabled(bool enabled)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].interactable = enabled;
        }

        if (!special)
        {
            buttons[3].interactable = false;
        }
    }
}
