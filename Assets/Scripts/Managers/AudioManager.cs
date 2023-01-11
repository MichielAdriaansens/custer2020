using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Range(0, 1)]
    public float masterVolume = 1;
    public Sound[] sounds;

    public bool sleepLoopOn = false;    

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _object = new GameObject("Audio" + i + " " + sounds[i].name);
            _object.transform.parent = this.transform;

            sounds[i].SetSource(_object.AddComponent<AudioSource>());
            
        }

        SetMasterVolume(masterVolume);

        //example in OtherScripts Call AudioManager.instance.PlaySound(insertname);
        
    }
    void SetMasterVolume(float volume)
    {
        for (int i =0; i < sounds.Length; i++)
        {
            sounds[i].soundSource.volume = volume;
        }
    }

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {

                if (SoundActive("LevelBackground") && sounds[i].name != "LevelBackground")
                {
                    StopSound("LevelBackground");
                   
                    StartCoroutine(WaitCallBG(sounds[i]));
                }
                    //playsound
                    sounds[i].Play();
                return;
            }
        }

        Debug.LogWarning("E Man! er is geen kaulo sound met de naam " + _name);
    }
    IEnumerator WaitCallBG(Sound sound)
    {
        yield return new WaitForSeconds(sound.soundSource.clip.length);

        // or || doet raar om de of andere reden 
        if (GameManager.instance != null && !GameManager.instance.roundOver)
        {
            PlaySound("LevelBackground");
        }
        if (BonusGameManager.instance != null && !CusterCtrl.instance.dead && !CusterCtrl.instance._passOut)
        {
            PlaySound("LevelBackground");
        }
      

        yield return new WaitForSeconds(0.1f);

        if (SoundActive(sound.name))
        {
            StopSound("LevelBackground");
        }
    }

    public void StopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Stop();
            }
        }
    }

    public void StopAllSound()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (SoundActive(sounds[i].name))
            {
                StopSound(sounds[i].name);
            }
        }
    }

    public bool SoundActive(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                if (sounds[i].soundSource.isPlaying)
                    return true; 
                else
                    return false;
            }
        }

        return false;
    }

    public void LevelUpSound()
    {
        PlaySound("LevelUp");
        StartCoroutine(LevelUpWait());
    }
    IEnumerator LevelUpWait()
    {
        yield return new WaitForSeconds(1.463f);
        PlaySound("PlayerScore");
    }

    public void StartBonus()
    {
        StartCoroutine(StartSoundB());
    }
    IEnumerator StartSoundB()
    {
        PlaySound("LevelStart");
        yield return new WaitForSeconds(6);
        StopSound("LevelStart");
    }

    public void KillSoundB()
    {
        int rng = Random.Range(0, 5);

        if(rng <= 1)
        {
            PlaySound("Ehit01");
        }
        else if(rng > 1 && rng < 4)
        {
            PlaySound("Ehit02");
        }
        else if(rng == 4)
        {
            PlaySound("Ehit03");
        }

       // StartCoroutine(KillSound());
    }
    IEnumerator KillSound()
    {
        if (!CusterCtrl.instance.dead)
        {
            PlaySound("PlayerHit");
            yield return new WaitForSeconds(0.5f);
            StopSound("PlayerHit");
        }
    }

    //Loop timerKO while passout
    public void TimerSoundLoopB()
    {
        StartCoroutine(LoopSoundTimer());
    }
    IEnumerator LoopSoundTimer()
    {
        sleepLoopOn = true;
        StopSound("LevelBackground");
        //wait for KO sound to Finish
        yield return new WaitForSeconds(1.5f);

        while(CusterCtrl.instance._passOut && !CusterCtrl.instance.nearDeath)
        {
            PlaySound("Sleep");

            yield return new WaitForSeconds(2.85f);
        }
        
        StopSound("Sleep");
        sleepLoopOn = false;

        if (!CusterCtrl.instance._passOut && !CusterCtrl.instance.dead)
        {
            yield return new WaitForSeconds(1.6f);
            PlaySound("LevelBackground");
        }
       
    }

    public void NearDeathSound()
    {
        if(!SoundActive("NearDeath"))
        {
            PlaySound("NearDeath");
        }
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip soundClip;
    public AudioSource soundSource;
    public bool soundLoop = false;


    public void SetSource(AudioSource _source)
    {
        soundSource = _source;
        _source.clip = soundClip;
        _source.loop = soundLoop;
    }

    public void Play()
    {
        soundSource.Play();
    }

    public void Stop()
    {
        soundSource.Stop();
    }
}