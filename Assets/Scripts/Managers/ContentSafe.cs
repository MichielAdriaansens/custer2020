using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentSafe : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("UnlockMode"))
        {
            PlayerPrefs.SetInt("UnlockMode", 0); 
        }

        PlayerPrefs.SetInt("BuyNoAd", 0);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

}
