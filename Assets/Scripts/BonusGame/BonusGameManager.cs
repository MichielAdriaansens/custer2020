using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusGameManager : MonoBehaviour
{
    public static BonusGameManager instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        if(_UI.activeInHierarchy == false)
        {
            _UI.SetActive(true);
        }
    }

    public GameObject _UI;
    public GameObject _backGround;
    bool bgColorChange = false;
    [Space]
    public GameObject playerLine;
    public int killScore = 0;
    public bool gameIsPlaying = false;
   
    void Start()
    {
        _backGround = GameObject.Find("Background01");
        _UI = GameObject.Find("_UIManagerB");
        CusterCtrl.instance.drinkPoints = 0;
        killScore = 0;
        bgColorChange = false;
        playerLine.SetActive(false);
        
    }

    public void InitializeStartLevel()
    {
        StartCoroutine(WaitStart());
    }
    IEnumerator WaitStart()
    {
        AudioManager.instance.PlaySound("IntroMusic");
        yield return new WaitForSeconds(1);

        //intro
        CusterCtrl.instance.setMoveDir = 1;
        AudioManager.instance.PlaySound("LevelBackground");

        while (CusterCtrl.instance.transform.position.x < 2.1f)
        {
            yield return null;
        }

        AudioManager.instance.StartBonus();

        CusterCtrl.instance.setMoveDir = 0;
        yield return new WaitForSeconds(0.5f);
        CusterCtrl.instance.Drink();

        yield return new WaitForSeconds(0.5f);
        CusterCtrl.instance.speed = 5;
        CusterCtrl.instance._patrol = true;

        yield return new WaitForSeconds(3);
        CusterCtrl.instance._patrol = false;
        CusterCtrl.instance.setMoveDir = 1;
        CusterCtrl.instance.speed = 2;

        while (CusterCtrl.instance.transform.position.x < 0f)
        {
            yield return null;
        }

        CusterCtrl.instance.setMoveDir = 0;

        yield return new WaitForSeconds(1);
        CusterAnim.instance.Flip(true);
        yield return new WaitForSeconds(1);
        CusterAnim.instance.Flip(false);
        yield return new WaitForSeconds(0.5f);
        //Custer Passes Out
        CusterCtrl.instance.PassOut();
        CusterCtrl.instance._passOut = true;
        CusterCtrl.instance.callTimer = true;

        //Set Line Active
        playerLine.SetActive(true);
        gameIsPlaying = true;
        //begin level
        SpawnEnemy.instance.StartSpawn(2,3);
    }

    public void GameOverStart()
    {
      
        SpawnEnemy.instance.StopSpawn();
        AudioManager.instance.PlaySound("PlayerHit");

        StartCoroutine(GameOver());
    }
    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2);
        bgColorChange = true;
        playerLine.SetActive(false);
        gameIsPlaying = false;
        AudioManager.instance.PlaySound("PlayerLose");
        yield return new WaitForSeconds(2);
        bgColorChange = false;
        
        UIManagerB.instance.GameOverScreenActive();

       
    }
    private void Update()
    {
        if (bgColorChange)
        {
            Color32 bgColor = _backGround.GetComponent<SpriteRenderer>().color;
            Color32 newColor = new Color32(152, 39, 39, 255);

            _backGround.GetComponent<SpriteRenderer>().color = Color32.Lerp(bgColor, newColor, 1f * Time.deltaTime);
        }
    }
}
