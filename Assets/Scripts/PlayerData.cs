using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    // Globally persistent stats
    public int money;
    public int experience;
    public int level;
    public string playerName;

    // Base battle stats
    public List<Stat> baseStats;

    private static GameObject instance;
        
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = gameObject;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
