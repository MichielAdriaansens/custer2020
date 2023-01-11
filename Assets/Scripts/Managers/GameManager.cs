using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public bool mobileMode;
    public int levelMode = 0;

    public float levelStartDuration = 1.5f; //10.7 match levelstart audio
    public int score = 0;
    public int playerLives = 3;
    public bool playerDied = false;
    public bool playerLevelUp = false;
    public bool roundOver = false;
    public bool letsGO = false;

    int roundsCounter = 0;

    [HideInInspector]
    public GameObject player;   //given by script on Player "PlayerCtrl"
    public int currentLevel = 0;
    public int levelUpCounter = 50;

    public GameObject playerRagdoll;
   // [HideInInspector]
    public bool playerDirLeft = false; //checks what direction player is looking when dead for Ragdoll spawn

    bool skip = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        levelMode = PlayerPrefs.GetInt("UnlockMode");
        SetGameSpeed();
        LevelStart();

        if(Application.platform == RuntimePlatform.Android)
        {
            mobileMode = true;
        }
    }

    public void ManageScore()
    {
        //Send Score info to UIManager
        UIManager.instance.ScoreCounterUpdate(score);
        //Update HighScore
        HighScoreManager.instance.UpdateHighScore(score);
    }

    public void GameOver()
    {
        //Remove PlayerRender
        if(player.transform.Find("PlayerRender")!= null)
        Destroy(player.transform.Find("PlayerRender").gameObject);

        //replace with Ragdoll
        GameObject rag = Instantiate(playerRagdoll, player.transform.Find("RagDollSpawn").transform);

        if (playerDirLeft)
        {
            rag.transform.localScale = new Vector3(-1, 1, 1);
        }

        //Game Over UI pop up
        StartCoroutine(WaitGameOver());
        
        //destroy thhis script.. actually removes GameManager from DontDestroyOnLoad, so it will be destroyed when new scene is loaded
        GameObject removeFromDontDestroy = new GameObject();
        this.transform.parent = removeFromDontDestroy.transform;
        }

    IEnumerator WaitGameOver()
    {
        yield return new WaitForSeconds(2.02f);
        //lose sound
        AudioManager.instance.PlaySound("PlayerLose");
        UIManager.instance.GameOverScreen();
    }
    public void PlayerWin()
    {
        playerLevelUp = true;
        roundOver = true;
        //reset levelupCounter
        levelUpCounter = 50;
        //currentlevel ++
        currentLevel++;
        //add Life + UpdateHUD
        playerLives++;
        UIManager.instance.LifeBar();

        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponentInChildren<PlayerAnimation>().controlActive = false;
        // overwinning animatie
        player.GetComponentInChildren<Animator>().SetBool("WinRound", true);

        ProjSpawner.instance.SpawnStop();

        //Wait for Reset + reset
        StartCoroutine(WaitBeforeReset());

        //setGameSpeed wordt opgeroepen in WaitBeforeReset Coroutine
    }
    public void PlayerDead()
    {
        playerDied = true;
        roundOver = true;
        playerLives--;
        //Update Hud lifePoints
        UIManager.instance.LifeBar();
        //Spawner Uit
        ProjSpawner.instance.SpawnStop();

        //controls uitschakelen
        player.GetComponent<PlayerController>().enabled = false;
        
        //Animatie Control uitschakelen
        if(player.GetComponentInChildren<PlayerAnimation>() != null)
           player.GetComponentInChildren<PlayerAnimation>().controlActive = false;


        if (AudioManager.instance.SoundActive("PlayerScore"))
            AudioManager.instance.StopSound("PlayerScore");
        //Vibrate
        Handheld.Vibrate();
        //Achtergrond Flitsen
        EnviromentManager.instance.BGDamage();

        if (playerLives > 0)
        {
            //Animatie afspelen
            player.GetComponentInChildren<Animator>().SetBool("GetHit", true);
          
            //even wachtten
            StartCoroutine(WaitBeforeReset());
            //reset Level als speler nog levens heeft. anders GameOver
        }
        else
        {
            //CamShake
            Camera.main.GetComponent<Shaker>().Shake(1, 1);
            roundsCounter = 0;

            GameOver();
        }
    }
    IEnumerator WaitBeforeReset()
    {
        if(playerLevelUp)
            yield return new WaitForSeconds(5);

        if (playerDied)
        {
            yield return new WaitForSeconds(2.02f);

            AudioManager.instance.PlaySound("PlayerLose");

            yield return new WaitForSeconds(3.9f);
        }
        ResetLevel();
    }


    void ResetLevel()
    {
        if (playerDied)
        { 
            playerDied = false;
        }

        if (playerLevelUp)
        {
            playerLevelUp = false;
        }
        StartCoroutine(LoadNewScene());
    }
    IEnumerator LoadNewScene()  //Calls level Start
    {
        roundsCounter++;

        //met behulp van Async kun je wachtten tot de Async operatie(sceneladen) is voltooid voordat code verder gaat
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        
        //wachtten tot scene geladen is + 1 frame
        while (!asyncLoad.isDone)
            yield return null;
        yield return null;
        //if getreadyintro gets skipped bool set default
        skip = false;

        SetGameSpeed();
     
        levelStartDuration = 0;
        LevelStart();
    }

    void LevelStart()
    {
        StartCoroutine(WaitBeforeLevelStart());
    }
    IEnumerator WaitBeforeLevelStart()
    {
        playerDirLeft = false;
        roundOver = false;

        if (roundsCounter == 0)
        {
            AudioManager.instance.PlaySound("LevelStart");

            //Call UI intro
            UIManager.instance.StartIntroScreen();
         }
        yield return new WaitForSeconds(levelStartDuration);

        if (!skip)
        {
            SetLevelActive();
        }

        yield return new WaitForSeconds(1);
        if(letsGO == false)
        {
            SetLevelActive();
        }
    }
    void SetLevelActive()
    {
        letsGO = true;
        player.GetComponent<PlayerController>().controlActive = true;
        player.GetComponentInChildren<PlayerAnimation>().controlActive = true;
        ProjSpawner.instance.SpawnStart();

        AudioManager.instance.PlaySound("LevelBackground");
    }

   public void SkipCountDown()
    {
        letsGO = true;
        skip = true;
        if (UIManager.instance.introSkip)
        {
            StopCoroutine(WaitBeforeLevelStart());

            UIManager.instance.StopIntroScreen();
            AudioManager.instance.StopSound("LevelStart");
            
            SetLevelActive();
        }
    }

    void SetGameSpeed()
    {
        if (currentLevel > 0)
        {
            ProjSpawner.instance.Classic = false;

        }
        //gamespeed default
        else
        {
            //playerspeed
            player.GetComponent<PlayerController>().SetPlayerSpeed(2f);
         
            //Arrowspeed
            ProjSpawner.instance.projectile.GetComponent<ProjArrow>().moveSpeed = 1.5F;

            //spawnspeed 
            ProjSpawner.instance.spawnRepeatSeconds = 1.75f;

            //background animation speed
            GameObject.FindGameObjectWithTag("AnimBG").GetComponent<Animator>().speed = 1;
        }
        //Gamespeed at level 1
        if (currentLevel >= 1)
        {
            player.GetComponent<PlayerController>().SetPlayerSpeed(3f);
            player.GetComponentInChildren<Animator>().SetFloat("MoveSpeed", 1.5f);

            ProjSpawner.instance.projectile.GetComponent<ProjArrow>().moveSpeed = 2F;
            //Fix it
            ProjSpawner.instance.spawnRepeatSeconds = 1f;

            GameObject.FindGameObjectWithTag("AnimBG").GetComponent<Animator>().speed = 2f;
        }

        //Gamespeed at level 2 and beyond
        if (currentLevel >= 2 && currentLevel < 10)
        {
            for (int i = 1; i < currentLevel; i++)
            {
                //verhoog of verlaag stats
                float playerSp = player.GetComponent<PlayerController>().moveSpeed;
                player.GetComponent<PlayerController>().SetPlayerSpeed(playerSp + 0.5f);

                float arrowSpeed = ProjSpawner.instance.projectile.GetComponent<ProjArrow>().moveSpeed;
                ProjSpawner.instance.projectile.GetComponent<ProjArrow>().moveSpeed = arrowSpeed + 0.15f; //easy 0.1f hard 0.25f

                float arrowRepeat = ProjSpawner.instance.spawnRepeatSeconds;
                ProjSpawner.instance.spawnRepeatSeconds = arrowRepeat - 0.1F; //easy 0.05F hard 0.1f

                float CloudSp = GameObject.FindGameObjectWithTag("AnimBG").GetComponent<Animator>().speed;
                GameObject.FindGameObjectWithTag("AnimBG").GetComponent<Animator>().speed = CloudSp + 0.5f;

            }
        }
        else if(currentLevel >= 10)
        {
            player.GetComponent<PlayerController>().SetPlayerSpeed(7.5f);

            ProjSpawner.instance.projectile.GetComponent<ProjArrow>().moveSpeed = 1.5f;

            ProjSpawner.instance.spawnRepeatSeconds = 1.75f;

            GameObject.FindGameObjectWithTag("AnimBG").GetComponent<Animator>().speed = 5.5f;

            //flappy bird mechanic
        }
    }
}
