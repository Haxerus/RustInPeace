using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
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

    Unit playerUnit;
    Unit enemyUnit;

    void Start()
    {
        state = BattleState.START;

        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerObj = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerObj.GetComponent<Unit>();

        GameObject enemyObj = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyObj.GetComponent<Unit>();

        descText.text = enemyUnit.unitName + " suddenly attacked!";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        actionHUD.SetActions(playerUnit.actions);

        yield return new WaitForSeconds(2f);

        PlayerTurn();
    }

    IEnumerator PlayerAction(int action)
    {
        actionHUD.SetEnabled(false);

        descText.text = playerUnit.unitName + " used " + playerUnit.actions[action].actionName + "!";

        bool enemyDead = playerUnit.actions[action].Effect(playerUnit, enemyUnit);

        playerHUD.SetHP(playerUnit.currentHP);
        enemyHUD.SetHP(enemyUnit.currentHP);
        
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
        descText.text = enemyUnit.unitName + " used " + enemyUnit.actions[action].actionName + "!";

        //yield return new WaitForSeconds(1f);

        bool playerDead = enemyUnit.actions[action].Effect(enemyUnit, playerUnit);

        playerHUD.SetHP(playerUnit.currentHP);
        enemyHUD.SetHP(enemyUnit.currentHP);

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
