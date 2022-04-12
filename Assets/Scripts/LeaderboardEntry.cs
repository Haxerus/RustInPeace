using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardEntry : MonoBehaviour
{
    public Text playerName;
    public Text level;
    public Text winCount;
    public Text moneyEarned;

    public void CreateEntry(string name, int lvl, int wins, int money)
    {
        playerName.text = name;
        level.text = lvl.ToString();
        winCount.text = wins.ToString();
        moneyEarned.text = money.ToString();
    }
}
