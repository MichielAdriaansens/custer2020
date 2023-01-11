using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public List<GameObject> csImages = new List<GameObject>();
    Image faderBlack;

    public GameObject _bliksem1;
    public GameObject _bliksem2;
    public GameObject _flash;

    public GameObject ripLogo;
    Color logoColor;

    bool skipIntro = false;
    void Awake()
    {
        GameObject custsceneholder = gameObject.transform.Find("CutScenes").gameObject;
        foreach (Transform child in custsceneholder.transform)
        {
            csImages.Add(child.gameObject);
        }

        faderBlack = transform.Find("Canvas/FaderBlack").GetComponent<Image>();
    }


    void Start()
    {
        csImages[0].SetActive(true);

       

        StartCoroutine(csTimeLine());

        StartCoroutine(WaitSkip());

        logoColor = ripLogo.GetComponent<Image>().color;
        logoColor.a = 0;

        ripLogo.GetComponent<Image>().color = logoColor;
    }


    IEnumerator csTimeLine()
    {
        yield return new WaitForSeconds(0.05f);

        StartCoroutine(Fader(true));
       // StartCoroutine(Zoom(5, csImages[0]));

        yield return new WaitForSeconds(0.5f);
        logoColor.a = 1;
        ripLogo.GetComponent<Image>().color = logoColor;

        AudioManager.instance.PlaySound("PlayerScore");
        //  StartCoroutine(Fader(false));

        yield return new WaitForSeconds(1.5f);
        csImages[0].SetActive(false);
        csImages[1].SetActive(true);

     //   StartCoroutine(Fader(true));
        StartCoroutine(Zoom(10,csImages[1]));
        AudioManager.instance.PlaySound("LetsGetIt");

        yield return new WaitForSeconds(2f);
        AudioManager.instance.StopSound("LetsGetIt");
        Destroy(ripLogo);

        //flash
        StartCoroutine(Flash());
        csImages[1].SetActive(false);

        AudioManager.instance.PlaySound("PlayerHit");
        yield return new WaitForSeconds(2f);
        StopAllCoroutines();
        GoToStart();

    }
    void GoToStart()
    {

        AudioManager.instance.StopAllSound();

        SceneManager.LoadScene(1);
    }
    //flash
    IEnumerator Flash()
    {
        Image bliksem1 = _bliksem1.GetComponent<Image>();
        Image bliksem2 = _bliksem2.GetComponent<Image>();
        Image flits = _flash.GetComponent<Image>();
        bliksem2.enabled = true;

        yield return new WaitForSeconds(0.5f);
        bliksem2.enabled = false;
        flits.enabled = true;

        yield return new WaitForSeconds(0.3f);
        flits.enabled = false;
        bliksem1.enabled = true;

        yield return new WaitForSeconds(0.5f);
        flits.enabled = true;
        bliksem1.enabled = false;
        yield return new WaitForSeconds(0.5f);
        flits.enabled = false;
    }
    
    //fadeIn/Out
    IEnumerator Fader(bool fadeOut)
    {
        if (!fadeOut)
        {
            for (float i = 0; i <= 1; i += 0.01f)
            {
                Color newColor = faderBlack.color;
                newColor.a = i;

                faderBlack.color = newColor;
                yield return null;
                
            }
        }
        else
        {
            for (float i = 1; i >= 0; i -= 0.01f)
            {
                Color newColor = faderBlack.color;
                newColor.a = i;

                faderBlack.color = newColor;
                yield return null;
               
            }
        }
    }
    //ZoomIn
    IEnumerator Zoom(float time, GameObject img)
    {
        yield return new WaitForEndOfFrame();

        for (float i = 0; i <= time; i += Time.deltaTime / 2)
        {
            float newScale = i / 5000;

            img.transform.localScale += new Vector3 (newScale,newScale, 1);
            yield return null;
        }
    }

    //Trigger bool to set skip Active
    IEnumerator WaitSkip()
    {
        yield return new WaitForSeconds(0.5f);
        skipIntro = true;
    }


    private void Update()
    {
        if (skipIntro && Input.anyKey)
        {
            GoToStart();
        }
    }
}
