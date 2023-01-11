using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SocialShare : MonoBehaviour
{

    string shareMessage()
    {
        
        string message = "type your whatever here";
        int r = Random.Range(0, 6);
        if (r == 0)
        {
            message = "Woawiee this amazing game from google play, jippie! A true blessing from the gods. I scored " + PlayerPrefs.GetInt("HighScore").ToString() + " points in Custer 2020!";
        }
        else if(r == 1)
        {
            message = "Did you ever hold a bananna to your ear and pretend it was a phone? Hihi, also i scored " + PlayerPrefs.GetInt("HighScore").ToString() + " points in Custer 2020. You can download it from google play. Fun stuff, good times!";
        }
        else if(r == 2)
        {
            message = "My fellow American, i did not have.. relations with that woman. I did however download Custer 2020 from the google play store for my Android mobile phone device and scored " + PlayerPrefs.GetInt("HighScore").ToString() + " points. Fantastic!";
        }
        else if(r == 3)
        {
            message = "Whoever gets a highscore of 1000 points wins a MILLION FICTIONAL DOLLARS!$$$ Transferred through the power of imagination. Incredible! So far i got " + PlayerPrefs.GetInt("HighScore").ToString() + "points. I'm getting close hihi. PS: found this game on google play, it's the best!";
        }
        else if (r == 4)
        {
            message = "Pew pew pew!! LAZER gun! I download this game from google play for mobile cellphone. " + PlayerPrefs.GetInt("HighScore").ToString() + " is the amount of points i have aquired while playing Custer 2020. Points are good, i feel happy. Good Times!";
        }
        else if(r == 5)
        {
            message = "Did you ever play Custer 2020, how did it make you feel? I scored " + PlayerPrefs.GetInt("HighScore").ToString() + " points, isn't that great? Jippie! I am the best!";
        }

        return message;
    }

    public void StartSSShare()
    {
        StartCoroutine(WaitBeforeSS());
    }
    IEnumerator WaitBeforeSS()
    {
        yield return new WaitForSeconds(0.4f);

        StartCoroutine(TakeSSAndShare());
    }
    private IEnumerator TakeSSAndShare()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        // To avoid memory leaks
        Destroy(ss);

        new NativeShare().AddFile(filePath).SetSubject("Custer 2020").SetText(shareMessage()).Share();

        // Share on WhatsApp only, if installed (Android only)
     //   if( NativeShare.TargetExists( "com.whatsapp" ) )
     //   	new NativeShare().AddFile( filePath ).SetText(shareMessage()).SetTarget( "com.whatsapp" ).Share();
      
    }
}
