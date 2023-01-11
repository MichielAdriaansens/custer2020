using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassOutBar : MonoBehaviour
{
    public GameObject _custer;
    public GameObject wakeUpEffect;
    bool ragActive = false;
    GameObject _rag;
    Canvas thisCanvas;
    public Image fill;
    public Image fill2;
    float posY = 1.5f;
    public bool warningOn = false;
    bool warningTrigger = false;
    // Start is called before the first frame update
    void Start()
    {
        thisCanvas = GetComponent<Canvas>();
        thisCanvas.enabled = false;
    }

    void Follow()
    {
        if(_custer.transform.Find("Bonus_Ragdoll(Clone)") == null)
        {
            transform.position = new Vector3(_custer.transform.position.x, posY, 0);

            if (ragActive)
            {
                thisCanvas.enabled = false;
                ragActive = false;
            }
        }
        else
        {
            if (!ragActive)
            {
                _rag = _custer.transform.Find("Bonus_Ragdoll(Clone)").gameObject;
                StartCoroutine(WaitCanvasActive());

                ragActive = true;
            }
            float _x = _rag.transform.Find("torso_ragdoll_Player").transform.position.x;
            transform.position = new Vector3(_x, posY, 0);
        }
    }
    IEnumerator WaitCanvasActive()
    {
        yield return new WaitForSeconds(1.5f);
        thisCanvas.enabled = true;
    }

    public void SetTimerUI(float score)
    {
        float scorePercent = CalculatePercentage(30, score,true);

        fill.fillAmount = scorePercent /100;
    }
    float CalculatePercentage(float max, float current, bool reverse)
    {
        float newCurrent = current;

        if (reverse)
        {
            newCurrent = max - current; // reverse percentageoutcome 0,25% will be 0,75%
        }

        float _percentage = (newCurrent / max) * 100;
        return _percentage;
    }
    public void SetDeathTimer(float score)
    {
        float scorePercent = CalculatePercentage(10, score,false);
        fill2.fillAmount = scorePercent / 100;
    }
    public void WarningGlowDeathUI(bool _switch)
    {
        if (!warningTrigger)
        {
            if (_switch)
            {
                warningOn = true;   //control While Loop blink
                warningTrigger = true; //control if the function may be called
                //start the glow blink
                StartCoroutine(Fill2Glow());
            }
        }
        if(!_switch)
        {
            warningOn = false;
            warningTrigger = false;
        }

    }
    IEnumerator Fill2Glow()
    {
        Color32 oldColor = fill2.color;

        Color32 newColor = fill2.color;
        newColor.r = 255;
        newColor.b = 07;
        newColor.a = 255;

        while (warningOn)
        {
            fill2.color = newColor;
            yield return new WaitForSeconds(1);
            fill2.color = oldColor;
            yield return new WaitForSeconds(0.75f);
        }

        fill2.color = oldColor;
    }
    public void DestroyPassOutBar()
    {
        Destroy(this.gameObject);
    }

    public void PassOutBarSucces()
    {
        StartCoroutine(passOutSuccesDelay());
    }
    IEnumerator passOutSuccesDelay()
    {
        yield return new WaitForSeconds(0.03f);
        GameObject effect = Instantiate(wakeUpEffect);
        effect.transform.position = transform.position;

        yield return new WaitForSeconds(1);
        Destroy(effect);
    }


    // Update is called once per frame
    void Update()
    {
        Follow();
    }
}
