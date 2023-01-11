using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss03 : MonoBehaviour
{
    SpriteRenderer sRender;
    Animator anim;
    public GameObject minion;
    public GameObject hitFx;
    public GameObject deadObj;

    public int lives = 5;

    float speed = 1;
    bool moveActive = true;
    public bool goLeft = false;
    public bool goDown = false;

    public bool gotHit = false;
    bool _dead = false;

    public float maxRng = 5;

    // Start is called before the first frame update
    void Start()
    {
        sRender = transform.Find("UFORender").GetComponent<SpriteRenderer>();
        anim = transform.Find("UFORender").GetComponent<Animator>();
        //start go up and down timer
        StartCoroutine(AttackTimer());
    }
    IEnumerator AttackTimer()
    {
        float _rng = Random.Range(1.5f, maxRng);
        yield return new WaitForSeconds(_rng);

        Attack();
    }
    void Attack()
    {
        if(goDown)
        {
            goDown = false;
        }
        else if(!gotHit)
        {
            goDown = true;
        }

        StartCoroutine(AttackTimer());
    }

    void MoveX()
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
    void MoveY()
    {
        if (goDown)
        {
            if (transform.position.y >= 0.5f)
            {
                transform.position = transform.position + (Vector3.down * speed) * Time.deltaTime;
            }
        }
        else if(transform.position.y <= 2.55f)
        {
            transform.position = transform.position + (Vector3.up * speed) * Time.deltaTime;
        }
    }

    void GotHit()
    {
        gotHit = true;
        if (goDown)
        {
            goDown = false;
        }

        AudioManager.instance.KillSoundB();

        lives--;
        speed++;

        maxRng -= 0.5f;

        GetComponent<BoxCollider2D>().enabled = false;

        //changeColor
        Color32 transCol = new Color32(255, 255, 255, 75);
        sRender.color = transCol;

        StartCoroutine(WhileHit());

        if(lives > 0)
        {
            Spawner();
        }
    }
    IEnumerator WhileHit()
    {
        float _rng = Random.Range(1.5f, 5);
        yield return new WaitForSeconds(_rng);

        GetComponent<BoxCollider2D>().enabled = true;
        Color fullCol = new Color(255, 255, 255, 255);
        sRender.color = fullCol;

        gotHit = false;
    }


    void Death()
    {
        AchievementManager.instance.KillBossesAchieve();

        transform.position = new Vector3(0, 2.55f, 0);

        GetComponent<BoxCollider2D>().enabled = false;
        moveActive = false;
        anim.SetBool("Dead", true);

        SpawnEnemy.instance.bossLevel++;
        BonusGameManager.instance.killScore++;

        StartCoroutine(WhileDead());
    }
    IEnumerator WhileDead()
    {
        yield return null;
        GameObject explode1 = Instantiate(hitFx);

        Vector3 newpos = this.transform.position;

        float minY = Random.Range(-0.2f, 0.25f);
        float minX = Random.Range(-0.3f, 0.3f);

        newpos.y += minY;
        newpos.x += minX;

        explode1.transform.position = newpos;
        AudioManager.instance.KillSoundB();
        yield return new WaitForSeconds(0.7f);

        minY = Random.Range(-0.2f, 0.25f);
        minX = Random.Range(-0.3f, 0.3f);

        newpos.y += minY;
        newpos.x += minX;

        GameObject explode2 = Instantiate(hitFx);
        explode2.transform.position = newpos;
        AudioManager.instance.KillSoundB();

        yield return new WaitForSeconds(0.5f);
        GameObject explode3 = Instantiate(hitFx);
        minY = Random.Range(-0.2f, 0.25f);
        minX = Random.Range(-0.3f, 0.3f);

        newpos.y += minY;
        newpos.x += minX;

        explode3.transform.position = newpos;
        AudioManager.instance.KillSoundB();

        yield return new WaitForSeconds(0.6f);
        GameObject explode4 = Instantiate(hitFx);
        explode4.transform.position = this.transform.position;
        AudioManager.instance.PlaySound("PlayerHit");
        //ragdoll
        GameObject _dead = Instantiate(deadObj);
        _dead.transform.position = this.transform.position;

        Destroy(transform.Find("UFORender").gameObject);

        yield return new WaitForSeconds(2);
        //Open Congratulations screen
        UIManagerB.instance.CongratsDisplay.SetActive(true);
        UIManagerB.instance.CalculateEnding();
        // SpawnEnemy.instance.bossMode = false;

    }

    void Spawner()
    {
        GameObject spawn1 = Instantiate(minion);
        spawn1.GetComponent<Enemy01>()._zigZag = false;
        spawn1.GetComponent<Enemy01>()._fall = false;
        spawn1.GetComponent<Enemy01>()._target = true;
        spawn1.GetComponent<Enemy01>().speed = 2;
        spawn1.transform.position = new Vector3(2.61f, 3.2f, 0);

        GameObject spawn2 = Instantiate(minion);
        spawn2.GetComponent<Enemy01>()._zigZag = false;
        spawn2.GetComponent<Enemy01>()._fall = false;
        spawn2.GetComponent<Enemy01>()._target = true;
        spawn2.GetComponent<Enemy01>().speed = 2;

        spawn2.transform.position = new Vector3(-2.61f, 3.2f, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Ecol" && !CusterCtrl.instance.dead)
        {
            if(lives > 0)
            {
                GotHit();
            }
            else if (!_dead)
            {
                _dead = true;
                Death();
            }
        }
    }

    void Update()
    {
        if (moveActive)
        {
            MoveX();
            MoveY();
        }        
    }
}
