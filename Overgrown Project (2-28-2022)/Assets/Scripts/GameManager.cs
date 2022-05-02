using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject planet;
    public GameObject mouseDisable;
    //public Animator transition;
    public BattleManager battleManager;

    public Camera menuCamera;
    private Camera playCamera;
    public Camera battleCamera;

    [SerializeField] public float minEnemySpawnWait, maxEnemySpawnWait;

    private bool isSpawning = false;
    private bool notPaused = true;
    private bool[] buffArray = new bool[3];

    [System.NonSerialized] public PlayerController playerScript;
    
    public GameObject playerClone;
    public GameObject rewardButton1;
    public GameObject rewardButton2;
    public GameObject rewardButton3;

    public GameObject panelMenu;
    public GameObject panelPlay;
    public GameObject panelCredits;
    public GameObject panelHelp;
    public GameObject panelExitConfirm;
    public GameObject panelBattle;
    public GameObject panelGameOver;
    public GameObject panelReward;
    public GameObject panelStatsOverworld;
    public GameObject panelStatsBattle;


    private int enemyCount = 0;

    public static GameManager Instance { get; private set; }

    public enum State { MENU, INIT, PLAY, GAMEOVER, EXITCONFIRM, BATTLE , REWARD};
    State _state;
    private bool _isSwitchingState;

    void SpawnObject()
    {
        enemyCount++;
        Vector3 EnemySpawnLocation = GameObject.FindGameObjectWithTag("EnemySpawnLocation").transform.position;
        if (!playerScript.isBattling)
        {
            Instantiate(enemyPrefab, EnemySpawnLocation, transform.rotation);
        }
        isSpawning = false;
    }

    public void SetCursorVisible(bool vis)
    {
        Cursor.visible = vis;
        mouseDisable.SetActive(!vis);
    }

    //click methods
    public void ClickPanelPlay() {
        SwitchState(State.INIT, 2, true);
    }

    public void ClickCredits()
    {
        panelCredits.SetActive(true);
        panelMenu.SetActive(false);
    }

    public void ClickHelp()
    {
        panelHelp.SetActive(true);
        panelMenu.SetActive(false);
    }

    public void ClickHelpCreditsExit()
    {
        panelCredits.SetActive(false);
        panelHelp.SetActive(false);
        panelMenu.SetActive(true);
    }

    public void ClickExitConfirm()
    {
        SwitchState(State.EXITCONFIRM);
    }

    public void ClickTakeReward()
    {
        panelBattle.SetActive(false);
        playCamera.enabled = (true);
        /*if (the button pressed == speed/health/damage)
        {
            (give that buff + "num" to stat)
        }*/
        panelReward.SetActive(false);
        SwitchState(State.PLAY, 2, true);
        //Time.timeScale = 1;
        battleCamera.enabled = (false);
    }

    public void UpdateStatText(GameObject statsPanel)
    {
        Text myText = statsPanel.GetComponentInChildren<Text>();
        myText.text = "<b>" + battleManager.playerCurrentHP + "/" + battleManager.playerMaxHP + "\n"
            + battleManager.playerDamage + "\n"
            + battleManager.playerSpeed + "\n</b>" + 0;
        //+ battleManager defense ;
    }

    public void Toggle_Stats(bool toggleOn)
    {
        if(_state != State.BATTLE)
        {
            if(toggleOn == true) {
                panelStatsOverworld.SetActive(true);
            }
            else if(toggleOn == false)
            {
                panelStatsOverworld.SetActive(false);
            }
            
        }else if (_state == State.BATTLE)
        {
            if (toggleOn == true)
            {
                panelStatsBattle.SetActive(true);
            }
            else if (toggleOn == false)
            {
                panelStatsBattle.SetActive(false);
            }
        }
    }

    public void ClickExitYes()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        playerScript.isBattling = false;
        /*CancelInvoke();
        panelPlay.SetActive(false);
        panelExitConfirm.SetActive(false);
        panelBattle.SetActive(false);
        PlayerController.isBattling = false;
        var enemyClones = GameObject.FindGameObjectsWithTag("EnemyClone");
        foreach (var enemyPrefab in enemyClones)
        {
            Destroy(enemyPrefab);
        }
        Destroy(playerClone, 0.5f);
        //SceneManager.LoadScene(0);
        SwitchState(State.MENU, 2, true);*/
    }

    public void ClickExitNo()
    {
        panelExitConfirm.SetActive(false);
        SwitchState(State.PLAY);
    }

    //end of click methods
    public void RandomReward(Text button)
    {
        /* Speed = [0]
         * Attack = [1]
         * Health = [2]
         * Defense
         * Chance to Hit (the enemy)
         * Refill Health
         * Dodge Chance
         * 
         */
        int num = Random.Range(0, 3);
        while (buffArray[num] == true)
        {
            num = Random.Range(0, 3);
        }

        switch (num)
        {
            case 0:
                button.text = "<b>Speed +" + Random.Range(1, 3) + "</b>";
                buffArray[0] = true;
                break;
            case 1:
                button.text = "<b>Attack +" + Random.Range(1, 3) + "</b>";
                buffArray[1] = true;
                break;
            case 2:
                button.text = "<b>Health +" + Random.Range(1, 3) + "</b>";
                buffArray[2] = true;
                break;
                /*case 1:
                    break;
                case 1:
                    break;*/
        }
    }


    void Start()
    {
        //_menuAudio = GetComponent<AudioSource>();
        //_menuAudio.enabled = true;
        //playerScript.isBattling = false;
        battleCamera.enabled = false;
        Time.timeScale = 1;
        menuCamera.enabled = true;
        Instance = this;
        SwitchState(State.MENU);
    }

    public void SwitchState(State newState, float delay = 0, bool hideCursor = false)
    {
        StartCoroutine(SwitchDelay(newState, delay, hideCursor));
        //EndState();
        //BeginState(newState);
    }

    IEnumerator SwitchDelay(State newState, float delay, bool hideCursor)
    {
        if (hideCursor)
        {
            //transition.SetTrigger("Start");
            SetCursorVisible(false);

        }

        _isSwitchingState = true;
        yield return new WaitForSecondsRealtime(delay);
        EndState();
        _state = newState;
        BeginState(newState);
        _isSwitchingState = false;

        //if (hideCursor)
        //{
            SetCursorVisible(true);
        //}
    }

    void BeginState (State newState)
    {
        switch (newState)
        {
            case State.MENU:
                //Time.timeScale = 0;
                menuCamera.enabled = true;
                battleCamera.enabled = false;
                //playCamera.enabled = false;
                Cursor.visible = true;
                panelMenu.SetActive(true);
                UpdateStatText(panelStatsOverworld);
                UpdateStatText(panelStatsBattle);
                //menuAudio.Play();   for later
                break;
            case State.INIT:
                enemyCount = 0;
                //battleManager.ResetStats();
                playerClone = (GameObject)Instantiate(playerPrefab, transform.position, Quaternion.identity);
                playCamera = playerClone.GetComponentInChildren<Camera>();
                //Instantiate(playerPrefab);
                //playCamera = Camera.main;
                playCamera.enabled = true;
                battleCamera.enabled = false;
                panelBattle.SetActive(false);
                //CameraFollow.target = playerClone.transform;
                //enable player camera
                menuCamera.enabled = false;
                panelPlay.SetActive(true);
                SwitchState(State.PLAY);
                break;
            case State.PLAY:
                playCamera.enabled = true;
                battleCamera.enabled = false;
                panelBattle.SetActive(false);
                panelReward.SetActive(false);
                panelPlay.SetActive(true);
                Time.timeScale = 1;

                break;
            case State.EXITCONFIRM:
                panelExitConfirm.SetActive(true);
                if (Time.timeScale == 1)
                {
                    notPaused = true;
                }
                Time.timeScale = 0;
                break;
            case State.BATTLE:
                //mouseInputDelay(2, true);
                playCamera.enabled = false;
                battleManager.BattleTextReset();
                panelStatsOverworld.SetActive(false);
                battleCamera.enabled = true;
                Time.timeScale = 0;
                panelBattle.SetActive(true);
                //BattleTransition(3);
                //Cursor.visible = true;
                //menuAudio.Play();   for later
                break;
            case State.REWARD:
                Time.timeScale = 0;
                panelPlay.SetActive(false);
                Destroy(battleManager.currentEnemy);
                //Destroy(playerScript.battlingEnemy);
                playerScript.isBattling = false;
                playerScript.battlingEnemy = null;
                battleManager.currentEnemy = null;
                //panelStats.SetActive(false);
                //panelExitConfirm.SetActive(false);
                panelBattle.SetActive(false);
                RandomReward(rewardButton1.GetComponentInChildren<Text>());
                RandomReward(rewardButton2.GetComponentInChildren<Text>());
                RandomReward(rewardButton3.GetComponentInChildren<Text>());
                panelReward.SetActive(true);
                break;
            case State.GAMEOVER:
                Time.timeScale = 0;
                panelPlay.SetActive(false);
                panelExitConfirm.SetActive(false);
                panelBattle.SetActive(false);
                panelGameOver.SetActive(true);
                //panelGameOver.SetActive(true);
                break;
        }
    }
    void Update()
    {
        switch (_state)
        {
            case State.MENU:
                //
                break;
            case State.INIT:
                break;
            case State.PLAY:
               playerScript = playerClone.GetComponent<PlayerController>();

                if (playerScript.isBattling)
                {
                    Time.timeScale = 0;
                    SwitchState(State.BATTLE, 2, true);
                }
                //CameraFollow.target = playerClone.transform;
                break;
            case State.EXITCONFIRM:
                break;
            case State.BATTLE:
                UpdateStatText(panelStatsBattle);

                break;
            case State.REWARD:
                break;
            case State.GAMEOVER:
                break;
        }
    }


    private void FixedUpdate()
    {
        switch (_state)
        {
            case State.MENU:
                planet.transform.Rotate(Vector3.up * 10f * Time.deltaTime);
                planet.transform.Rotate(Vector3.forward * 6f * Time.deltaTime);
                //enemyPrefab.transform.Rotate(Vector3.up * 30f * Time.deltaTime);
                break;
            case State.INIT:
                break;
            case State.PLAY:

                if (!isSpawning)
                {
                    //if(enemyCount == 5 || playerScript.isBattling == true){
                    if (enemyCount == 5)
                    {
                        break;
                    }
                    float timer = Random.Range(minEnemySpawnWait, maxEnemySpawnWait);
                    //print(timer);
                    Invoke(nameof(SpawnObject), timer);
                    isSpawning = true;
                }
                

                break;
            case State.EXITCONFIRM:
                break;
            case State.BATTLE:
                break;
            case State.REWARD:
                break;
            case State.GAMEOVER:
                break;
        }
    }

    void EndState()
    {
        switch (_state)
        {
            case State.MENU:
                panelMenu.SetActive(false);
                //Time.timeScale = 1;
                break;
            case State.INIT:
                break;
            case State.PLAY:
                break;
            case State.EXITCONFIRM:
                panelExitConfirm.SetActive(false);
                if (notPaused)
                {
                    Time.timeScale = 1;
                }
                break;
            case State.BATTLE:
                //panelBattle.SetActive(false);
                //Time.timeScale = 1;
                break;
            case State.REWARD:
                //reset buffArray checker
                for (int i = 0; i < buffArray.Length; i++) { 
                    buffArray[i] = false;
                }
                break;
            case State.GAMEOVER:
                //panelPlay.SetActive(false);
                //panelGameOver.SetActive(false);
                break;
        }
    }

}