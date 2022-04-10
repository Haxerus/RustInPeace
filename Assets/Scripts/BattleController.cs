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

    IEnumerator PlayerTurn(Action action)
    {
        ExecuteAction(action, playerActor, enemyActor);
        yield return new WaitForSeconds(1.0f);
        enemyHUD.SetHP(enemyActor.currentHP);
        yield return new WaitForSeconds(1.0f);
    }

    IEnumerator EnemyTurn(Action action)
    {
        ExecuteAction(action, enemyActor, playerActor);
        yield return new WaitForSeconds(1.0f);
        playerHUD.SetHP(playerActor.currentHP);
        yield return new WaitForSeconds(1.0f);
    }

    void ExecuteAction(Action action, BattleActor user, BattleActor target)
    {
        battleText.text = user.displayName + " used " + action.name + "!";
        action.Effect(user, target);
    }

    IEnumerator ProcessTurn(int action)
    {
        state = BattleState.TURN;

        Action playerAction = playerActor.actions[action];
        Action enemyAction = enemyActor.actions[Random.Range(0, enemyActor.actions.Count)];

        // 0 = player first
        // 1 = enemy first
        // 2 = double counter
        int turnOrder = 0;

        // Determine turn order based on speed
        // Resolve speed ties with a coin flip
        if (playerActor.GetModifiedSpeed() > enemyActor.GetModifiedSpeed())
            turnOrder = 0;
        else if (enemyActor.GetModifiedSpeed() > playerActor.GetModifiedSpeed())
            turnOrder = 1;
        else
            turnOrder = Random.Range(0, 2) == 0 ? 0 : 1;

        // Counterattacks have priority
        if (playerAction.type == Action.ActionType.COUNTER && enemyAction.type == Action.ActionType.COUNTER)
            turnOrder = 2;
        else if (enemyAction.type == Action.ActionType.COUNTER && playerAction.type != Action.ActionType.COUNTER)
            turnOrder = 1;
        else if (playerAction.type == Action.ActionType.COUNTER && enemyAction.type != Action.ActionType.COUNTER)
            turnOrder = 0;

        switch (turnOrder)
        {
            case 0:
                yield return StartCoroutine(DisplayActionText(playerActor, playerAction));

                if (playerAction.type == Action.ActionType.COUNTER && enemyAction.type != Action.ActionType.ATTACK)
                {
                    yield return StartCoroutine(DisplayText("But it failed!"));
                }
                else
                {
                    playerAction.Effect(playerActor, enemyActor);
                    enemyHUD.SetHP(enemyActor.currentHP);
                }

                if (!enemyActor.IsDead())
                {
                    yield return StartCoroutine(DisplayActionText(enemyActor, enemyAction));
                    enemyAction.Effect(enemyActor, playerActor);
                    playerHUD.SetHP(playerActor.currentHP);
                }

                break;
            case 1:
                yield return StartCoroutine(DisplayActionText(enemyActor, enemyAction));

                if (enemyAction.type == Action.ActionType.COUNTER && playerAction.type != Action.ActionType.ATTACK)
                {
                    yield return StartCoroutine(DisplayText("But it failed!"));
                }
                else
                {
                    enemyAction.Effect(enemyActor, playerActor);
                    playerHUD.SetHP(playerActor.currentHP);
                }


                if (!playerActor.IsDead())
                {
                    yield return StartCoroutine(DisplayActionText(playerActor, playerAction));
                    playerAction.Effect(playerActor, enemyActor);
                    enemyHUD.SetHP(enemyActor.currentHP);
                }

                break;
            case 2:
                yield return StartCoroutine(DisplayActionText(playerActor, playerAction));
                yield return StartCoroutine(DisplayActionText(enemyActor, enemyAction));
                yield return StartCoroutine(DisplayText("But nothing happened!"));
                break;
        }

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
