using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockModeUI : MonoBehaviour
{
   public GameObject buttonHighlight;
   public Transform _custerButton;
   public Transform _alienButton;

    public Sprite alienSelect;
    public Sprite custerSelect;

    public Sprite custerUnSelect;
    public Sprite alienUnselect;


    public void Start()
    {
        if (!PlayerPrefs.HasKey("UnlockMode"))
        {
            PlayerPrefs.SetInt("UnlockMode", 0);
        }
        else if(PlayerPrefs.GetInt("UnlockMode") == 0)
        {
           CusterButton();
        }
        else if(PlayerPrefs.GetInt("UnlockMode") == 1)
        {
            AlienButton();
        }
    }

    public void CusterButton()
    {
        buttonHighlight.transform.position = _custerButton.transform.position;

        _custerButton.transform.GetComponent<Image>().sprite = custerSelect;
        _alienButton.GetComponent<Image>().sprite = alienUnselect;

        PlayerPrefs.SetInt("UnlockMode", 0);
    }

    public void AlienButton()
    {
        AchievementManager.instance.UnlockAlienAchieve();

        buttonHighlight.transform.position = _alienButton.transform.position;

        _alienButton.GetComponent<Image>().sprite = alienSelect;
        _custerButton.transform.GetComponent<Image>().sprite = custerUnSelect;

        PlayerPrefs.SetInt("UnlockMode", 1);
    }
}
