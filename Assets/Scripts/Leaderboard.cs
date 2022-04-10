using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    private int points { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        points = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
