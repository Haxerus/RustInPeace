using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    public InputField playerName;

    PlayerData playerData;

    void Start()
    {
        GameObject pObj = GameObject.Find("PlayerData");
        playerData = pObj.GetComponent<PlayerData>();
        playerName.text = playerData.playerName;
    }

    public void ChangeScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void UpdateName(InputField field)
    {
        playerData.playerName = field.text;
    }
}
