using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robo01 : MonoBehaviour
{
    public GameObject deadspawn;
    public GameObject hitFx;
    
    Animator anim;
    SpriteRenderer sRender;
    BoxCollider2D col2d;
    float speed = 1;

    bool moveOn = true;
    public bool left = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = transform.Find("RoboRender").GetComponent<Animator>();
        sRender = transform.Find("RoboRender").GetComponent<SpriteRenderer>();
        col2d = GetComponent<BoxCollider2D>();

    }

    void Move()
    {
        if (!left)
        {
            if (sRender.flipX == true)
                sRender.flipX = false;

            transform.position = transform.position + (Vector3.right * speed) * Time.deltaTime;
        }
        else
        {
            if(sRender.flipX == false)
                sRender.flipX = true;

            transform.position = transform.position + (Vector3.left * speed) * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Ecol" && !CusterCtrl.instance.dead)
        {
            GameObject _bang = Instantiate(hitFx);
            Vector3 newPos = this.transform.position;
            newPos.z = 0;
            newPos.y = 0.68f;
            _bang.transform.position = newPos;
          //  _bang.transform.parent = null;


            Dead();

        }
    }
    void Dead()
    {
        moveOn = false;
        col2d.enabled = false;
        anim.SetBool("Hit", true);
        //audio
        AudioManager.instance.KillSoundB();

        StartCoroutine(StartDeath());
        
    }
    IEnumerator StartDeath()
    {
        yield return new WaitForSeconds(0.5f);
        BonusGameManager.instance.killScore++;

        //explode Sound
        AudioManager.instance.KillSoundB();

        // spawn Explode
        GameObject _byeRobo = Instantiate(deadspawn);
        _byeRobo.transform.position = this.transform.position;

        //Destroy this gameobj
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(moveOn)
            Move();

        OutOfBounds();
    }

    void OutOfBounds()
    {
        if(transform.position.x <= -5)
        {
            Destroy(this.gameObject);
        }
        else if(transform.position.x >= 5)
        {
            Destroy(this.gameObject);
        }
    }
}
