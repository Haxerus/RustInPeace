using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BattleState { START, INPUT, TURN, WON, LOST }

public class BattleController : MonoBehaviour
{
    public BattleState state;

    public GameObject player;
    public GameObject enemy;

    public Text battleText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public ActionHUD actionHUD;

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
        CreatePlayer();
        CreateEnemy();

        Texture2D tex = Resources.Load<Texture2D>("Sprites/dog");
        SpriteRenderer playerSR = player.GetComponent<SpriteRenderer>();
        playerSR.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.40f, 0.15f));

        battleText.text = enemyActor.displayName + " suddenly attacked!";

        playerHUD.SetHUD(playerActor);
        enemyHUD.SetHUD(enemyActor);

        /*
        actionHUD.SetActions(playerActor.actions);*/

        yield return new WaitForSeconds(2f);

        WaitForInput();
    }

    IEnumerator PlayerAction(int action)
    {
        actionHUD.SetEnabled(false);

        battleText.text = playerActor.displayName + " used " + playerActor.actions[action].actionName + "!";

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
            state = BattleState.TURN;
            StartCoroutine(EnemyTurn());
        }


    }    

    IEnumerator EnemyTurn()
    {
        int action = Random.Range(0, 3);
        battleText.text = enemyActor.displayName + " used " + enemyActor.actions[action].actionName + "!";

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
            state = BattleState.INPUT;
        }
    }

    IEnumerator ProcessTurn(Action playerAction)
    {
        

        yield return new WaitForSeconds(2f);
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

        StartCoroutine(PlayerAction(action));
    }

    public void BackToMain() {
        SceneManager.LoadScene("MainScene");
    }

    private void CreatePlayer()
    {
        playerActor = player.GetComponent<BattleActor>();

        playerActor.displayName = playerData.playerName;
        playerActor.maxHP = playerData.health;
        playerActor.currentHP = playerData.health;
        playerActor.attack = playerData.attack;
        playerActor.defense = playerData.defense;
        playerActor.speed = playerData.speed;
    }

    private void CreateEnemy()
    {
       enemyActor = enemy.GetComponent<BattleActor>();

        enemyActor.displayName = "p-bot";
        enemyActor.maxHP = 8;
        enemyActor.currentHP = 8;
        enemyActor.attack = 2;
        enemyActor.defense = 2;
        enemyActor.speed = 3;
    }
}
