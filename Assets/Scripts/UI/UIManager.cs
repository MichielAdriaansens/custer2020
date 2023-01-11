using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    Text scoreCounter;
    public List<Image> lifePoints = new List<Image>();

    public GameObject lifeBar;

    public GameObject GOScreem;

    public GameObject mobileControls;

    public GameObject GetReadyScreen;

    public GameObject skipIntroButton;

    bool bloop;

    public bool introSkip = false;

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

        if(lifeBar == null)
        {
            Debug.Log("Assign lifeBar in UI manager");
        }
        GetReadyScreen.SetActive(false);
    }

    private void Start()
    {
        scoreCounter = GameObject.Find("_UIManager/HUD/InGame/ScoreCounter").GetComponent<Text>();

        foreach (Transform Child in lifeBar.transform)
        {
            lifePoints.Add(Child.GetComponent<Image>());
        }

        LifeBar();
        GameManager.instance.ManageScore();

        if(GOScreem)
        GOScreem.SetActive(false);

        //Show Attack Button
        if (GameManager.instance.mobileMode)
        {
            mobileControls.SetActive(true);
        }
        else
        {
            mobileControls.SetActive(false);
        }
        skipIntroButton.SetActive(false);

    }

    public void ScoreCounterUpdate(int newScore)
    {
        scoreCounter.text = newScore.ToString();
    }

    public void LifeBar()
    {
        for (int i = 0; i < lifePoints.Count; i++)
        {
 
            int pointOrder = i;

            if (pointOrder < GameManager.instance.playerLives -1) //-1 ivbm Last Life.. geen punten zichtbaar in HUD
            {
                Color newColor = lifePoints[i].color;
                newColor.a = 1;
                lifePoints[i].color = newColor;
            }
            else
            {
                Color newColor = lifePoints[i].color;
                newColor.a = 0;
                lifePoints[i].color = newColor;
            }


        }
    }

    public void StartIntroScreen()
    {
        GetReadyScreen.SetActive(true);
        StartCoroutine(GetReadyActive());
    }
    IEnumerator GetReadyActive()
    {
        bloop = true;
        Text GR = GetReadyScreen.transform.Find("TextReady").GetComponent<Text>();
        Image imgGo = GetReadyScreen.transform.Find("IMGStart").GetComponent<Image>();
        Color newColor = GR.color;
        newColor.a = 0;
        GR.color = newColor;
        imgGo.color = newColor;

        yield return new WaitForSeconds(1f);
        StartCoroutine(TextBlink(bloop, GR,newColor));

        yield return new WaitForSeconds(6);
        bloop = false;
        introSkip = true;
        skipIntroButton.SetActive(true);

        newColor.a = 1;
        GR.color = newColor;
        GR.text = "3";
        yield return new WaitForSeconds(1f);
        GR.text = "2";
        yield return new WaitForSeconds(1f);
        GR.text = "1";
        yield return new WaitForSeconds(1f);
        
        if (skipIntroButton == true)
        {
            skipIntroButton.SetActive(false);
        }

        GR.gameObject.SetActive(false);

        newColor.a = 1;
        imgGo.color = newColor;
        yield return new WaitForSeconds(1f);
        GetReadyScreen.SetActive(false);

    }
    IEnumerator TextBlink(bool _bloop, Text _GR,Color _newColor)
    {
        yield return null;

        while (_bloop)
        {
            _newColor.a = 1;
            _GR.color = _newColor;

            yield return new WaitForSeconds(0.5f);

            _newColor.a = 0;
            _GR.color = _newColor;

            yield return new WaitForSeconds(0.5f);

            _bloop = bloop;
        }
    }

    public void StopIntroScreen()
    {
        skipIntroButton.SetActive(false);
        
        StopCoroutine(GetReadyActive());

        Text _gr = GetReadyScreen.transform.Find("TextReady").GetComponent<Text>();
        Color newC = _gr.color;

        newC.a = 0;

        _gr.color = newC;

        StartCoroutine(StartGo());

    }
    IEnumerator StartGo()
    {
        yield return null;
        Image imgGo = GetReadyScreen.transform.Find("IMGStart").GetComponent<Image>();
        Color newColor = imgGo.color;
        newColor.a = 1;

        imgGo.color = newColor;

        yield return new WaitForSeconds(1f);
        GetReadyScreen.SetActive(false);
    }
    public void GameOverScreen()
    {
        GOScreem.SetActive(true);
    }

    public void EndGameButton()
    {
        AudioManager.instance.StopSound("PlayerLose");

        if(AdsManager.instance.runAds)
            AdsManager.instance.ShowAd();

        SceneManager.LoadScene(1);
    }
}
