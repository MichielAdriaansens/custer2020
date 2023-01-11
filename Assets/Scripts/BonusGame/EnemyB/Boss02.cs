using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss02 : MonoBehaviour
{
    public GameObject hitBox;
    public GameObject blood;
    Animator anim;
    Rigidbody2D rb;
    Color32 curColor;
    Color32 newColor;

    [Header ("Stats")]
    public int lives =3;
    public float speed = 2;
    public bool bounceMode = false;
    bool gotHit = false;
    public bool outOfBounds = false;
    float receiveForse = 3;
    public bool bornIntro;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (bornIntro)
        {
            curColor = GetComponent<SpriteRenderer>().color;
            newColor = curColor;
            newColor.a = 0;

            hitBox.SetActive(false);
            rb.isKinematic = true;

            StartCoroutine(Born());
        }
    }
    //intro/birth Optional
    IEnumerator Born()
    {
        yield return new WaitForSeconds(1f);
        rb.isKinematic = false;
        hitBox.SetActive(true);
    }

    //movement pattern all over the place

    //standard Attack.. drop shit

    //Get Hit/Attack when Hit activate physics and bounce around.. with every hit increase force

    //Death
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.transform.tag == "Ecol")
        {
            if (!gotHit)
            {
                gotHit = true;
                StartCoroutine(Invinsible());
                lives--;
                Instantiate(blood,this.transform);

                if (lives > 0)
                {
                    Vector3 forceDir = transform.position - collision.transform.position;
                    forceDir.Normalize();

                    rb.velocity = Vector2.zero;
                    rb.AddForce(forceDir * (receiveForse * 100), ForceMode2D.Force);
                }
                else
                {
                    rb.velocity = Vector2.zero;
                    rb.isKinematic = true;
                    rb.sharedMaterial = null;
                    anim.SetBool("Dead", true);
                    hitBox.SetActive(false);
                    StartCoroutine(waitDeath());

                    SpawnEnemy.instance.bossLevel++;
                    BonusGameManager.instance.killScore++;
                    SpawnEnemy.instance.bossMode = false;
                }
            }
        }
    }
    IEnumerator waitDeath()
    {
        yield return new WaitForSeconds(0.6f);
        Destroy(this.gameObject);
    }
    private IEnumerator Invinsible()
    {
        yield return new WaitForSeconds(0.3f);
        gotHit = false;
        
    }

    void RecoverOffScreen()
    {
        float _x =  Random.Range(-2.45f, 2.45f);

        transform.position = new Vector3(_x, 2.8f, 0);
        outOfBounds = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -0.6f || transform.position.y > 3.5f)
        {
            outOfBounds = true;
        }
        if(transform.position.x < -3.5f || transform.position.x > 3.5f)
        {
            outOfBounds = true;
        }

        if (outOfBounds)
        {
            RecoverOffScreen();
        }

        if (CusterCtrl.instance.dead)
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }
}
