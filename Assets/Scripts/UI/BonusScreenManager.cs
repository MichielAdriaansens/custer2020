using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BonusScreenManager : MonoBehaviour
{
    public static BonusScreenManager instance;

    public GameObject _noAdsButton;
    public Sprite checkMark;


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
    // Start is called before the first frame update
    void Start()
    {
    //    BonusScreenGotNoAd();
    }

    public void BonusScreenGotNoAd()
    {
        //checkpurchase removead
 //       if (IAPManager.instance.CheckPurchased(IAPManager.instance.removeAds))
//        {
            _noAdsButton.transform.GetComponent<Image>().sprite = checkMark;
            _noAdsButton.transform.GetComponent<Button>().enabled = false;
            _noAdsButton.GetComponentInChildren<Text>().enabled = false;
//        }
    }
}
