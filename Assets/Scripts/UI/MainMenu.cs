using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GooglePlayGames;


public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;

    public Text highScore;

    public GameObject ErrorLogin;

    public GameObject noAdsButton;

    public GameObject MainMenuScreen;

    public GameObject BonusScreen;

    public GameObject unLockMode;

    public GameObject shareScreen;

    public bool _online = false;

    public bool errorDisplay = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        MainMenuScreen.SetActive(true);
        BonusScreen.SetActive(true);
        BonusScreen.GetComponent<Canvas>().enabled = false;

        shareScreen.SetActive(true);
        shareScreen.GetComponent<Canvas>().enabled = false;

        if (PlayerPrefs.GetInt("BuyNoAd") != 1)
        {
            unLockMode.SetActive(false);
        }

        highScore.text = "High score: " + PlayerPrefs.GetInt("HighScore").ToString();

        StartCoroutine(WaitFrame());

    }
    IEnumerator WaitFrame()
    {
        yield return null;

        //verstuur naar Leaderboard
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            PlayService.instance.ScoreToLB(PlayerPrefs.GetInt("HighScore"), 0);
            PlayService.instance.ScoreToLB(PlayerPrefs.GetInt("DrinkHighScore"), 1);
            PlayService.instance.ScoreToLB(PlayerPrefs.GetInt("KillHighScore"), 2);

            if (!_online) 
            {
                _online = true;
            }
        }
        else
        {
            // Wacht een seconde en probeer opnieuw je score te updaten.. als speler nog niet is ingelogd
            yield return new WaitForSeconds(1);

            if (!_online) 
            {
                StartCoroutine(WaitFrame());
            }
        }
    }
    public void PlayButton()
    {
        SceneManager.LoadScene(2);

        AchievementManager.instance.UnlockClassicAchieve();
    }

    public void ShowErrorLogin()
    {
        if (ErrorLogin != null)
        {
            errorDisplay = true;
            ErrorLogin.SetActive(true);
        }
    }
    public void CloseErrorLogin()
    {
        errorDisplay = false;
        ErrorLogin.SetActive(false);
    }
    public void UnableRemoveAdsButton()
    {
        noAdsButton.SetActive(false);
    }
    public void BonusScreenButton()
    {
        SceneManager.LoadScene(3);

        /*
        if (PlayerPrefs.GetInt("HighScore") < 350)
        {
            MainMenuScreen.GetComponent<Canvas>().enabled = false;
            BonusScreen.GetComponent<Canvas>().enabled = true;
        }
        else
        {
            SceneManager.LoadScene(3);
        }
        */
    }
    public void ReturnButton()
    {
        MainMenuScreen.GetComponent<Canvas>().enabled = true;

        if (BonusScreen.GetComponent<Canvas>().enabled)
            BonusScreen.GetComponent<Canvas>().enabled = false;
        else if(shareScreen.GetComponent<Canvas>().enabled)
            shareScreen.GetComponent<Canvas>().enabled = false;
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    public void UnlockModeActive()
    {
        unLockMode.SetActive(true);
    }

    public void shareScreenButton()
    {
        MainMenuScreen.GetComponent<Canvas>().enabled = false;
        shareScreen.GetComponent<Canvas>().enabled = true;
        ShareScreen.instance.SetShareScreen();
    }

    //Temp
    public void PPDelete()
    {
        PlayerPrefs.DeleteAll();
    }
}
