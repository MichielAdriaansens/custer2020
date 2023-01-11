using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class PlayService : MonoBehaviour
{
    public static PlayService instance;
        
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {

        PlayGamesPlatform.Activate();

        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            MainMenu.instance._online = true;
        }
        else
        {
            //  Login();
            StartCoroutine(BackUpLogIn());
        }
        
    }
    IEnumerator BackUpLogIn()
    {
        yield return new WaitForSeconds(1.5f);
        Login();
    }

    public void Login()
    {
        Social.localUser.Authenticate((bool succes) => 
        {
            if (!succes)
            {
                if(!MainMenu.instance.errorDisplay)
                    MainMenu.instance.ShowErrorLogin();
            }
            else
            {
                if(MainMenu.instance != null)
                {
                    if(MainMenu.instance.errorDisplay)
                        MainMenu.instance.CloseErrorLogin();
                }
            }

        });
    }


    public void GetPurchased(string pID)
    {
        //authenticate being logged in
        if(PlayGamesPlatform.Instance.IsAuthenticated())
        {
            //CheckPurchase if ID hasreceipt
            if (IAPManager.instance.CheckPurchased(pID))
            {
                IAPManager.instance.UnlockRemoveAds();
            }
        }
         else
        {
            MainMenu.instance.ShowErrorLogin();
       //     Debug.LogWarning("GetPurchased not Logged in");
        }
    }

    public void ScoreToLB(int _score, int board)
    {
        Social.localUser.Authenticate((bool succes) => 
        {
            if (succes)
            {
                //reportscore does not work outside localuser.authenticate
                if (board == 0)
                {
                    Social.ReportScore(_score, GPGSIds.leaderboard_high_score, (bool _succes) =>
                    {

                    });
                }
                else if(board == 1)
                {
                    //drink
                    Social.ReportScore(_score, GPGSIds.leaderboard_drink_score, (bool _succes) =>
                    {

                    });
                }
                else if (board == 2)
                {
                    Social.ReportScore(_score, GPGSIds.leaderboard_kill_score, (bool _succes) =>
                    {

                    });
                }
            }
     /*       else
            {
                Debug.LogWarning("ScoreToLB No Login");
               //  Login();
            }
     */
        });

    }


    public void ShowLeaderBoard()
    {
        Social.localUser.Authenticate((bool succes) => 
        {
            if (succes)
            {
                PlayGamesPlatform.Instance.ShowLeaderboardUI();
            }
            else
            {
              //             Debug.LogWarning("ShowLB no Login");

                MainMenu.instance.ShowErrorLogin();
                Login();
            }
        });
    }
}
