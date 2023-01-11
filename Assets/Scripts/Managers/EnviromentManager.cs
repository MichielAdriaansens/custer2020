using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentManager : MonoBehaviour
{
    public static EnviromentManager instance;
    bool flashSwitch = false;
    public GameObject bg02;
    public GameObject bg01;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        
    }

   
    public void BGDamage()
    {
        flashSwitch = true;

        StartCoroutine(FlashTimer());
    }
    IEnumerator FlashTimer()
    {
        StartCoroutine(FlashBG());

        float CloudSp = GameObject.FindGameObjectWithTag("AnimBG").GetComponent<Animator>().speed;
        GameObject.FindGameObjectWithTag("AnimBG").GetComponent<Animator>().speed = CloudSp + 4f;

        yield return new WaitForSeconds(2.1f);

        GameObject.FindGameObjectWithTag("AnimBG").GetComponent<Animator>().speed = CloudSp;
        flashSwitch = false;
        
    }
    IEnumerator FlashBG()
    {
        bool _fSwitch = true;

        yield return null;

        while (_fSwitch)
        {
            bg02.SetActive(true);
            bg01.SetActive(false);
            yield return new WaitForSeconds(0.15f);
            bg02.SetActive(false);
            bg01.SetActive(true);
            yield return new WaitForSeconds(0.15f);
            _fSwitch = flashSwitch;
        }
    
        bg02.SetActive(false);
        bg01.SetActive(true);
    }
}
