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

    public VictoryHUD victoryHUD;
    public GameObject lossHUD;

    GameObject player;
    GameObject enemy;

    BattleActor playerActor;
    BattleActor enemyActor;

    PlayerData playerData;
    InventoryData invData;

    int moneyReward = 100;
    int expReward = 100;

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
        playerHUD.ClearBuffIcons();
        enemyHUD.ClearBuffIcons();

        foreach (Buff b in playerActor.buffs)
        {
            playerHUD.AddBuffIcon(b);
        }

        foreach (Buff b in enemyActor.buffs)
        {
            enemyHUD.AddBuffIcon(b);
        }

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

        victoryHUD.SetVisible(false);
        lossHUD.SetActive(false);

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

        // Make speed actually do something
        if (enemyAction.type == Action.ActionType.ATTACK && playerAction.type == Action.ActionType.ATTACK)
        {
            int playerSpd = playerActor.GetStat("speed");
            int enemySpd = enemyActor.GetStat("speed");

            playerFirst = playerSpd >= enemySpd;
        }

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

        yield return new WaitForSeconds(1f);

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
            battleText.text = string.Format("{0} was defeated! You won!", enemyActor.displayName);

            bool lvl = playerData.GainEXP(expReward);
            playerData.GainMoney(moneyReward);
            playerData.battlesWon++;

            victoryHUD.UpdateHUD(moneyReward, expReward, lvl);
            victoryHUD.SetVisible(true);
        }
        else if (state == BattleState.LOST)
        {
            battleText.text = "You were defeated. The battle was lost...";
            lossHUD.SetActive(true);
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

    public void ViewLeaderboard()
    {
        SceneManager.LoadScene("LeaderboardScene");
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

        List<Stat> modifiedStats = new List<Stat>();

        foreach (Stat s in playerData.baseStats)
        {
            modifiedStats.Add(new Stat { name=s.name, value=s.value });
        }

        ApplyEquipmentStats(head, modifiedStats);
        ApplyEquipmentStats(body, modifiedStats);
        ApplyEquipmentStats(legs, modifiedStats);

        playerActor = player.GetComponent<BattleActor>();

        playerActor.displayName = playerData.playerName;
        playerActor.level = playerData.level;
        playerActor.LoadStats(modifiedStats);
        playerActor.AddLevelBonus();
        playerActor.UpdateHealth();
    }

    private void ApplyEquipmentStats(Equipment eq, List<Stat> stats)
    {
        if (eq != null)
        {
            if (eq.action != null)
            {
                int actionIdx = playerActor.actions.FindIndex(a => a.type == eq.action.type);

                if (actionIdx != -1)
                    playerActor.actions[actionIdx] = eq.action;
            }
            

            foreach (Stat s in eq.stats)
            {
                Stat match = stats.Find(x => x.name == s.name);
                if (match != null)
                {
                    match.value += s.value;
                }
            }
        }
    }

    private void LoadEnemy()
    {
        enemy = Instantiate(Resources.Load<GameObject>("Prefabs/EnemyDrone"), enemyPosition);

        enemyActor = enemy.GetComponent<BattleActor>();
        enemyActor.AddLevelBonus();
        enemyActor.UpdateHealth();
    }
}
