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

        enemyLevel = Random.Range(1, 1 * battleManager.playerLevel);
        enemyCurrentHP = 25; //+ Random.Range(0, 5 + battleManager.playerLevel);
        enemyMaxHP = enemyCurrentHP;
        enemyDamage = 15;
        enemySpeed = 0;

    }
}
