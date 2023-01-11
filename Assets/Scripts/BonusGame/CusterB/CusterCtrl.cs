using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CusterCtrl : MonoBehaviour
{
    public static CusterCtrl instance;
    private void Awake()
    {
        if (instance == null)
            instance = this; 
        else
            Destroy(this);

        hitBox = GetComponent<BoxCollider2D>();
    }

    public GameObject custerRender;
    public GameObject ragDoll;
    GameObject newRag;
    BoxCollider2D hitBox;
    public GameObject blood;
    public PassOutBar passOutBar;
   
    [Header("Movement")]
    public float speed = 1;
    public int moveDir = 0;
    public int setMoveDir = 0;
    bool goLeft;

    float rightBorder = 2.1f;
    [Header("AI State")]
    public bool drink;
    public bool _patrol;
    public bool _passOut;
    public bool _isDead;

    [HideInInspector]
    public bool dead = false;
    [HideInInspector]
    public bool drinkForRag = false;
    //Timer
  //  [HideInInspector]
    public bool callTimer = false;
    [HideInInspector]
    public bool TimerIsActive = false;

    [Header("Timer")]
    public float timeScore = 30;
    float standUpScore = 0;
    public float timeStep = 1f;
    public bool timerPause = false;
    [Space]
    public bool neglectTrigger = false;
    public bool neglectActive = false;

    public bool deathTimerActive = false;
    public float maxDeathTime = 15;
    public bool nearDeath = false;
    [Space]    
    public float drinkTime = 30;
    public float _passOutTimer = 0;
    [Header("Points")]
    public int drinkPoints = 0;
    int drinkAmount = 0;

    // -1 = left, 0 = Idle, 1 = Right
    public void Move(int dir)
    {
        moveDir = dir;

        if (!dead)
        {
            if (dir == 1 && transform.position.x < 2.5)
                transform.Translate(Vector2.right * speed * Time.deltaTime);
            else if (dir == -1 && transform.position.x > -2.5)
                transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }

    public void Patrol()
    {
        if (!goLeft)
        {
            if (transform.position.x < 2.5)
            {
                Move(1);
            }
            else
            {
                goLeft = true;
            }
        }
        else
        {
            if(transform.position.x > -2.5)
            {
                Move(-1);
            }
            else
            {
                goLeft = false;
            }
        }
    }
    
    //Custer Drinks Gets points gradually while drinking
    public void Drink()
    {
        if (!dead)
        {
            drink = true;
            drinkAmount++;
            drinkForRag = true;
            CusterAnim.instance.DrinkAnim();

            if (drinkAmount != 1)
            {
                AudioManager.instance.PlaySound("LevelUp");
            }
        }
    }
   
    public void PassOut()
    {
        if(!dead)
            _passOut = true;

        timerPause = true;
        drink = false;
        _patrol = false;
        speed = 2;
        Move(0);

        hitBox.enabled = false;
        custerRender.SetActive(false);

        AudioManager.instance.PlaySound("KO");
        AudioManager.instance.TimerSoundLoopB();

        if (passOutBar != null)
            passOutBar.SetTimerUI(30);

        if (newRag == null)
        {
            newRag = Instantiate(ragDoll, this.transform);
            newRag.transform.position = transform.position;
            if (CusterAnim.instance.faceLeft)
            {
                newRag.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    public void Death()
    {
        dead = true;
        _passOut = false;
        AudioManager.instance.StopAllSound();
        if (newRag == null)
        {
            PassOut(); //activates Ragdoll first to explode
            StartCoroutine(CusterExplode());
        }
        else
        {
            StartCoroutine(CusterExplode());
        }
        Handheld.Vibrate();
        Camera.main.GetComponent<Shaker>().Shake(1, 1f);
        //verplaats naar gameManager Gameover functie
        BonusGameManager.instance.GameOverStart();
        passOutBar.DestroyPassOutBar();
    }
    IEnumerator CusterExplode()
    {
        yield return new WaitForSeconds(0.1f);

        List<Transform> limbs = new List<Transform>();
        foreach (Transform child in newRag.transform)
        {
            limbs.Add(child);
        }
        for (int i = 0; i < limbs.Count; i++)
        {
           
            if (limbs[i].GetComponent<Rigidbody2D>())
            {
                Rigidbody2D rb = limbs[i].GetComponent<Rigidbody2D>();
               // limbs[i].gameObject.layer = 8;

                rb.velocity = Vector2.zero;
                rb.rotation = 0;
                

                if (limbs[i].transform.name != "LegLower_ragdoll_Player" && limbs[i].transform.name != "ArmLower_ragdoll_Player" && limbs[i].transform.name != "beer_bottle")
                {
                      
                    if(limbs[i].GetComponent<HingeJoint2D>()!= null)
                          limbs[i].GetComponent<HingeJoint2D>().enabled = false;


                    if (limbs[i].transform.name == "Hat_ragdoll_Player")
                        limbs[i].gameObject.layer = 13;
                  //    Random.InitState(System.DateTime.Now.Millisecond);
                    int y = Random.Range(2, 6);
                  //      Random.InitState(System.DateTime.Now.Millisecond);
                    int x = Random.Range(-2, 2);
                    Vector2 _force = new Vector2(x, y);

                    rb.velocity = _force;
                    rb.AddTorque(x);

                    if(limbs[i].transform.name != "torso_ragdoll_Player")
                    {
                        int r = Random.Range(5, 10);
                        int plusMin = Random.Range(0, 2);
                        if(plusMin == 0) { r = r * -1; }

                        rb.AddTorque(r);
                    }
                        

                    if(limbs[i].childCount == 0)
                      {
                          if(limbs[i].transform.name != "Hat_ragdoll_Player")
                          {
                              GameObject particleEffect = Instantiate(blood, limbs[i].transform);
                              particleEffect.transform.position = limbs[i].transform.position;
                          }
                      }
                    

                }

            }
        }

    }

    public void WakeUpDone()
    {
        _passOut = false;
        callTimer = false;
        TimerIsActive = false;

        //reset timeScore
        timeScore = 30;
        timeStep = 1;
        CalculateDrinkTime(_passOutTimer);
        
        //reset States
        drinkForRag = false;
        _patrol = false;


        hitBox.enabled = true;
        custerRender.SetActive(true);

        //transform.position.x = waar de transform.X van de ragdoll LegsLower voor het laatst was
        Vector3 newPos = newRag.transform.Find("LegLower_ragdoll_Player").transform.position;

        Destroy(newRag); //als dit error geeft verrander newrag naar null

        //transform.position.y = default positie (voor het geval op zijn muts overeind staat)
        newPos.y = 0;

        transform.position = newPos;

        //fireworks particle effect && Wait a second and cal Next action
        passOutBar.PassOutBarSucces();

        StartCoroutine(WaitAfterWakeUp());

        if(AudioManager.instance.SoundActive("Sleep"))
            AudioManager.instance.StopSound("Sleep");

        AudioManager.instance.PlaySound("PlayerScore");
    }
    IEnumerator WaitAfterWakeUp() //Start Drink
    {
        yield return new WaitForSeconds(1f);

        StartCoroutine(GetDrink());
    }
    IEnumerator GetDrink()
    {
        if (!_passOut)
        {
            setMoveDir = 1;
            while (transform.position.x < rightBorder)
            {
                yield return null;
            }

            setMoveDir = 0;
            yield return new WaitForSeconds(0.5f);
            Drink();
            yield return new WaitForSeconds(0.5f);

            //Change for difficulty mechanic.. Maybe
            speed = 2;
            _patrol = true;

            //start timer for pass out
            DrinkTimerStart(drinkTime);

        }
    }

    void DrinkTimerStart(float time)
    {
        //coroutine
        StartCoroutine(DrinkTimer(time));
    }
    IEnumerator DrinkTimer(float _time)
    {
        float _drinkTime = _time;

        //start punten per seconde zolang custer drinkt
        StartCoroutine(DrinkPointsScore());

        yield return new WaitForSeconds(_drinkTime);

        PassOut();
    }
    IEnumerator DrinkPointsScore()
    {
        if(drinkPoints == 0)
        {
            drinkPoints += 1;
        }
        while(drink)
        {
           
            yield return new WaitForSeconds(0.5f);
            drinkPoints++;
        }

        yield return null;
    }

    float CalculateAngle(GameObject go)
    {
        Vector3 relativeToWorld = go.transform.TransformDirection(transform.up);
        Vector2 target = relativeToWorld;

        float _angle = Vector2.Angle(Vector2.up, target);
        return _angle;
    }
    void CalculateDrinkTime(float time)
    {
        float passOutTime = time;

        if (passOutTime < 20)
        {
            drinkTime = 60;
        }
        if (passOutTime > 20 && passOutTime < 40)
        {
            drinkTime = 30;
        }
        if (passOutTime > 40)
        {
            drinkTime = 15;
        }

        //reset _passoutTimer
        _passOutTimer = 0;
    }

    // WakeUpCounter
    void GetUpTimer()
    {
        if (!timerPause)
        {
            if(timeScore > 0)
            {
                timeScore -= 1 * (Time.deltaTime * timeStep);
                if (passOutBar != null)
                    passOutBar.SetTimerUI(timeScore);
            }
            else if (timeScore < 0.1f && !dead)
            {
                WakeUpDone();
                callTimer = false;
            }
        }
    }
    IEnumerator TimerWakeUp()
    {
        TimerIsActive = true;
        for (float i = timeScore; i > 0; i--)
        {
            if (!timerPause)
            {
                yield return new WaitForSeconds(timeStep);
               
            }
            else //TimerPause is Active
            {
                while (timerPause)
                {
                    yield return new WaitForSeconds(0.01f);
                  
                }
            }

            if (timeScore > -1 && !timerPause)
            {
                timeScore--;
                //updateUI
                if(passOutBar != null)
                    passOutBar.SetTimerUI(timeScore);
            }
            if(timeScore == 0 && !dead)
            {
                WakeUpDone();
            }
        }

    }
    public void ActivateNeglectTimer()
    {
        StartCoroutine(WaitNeglectTimer());
    }
    IEnumerator WaitNeglectTimer()
    {
        //wait before neglect timer starts
        for (int i = 0; i < 10; i++)
        {
            //print(i);
            if (timerPause && !dead)
            {
                yield return new WaitForSeconds(1);
            }
            else
            {
                break;
            }
        }

        //set bool active to make timer run in Update
        neglectActive = true;

    }
    void NeglectTimer()
    {
        if (timerPause)
        {
            if (timeScore <= 30)
            {
                timeScore += 2.5f * Time.deltaTime;
                if (passOutBar != null)
                    passOutBar.SetTimerUI(timeScore);
            }
            else if (timeScore > 30f && !dead)
            {
                 
                deathTimerActive = true;
               
                neglectActive = false;
            }
        }
    }
    void DeathCountTimer()
    {
        passOutBar.WarningGlowDeathUI(true); //trigger bool to run once in PassOutBar script
        if (timerPause)
        {
            if (maxDeathTime > 0)
            {
                
                if(maxDeathTime < 2.4f)
                {
                    nearDeath = true;
                }

                if (maxDeathTime > 2.2f && maxDeathTime < 2.4f)
                {
                   
                    AudioManager.instance.NearDeathSound();
                }


                maxDeathTime -= 1f * Time.deltaTime;
                if (passOutBar != null)
                    passOutBar.SetDeathTimer(maxDeathTime);
            }
            else
            {
                Death();
                nearDeath = false;
                deathTimerActive = false;
                passOutBar.WarningGlowDeathUI(false);
            }
            
        }
        else
        {
            nearDeath = false;
            maxDeathTime = 15f;
            if (passOutBar != null)
                passOutBar.SetDeathTimer(maxDeathTime);

            timeScore = 30;
            deathTimerActive = false;
            passOutBar.WarningGlowDeathUI(false);
            if (!AudioManager.instance.sleepLoopOn && _passOut)
            {
                AudioManager.instance.TimerSoundLoopB();
            }
        }
    }

    //Keeps track of Time how long PassedOut
    void PassOutTimer()
    {
        _passOutTimer += Time.deltaTime;
    }

    void Update()
    {
        if(_patrol)
            Patrol();
        else
            Move(setMoveDir);

        if (_isDead)
        {
            Death();
            _isDead = false;
        }

        if (_passOut)
        {
            PassOutTimer();
           
            
            if (newRag != null) 
            {
                float feetAngle = CalculateAngle(newRag.transform.Find("LegLower_ragdoll_Player").gameObject);
                float torsoAngle = CalculateAngle(newRag.transform.Find("torso_ragdoll_Player").gameObject);

                float avarageScore = (feetAngle + torsoAngle) / 2;
                standUpScore = avarageScore;
              //  Debug.LogWarning(avarageScore);


                float headY = newRag.transform.Find("Head_ragdoll_Player").position.y;

                //start Wake Up Counter
                if (callTimer)
                {
                   // StartCoroutine(TimerWakeUp());
                    GetUpTimer();
                }

                //Pause Conditions
                if (headY < 0.5f)
                {
                    timerPause = true;
                    if (callTimer)
                    {
                        callTimer = false;
                    }

                    if (!neglectTrigger)
                    {
                        ActivateNeglectTimer(); //activate yield before starting NeglectTimer function
                        neglectTrigger = true;
                    }
                    
                }
                else
                {
                    if (timerPause)
                    {
                        timerPause = false;
                    }

                    neglectTrigger = false;
                    if(neglectActive)
                        neglectActive = false;
                }
                //neglectTimer on/off
                if (neglectActive)
                {
                    NeglectTimer();
                }
                //DeathTimer on/off
                if (deathTimerActive)
                {
                    DeathCountTimer();
                }

                //TimeStep Switch Conditions
                if (avarageScore < 40 && avarageScore > 30)
                {
                    timeStep = 1.5f;
                }
                else if(avarageScore < 30 && avarageScore > 25)
                {
                    timeStep = 2f;
                }
                else if(avarageScore < 25 && avarageScore >15)
                {
                    timeStep = 2.5f;
                }
                else if(avarageScore < 15 && avarageScore > 5)
                {
                    timeStep = 3.5f;
                }
                else if(avarageScore < 5)
                {
                    timeStep = 5f;
                }

            }
            
            
            
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Enemy")
        {
            if (!dead)
                Death();
        }
    }
    
}
