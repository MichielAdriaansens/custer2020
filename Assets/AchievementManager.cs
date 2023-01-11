using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        
    }

    // Start is called before the first frame update
    public void OpenAchievementPanel()
    {
        Social.ShowAchievementsUI();
    }

    public void UnlockClassicAchieve() //done
    {
        Social.ReportProgress(GPGSIds.achievement_classic, 100f, null);
    }

    public void UnlockAlienAchieve() //done
    {
        //play Classic with Alien
        Social.ReportProgress(GPGSIds.achievement_unlockalien, 100f, null);
    }

    public void HighScore1()
    {
        //HS 250

        Social.ReportProgress(GPGSIds.achievement_highscore_250, 100f, null);

    }
    public void HighScore2()
    {
        //500
        Social.ReportProgress(GPGSIds.achievement_highscore_500, 100f, null);
    }
    public void HighScore3()
    {
        //1000
        Social.ReportProgress(GPGSIds.achievement_highscore_1000, 100f, null);
    }

    public void DrinkPointsAchieve1() //done
    {
        //250 drink points
        Social.ReportProgress(GPGSIds.achievement_drink_250, 100f, null);
    }

    public void SixSixSixAchieve() //done
    {
        //666 kill points
        Social.ReportProgress(GPGSIds.achievement_666, 100f, null);
    }

    public void KillBossesAchieve() //done
    {
        //all bosses dead
        Social.ReportProgress(GPGSIds.achievement_kill_all_bosses, 100f, null);
    }

    public void KillPoints1()
    {
        //1000 kill points
        Social.ReportProgress(GPGSIds.achievement_1000kill, 100f, null);
    }

    public void Drink500Achieve()
    {
        Social.ReportProgress(GPGSIds.achievement_drink_500, 100f, null);
    }

    public void classic69Achieve()
    {
        Social.ReportProgress(GPGSIds.achievement_69, 100f, null);
    }
}
