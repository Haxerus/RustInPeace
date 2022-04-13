using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardController : MonoBehaviour
{
    public GameObject entriesContainer;
    public GameObject entryPrefab;

    PlayerData playerData;

    void Start()
    {
        GameObject playerDataObj = GameObject.Find("PlayerData");
        if (playerDataObj)
            playerData = playerDataObj.GetComponent<PlayerData>();

        AddEntry(playerData.playerName, playerData.level, playerData.battlesWon, playerData.moneyEarned);

        // Load existing entries
        for (int i = 0; i < 8; i++)
        {
            string nameKey = string.Format("LBEntry{0}Name", i);
            string levelKey = string.Format("LBEntry{0}Level", i);
            string winsKey = string.Format("LBEntry{0}Wins", i);
            string moneyKey = string.Format("LBEntry{0}Money", i);

            string savedName = "---";
            int savedLevel = 0;
            int savedWins = 0;
            int savedMoney = 0;

            int loads = 0;

            if (PlayerPrefs.HasKey(nameKey))
            {
                loads++;
                savedName = PlayerPrefs.GetString(nameKey);
            }

            if (PlayerPrefs.HasKey(levelKey))
            {
                loads++;
                savedLevel = PlayerPrefs.GetInt(levelKey);
            }

            if (PlayerPrefs.HasKey(winsKey))
            {
                loads++;
                savedWins = PlayerPrefs.GetInt(winsKey);
            }

            if (PlayerPrefs.HasKey(moneyKey))
            {
                loads++;
                savedMoney = PlayerPrefs.GetInt(moneyKey);
            }

            if (loads == 4)
            {
                AddEntry(savedName, savedLevel, savedWins, savedMoney);
            }
        }

        SaveEntries();
    }

    private void AddEntry(string name, int level, int wins, int money)
    {
        GameObject newEntryObj = Instantiate(entryPrefab, entriesContainer.transform);
        LeaderboardEntry entry = newEntryObj.GetComponent<LeaderboardEntry>();
        entry.CreateEntry(name, level, wins, money);
    }

    private void SaveEntries()
    {
        for (int i = 1; i < entriesContainer.transform.childCount; i++)
        {
            string nameKey = string.Format("LBEntry{0}Name", i-1);
            string levelKey = string.Format("LBEntry{0}Level", i-1);
            string winsKey = string.Format("LBEntry{0}Wins", i-1);
            string moneyKey = string.Format("LBEntry{0}Money", i-1);

            LeaderboardEntry entry = entriesContainer.transform.GetChild(i).GetComponent<LeaderboardEntry>();

            PlayerPrefs.SetString(nameKey, entry.playerName.text);
            PlayerPrefs.SetInt(levelKey, int.Parse(entry.level.text));
            PlayerPrefs.SetInt(winsKey, int.Parse(entry.winCount.text));
            PlayerPrefs.SetInt(moneyKey, int.Parse(entry.moneyEarned.text));
        }

        PlayerPrefs.Save();
    }
}
