using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndjunScript : MonoBehaviour
{
    GameManager gm;

    Animator anim;
    public GameObject corpse;
    public float _hp = 50;
    public bool inContact;

    private void Start()
    {
        gm = GameManager.instance;

        anim = GetComponentInChildren<Animator>();
        _hp = gm.levelUpCounter; 
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            inContact = true;

            //laat player animationScript weten dat er contact is
            col.GetComponentInChildren<PlayerAnimation>().atIndjun = inContact;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            inContact = false;
            col.GetComponentInChildren<PlayerAnimation>().atIndjun = inContact;
        }
    }

    void Dead()
    {
        Instantiate(corpse,transform.position,Quaternion.identity);

        Destroy(gameObject);
    }

    public void GetHit()
    {
        if (gm.playerDied == false && inContact && !gm.playerDirLeft)
        {
            anim.SetBool("GetHit", true);
            _hp--;
            gm.levelUpCounter--;
            gm.score++;



            gm.ManageScore();

            if (_hp <= 0)
            {
                Dead();
                gm.PlayerWin();

                //turn off current playerscore if playing
                if (AudioManager.instance.SoundActive("PlayerScore"))
                {
                    AudioManager.instance.StopSound("PlayerScore");
                }

                AudioManager.instance.LevelUpSound();
            }
            else
            {
                //WinSound
                if (!AudioManager.instance.SoundActive("PlayerScore"))
                {
                    AudioManager.instance.PlaySound("PlayerScore");
                }
            }
        }
    }

    void Update()
    {
        // conditions to take/when given damage
        
       if (!gm.mobileMode && Input.GetKeyDown(KeyCode.Space))
       {
            GetHit();
       }
        
    }
}
