using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BattleState { START, INPUT, TURN, WON, LOST }

public class BattleController : MonoBehaviour
{
    public BattleState state;

    public Text battleText;

    public Transform playerPosition;
    public Transform enemyPosition;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public ActionHUD actionHUD;

    GameObject player;
    GameObject enemy;

    BattleActor playerActor;
    BattleActor enemyActor;

    PlayerData playerData;

    void Start()
    {
        state = BattleState.START;

        GameObject playerDataObj = GameObject.Find("PlayerData");
        playerData = playerDataObj.GetComponent<PlayerData>();

        StartCoroutine(SetupBattle());
    }

    void RefreshUI()
    {
        playerHUD.SetHP(playerActor.currentHP);
        enemyHUD.SetHP(enemyActor.currentHP);
    }

    IEnumerator SetupBattle()
    {
        LoadPlayer();
        LoadEnemy();

        battleText.text = enemyActor.displayName + " suddenly attacked!";

        playerHUD.SetHUD(playerActor);
        enemyHUD.SetHUD(enemyActor);

        actionHUD.SetActions(playerActor.actions);

        yield return new WaitForSeconds(2f);

        WaitForInput();
    }

    IEnumerator ProcessTurn(int action)
    {
        state = BattleState.TURN;

        Action playerAction = playerActor.actions[action];
        playerAction.user = playerActor;
        playerAction.target = enemyActor;

        Action enemyAction = enemyActor.actions[Random.Range(0, enemyActor.actions.Count)];
        enemyAction.user = enemyActor;
        enemyAction.target = playerActor;

        Action first = playerAction;
        Action second = enemyAction;

        // Decide turn order
        if (enemyAction.type == Action.ActionType.DEFENSE)
        {
            first = enemyAction;
            second = playerAction;
        }

        if (playerAction.type == Action.ActionType.DEFENSE)
        {
            first = playerAction;
            second = enemyAction;
        }

        if (enemyAction.type == Action.ActionType.SPEED)
        {
            first = enemyAction;
            second = playerAction;
        }

        if (playerAction.type == Action.ActionType.SPEED)
        {
            first = playerAction;
            second = enemyAction;
        }

        // Execute actions

        yield return StartCoroutine(DisplayActionText(first.user, first));

        first.Effect();
        RefreshUI();

        if (!second.user.IsDead())
        {
            yield return StartCoroutine(DisplayActionText(second.user, second));

            second.Effect();
            RefreshUI();
        }

        // End of Turn

        if (playerActor.IsDead())
            state = BattleState.LOST;
        else if (enemyActor.IsDead())
            state = BattleState.WON;

        yield return new WaitForSeconds(1f);

        if (state == BattleState.WON || state == BattleState.LOST)
        {
            EndBattle();
        } else
        {
            WaitForInput();
        }
    }

    void WaitForInput()
    {
        state = BattleState.INPUT;
        battleText.text = "Choosing an action...";
        actionHUD.SetEnabled(true);
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            battleText.text = "You won!";
        }
        else if (state == BattleState.LOST)
        {
            battleText.text = "The battle was lost...";
        }
    }

    public void OnAction(int action)
    {
        if (state != BattleState.INPUT)
            return;

        actionHUD.SetEnabled(false);

        StartCoroutine(ProcessTurn(action));
    }

    public void BackToMain() {
        SceneManager.LoadScene("MainScene");
    }

    private IEnumerator DisplayActionText(BattleActor user, Action action)
    {
        battleText.text = user.displayName + " used " + action.name + "!";
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator DisplayText(string s)
    {
        battleText.text = s;
        yield return new WaitForSeconds(1f);
    }

    private void LoadPlayer()
    {
        player = Resources.Load<GameObject>("Prefabs/Player");
        Instantiate(player, playerPosition);

        playerActor = player.GetComponent<BattleActor>();

        playerActor.displayName = playerData.playerName;
        playerActor.maxHP = playerData.health;
        playerActor.currentHP = playerData.health;
        playerActor.attack = playerData.attack;
        playerActor.defense = playerData.defense;
        playerActor.speed = playerData.speed;
    }

    private void LoadEnemy()
    {
        enemy = Resources.Load<GameObject>("Prefabs/EnemyDrone");
        Instantiate(enemy, enemyPosition);

        enemyActor = enemy.GetComponent<BattleActor>();
    }
}
