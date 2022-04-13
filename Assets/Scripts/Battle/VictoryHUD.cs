using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryHUD : MonoBehaviour
{
    public Text moneyText;
    public Text xpText;
    public Text leveledUp;

    public void UpdateHUD(int money, int xp, bool didLevel)
    {
        moneyText.text = String.Format("You earned ${0}", money);
        xpText.text = String.Format("You gained {0} EXP", xp);
        leveledUp.gameObject.SetActive(didLevel);
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}
