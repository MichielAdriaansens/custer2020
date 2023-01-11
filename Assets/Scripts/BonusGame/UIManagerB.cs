using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagerB : MonoBehaviour
{
    [HideInInspector]
    public static UIManagerB instance;

    public GameObject scoreDisplay;
    public GameObject InstructionDisplay;
    public GameObject inGameDisplay;
    public GameObject gameOverDisplay;
    [Header("ending")]
    public GameObject CongratsDisplay;
    public GameObject GoodEnd;
    public GameObject BadEnd;
    [Space]
    public Text drinkScore;
    public Text killScore;
    public Text infoText;
    public Text DrinkResult;
    bool textBlink = false;
    bool bad = false;

    public GameObject continueButton;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    private void Start()
    {
        gameOverDisplay.SetActive(false);
        scoreDisplay.SetActive(false);
        InstructionDisplay.SetActive(true);
        infoText.gameObject.SetActive(false);
        inGameDisplay.SetActive(true);
        GoodEnd.SetActive(false);
        BadEnd.SetActive(false);
        continueButton.SetActive(false);
        CongratsDisplay.SetActive(false);
    }

    
    void UpdateDrinkScoreUI()
    {
        if (CusterCtrl.instance.drink)
        {
            drinkScore.text = CusterCtrl.instance.drinkPoints.ToString();

            HighScoreManager.instance.HighDrinkScore(CusterCtrl.instance.drinkPoints);
        }
    }
    void UpdateKillScore()
    {
        killScore.text = BonusGameManager.instance.killScore.ToString();

        HighScoreManager.instance.HighKillScore(BonusGameManager.instance.killScore);
    }

    public void InstructionStartButton()
    {
        InstructionDisplay.SetActive(false);

        BonusGameManager.instance.InitializeStartLevel();
        StartCoroutine(ShowScoreDelay());

        infoText.gameObject.SetActive(true);
        infoText.text = "Get Ready";
        InfoBlinkStart();
        
    }
    IEnumerator ShowScoreDelay()
    {
        yield return null;
        bool check = false;

        while (!check)
        {
            yield return new WaitForSeconds(0.01f);
           check = BonusGameManager.instance.gameIsPlaying;
        }

        scoreDisplay.SetActive(true);

        textBlink = false;
        infoText.text = "START!!!";

        yield return new WaitForSeconds(1.5f);

        infoText.gameObject.SetActive(false);
    }

    void InfoBlinkStart()
    {
        textBlink = true;
        StartCoroutine(InfoBlink());
    }
    IEnumerator InfoBlink()
    {
        bool check = true;
        Color newColor = infoText.color;
        

        while (check)
        {
            newColor.a = 1;
            infoText.color = newColor;
            yield return new WaitForSeconds(1);
            
            newColor.a = 0;
            infoText.color = newColor;
            yield return new WaitForSeconds(0.5f);

            check = textBlink;
        }

        newColor.a = 1;
        infoText.color = newColor;
    }
    //infoBlinkStop = textblink bool uitzetten

    public void GameOverScreenActive()
    {
        gameOverDisplay.SetActive(true);
    }

    public void RetryButton()
    {
        AudioManager.instance.StopAllSound();
     
        if (AdsManager.instance.runAds)
            AdsManager.instance.ShowAd();

        SceneManager.LoadScene(3);
    }
    public void QuitButton()
    {
        AudioManager.instance.StopAllSound();

        if (AdsManager.instance.runAds)
            AdsManager.instance.ShowAd();

        SceneManager.LoadScene(1);
    }

    public void ContinueButton()
    {
        CongratsDisplay.SetActive(false);
        SpawnEnemy.instance.bossMode = false;

        //activate Death
        if (bad)
            SpawnEnemy.instance.badEnding = true;
    }

    public void CalculateEnding()
    {
        DrinkResult.text = CusterCtrl.instance.drinkPoints.ToString();

        StartCoroutine(CalculateEndWait());

    }
    IEnumerator CalculateEndWait()
    {
        yield return new WaitForSeconds(1.5f);

        if (CusterCtrl.instance.drinkPoints >= 500)
        {
            //good Ending
            GoodEnd.SetActive(true);
            BadEnd.SetActive(false);
        }
        else
        {
            //bad Ending
            GoodEnd.SetActive(false);
            BadEnd.SetActive(true);

            bad = true;
        }

        continueButton.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        if (scoreDisplay != null)
        {
            UpdateDrinkScoreUI();
            UpdateKillScore();
        }
    }
}
