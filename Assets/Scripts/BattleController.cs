using System.Collections;
using System.Collections.Generic;
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
    InventoryData invData;

    void Start()
    {
        state = BattleState.START;

        GameObject playerDataObj = GameObject.Find("PlayerData");
        if (playerDataObj)
            playerData = playerDataObj.GetComponent<PlayerData>();

        GameObject invDataObj = GameObject.Find("InventoryData");
        if (invDataObj)
            invData = invDataObj.GetComponent<InventoryData>();

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
        Action enemyAction = enemyActor.actions[Random.Range(0, enemyActor.actions.Count)];

        // Decide turn order
        bool playerFirst = true;

        if (enemyAction.type == Action.ActionType.DEFENSE)
        {
            playerFirst = false;
        }

        if (playerAction.type == Action.ActionType.DEFENSE)
        {
            playerFirst = true;
        }

        if (enemyAction.type == Action.ActionType.SPEED)
        {
            playerFirst = false;
        }

        if (playerAction.type == Action.ActionType.SPEED)
        {
            playerFirst = true;
        }

        // Execute actions
        if (playerFirst)
        {
            yield return StartCoroutine(DisplayActionText(playerActor, playerAction));

            playerAction.Effect(playerActor, enemyActor);
            RefreshUI();

            if (!enemyActor.IsDead())
            {
                yield return StartCoroutine(DisplayActionText(enemyActor, enemyAction));

                enemyAction.Effect(enemyActor, playerActor);
                RefreshUI();
            }
        }
        else
        {
            yield return StartCoroutine(DisplayActionText(enemyActor, enemyAction));

            enemyAction.Effect(enemyActor, playerActor);
            RefreshUI();

            if (!playerActor.IsDead())
            {
                yield return StartCoroutine(DisplayActionText(playerActor, playerAction));

                playerAction.Effect(playerActor, enemyActor);
                RefreshUI();
            }
        }

        yield return new WaitForSeconds(0.5f);

        playerActor.ProcessBuffs();
        enemyActor.ProcessBuffs();
        RefreshUI();

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
        player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"), playerPosition);

        // Apply stats from equipment
        Equipment head = null;
        Equipment body = null;
        Equipment legs = null;

        if (invData.GetEquipment(0))
            legs = invData.GetEquipment(0) as Equipment;

        if (invData.GetEquipment(1))
            body = invData.GetEquipment(1) as Equipment;

        if (invData.GetEquipment(2))
            legs = invData.GetEquipment(2) as Equipment;

        List<Stat> modifiedStats = new List<Stat>(playerData.baseStats);

        if (head != null)
        {
            foreach (Stat s in head.stats)
            {
                Stat match = modifiedStats.Find(x => x.name == s.name);
                if (match != null)
                {
                    match.value += s.value;
                }
            }
        }

        if (body != null)
        {
            foreach (Stat s in body.stats)
            {
                Stat match = modifiedStats.Find(x => x.name == s.name);
                if (match != null)
                {
                    match.value += s.value;
                }
            }
        }
        
        if (legs != null)
        {
            foreach (Stat s in legs.stats)
            {
                Stat match = modifiedStats.Find(x => x.name == s.name);
                if (match != null)
                {
                    match.value += s.value;
                }
            }
        }

        playerActor = player.GetComponent<BattleActor>();

        playerActor.displayName = playerData.playerName;
        playerActor.LoadStats(modifiedStats);
    }

    private void LoadEnemy()
    {
        enemy = Instantiate(Resources.Load<GameObject>("Prefabs/EnemyDrone"), enemyPosition);

        enemyActor = enemy.GetComponent<BattleActor>();
        enemyActor.UpdateHealth();
    }
}
