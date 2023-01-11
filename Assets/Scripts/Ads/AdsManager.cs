using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
    public static AdsManager instance;

    private string googlePlayID = "3561094";

    public int adPlayCount = 1;

    public bool runAds = true;

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

    private void Start()
    {
        InitializeAds();
    }

    private void InitializeAds()
    {
        Advertisement.Initialize(googlePlayID,true);
    }

    public void ShowAd()
    {
        if (adPlayCount != 0)
        {
            if (PlayerPrefs.HasKey("AdCount"))
            {
                if (PlayerPrefs.GetInt("AdCount") >= adPlayCount)
                {
                    if (Advertisement.IsReady("video"))
                    {
                        Advertisement.Show("video");
                    }
                    PlayerPrefs.SetInt("AdCount", 0);
                }
                else
                {
                    PlayerPrefs.SetInt("AdCount", PlayerPrefs.GetInt("AdCount") + 1);
                }
            }
            else
            {
                PlayerPrefs.SetInt("AdCount", 0 + 1);
            }
        }
        else
        {
            if (Advertisement.IsReady("video"))
            {
                Advertisement.Show("video");
            }
        }
    }
}
