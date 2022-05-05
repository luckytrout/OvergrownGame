using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private BattleManager battleManager;
    public int enemyLevel;
    public int enemyCurrentHP;
    public int enemyMaxHP;
    public int enemyDamage;
    public int enemySpeed;

    // Start is called before the first frame update
    void Start()
    {
        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();

        enemyLevel = Random.Range(1, battleManager.playerLevel);
        enemyCurrentHP = 25 + Random.Range(0, battleManager.playerMaxHP/3);
        enemyMaxHP = enemyCurrentHP;
        enemyDamage = (int)Random.Range((battleManager.playerDamage/2) + 5, battleManager.playerDamage);
        enemySpeed = 0 + (int)Random.Range(0, (battleManager.playerSpeed + 2)); //0,1,2,3 -->> 1/2 chance for 2 or 3 enemy goes faster

    }
}
