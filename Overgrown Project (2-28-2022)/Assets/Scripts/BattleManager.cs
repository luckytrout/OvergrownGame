using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    //public GameObject mouseDisable;
    public PlayerHealthBar playerHealthBar;
    public EnemyHealthBar enemyHealthBar;
    public GameManager gameManager;
    PlayerController playerScript;
    public GameObject mouseDisable;

    public int playerCurrentHP;
    public int playerLevel;
    public int playerMaxHP;
    public int playerXP;
    public float playerSpeed;
    public int playerDamage;

    public GameObject currentEnemy;
    //private GameObject playerObject = null;
    private EnemyBehavior enemyStats;
    /*public int enemyLevel = 1;
    public int enemyCurrentHP = 25;
    public int enemyMaxHP = 25;
    public int enemyDamage = 10;*/

    public int enemyLevel;
    public int enemyCurrentHP;
    public int enemyMaxHP;
    public int enemyDamage;
    public int enemySpeed;

    public Text battleText1;
    public Text battleText2;
    public Text battleText3;
    public Text battleText4;


    //PlayerController playerScript = playerObject.GetComponent<PlayerController>();

    // Start is called before the first frame update
    void Start()
    {
        playerCurrentHP = 100;
        playerLevel = 1;
        playerMaxHP = 100;
        playerXP = 0;
        playerSpeed = 0;
        playerDamage = 10;

        playerHealthBar.SetDefaultHealth(playerMaxHP);
        enemyHealthBar.SetDefaultHealth(enemyMaxHP);
    }

    /*public void SetCursorVisible(bool vis)
    {
        Cursor.visible = vis;
        mouseDisable.SetActive(!vis);
        
    }*/

    public void disableMouseInput(bool disable)
    {
        mouseDisable.SetActive(disable);
    }

    void delay(float setDelay)
    //void BattleDelay(float setDelay)
    {
        
        StartCoroutine(delayCo(setDelay));
        
        //StartCoroutine(AttackDelay(setDelay));
    }

    IEnumerator delayCo(float delay)
    {
        gameManager.SetCursorVisible(false);
        
        if (playerSpeed >= enemySpeed) //if player is faster than enemy
        {
            yield return new WaitForSecondsRealtime(delay);
            DealDamage(playerDamage);
            if (enemyCurrentHP <= 0)
            {
                yield return new WaitForSecondsRealtime(delay);
                gameManager.SwitchState(GameManager.State.REWARD);

            }
            else
            {
                yield return new WaitForSecondsRealtime(delay);
                TakeDamage(enemyDamage);
                if (playerCurrentHP <= 0)
                {
                    yield return new WaitForSecondsRealtime(1);
                    gameManager.SwitchState(GameManager.State.GAMEOVER);
                }
            }
        }
        else
        {
            yield return new WaitForSecondsRealtime(delay);
            TakeDamage(enemyDamage);
            if (playerCurrentHP <= 0)
            {
                yield return new WaitForSecondsRealtime(1);
                gameManager.SwitchState(GameManager.State.GAMEOVER);
            }
            else
            {
                yield return new WaitForSecondsRealtime(delay);
                DealDamage(playerDamage);

                if (enemyCurrentHP <= 0)
                {
                    yield return new WaitForSecondsRealtime(delay);
                    gameManager.SwitchState(GameManager.State.REWARD);

                }
            }
        }
        gameManager.SetCursorVisible(true);
    }

    public void ClickAttack()
    {
        delay(1);
    }

    public void ResetStats()
    {
        playerHealthBar.SetDefaultHealth(playerMaxHP);
        enemyHealthBar.SetDefaultHealth(enemyMaxHP);

        playerCurrentHP = 100;
        playerLevel = 1;
        playerMaxHP = 100;
        playerXP = 0;
        playerSpeed = 0;
        playerDamage = 10;

        currentEnemy = null;
        enemyLevel = 1;
        enemyCurrentHP = 25;
        enemyMaxHP = 15;
        enemySpeed = 0;

    }

    void TakeDamage(int damage)
    {
        playerCurrentHP -= enemyDamage;
        playerHealthBar.SetCurrentHealth(playerCurrentHP);
        playerHealthBar.TextChangeCurrent(playerCurrentHP, playerMaxHP);
        BattleTextChange(false, true);
    }

    void DealDamage(int damage)
    {
        enemyCurrentHP -= playerDamage;
        enemyHealthBar.SetCurrentHealth(enemyCurrentHP);
        enemyHealthBar.TextChangeCurrent(enemyCurrentHP, enemyMaxHP);
        BattleTextChange(true, false);
    }

    public void SetEnemyStats(EnemyBehavior enemyStats)
    {
        enemyLevel = enemyStats.enemyLevel;
        enemyCurrentHP = enemyStats.enemyCurrentHP;
        enemyMaxHP = enemyStats.enemyMaxHP;
        enemyDamage = enemyStats.enemyDamage;
        enemySpeed = enemyStats.enemySpeed;
        enemyHealthBar.SetDefaultHealth(enemyCurrentHP);

    }

    /*public void UpdateEnemyClone(EnemyBehavior enemyStats)
    {
        enemyHealthBar.SetDefaultHealth(enemyCurrentHP);
        enemyStats.enemyLevel = enemyLevel; 
        enemyStats.enemyCurrentHP = enemyCurrentHP; 
        enemyStats.enemyMaxHP = enemyMaxHP;
        enemyStats.enemyDamage = enemyDamage; 
        enemyStats.enemySpeed = enemySpeed;
    }*/

    public void BattleTextChange(bool playerAttacked, bool enemyAttacked)
    {
        battleText1.text = battleText2.text;
        battleText2.text = battleText3.text;
        battleText3.text = battleText4.text;

        if (playerAttacked)
        {
            battleText4.text = "<b>Enemy</b> took " + playerDamage + " damage";
        }else if (enemyAttacked)
        {
            battleText4.text = "<b>Player</b> took " + enemyDamage + " damage";
        }
            //else { battleText2.text = "Player took damage" 

    }

    public void BattleTextReset()
    {
        battleText1.text = "";
        battleText2.text = "";
        battleText3.text = "";
        battleText4.text = "";
    }

    /*void UpdatePlayerStats ()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerScript = playerObject.GetComponent<PlayerController>();
        //playerScript.level = playerLevel;
        //playerScript.currentHP = playerCurrentHP;
        //playerScript.maxHP = playerMaxHP;
        //playerScript.XP = playerXP;
        //playerScript.currentHP = playerCurrentHP;
        //playerScript.damage = playerDamage;
    }*/

    // Update is called once per frame
    void Update()
    {

        if (gameManager.playerScript != null) {
            playerScript = gameManager.playerScript;

            if (playerScript.battlingEnemy != null)
            {
                currentEnemy = playerScript.battlingEnemy;
                //Debug.Log(currentEnemy);
                enemyStats = (playerScript.battlingEnemy).GetComponent<EnemyBehavior>();
                SetEnemyStats(enemyStats);
                playerScript.battlingEnemy = null;
            }
        }

        //UpdateEnemyClone(enemyStats);

    }



    

}
