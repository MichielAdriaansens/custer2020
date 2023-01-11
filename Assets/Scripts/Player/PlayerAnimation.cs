using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    PlayerController pCtrl;
    Animator anim;
    [HideInInspector]
    public bool faceRight = true;
    public bool controlActive = false;

    [HideInInspector]
    public bool atIndjun;
    // Start is called before the first frame update
    void Start()
    {
        pCtrl = transform.parent.GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
    }

   public void Attack01Anim()
    {

        anim.SetBool("Attack", true);


    }

    void MoveAnim()
    {
        if (pCtrl.moveDir.x != 0)
        {
            if (atIndjun && faceRight)
            {
                anim.SetBool("Walk", false);
            }
            else
            {
                anim.SetBool("Walk", true);
            }
        }
        else
        {
            anim.SetBool("Walk", false);
        }

        if (pCtrl.moveDir.x < 0) //move Left
        {
            if (faceRight)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                faceRight = false;
                //kijkrighting player versturen GameManager
                GameManager.instance.playerDirLeft = true;
            }
        }
        else if (pCtrl.moveDir.x > 0) //move right
        {
            if (!faceRight)
            {
                transform.localScale = new Vector3(1, 1, 1);
                faceRight = true;
                //kijkrighting player versturen GameManager
                GameManager.instance.playerDirLeft = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (controlActive)
        {
            MoveAnim();
        }
    }
}
