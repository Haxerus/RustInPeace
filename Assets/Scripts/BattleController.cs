using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleController : MonoBehaviour
{
    public BattleState state;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    public Text descText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public ActionHUD actionHUD;

    BattleActor playerActor;
    BattleActor enemyActor;

    void Start()
    {
        state = BattleState.START;

        GameObject playerDataObj = GameObject.Find("PlayerData");
        PlayerData data = playerDataObj.GetComponent<PlayerData>();

        Debug.Log(data.health);

        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerObj = Instantiate(playerPrefab, playerBattleStation);
        playerActor = playerObj.GetComponent<BattleActor>();

        GameObject enemyObj = Instantiate(enemyPrefab, enemyBattleStation);
        enemyActor = enemyObj.GetComponent<BattleActor>();

        descText.text = enemyActor.displayName + " suddenly attacked!";

        playerHUD.SetHUD(playerActor);
        enemyHUD.SetHUD(enemyActor);

        actionHUD.SetActions(playerActor.actions);

        yield return new WaitForSeconds(2f);

        PlayerTurn();
    }

    IEnumerator PlayerAction(int action)
    {
        actionHUD.SetEnabled(false);

        descText.text = playerActor.displayName + " used " + playerActor.actions[action].actionName + "!";

        bool enemyDead = playerActor.actions[action].Effect(playerActor, enemyActor);

        playerHUD.SetHP(playerActor.currentHP);
        enemyHUD.SetHP(enemyActor.currentHP);
        
        yield return new WaitForSeconds(2f);

        if (enemyDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }


    }    

    IEnumerator EnemyTurn()
    {
        int action = Random.Range(0, 3);
        descText.text = enemyActor.displayName + " used " + enemyActor.actions[action].actionName + "!";

        //yield return new WaitForSeconds(1f);

        bool playerDead = enemyActor.actions[action].Effect(enemyActor, playerActor);

        playerHUD.SetHP(playerActor.currentHP);
        enemyHUD.SetHP(enemyActor.currentHP);

        yield return new WaitForSeconds(2f);

        if (playerDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void PlayerTurn()
    {
        state = BattleState.PLAYERTURN;
        descText.text = "Choosing an action...";
        actionHUD.SetEnabled(true);
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            descText.text = "You won!";
        }
        else if (state == BattleState.LOST)
        {
            descText.text = "The battle was lost...";
        }
    }

    public void OnAction(int action)
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAction(action));
    }

    public void BackToMain() {
        SceneManager.LoadScene("MainScene");
    }
}
