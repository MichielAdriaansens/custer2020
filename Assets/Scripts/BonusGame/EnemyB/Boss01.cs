using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss01 : MonoBehaviour
{
    Animator anim;
    public BoxCollider2D hitBox;
    Color32 lvlColor;
    Color32 newColor;
    public GameObject shield;
    public GameObject fx;
    public GameObject fxSparks;
    public GameObject projectile1;

    public int difficultyLvl = 1;  //0 easy,1 medium,2 hard
    public int lives = 3;
    public float speed = 1;
    public bool shieldOn = false;

    bool _move = false;
    bool goLeft = false;

    bool _dead = false;
    bool gotHit = false;
    [Header ("AutoAttack")]
    public bool agressiveBegin = false;
    public bool _load = false;
    public bool isAttacking = false;
    int attType = 0;

    private void Start()
    {
        anim = GetComponent<Animator>();
        hitBox = GetComponent<BoxCollider2D>();
        Shield(false);

        ColorChange(difficultyLvl);
        if(difficultyLvl == 1)
        {
            lives = 5;
            speed = 1.5f;
        }
        else if(difficultyLvl == 2)
        {
            lives = 8;
            speed = 2;
        }

        
        StartCoroutine(BossStartUp());

        Invoke("AttackLoader", 1.5f);
    }
    IEnumerator BossStartUp()
    {
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("Load", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Load", false);
        yield return new WaitForSeconds(0.25f);
        _move = true;

        if(difficultyLvl == 1)
        {
            callShield();
        }   
        else if(difficultyLvl == 2)
        {
            Shield(true);
        }
    }

    void ColorChange(int _level)
    {
        if(_level == 1)
        {
            newColor.r = 255;
            newColor.b = 255;
            newColor.g = 255;

            newColor.a = 255;
        }
        else if(_level == 2)
        {
            newColor.r = 255;
            newColor.b = 0;
            newColor.g = 255;

            newColor.a = 255;
        }
        else if (_level == 0)
        {
            newColor.r = 190;
            newColor.b = 0;
            newColor.g = 145;

            newColor.a = 255;
        }

        GetComponent<SpriteRenderer>().color = newColor;
    }

    void Move()
    {
        if (!goLeft)
        {
            if (transform.position.x <= 2.75f)
            {
                transform.position = transform.position + (Vector3.right * speed) * Time.deltaTime;
            }
            else
            {
                goLeft = true;
            }
        }
        else
        {
            if (transform.position.x >= -2.75f)
            {
                transform.position = transform.position + (Vector3.left * speed) * Time.deltaTime;
            }
            else
            {
                goLeft = false;
            }
        }
    }
    
    void AttackLoader()
    {
        agressiveBegin = true;
        _load = true;
        //bepaal welke attack
        int rngAttack = 0;
        if(difficultyLvl == 1)
        {
            rngAttack = Random.Range(0, 4);
        }
        else if(difficultyLvl == 2)
        {
            rngAttack = Random.Range(0, 3);
        }

        if (rngAttack == 2)
        {
            //attack02
            attType = 1;
        }
        else
        {
            //attack01
            attType = 0;
        }


        //bepaal load tijd
        float _time = Random.Range(1f, 5);
        StartCoroutine(ALoad(_time));

    }
    IEnumerator ALoad(float t)
    {
        
        yield return new WaitForSeconds(t);
        if (!gotHit && !isAttacking)
        {
            if (attType == 0)
            {
                Attack01();
                yield return new WaitForSeconds(1.2f);
                _load = false; //reset bool to activate AttackLoader in Update
            }
            else if(attType == 1)
            {
                Attack02();
            }

        }
        
       
    }

    void Attack01()
    {
        isAttacking = true;
        if (shieldOn)
            Shield(false);

        _move = false;
        anim.SetBool("Load", true);

        StartCoroutine(WhileAttack01());
    }
    IEnumerator WhileAttack01()
    {
        float loadTime = 0.5f;

        if(difficultyLvl == 2)
        {
            loadTime = 0.25f;
        }
        else if(difficultyLvl == 1)
        {
            loadTime = 0.37f;
        }

        yield return new WaitForSeconds(loadTime);
        //attack anim
        anim.SetBool("Attack", true);
        anim.SetBool("Load", false);
        yield return new WaitForSeconds(0.3f);
        if (!gotHit)
        {
            //instantiate projectile
            GameObject projectile = Instantiate(projectile1);
            Vector3 spawnPos = this.transform.position;
            spawnPos.y = spawnPos.y - 0.66f;

            projectile.transform.position = spawnPos;
        }

        if (difficultyLvl == 2)
        {
            Shield(true);
        }
        else if(difficultyLvl == 1)
        {
            int rng = Random.Range(0, 3);
            if(rng == 2)
            {
                callShield();
            }
        }
        yield return new WaitForSeconds(0.3f);
        
        _move = true;
        isAttacking = false;
    }

    void Attack02()
    {
        isAttacking = true;
        if (shieldOn)
            Shield(false);
        speed += 1;
        anim.SetBool("Load", true);
        StartCoroutine(WhileAttack02());
    }
    IEnumerator WhileAttack02()
    {
        _move = false;
        float loadTime = 1;
        if(difficultyLvl == 2)
        {
            loadTime = 0.4f;
        }
        else if(difficultyLvl == 1)
        {
            loadTime = 0.6f;
        }

        yield return new WaitForSeconds(loadTime);
        _move = true;

        if(difficultyLvl == 1)
        {
            int r = Random.Range(0, 4);
            if(r == 3)
            {
                callShield();
            }
        }

        int attCount = Random.Range(3, 8);
        float interval = Random.Range(0.2f, 1);

        for(int i = 0; i < attCount; i++)
        {
            if (gotHit)
            {
                break;
            }
            //instantiate projectile
            GameObject projectile = Instantiate(projectile1);
            Vector3 spawnPos = this.transform.position;
            spawnPos.y = spawnPos.y - 0.66f;
            projectile.transform.position = spawnPos;

            yield return new WaitForSeconds(interval);
        }

        yield return new WaitForSeconds(0.25f);
        if (difficultyLvl == 2)
        {
            Shield(true);
        }
        isAttacking = false;
        speed -= 1;
        anim.SetBool("Load", false);
        _load = false;
    }

    void Shield(bool active)
    {
        if (active)
        {
            if (!gotHit || !_dead)
            {
                shieldOn = true;
                hitBox.isTrigger = false;
                shield.SetActive(true);
            }
        }
        else
        {
            shieldOn = false;
            hitBox.isTrigger = true;
            shield.SetActive(false);
        }
    }
    void callShield()
    {
        float rng = Random.Range(1, 3);

        StartCoroutine(tempShield(rng));
    }
    IEnumerator tempShield(float t)
    {
        Shield(true);
        yield return new WaitForSeconds(t);
        if (shieldOn)
            Shield(false);
    }
    
    void GotHit()
    {
        gotHit = true;

        anim.SetBool("Hit", true);
        lives--;
        AudioManager.instance.PlaySound("Ehit03");

        _move = false;
        _load = false;
        hitBox.isTrigger = false;

        StartCoroutine(WhileHit());
    }
    IEnumerator WhileHit()
    {
        GameObject spark = Instantiate(fxSparks);
        spark.transform.position = this.transform.position;

        newColor = GetComponent<SpriteRenderer>().color;
        newColor.g = 0;
        newColor.b = 0;

        GetComponent<SpriteRenderer>().color = newColor;

        yield return new WaitForSeconds(0.6f);

        ColorChange(difficultyLvl);

        Vector3 newPos = transform.position;
        newPos.x = Random.Range(-2.8f, 2.8f);
        transform.position = newPos;

        if(difficultyLvl == 1)
        {
            //shield
            callShield();
        }
        else if(difficultyLvl == 2)
        {
            Shield(true);
        }

        gotHit = false;
        _move = true;

        if(!shieldOn)
            hitBox.isTrigger = true;
        speed++;

        if(spark != null)
        {
            Destroy(spark);
        }
    }


    void Death()
    {
        if(shieldOn)
            Shield(false);

        GameObject spark = Instantiate(fxSparks);
        spark.transform.position = this.transform.position;

        newColor = GetComponent<SpriteRenderer>().color;
        newColor.g = 0;
        newColor.b = 0;
        GetComponent<SpriteRenderer>().color = newColor;

        _move = false;
        SpawnEnemy.instance.bossLevel++;
        BonusGameManager.instance.killScore++;
        StartCoroutine(WhileDead());
    }
    IEnumerator WhileDead()
    {
        AudioManager.instance.PlaySound("PlayerHit");
        anim.SetBool("Hit", true);
        yield return new WaitForSeconds(0.5f);
      
        _move = false;

        GameObject blood = Instantiate(fx);
        GameObject spark = Instantiate(fxSparks);
        blood.transform.position = this.transform.position;
        spark.transform.position = this.transform.position;

        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1);
        SpawnEnemy.instance.bossMode = false;
        Destroy(gameObject);
    }

    private void Update()
    {
        if (_move)
            Move();

        if (!_load && agressiveBegin)
            AttackLoader();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        {
            if (lives > 0)
            {
                GotHit();
            }
            else if(!_dead)
            {
                _dead = true;
                Death();
            }
        }
    }
}
