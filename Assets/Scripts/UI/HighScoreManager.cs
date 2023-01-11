using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager instance;
 
    
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }
        if (!PlayerPrefs.HasKey("DrinkHighScore"))
        {
            PlayerPrefs.SetInt("DrinkHighScore", 0);
        }
        if (!PlayerPrefs.HasKey("KillHighScore"))
        {
            PlayerPrefs.SetInt("KillHighScore", 0);
        }
    }

    public void UpdateHighScore(int _score)
    {
        if (_score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", _score);
        }

        if (_score == 69)
        {
            //69
            AchievementManager.instance.classic69Achieve();
        }

        if (_score == 250)
        {
            AchievementManager.instance.HighScore1();
        }

        if(_score == 500)
        {
            AchievementManager.instance.HighScore2();
        }

        if(_score == 1000)
        {
            AchievementManager.instance.HighScore3();
        }
    }
    public void HighDrinkScore(int _score)
    {
        if (_score > PlayerPrefs.GetInt("DrinkHighScore"))
        {
            PlayerPrefs.SetInt("DrinkHighScore", _score);
        }
        //achievement
        if(_score == 250)
            AchievementManager.instance.DrinkPointsAchieve1();

        if(_score == 500)
        {
            //drink 500
            AchievementManager.instance.Drink500Achieve();
        }
        
    }
    public void HighKillScore(int _score)
    {
        if (_score > PlayerPrefs.GetInt("KillHighScore"))
        {
            PlayerPrefs.SetInt("KillHighScore", _score);
        }
        //achievement
        if(_score == 666)
            AchievementManager.instance.SixSixSixAchieve();
        
        if (_score == 1000)
            AchievementManager.instance.KillPoints1();
    }
}
