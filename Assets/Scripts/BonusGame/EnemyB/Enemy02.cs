using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy02 : MonoBehaviour
{
    public int unitType = 0;
    Animator anim;
    Color32 newColor;
    public GameObject hitFx;
    public Transform target;
    public float tSpot;
    bool targetPassOut = false;
    public bool spotted = false;
    public int spotCount = 0;
    bool isAttacking = false;
    public int attackCounter = 0;
    bool isRecovering = false;
    float limitY;

    [Header("Movement")]
    public float speedY = 1;
    public float speedX = 1;

    public bool moveY;
    public bool moveX;
    bool _goLeft = false;


    private void Start()
    {
        limitY = transform.position.y;
        anim = GetComponent<Animator>();

        //set trigger for target detection in Update
        if (CusterCtrl.instance._passOut)
            targetPassOut = false;
        else
            targetPassOut = true;
        

        target = GameObject.FindWithTag("Player").transform;
        if(transform.position.x > 0)
        {
            _goLeft = true;
        }

        if(unitType == 0)
        {

            newColor = Color.blue;
            newColor.a = 255;
            GetComponent<SpriteRenderer>().color = newColor;
        
            int rng = Random.Range(0, 2);
            if(rng == 0)
            {
                speedX = 4;
                speedY = 0.5f;
            }
            else if(rng == 1)
            {
                speedX = 5;
                speedY = 1.2f;
            }
        }
        else if(unitType == 1)
        {
            newColor = Color.magenta;
            newColor.a = 255;
            GetComponent<SpriteRenderer>().color = newColor;

            moveX = true;
            moveY = false;
        }
    }
    void Fall()
    {
        transform.position = transform.position + (Vector3.down * speedY) * Time.deltaTime;

    }
    void FlyUp()
    {
        transform.position = transform.position + (Vector3.up * speedY) * Time.deltaTime;
    }
    void ZigZag()
    {
        if (_goLeft == true)
        {
            if (transform.position.x > -2.8f)
            {
                transform.position = transform.position + (Vector3.left * speedX) * Time.deltaTime;
            }
            else
            {
                _goLeft = false;
            }
        }
        else
        {
            if (transform.position.x < 2.8f)
            {
                transform.position = transform.position + (Vector3.right * speedX) * Time.deltaTime;
            }
            else
            {
                _goLeft = true;
            }
        }
    }
    void DetermineTarget()
    {
        if (CusterCtrl.instance._passOut && !targetPassOut)
        {
            if (GameObject.FindWithTag("Ragdoll") != null)
            {
                target = GameObject.FindWithTag("Ragdoll").transform.Find("torso_ragdoll_Player").transform;
            }
            targetPassOut = false;
        }
        else if (!CusterCtrl.instance._passOut && targetPassOut)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetPassOut = true;
        }
    }
    void ScanTarget()
    {
        if(target != null)
            tSpot = target.transform.position.x - transform.position.x;

        int inSight = Mathf.RoundToInt(tSpot);

        if(inSight != 0)
        {
            spotted = false;
        }
        if (!spotted)
        {
            if (inSight == 0)
            {
                if(spotCount == 0)
                {
                    //set attackCounter
                    int rng = Random.Range(1, 6);
                    attackCounter = rng;
                }

                spotCount++;
                if (!isAttacking)
                {
                    AttackLoop();
                }
                spotted = true;
            }
        }
    }
    void AttackLoop()
    {
        if(spotCount >= attackCounter)
        {
            Attack();
          //  Debug.LogWarning("BINGO");
            spotCount = 0;
        }
    }
    void Attack()
    {
        isAttacking = true;
        anim.SetBool("Attack",true);
        moveX = false;
        moveY = true;
        
        if (attackCounter < 2)
            speedY = 1.5f;
        else if (attackCounter <3 && attackCounter > 1)
            speedY = 2f;
        else if (attackCounter > 2 && attackCounter <5)
        {
            speedY = 2.5f;
        }
        else if (attackCounter == 5)
        {
            speedY = 3;
        }

    }
    void Recover()
    {
        if (transform.position.y < 0.5f)
        {
            isRecovering = true;
           // isAttacking = false;
            anim.SetBool("Attack", false);
        }
        if (isRecovering)
        {
            if (transform.position.y < limitY)
            {
                FlyUp();
            }
            else
            {
                isRecovering = false;
                isAttacking = false;
                moveX = true;
                moveY = false;
            }
        }
    }
    void OutOfBounds()
    {
        if(transform.position.y < -0.5f)
        {
            Destroy(this.gameObject);

        }
    }
    // Update is called once per frame
    void Update()
    {
        DetermineTarget();

        if (unitType == 1)
        {
            ScanTarget();
            if (isAttacking)
            {
                Recover();
            }
        }
        if (moveY)
        {
            if (!isRecovering)
            {
                Fall();
            }
        }
        if (moveX)
        {
            ZigZag();
        }

        OutOfBounds();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Ecol" && !CusterCtrl.instance.dead)
        {

            anim.SetBool("Dead",true);
            GetComponent<BoxCollider2D>().enabled = false;

            GameObject fx = Instantiate(hitFx);
            Vector3 newPos = this.transform.position;
            newPos.z = 0;

            fx.transform.position = newPos;
            
            BonusGameManager.instance.killScore++;
            AudioManager.instance.KillSoundB();
            StartCoroutine(waitDead());
           
        }

    }
    IEnumerator waitDead()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(this.gameObject);
    }
}
