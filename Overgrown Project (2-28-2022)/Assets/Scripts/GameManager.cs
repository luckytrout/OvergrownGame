using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject planet;
    public GameObject mouseDisable;
    public Animator transition;
    public bool battlingTransition = false;
    public BattleManager battleManager;

    public Camera menuCamera;
    private Camera playCamera;
    public Camera battleCamera;

    [SerializeField] public float minEnemySpawnWait, maxEnemySpawnWait;

    private bool isSpawning = false;
    private bool notPaused = true;
    private bool[] buffArray = new bool[4];

    [System.NonSerialized] public PlayerController playerScript;
    
    public GameObject playerClone;
    public GameObject rewardButton1;
    public GameObject rewardButton2;
    public GameObject rewardButton3;
    private bool toggleStatsPanel = false;
    public CanvasGroup battleTextGroup;
    private State previousState;

    public GameObject panelMenu;
    public GameObject panelPlay;
    public GameObject panelCredits;
    public GameObject panelHelp;
    public GameObject panelAudioForPlay;
    public GameObject panelAudioForMenu;
    public GameObject panelExitConfirm;
    public GameObject panelBattle;
    public GameObject panelGameOver;
    public GameObject panelReward;
    public GameObject panelStatsOverworld;
    public GameObject panelStatsBattle;


    [System.NonSerialized] public int enemyCount = 0;

    public static GameManager Instance { get; private set; }

    public enum State { MENU, INIT, PLAY, GAMEOVER, EXITCONFIRM,AUDIOPAUSE, BATTLE , REWARD};
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
        panelHelp.SetActive(false);
        panelCredits.SetActive(false);
        panelAudioForMenu.SetActive(false);
        panelMenu.SetActive(true);
    }

    public void ClickExitConfirm()
    {
        previousState = _state;
        SwitchState(State.EXITCONFIRM);
    }

    public void ClickAudioMenu()
    {
        panelAudioForMenu.SetActive(true);
        panelMenu.SetActive(false);
    }

    public void ClickAudioMenuPause()
    {
        previousState = _state;
        SwitchState(State.AUDIOPAUSE);
    }



    public void ClickTakeReward(Button selectedButton)
    {
        /* Refill Health [0]
         * Speed = [1]
         * Attack = [2]
         * Health = [3]
         * Defense
         * Chance to Hit (the enemy)
         * 
         * Dodge Chance
         * 
         */

        panelBattle.SetActive(false);
        playCamera.enabled = (true);
        TMP_Text myText;
        myText = selectedButton.GetComponentInChildren<TMP_Text>();
        string buffText = myText.text;
        int buffIncrease = 0;

        if (buffText.Any(char.IsDigit)) {
            string resultString = Regex.Match(buffText, @"\d+").Value;
            buffIncrease = int.Parse(resultString);
        }

        switch (buffText)
        {
            case string a when a.Contains("Refill Health"):
                battleManager.playerCurrentHP = battleManager.playerMaxHP;
                battleManager.playerHealthBar.SetCurrentHealth(battleManager.playerMaxHP);
                break;
            case string b when b.Contains("Speed"):
                battleManager.playerSpeed += buffIncrease;
                break;
            case string c when c.Contains("Attack"):
                battleManager.playerDamage += buffIncrease;
                break;
            case string d when d.Contains("Health"):
                battleManager.playerMaxHP += buffIncrease;
                battleManager.playerCurrentHP += buffIncrease;
                break;
        }

        enemyCount--;
        panelReward.SetActive(false);
        SwitchState(State.PLAY, 2, true);
        //Time.timeScale = 1;
        battleCamera.enabled = (false);
    }

    public void UpdateStatText(GameObject statsPanel)
    {
        TMP_Text myText;
        myText = statsPanel.GetComponentInChildren<TMP_Text>();
        myText.text = "<b>" + battleManager.playerLevel + "\n"
            + battleManager.playerCurrentXP + "/" + battleManager.playerXPCap + "\n"
            + battleManager.playerCurrentHP + "/" + battleManager.playerMaxHP + "\n"
            + battleManager.playerDamage + "\n"
            + battleManager.playerSpeed + "\n"
            + 0 + "</b>";
        //+ battleManager defense ;
    }

    public void UpdateFinalStats(GameObject gameOver_statsPanel)
    {
        TMP_Text myText;
        myText = gameOver_statsPanel.GetComponentInChildren<TMP_Text>();
        myText.text = "<b>Level: " + battleManager.playerLevel + "\n"
            + "Health: " + battleManager.playerMaxHP + "\n"
            + "Damage: " + battleManager.playerDamage + "\n"
            + "Speed: " + battleManager.playerSpeed + "\n</b>";
        //+ battleManager defense ;
    }

    public void Toggle_Stats(bool toggleOn)
    {
        toggleStatsPanel = toggleOn;
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
        panelAudioForPlay.SetActive(false);
        if (previousState == State.BATTLE)
        {
            SwitchState(State.BATTLE);
        }
        else
        {
            SwitchState(State.PLAY);
        }


    }


    //end of click methods
    public void RandomReward(TMP_Text button)
    {
        /* Speed = [0]
         * Attack = [1]
         * Health = [2]
         * Defense
         * Chance to Hit (the enemy)
         * Refill Health
         * Dodge Chance
         * 
         * use size of buff array for number of available buffs
         */

        int num = Random.Range(0, buffArray.Length);
        while (buffArray[num] == true)
        {
            num = Random.Range(0, buffArray.Length);
        }

        switch (num)
        {
            case 0:
                button.text = "<b>Refill Health</b>";
                buffArray[0] = true;
                break;
            case 1:
                button.text = "<b>Speed +" + Random.Range(1, 3) + "</b>";
                buffArray[1] = true;
                break;
            case 2:
                button.text = "<b>Attack +" + Random.Range(1, 4) + "</b>";
                buffArray[2] = true;
                break;
            case 3:
                button.text = "<b>Health +" + Random.Range(5, 10) + "</b>";
                buffArray[3] = true;
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
        battleTextGroup.alpha = 0f;
        SwitchState(State.MENU, 2, false);
    }

    public void SwitchState(State newState, float delay = 0, bool hideCursor = false)
    {
        StartCoroutine(SwitchDelay(newState, delay, hideCursor));
        //EndState();
        //BeginState(newState);
    }

    IEnumerator SwitchDelay(State newState, float delay, bool hideCursor)
    {
        //transitioning = true;
        if (hideCursor)
        {
            if (!battlingTransition)
            {
                transition.SetTrigger("End");
            }
            //transition.
            //transition.SetTrigger("Start");
            SetCursorVisible(false);
        }

        _isSwitchingState = true;
        panelStatsBattle.SetActive(false);
        panelStatsOverworld.SetActive(false);

        yield return new WaitForSecondsRealtime(delay);
        EndState();
        _state = newState;
        BeginState(newState);
        _isSwitchingState = false;

        //transitioning = false;
        if (hideCursor) {
        transition.SetTrigger("Start");
        }
        SetCursorVisible(true);
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
            case State.AUDIOPAUSE:
                panelAudioForPlay.SetActive(true);
                if (Time.timeScale == 1)
                {
                    notPaused = true;
                }
                Time.timeScale = 0;
                break;
            case State.BATTLE:
                //mouseInputDelay(2, true);
                playCamera.enabled = false;
                panelStatsOverworld.SetActive(false);
                battleManager.playerHealthBar.TextChangeCurrent(battleManager.playerCurrentHP, battleManager.playerMaxHP);
                battleManager.playerHealthBar.SetMaxHealth(battleManager.playerMaxHP);
                battleManager.playerHealthBar.SetCurrentHealth(battleManager.playerCurrentHP);
                battleTextGroup.alpha = 0f;
                battleCamera.enabled = true;
                Time.timeScale = 0;
                panelBattle.SetActive(true);
                //BattleTransition(3);
                //Cursor.visible = true;
                //menuAudio.Play();   for later
                break;
            case State.REWARD:
                Time.timeScale = 0;
                panelPlay.SetActive(true);
                Destroy(battleManager.currentEnemy);
                //Destroy(playerScript.battlingEnemy);
                playerScript.isBattling = false;
                playerScript.battlingEnemy = null;
                battleManager.currentEnemy = null;
                battleManager.UpdateXPBar();
                //panelStats.SetActive(false);
                //panelExitConfirm.SetActive(false);
                panelBattle.SetActive(false);
                RandomReward(rewardButton1.GetComponentInChildren<TMP_Text>());
                RandomReward(rewardButton2.GetComponentInChildren<TMP_Text>());
                RandomReward(rewardButton3.GetComponentInChildren<TMP_Text>());
                panelReward.SetActive(true);
                break;
            case State.GAMEOVER:
                Time.timeScale = 0;
                panelPlay.SetActive(false);
                panelExitConfirm.SetActive(false);
                panelBattle.SetActive(false);
                panelGameOver.SetActive(true);
                UpdateFinalStats(panelGameOver);
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
                    battlingTransition = true;
                    SwitchState(State.BATTLE, 2, true);
                    Time.timeScale = 0;
                }
                //CameraFollow.target = playerClone.transform;
                break;
            case State.EXITCONFIRM:
                break;
            case State.AUDIOPAUSE:
                break;
            case State.BATTLE:
                if (battleManager._battleProcessing)
                {
                    SetCursorVisible(false);
                }
                else if (!battleManager._battleProcessing)
                {
                    SetCursorVisible(true);
                }
                break;
            case State.REWARD:
                break;
            case State.GAMEOVER:
                toggleStatsPanel = false;
                break;
        }

        UpdateStatText(panelStatsBattle);
        UpdateStatText(panelStatsOverworld);

        //toggle stats panel depending on game state

        if (!_isSwitchingState)
        {
            if (_state != State.BATTLE)
            {
                panelStatsBattle.SetActive(false);

                if(_state == State.PLAY)
                {
                    if (toggleStatsPanel == true)
                    {
                        panelStatsOverworld.SetActive(true);
                    }
                    else
                    {
                        panelStatsOverworld.SetActive(false);
                    }
                }
            }
            else if (_state == State.BATTLE)
            {
                panelStatsOverworld.SetActive(false);

                if (toggleStatsPanel == true)
                {
                    panelStatsBattle.SetActive(true);
                }
                else
                {
                    panelStatsBattle.SetActive(false);
                }
            }
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
            case State.AUDIOPAUSE:
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
            case State.AUDIOPAUSE:
                panelAudioForPlay.SetActive(false);
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

                UpdateStatText(panelStatsOverworld);
                UpdateStatText(panelStatsBattle);
                battleManager.BattleTextReset();

                break;
            case State.GAMEOVER:
                //panelPlay.SetActive(false);
                //panelGameOver.SetActive(false);
                break;
        }
    }

}