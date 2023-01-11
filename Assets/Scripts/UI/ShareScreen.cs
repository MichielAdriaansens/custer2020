using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShareScreen : MonoBehaviour
{
    public static ShareScreen instance;

    public GameObject playerIMG;
    public GameObject enemyIMG;

   public Sprite[] player1;
   public Sprite[] player2;
   public Sprite[] enemy1;
   public Sprite[] enemy2;

    public Text scoreDisplay;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    int _random()
    {
        int r = Random.Range(0, 2);
        return r;
    }

    // Start is called before the first frame update
    public void SetShareScreen()
    {
        //bepaal welke images
        if(PlayerPrefs.GetInt("UnlockMode") == 0)
        {
            // custer
            playerIMG.GetComponent<Image>().sprite = player1[_random()];

            enemyIMG.GetComponent<Image>().sprite = enemy1[_random()];
        }
        else
        {
            //alien
            playerIMG.GetComponent<Image>().sprite = player2[_random()];

            enemyIMG.GetComponent<Image>().sprite = enemy2[_random()];
        }
        //bepaal Score
        scoreDisplay.text = PlayerPrefs.GetInt("HighScore").ToString();

        InitializeSS();
    }

    void InitializeSS()
    {
        GetComponent<SocialShare>().StartSSShare();
    }
}
